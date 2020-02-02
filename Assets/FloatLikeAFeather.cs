using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLikeAFeather : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().drag = 8;

        ogPos = transform.position;

    }

    Vector3 ogPos;

    float sine = 0;
    // Update is called once per frame
    void Update()
    {
        float x = Mathf.PingPong(sine, 2) - 1;
        Vector3 pos = ogPos + transform.right * x * Mathf.Lerp(2 , 1, Mathf.Abs(x));
        transform.position = new Vector3(pos.x, transform.position.y , pos.z);

        sine += Time.deltaTime * 2;
    }
}
