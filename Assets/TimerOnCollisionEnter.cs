using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerOnCollisionEnter : Timer
{
    // Start is called before the first frame update
    protected override void Start()
    {
        //nothing
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Boulder")
        StartCoroutine(timer());
    }
}
