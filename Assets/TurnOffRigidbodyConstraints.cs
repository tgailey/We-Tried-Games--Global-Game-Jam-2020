using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffRigidbodyConstraints : MonoBehaviour
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
            Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody body in bodies)
            {
                body.constraints = RigidbodyConstraints.None;
            }
        }
    }
}
