using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAfterObject : MonoBehaviour
{
    public GameObject objectToChase;
    public float speed = 5;
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = objectToChase.transform.position - transform.position;
        transform.LookAt(objectToChase.transform);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(dir * speed * Time.deltaTime, ForceMode.Force);   
    }
}
