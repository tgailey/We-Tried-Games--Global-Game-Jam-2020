using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollidersOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boulder")
        {
            Rigidbody[] colliders = gameObject.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody col in colliders)
            {
                col.useGravity = true;
                col.constraints = RigidbodyConstraints.None;
                col.GetComponent<Collider>().enabled = true;
            }
        }
    }
}
