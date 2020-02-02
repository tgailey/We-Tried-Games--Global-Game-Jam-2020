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
    public TextMeshProUGUI NailItText;

    private int numberInPlace;

    public bool startLevel = false;

    public float acceptableDistance = 0.5f;

    private GameObject player;
    private NailShooter nailShooter { get { return player.GetComponent<NailShooter>(); } }
    private GrabItem grabItem { get { return player.GetComponent<GrabItem>(); } }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        NailItText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        NailItText.gameObject.SetActive(false);

        if (startLevel == true)
        {
            if (grabItem.grab && Goals.Contains(grabItem.grabbedItem.gameObject))
            {
                int index = Goals.IndexOf(grabItem.grabbedItem.gameObject);
                Debug.Log("DISTANCE: " + Vector3.Distance(Goals[index].transform.position, goalIcons[index].transform.position));
                if (Vector3.Distance(Goals[index].transform.position, goalIcons[index].transform.position) < acceptableDistance)
                {
                    NailItText.gameObject.SetActive(true);

                    if (Goals[index].GetComponent<FixedJoint>() != null || Goals[index].GetComponent<RelationshipNoter>() != null)
                    {
                        grabItem.grab = false;

                        numberInPlace++;

                        List<GameObject> connectObjects = StaticHelpers.GetAllConnectedGameObjects(Goals[index]);
                        Debug.Log("connected objects to goal : " + connectObjects.Count);
                        foreach (GameObject obj in connectObjects)
                        {
                            Debug.Log("connected object" + obj.name);
                            GameObject.Destroy(obj.GetComponent<FixedJoint>());
                            //GameObject.Destroy(obj.GetComponent<Rigidbody>());
                            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        }

                        StoryHandler.instance.MoveNext();
                    }
                }
            }
        }

        numberOfObjectsText.text = numberInPlace + " / " + goalIcons.Count;
    }

    public void StartGoalTracking()
    {
        startLevel = true;
    }
}
