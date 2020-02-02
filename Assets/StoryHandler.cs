using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class StoryHandler : MonoBehaviour
{
    public static StoryHandler instance;

    private void Awake()
    {
        instance = this;
        GoalTracker1.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        meteor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            MoveNext();
        }   
    }

    private int storyPart = 0;
    [Header("STORY PART 0 VARIABLES")]
    public GoalTracking GoalTracker0;
    [Space]
    [Header("STORY PART 1 VARIABLES")]
    public GoalTracking GoalTracker1;
    public GameObject meteor;
    public fadeInText story_text;

    private GameObject player;

    public void MoveNext()
    {
        switch (storyPart)
        {
            case 0:
                GoalTracker0.gameObject.SetActive(false);
                GoalTracker1.gameObject.SetActive(true);

                StartCoroutine(StartPartOne());

                break;
        }

        storyPart++;
    }

    public IEnumerator StartPartOne()
    {
        player.GetComponent<RigidbodyFirstPersonController>().enabled = false;

        while (true)
        {

            Vector3 lookDir = GoalTracker1.Goals[0].transform.position - Camera.main.transform.position;
            Quaternion q = Quaternion.LookRotation(lookDir);
            Quaternion oldrot = player.transform.rotation;
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, q, Time.deltaTime * 45);

            if (oldrot == player.transform.rotation)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        meteor.SetActive(true);

        //todo - this shouldn't be a time, but instead set off when the meteor triggers the destruction
        yield return new WaitForSeconds(1.5f);

        story_text.FadeInFadeOut();
        yield return new WaitForSeconds(story_text.time);

        meteor.SetActive(false);

        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        player.GetComponent<RigidbodyFirstPersonController>().mouseLook.m_CharacterTargetRot = player.transform.rotation;
        GoalTracker1.startLevel = true;


    }
}
