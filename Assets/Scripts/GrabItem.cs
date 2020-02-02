using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GrabItem : MonoBehaviour
{
	private RaycastHit hit;
	[SerializeField] float grabDistanceLimit = 6;
	[SerializeField] Transform grabPoint;
	public Transform grabbedItem;
	private Coroutine grabItemRoutine;
	//private GameObject underWeight;
	public bool grab = false;
	private Camera head { get { return Camera.main; } }
	[SerializeField] float defaultHoldDistance = 2.5f;
	private float currentHoldDistance;
	private bool rotate;

	// Start is called before the first frame update
	void Start()
    {
		currentHoldDistance = defaultHoldDistance;
	}



    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (grabbedItem == null)
			{
				Ray ray = head.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, grabDistanceLimit, (1 << 8) | (1 << 9) | (1 << 0), QueryTriggerInteraction.Ignore) && hit.collider.GetComponent<Rigidbody>() != null)
				{
					grab = true;

                    List<GameObject> connectedObjects = StaticHelpers.GetAllConnectedGameObjects(hit.collider.gameObject);
                    foreach(GameObject obj in connectedObjects)
                    {
                        if (obj.GetComponent<Rigidbody>() == null || obj.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll)
                        {
                            grab = false;
                            Debug.Log("CANT GRAB THIS");
                            break;
                        }
                    }

                    if (grab)
					    grabItemRoutine = StartCoroutine(holditem());
				}
			}
			else
			{
				grab = false;
			}
		}
		if (Input.GetMouseButtonDown(1) && grabbedItem != null)
		{
			rotate = true;
			StartCoroutine(RotateObject());

		}
		else if (Input.GetMouseButtonUp(1) && grabbedItem != null)
		{
			rotate = false;
		}
		currentHoldDistance = Mathf.Clamp(Input.mouseScrollDelta.y + currentHoldDistance, 1.5f, 8f);
		grabPoint.localPosition = new Vector3(0, 0, currentHoldDistance);
	}

	private IEnumerator RotateObject()
	{
		GetComponent<RigidbodyFirstPersonController>().enabled = false;

		Rigidbody grabbedrb = grabbedItem.GetComponent<Rigidbody>();
		while (rotate)
		{
			Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			grabbedrb.angularVelocity = transform.worldToLocalMatrix * new Vector3(-mouseMovement.y, -mouseMovement.x, 0f) * 10 * mouseMovement.magnitude;
			yield return new WaitForFixedUpdate();
		}

		GetComponent<RigidbodyFirstPersonController>().enabled = true;
	}

	private IEnumerator holditem()
	{
		grabbedItem = hit.collider.gameObject.transform;
		Transform ogParent = grabbedItem.transform.parent;
		Rigidbody grabbedrb = grabbedItem.GetComponent<Rigidbody>();
		//GameObject copy = Instantiate(grabbedItem.gameObject);
		//copy.name = "collider cheat object";
		//copy.transform.rotation = grabbedItem.rotation;
		//Destroy(copy.GetComponent<Rigidbody>());
		//copy.GetComponent<Renderer>().enabled = false;
		//Physics.IgnoreCollision(transform.GetComponent<Collider>(), grabbedItem.GetComponent<Collider>(), true);
		//Physics.IgnoreCollision(transform.GetComponent<Collider>(), copy.GetComponent<Collider>(), true);
		//float mag = copy.GetComponent<Collider>().bounds.size.magnitude;
		//float size = mag / (mag + 1f);
		//float ratio = Mathf.Abs(mag - size);
		//copy.transform.localScale /= size;
		//copy.transform.parent = grabbedItem;
		//copy.transform.localPosition = Vector3.zero;

		grabbedrb.useGravity = false;
		grabbedItem.transform.parent = grabPoint.transform;

		Quaternion ogRotation = grabbedItem.rotation;
		float time = 0f;
		Collider grabbedItemCollider = grabbedItem.GetComponent<Collider>();
		while (grab)
		{
			grabbedrb.velocity = Vector3.zero;
			time += Time.fixedDeltaTime;
			grabbedrb.velocity = (grabPoint.position - grabbedItem.position) * Time.deltaTime * 1000;
			//grabbedItem.position = Vector3.MoveTowards(grabbedItem.position, grabPoint.position, Time.fixedDeltaTime * 10);
			//grabbedItem.rotation = Quaternion.Lerp(ogRotation, Quaternion.identity, time * 2);
			yield return new WaitForFixedUpdate();
		}
		grabbedItem.transform.parent = ogParent;
		grabbedrb.useGravity = true;
		//currentHoldDistance = defaultHoldDistance;
		//Destroy(copy);
		Physics.IgnoreCollision(transform.GetComponent<Collider>(), grabbedItem.GetComponent<Collider>(), false);
		grabbedItem = null;
		grabItemRoutine = null;
	}
}
