using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : MonoBehaviour
{
    public bool isClimbingLadder = false;
    public Vector3 LadderVector = Vector3.zero;
    public float climbSpeed = 8f;
    private UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controller;
    private Rigidbody rb;
    private Transform LadderTransform;

  
    private Vector3 lastPostition;
    void Start()
    {
        //get some necessary stuff so I don't have to keep asking for the ref's
        controller = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        rb = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if(isClimbingLadder)
        {
            //run a raycast to see where the ladder is now
            RaycastHit hit;
            if (Physics.Raycast(transform.position, LadderTransform.forward, out hit))
            {
                Vector3 offset = (hit.point - lastPostition); 
                //print(offset);
                transform.position += offset; //adjust position by how much the ladder has moved
            }

            //move the player on the ladder vector
            transform.position += Input.GetAxis("Vertical")*(LadderVector* climbSpeed * Time.deltaTime);
            transform.position += Input.GetAxis("Horizontal") * (LadderTransform.right * climbSpeed * Time.deltaTime);
            // this allows the player to strafe to fall off the ladder 
            // note this will not have ANY momentum player will drop immediately

            if (Physics.Raycast(transform.position, LadderTransform.forward, out hit))
            {
                lastPostition = hit.point; //capture current ladder position
            }
        }
    }



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbingLadder = true; //set state
            LadderTransform = other.gameObject.transform; //get the ladder trigger volume transform
            controller.isClimbing = true; //set state in player controller ... code in the player controller disables movement, but not camera
            rb.useGravity = false; //turn off gravity
            
            rb.velocity = Vector3.zero; //zero out any previous momentum

            RaycastHit hit;
            if (Physics.Raycast(transform.position, LadderTransform.forward, out hit))
            {
                lastPostition = hit.point; //set initial position of player & ladder
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            LadderVector = other.gameObject.transform.up; //get the new vector of the ladder, we're assuming it's moving

            //print("setting ladder vector: " + other.gameObject.transform.up);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder")) //we fell off ... so turn stuff back on 
        {
            //print("Leaving collision");
            isClimbingLadder = false;
            LadderVector = Vector3.zero;
            
            controller.isClimbing = false;
            rb.useGravity = true;
        }
    }
}
