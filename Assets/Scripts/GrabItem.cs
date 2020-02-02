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

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (grabbedItem == null)
			{
				Ray ray = head.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, grabDistanceLimit, (1 << 8) | (1 << 9) | (1 << 0) | (1 << 11), QueryTriggerInteraction.Ignore) && hit.collider.GetComponent<Rigidbody>() != null)
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
					{
						if (hit.collider.gameObject.layer == 11)
						{
							grabItemRoutine = StartCoroutine(holdladder());
						}
						else
						{
							grabItemRoutine = StartCoroutine(holditem());
						}
					}
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

	private IEnumerator holdladder()
	{
		grabbedItem = hit.collider.gameObject.transform;
		Transform ogParent = grabbedItem.transform.parent;
		Rigidbody grabbedrb = grabbedItem.GetComponent<Rigidbody>();

		grabbedrb.useGravity = false;
		grabbedItem.transform.parent = grabPoint.transform;

		Quaternion ogRotation = grabbedItem.rotation;
		float time = 0f;
		foreach (Collider coll in grabbedItem.GetComponentsInChildren<Collider>())
		{
			Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, true);
		}
		while (grab)
		{
			Vector3 grabbedCenter = grabbedItem.transform.position + grabbedItem.transform.up;
			grabbedrb.velocity = Vector3.zero;
			time += Time.fixedDeltaTime;
			grabbedrb.velocity = (grabPoint.position - grabbedCenter) * Time.deltaTime * 1000;
			//grabbedItem.position = Vector3.MoveTowards(grabbedItem.position, grabPoint.position, Time.fixedDeltaTime * 10);
			grabbedItem.localRotation = Quaternion.Lerp(ogRotation, Quaternion.identity, time * 2);
			yield return new WaitForFixedUpdate();
		}
		grabbedItem.transform.parent = ogParent;
		grabbedrb.useGravity = true;
		//currentHoldDistance = defaultHoldDistance;
		//Destroy(copy);
		foreach (Collider coll in grabbedItem.GetComponentsInChildren<Collider>())
		{
			Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, false);
		}
		grabbedItem = null;
		grabItemRoutine = null;
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

		grabbedrb.useGravity = false;
		grabbedItem.transform.parent = grabPoint.transform;

		Quaternion ogRotation = grabbedItem.rotation;
		float time = 0f;
		foreach (Collider coll in grabbedItem.GetComponents<Collider>())
		{
			Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, true);
		}
		while (grab)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				grabbedrb.transform.rotation = Quaternion.identity;
			}
			Renderer r = grabbedItem.GetComponent<Renderer>();
			if (r == null)
				r = grabbedItem.GetComponentInChildren<Renderer>();
			Vector3 grabbedCenter = r.bounds.center;
			grabbedrb.velocity = Vector3.zero;
			time += Time.fixedDeltaTime;
			grabbedrb.velocity = (grabPoint.position - grabbedCenter) * Time.deltaTime * 1000;
			//grabbedItem.position = Vector3.MoveTowards(grabbedItem.position, grabPoint.position, Time.fixedDeltaTime * 10);
			//grabbedItem.rotation = Quaternion.Lerp(ogRotation, Quaternion.identity, time * 2);
			yield return new WaitForFixedUpdate();
		}
		grabbedItem.transform.parent = ogParent;
		grabbedrb.useGravity = true;
		//currentHoldDistance = defaultHoldDistance;
		//Destroy(copy);
		foreach (Collider coll in grabbedItem.GetComponents<Collider>())
		{
			Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, false);
		}
		grabbedItem = null;
		grabItemRoutine = null;
	}
}
