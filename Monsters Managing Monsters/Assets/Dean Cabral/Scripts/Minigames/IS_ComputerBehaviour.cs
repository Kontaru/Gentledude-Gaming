using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IS_ComputerBehaviour : QuestPart {

    public bool BL_IsPlaying;
    public GameObject labelObject;
    public GameObject repairObject;
    public Image repairBar;
    public Text timerText;

    private int timer;

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
    void OnEnable () {

        InitialiseGame(true);
    }
	
	// Update is called once per frame
	void Update () {

        if (!BL_IsPlaying) return;

        CheckEndCondition();
        CheckInput();
        DamageComputer();
        UpdateUI();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) RepairComputer();
    }

    private void UpdateUI()
    {
        timerText.text = timer.ToString();
    }

    private void InitialiseGame(bool firstFlag)
    {
        timer = 5;
        repairBar.fillAmount = 0;
        StopAllCoroutines();
        if (!firstFlag) StartCoroutine(RunTimer());
    }    

    private void RepairComputer()
    {        
        if (repairBar.fillAmount < 1f) repairBar.fillAmount += 0.1f;
        if (repairBar.fillAmount >= 1f)
        {
            BL_MinigameComplete = true;
            ShowFixedLabel();
        }
    }

    private void DamageComputer()
    {
        if (repairBar.fillAmount > 0f) repairBar.fillAmount -= 0.3f * Time.deltaTime;
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

    private void ShowFixedLabel()
    {
        labelObject.GetComponentInChildren<Text>().text = "Fixed Printer";
        labelObject.transform.GetChild(1).gameObject.SetActive(false);
        repairObject.SetActive(false);
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
