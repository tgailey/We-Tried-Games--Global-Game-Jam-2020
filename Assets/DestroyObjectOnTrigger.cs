using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOnTrigger : MonoBehaviour
{
    public GameObject objectToDestroy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boulder")
        {
            GameObject.Destroy(objectToDestroy);
        }
    }
}
