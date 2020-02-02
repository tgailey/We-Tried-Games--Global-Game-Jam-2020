using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTracking : MonoBehaviour
{
    public GameObject Goal;
    private GameObject goalIcon;
    public Material TransparentMaterial;
    // Start is called before the first frame update
    void Start()
    {
        goalIcon = new GameObject();
        List<Material> materials = new List<Material>();
        for(int i = 0; i < Goal.GetComponent<Renderer>().materials.Length; i++)
        {
            Material oldMat = Goal.GetComponent<Renderer>().materials[i];

            Material newMat = new Material(TransparentMaterial);
            newMat.color = new Color(oldMat.color.r, oldMat.color.g, oldMat.color.b, newMat.color.a);

            materials.Add(newMat);
        }

        goalIcon.AddComponent<MeshFilter>().sharedMesh = Goal.GetComponent<MeshFilter>().sharedMesh;
        goalIcon.AddComponent<MeshRenderer>().sharedMaterials = materials.ToArray();

        goalIcon.transform.position = Goal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
