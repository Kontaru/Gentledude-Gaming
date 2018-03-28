using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;    

    public GameObject dialoguePanel;
    public Text nameText,dialogueText;

    public GameObject choicePanel;
    public Text choice1Text, choice2Text;

    private Queue<string> sentences;
    private Dialogue currentDialogue;

    private bool choiceSelected;
    private int currentState;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start () {

        sentences = new Queue<string>();
        choiceSelected = false;
    }

    public void StartDialogue(Dialogue dialogue, int state)
    {
        currentDialogue = dialogue;
        currentState = state;
        ShowDialogueBox();

        switch (currentState)
        {
            case 0:
                QueueIdle();
                break;
            case 1:
                QueueQuestRecieved();
                break;
            case 2:
                QueueAccepted();
                break;
            case 3:
                QueueDeclined();
                break;
            case 4:
                QueueSucceeded();
                break;
            case 5:
                QueueFailed();
                break;
        }        

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndConversation();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndConversation()
    {
        if (currentDialogue.hasChoice && !choiceSelected && currentState != 4 && currentState != 5)
        {
            choicePanel.SetActive(true);
            choice1Text.text = currentDialogue.option1;
            choice2Text.text = currentDialogue.option2;
        }
        else
        {
            dialoguePanel.SetActive(false);
            choiceSelected = false;
        }       
    }	

    public void SelectedOption1()
    {
        choiceSelected = true;
        choicePanel.SetActive(false);
        StartDialogue(currentDialogue, 2);
    }

    public void SelectedOption2()
    {
        choiceSelected = true;
        choicePanel.SetActive(false);
        StartDialogue(currentDialogue, 3);
    }

    private void ShowDialogueBox()
    {
        dialoguePanel.SetActive(true);
        nameText.text = currentDialogue.name;
        sentences.Clear();
    }

    private void QueueIdle()
    {
        foreach (string sentence in currentDialogue.idleSentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    private void QueueQuestRecieved()
    {
        foreach (string sentence in currentDialogue.questSentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    private void QueueAccepted()
    {
        foreach (string sentence in currentDialogue.questAcceptedSentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    private void QueueDeclined()
    {
        foreach (string sentence in currentDialogue.questDeclinedSentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    private void QueueSucceeded()
    {
        foreach (string sentence in currentDialogue.questSucceededSentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    private void QueueFailed()
    {
        foreach (string sentence in currentDialogue.questFailedSentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
}
