using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class TutorialSequence : MonoBehaviour {

    public GameObject fadeObject;
    public Image fadeImage;
    public GameObject traffic;

    public GameObject player;

    void Start () {

        StartCoroutine(FadeIn(5, fadeImage));
	}

    IEnumerator FadeIn(float t, Image i)
    {
        fadeObject.SetActive(true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        fadeObject.SetActive(false);

        yield return new WaitForSeconds(3);

        player.SetActive(true);
        traffic.SetActive(false);
        CameraFollow.instance.otherLook = null;

        yield return new WaitForSeconds(2);
        StartCoroutine(TextMessage());        
    }

    IEnumerator TextMessage()
    {
        PDAHandler.instance.ShowMomText();
        yield return new WaitForSeconds(5);
        PDAHandler.instance.OnClickClose();
    }
}
