using UnityEngine;

[System.Serializable]
public class Dialogue {

    public string name;             //The name of our dialogue file
    public TextAsset textAsset;     //The .txt file
    [TextArea(2, 10)]
    public string description;      //Description of text

}
