using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Fungus;

public class TutorialSequence : MonoBehaviour {

    public GameObject fadeObject;
    public Image fadeImage;
    public GameObject skipBtn;
    public GameObject taxi;
    public GameObject traffic;
    public GameObject player;
    public GameObject hrHead;
    public Transform target;
    public static TutorialSequence instance;

    [Header("Fungus")]
    public Flowchart fung;

    #region Typical Singleton Format

    void Awake()
    {

        //Singleton stuff
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    void Start () {

        StartCoroutine(FadeIn(5, fadeImage));
	}

    public IEnumerator HRHeadIntroduction()
    {
        PC_Move.BL_canMove = false;

        NavMeshAgent agent = hrHead.GetComponent<NavMeshAgent>();
        CameraFollow.instance.otherLook = hrHead;
        Vector3 agentPos = agent.transform.position;
        agent.destination = target.position;

        yield return new WaitForSeconds(4);

        CameraFollow.instance.otherLook = null;
        StartCoroutine(InitiateMachicoConvo(agent, agentPos));
    }

    private IEnumerator InitiateMachicoConvo(NavMeshAgent agent, Vector3 agentPos)
    {
        Flowchart.BroadcastFungusMessage("MachicoIntroduction");

        do {
            yield return null;
        } while (fung.GetBooleanVariable("bl_textCycleOver") == false);

        agent.destination = agentPos;
        CameraFollow.instance.otherLook = hrHead;
        yield return new WaitForSeconds(0.31f);

        // Once convo is over...        
        Flowchart.BroadcastFungusMessage("MachicoIntroLeave");
        do
        {
            yield return null;
        } while (fung.GetBooleanVariable("bl_textCycleOver") == false);

        CameraFollow.instance.otherLook = null;

        yield return new WaitForSeconds(0.5f);
        // After she leaves...
        hrHead.SetActive(false);
        PC_Move.BL_canMove = true;

        GameObject.Find("TutorialTrigger").SetActive(false);
        EndDaySummary.instance.QuestGained(CurrentTasks.instance.currentTask[0].name);
        skipBtn.SetActive(true);
    }

    IEnumerator FadeIn(float t, Image i)
    {
        player.SetActive(false);
        fadeObject.SetActive(true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        fadeObject.SetActive(false);

        CameraFollow.instance.otherLook = taxi;
        yield return new WaitForSeconds(3.0f);

        StartCoroutine(TextMessage());
        yield return new WaitForSeconds(0.5f);

        player.SetActive(true);
        traffic.SetActive(false);
        CameraFollow.instance.otherLook = null;
        PC_Move.BL_canMove = false;
   
    }

    IEnumerator TextMessage()
    {
        PDAHandler.instance.ShowMomText();
        yield return new WaitForSeconds(2);
        PC_Move.BL_canMove = true;
        yield return new WaitForSeconds(2);
        PDAHandler.instance.HideMomText();
    }
}
