using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    //Oh no, there's so much, so let's go through step by step

    public string name;                     //The name of our sound file. This name is called as a string whenever you use the Play() function in the AudioManager - check that out for more details

    [TextArea(2, 10)]
    public string description;              //A description of the sound. Please fill this in so everyone else knows what the sound file is for.

    [HideInInspector]
    public AudioSource source;              //The source of sound.

    #region ~ Basic volume variables ~

    [Header("Basic Controls")]
    public AudioClip clip;                  //The audio clip. Self explanatory

    [Range(0f, 1f)]
    public float volume;                    //Our volume. Also self explanatory.
    [Range(0f, 3f)]
    public float pitch;                     //Our pitch. Same thing.

    public bool loop;                       //Are we looping? Note, you can't stop a loop unless if you call Stop() on the whole sound clip

    #endregion

    #region ~ Advanced volume variables - AudioLerping ~

    [Header("Advanced Controls")]

    public bool Fade;                       //Do we start fading the audio?

    public float fadeDuration = 3;          //How long do we want to stretch out the sound for?

    [Range(0f, 1f)]
    public float startVol = 0;              //Our starting volume for the lerp

    [Range(0f, 1f)]
    public float endVol = 0.5f;             //Our volume at the end of lerp

    public bool useGraph;                   //Do we want the audiolerp to follow a graph?
    public AnimationCurve AC;               //Our sound will follow the shape of the graph

    [HideInInspector]
    public bool wasFading;                  //For the AudioManager
    [HideInInspector]
    public float fadeTime;                  //The 't' in lerp
    [HideInInspector]
    public float fadeStartTime;             //Time.time at beginning of lerp

    #endregion

    void Start()
    {
        //Do we fade audio?
        if (Fade)
            //If yes, set our volume
            volume = startVol;
    }

    //Function is called by AudioManager
    public void LerpVolume()
    {
        //Are we using the graph?
        if (useGraph)
        {
            //If yes, we increase our fade time, which iterates through a graph much like what lerp does.
            fadeTime += Time.deltaTime;
            source.volume = AC.Evaluate(fadeTime);
        }
        else
        {
            //Otherwise, we do a normal lerp
            fadeTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, endVol, fadeTime / fadeDuration);
        }
    }


}
