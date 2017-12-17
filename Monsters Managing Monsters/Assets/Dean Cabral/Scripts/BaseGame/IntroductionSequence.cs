using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionSequence : MonoBehaviour {

    public GameObject disclaimerScreen;
    public GameObject savingScreen;
    public GameObject mainMenu;
    public GameObject saveIcon;
    public Text disclaimerText;
    public Text savingText;
    public Text skipText;
    public Image menuBG;

    void Start()
    {
        SequenceOrder();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            HideAllScreens();
            mainMenu.SetActive(true);
        }
    }

    private void SequenceOrder()
    {
        StartCoroutine(LoadDisclaimer());
        StartCoroutine(LoadSaving());
        StartCoroutine(LoadMenu());
    }

    private void HideAllScreens()
    {
        disclaimerScreen.SetActive(false);
        savingScreen.SetActive(false);
        skipText.gameObject.SetActive(false);
        HideSave();
    }

    public IEnumerator LoadDisclaimer()
    {
        yield return null;
        StartCoroutine(FadeTextToFullAlpha(4, disclaimerText, disclaimerScreen));
    }

    public IEnumerator LoadSaving()
    {
        yield return new WaitForSeconds(8);
        ShowSave();
        StartCoroutine(FadeTextToFullAlpha(4, savingText, savingScreen));
    }

    public IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(16);
        mainMenu.SetActive(true);
        skipText.gameObject.SetActive(false);
        StartCoroutine(FadeImageToFullAlpha(6, menuBG));

    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i, GameObject o)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }

        if (i.color.a >= 1.0f)
        {
            StartCoroutine(FadeTextToZeroAlpha(4, i, o));
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i, GameObject o)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }

        if (i.color.a <= 0.0f)
        {
            o.SetActive(false);
            if (o.name == "SavingScreen") HideSave();
        }
    }

    public IEnumerator FadeImageToFullAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    private void ShowSave()
    {
        saveIcon.SetActive(true);
    }

    private void HideSave()
    {
        saveIcon.SetActive(false);
    }
}
