using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameras : MonoBehaviour
{
    public float timeToPosition;
    public Camera cam;
    public void SwitchCamera()
    {
        //eventually lerp between them
        cam.gameObject.SetActive(true);

        GameObject.Destroy(this.gameObject);
    }
}
