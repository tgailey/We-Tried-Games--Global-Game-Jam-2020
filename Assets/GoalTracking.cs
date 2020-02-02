using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalTracking : MonoBehaviour
{
    public List<GameObject> Goals = new List<GameObject>();
    private List<GameObject> goalIcons;
    public Material TransparentMaterial;
    public TextMeshProUGUI numberOfObjectsText;
    public TextMeshProUGUI HoldItText;

    private int numberInPlace;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject goal in Goals)
        {
            goalIcons = new List<GameObject>();
            GameObject goalIcon = new GameObject();
            List<Material> materials = new List<Material>();
            for (int i = 0; i < goal.GetComponentInChildren<Renderer>().materials.Length; i++)
            {
                Material oldMat = goal.GetComponentInChildren<Renderer>().materials[i];

                Material newMat = new Material(TransparentMaterial);

                newMat.SetColor("Color_19C6C157", new Color(oldMat.color.r, oldMat.color.g, oldMat.color.b, newMat.GetColor("Color_19C6C157").a));

                materials.Add(newMat);
            }

            goalIcon.AddComponent<MeshFilter>().sharedMesh = goal.GetComponentInChildren<MeshFilter>().sharedMesh;
            goalIcon.AddComponent<MeshRenderer>().materials = materials.ToArray();

            goalIcon.transform.position = goal.transform.position;
            goalIcon.transform.localScale = goal.transform.lossyScale;
            goalIcon.transform.rotation = goal.transform.rotation;

            goalIcons.Add(goalIcon);
        }

        numberOfObjectsText.text = "0 / " + goalIcons.Count;
        HoldItText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
