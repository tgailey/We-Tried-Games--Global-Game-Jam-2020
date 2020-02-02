using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    public Vector3 EulerAnglesAddedPerSecond;
    private Vector3 trackedEulers;
    private void Start()
    {
        trackedEulers = transform.localEulerAngles;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 eulersToAdd = EulerAnglesAddedPerSecond * Time.deltaTime;
        trackedEulers = new Vector3((trackedEulers.x + eulersToAdd.x) % 360, (trackedEulers.y + eulersToAdd.y) % 360, (trackedEulers.z + eulersToAdd.z) % 360);

        transform.localEulerAngles = trackedEulers;
    }
}
