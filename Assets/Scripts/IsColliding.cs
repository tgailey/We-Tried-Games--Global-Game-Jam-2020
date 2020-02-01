using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IsColliding : MonoBehaviour
{
	public List<GameObject> collisions = new List<GameObject>();
	//public bool isColliding { get { return collisions.Count == 0; } }
	public bool isColliding;

	void Start()
    {
        
    }

    void FixedUpdate()
    {
		isColliding = collisions.Count != 0;
	}

	private void OnCollisionEnter(Collision collision)
	{
		collisions.Add(collision.gameObject);
	}
	private void OnCollisionExit(Collision collision)
	{
		collisions.Remove(collision.gameObject);
	}
}
