﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using MoreLinq;
using TMPro;

public class NailShooter : MonoBehaviour
{
    private GameObject player;
    public GameObject nail_prefab;
    public Transform spawnPoint;
    public TextMeshProUGUI removeNailText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aimingReticleBaseColor = aimingReticle.GetComponent<Image>().color;
        aimingReticleNailColor.a = aimingReticleBaseColor.a;
        aimingReticlePickupColor.a = aimingReticleBaseColor.a;
    }

    [Range(0.5f, 10f)]
    public float nail_distance;
    [Range(1f, 75f)]
    public float nail_speed = 75f;

    public Animator aimingReticle;
    private Color aimingReticleBaseColor;
    public Color aimingReticleNailColor;
    public Color aimingReticlePickupColor;
    // Update is called once per frame
    void Update()
    {
        removeNailText.gameObject.SetActive(false);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500, (1 << 8) | (1 << 9) | (1 << 0), QueryTriggerInteraction.Collide))
        {
            //nail
            if (hit.collider.gameObject.layer == 9)
            {
                aimingReticle.GetComponent<Image>().color = aimingReticleNailColor;
                removeNailText.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(1))
                {
                    StartCoroutine(LiftNail(hit));
                }
            }
            else
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    aimingReticle.GetComponent<Image>().color = aimingReticlePickupColor;
                }
                else
                    aimingReticle.GetComponent<Image>().color = aimingReticleBaseColor;

                if (Input.GetMouseButtonDown(0))
                {
					GetComponent<AudioSource>().Play();
                    ShootNail();
                }
            }
        }
        else
        {
            aimingReticle.GetComponent<Image>().color = aimingReticleBaseColor;
        }
    }

    private void ShootNail()
    {
        List<RaycastHit> results = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 500, (1 << 8) | (1 << 0), QueryTriggerInteraction.Ignore).DistinctBy(r => r.collider.gameObject).OrderBy(r => r.distance).ToList();
        Debug.Log("hit " + results.Count + " targets");
        if (results.Count > 1)
        {
            Debug.Log("results more than 1");
            for (int i = 1; i < results.Count; i++)
            {
                float distance = results[i].distance - results[0].distance;
                if (distance < nail_distance)
                {
                    //results[i - 1].collider.GetComponent<Rigidbody>().isKinematic = true;
                    //results[i - 1].collider.transform.parent = results[i].collider.transform;

                    //results[i - 1].collider.gameObject.AddComponent<FixedJoint>().connectedBody = results[i].collider.GetComponent<Rigidbody>();

                    if (results[i - 1].collider.gameObject.GetComponent<FixedJoint>() == null || results[i - 1].collider.gameObject.GetComponent<FixedJoint>().connectedBody != results[i].collider.GetComponent<Rigidbody>())
                    {
                        if (results[i - 1].collider.gameObject.GetComponent<Rigidbody>() == null)
                        {
                            results[i - 1].collider.gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        }
                        if (results[i].collider.gameObject.GetComponent<Rigidbody>() == null)
                        {
                            results[i].collider.gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        }

                        results[i - 1].collider.gameObject.AddComponent<FixedJoint>().connectedBody = results[i].collider.GetComponent<Rigidbody>();
                        results[i].collider.gameObject.AddComponent<RelationshipNoter>().relationshipObject = results[i - 1].collider.gameObject;
                    }

                    Debug.Log("close enough");
                }
                else
                {
                    Debug.Log("not close enough - distance " + distance);
                }
            }
        }
        if (results.Count > 0)
        {
            GameObject insNail = GameObject.Instantiate(nail_prefab);
            for (int i = 1; i < results.Count; i++)
            {
                float distance = results[i].distance - results[0].distance;
                if (distance < nail_distance)
                    nails.Add(new Nail(insNail, results[i - 1].collider.gameObject, results[i].collider.gameObject));
            }
            StartCoroutine(ShootNail(insNail, results[0]));
        }
        aimingReticle.SetTrigger("Shoot");
    }
    private IEnumerator ShootNail(GameObject nail, RaycastHit hit)
    {
        //nail.transform.forward = -Camera.main.transform.forward;
        nail.transform.localScale = Vector3.one * nail_distance * 1.1f;

        //Vector3 startPos = Camera.main.transform.position;
        Vector3 newDir = (spawnPoint.position - hit.point).normalized;
        Vector3 startPos = spawnPoint.position + newDir * nail_distance;
        //Vector3 endPos = hit.point + Camera.main.transform.forward * nail_distance / 2.2f;
        Vector3 endPos = hit.point - newDir * nail_distance / 2.2f;

        float distance_traveled = 0;
        nail.transform.position = startPos;
        nail.transform.LookAt(hit.point);
        nail.transform.forward = -nail.transform.forward;
        while (distance_traveled < Vector3.Distance(startPos, endPos))
        {
            nail.transform.position += (endPos - startPos).normalized * Time.deltaTime * nail_speed;
            distance_traveled += Time.deltaTime * nail_speed;
            yield return new WaitForEndOfFrame();
        }
        nail.transform.position = endPos;

        //hit.collider.gameObject.AddComponent<FixedJoint>().connectedBody = nail.GetComponent<Rigidbody>();

        nail.AddComponent<FixedJoint>().connectedBody = hit.collider.gameObject.GetComponent<Rigidbody>();

        //GameObject holder = new GameObject();
        //holder.transform.parent = hit.collider.transform;
        ////insNail.transform.position = hit.point + Camera.main.transform.forward * nail_distance / 2.2f;
        //nail.transform.parent = holder.transform;
    }

    private IEnumerator LiftNail(RaycastHit hit)
    {
        aimingReticle.SetTrigger("Reverse");

        GameObject.Destroy(hit.collider.gameObject.GetComponent<FixedJoint>());

        Vector3 startPos = hit.collider.transform.position;
        Vector3 endPos = hit.collider.transform.position + hit.collider.transform.forward * nail_distance;

        float distance_traveled = 0;
        while (distance_traveled < Vector3.Distance(startPos, endPos))
        {
            hit.collider.transform.position += (endPos - startPos).normalized * Time.deltaTime;
            distance_traveled += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);

        List<Nail> theseNails = nails.Where(n => n.nailObj == hit.collider.gameObject).ToList();
        Debug.Log("count of nail objects correlating to this gameobject: " + theseNails.Count);
        Debug.Log("count of nails: " + nails.Count);
        foreach (Nail nail in theseNails)
        {
            nails.Remove(nail);
            List<Nail> validNails = nails.Where(n => n.childObject == nail.childObject && n.parentObject == nail.parentObject).ToList();
            if (validNails.Count == 0)
            {
                Debug.Log("T");
                //nail.childObject.GetComponent<Rigidbody>().isKinematic = false;
                //nail.childObject.transform.parent = null;
                if (nail.childObject != null)
                {
                    GameObject.Destroy(nail.childObject.GetComponent<FixedJoint>());
                    GameObject.Destroy(nail.parentObject.GetComponent<RelationshipNoter>());
                }
                else
                {
                    Debug.Log("WHAT?");
                }
            }
        }
        Debug.Log("count of nails: " + nails.Count);
        if (hit.collider.gameObject != null)
            GameObject.Destroy(hit.collider.gameObject);

        yield break;
    }

    [SerializeField]
    private List<Nail> nails = new List<Nail>();

    [System.Serializable]
    private class Nail
    {
        public GameObject nailObj;

        public GameObject childObject;
        public GameObject parentObject;

        public Nail (GameObject nail, GameObject child, GameObject parent)
        {
            nailObj = nail;
            childObject = child;
            parentObject = parent;
        }
    }
}
