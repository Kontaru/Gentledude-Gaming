using UnityEngine;
using System.Collections;

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(1, 4)]
    public string[] idleSentences;
    [TextArea(1, 4)]
    public string[] questSentences;
    [TextArea(1, 4)]
    public string[] questAcceptedSentences;
    [TextArea(1, 4)]
    public string[] questDeclinedSentences;
    [TextArea(1, 4)]
    public string[] questSucceededSentences;
    [TextArea(1, 4)]
    public string[] questFailedSentences;

    public bool hasChoice;
    public string option1, option2;

}
