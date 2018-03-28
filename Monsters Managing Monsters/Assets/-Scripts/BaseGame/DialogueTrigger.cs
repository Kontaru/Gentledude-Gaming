using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;   

    public void TriggerDialogue(int state)
    {
        // States
        // 0 - Idle
        // 1 - Recieved
        // 2 - Accepted
        // 3 - Declined
        // 4 - Succeeded
        // 5 - Failed

        DialogueManager.instance.StartDialogue(dialogue, state);
    }
}
