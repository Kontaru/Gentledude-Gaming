using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HYL_IS_QTE : QuestPart {

    public bool BL_IsPlaying;
    public GameObject labelObject;
    public GameObject repairObject;
    public Image repairBar;
    public Text timerText;

    private int timer;

    public KeyCode[] KC_Sequence;
    public int currentKey;

    private void OnTriggerEnter(Collider other)
    {
        if (!BL_IsInteractable) return;

        if (other.gameObject.name == "PC")
        {
            InitialiseGame(false);
            ShowRepairObject();
            BL_IsPlaying = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!BL_IsInteractable) return;

        if (other.gameObject.name == "PC")
        {
            ShowLabelObject();
            BL_IsPlaying = false;
        }
    }

    // Use this for initialization
    void Start () {

        InitialiseGame(true);
    }

    public void Update()
    {
        if (BL_IsInteractable)
        {
            do
            {
                for (int key = 0; key < KC_Sequence.Length; key++)
                {
                    if (currentKey == key)
                    {
                        if (Input.GetKeyDown(KC_Sequence[key]))
                        {
                            currentKey++;
                        }
                    }
                }
            } while (currentKey < KC_Sequence.Length);
        }
    }

    void Repair()
    {

    }

    void Damage()
    {

    }

    private void UpdateUI()
    {
        timerText.text = timer.ToString();
    }

    private void ShowRepairObject()
    {
        repairObject.SetActive(true);
        labelObject.SetActive(false);
    }

    private void ShowLabelObject()
    {
        labelObject.SetActive(true);
        repairObject.SetActive(false);
    }

    private void InitialiseGame(bool firstFlag)
    {
        timer = 5;
        repairBar.fillAmount = 0;
        StopAllCoroutines();
        if (!firstFlag) StartCoroutine(RunTimer());
    }

    IEnumerator RunTimer()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
        }

        BL_MinigameComplete = true;
    }
}
