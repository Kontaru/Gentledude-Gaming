using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;                                          //Sound array

    public static AudioManager instance;

    #region Typical Singleton Format

    void Awake () {

        //Singleton stuff
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //Start loading sounds and initialising them
        //Look through our Sound array
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();  //Start creating AudioSource on the AudioManager
            s.source.clip = s.clip;                             // Sound clip

            s.source.volume = s.volume;                         //Adding volume
            s.source.pitch = s.pitch;                           //Adding pitch
            s.source.loop = s.loop;                             //Do we loop?
        }
	}

    #endregion

    void Start()
    {
        //If we have a theme song, play it!
        //WARNING!! Calling it HERE will play it constantly through the game. If you want different themes in different scenes, suggest you don't call your theme song "Theme"
        Play("Theme");
    }

    void Update()
    {
        //Refer to AudioLerp() function below for further details
        //This constantly checks all our sound files
        foreach (Sound s in sounds)
        {
            if (s.Fade)                                             //For each of our sounds, check if the isLerping boolean is toggled
            {
                if (!s.wasFading)                                   //Has it already started lerping?
                {
                    s.wasFading = true;                             //If it hasn't, then make it true!
                    s.fadeStartTime = Time.time;                    //Set the start time to now
                    s.fadeTime = 0;                                  
                }
                s.LerpVolume();
            }
        } 
    }

    #region Music controls

    //PLAY function - Call AudioManager.instance.Play("[INSERT NAME OF AUDIO IN AUDIOMANAGER]")
    public void Play(string name)
    {
        //Find the file in the array of sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //If it's null, break the loop so we don't start throwing errors
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        //Otherwise, play the song
        if (!s.source.isPlaying)
            s.source.Play();
    }

    //STOP function - Call AudioManager.instance.Stop("[INSERT NAME OF AUDIO IN AUDIOMANAGER]")
    public void Stop(string name)
    {
        //Find the file in the array of sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //If it's null, break the loop so we don't start throwing errors
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        //Otherwise, stop the song
        s.source.Stop();
    }

    //AUDIOFADE function - Call AudioManager.instance.AudioFade("[INSERT NAME OF AUDIO IN AUDIOMANAGER]")
    //Mostly for buttons
    public void AudioFade(string name)
    {
        //Find the file in the array of sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //If it's null, break the loop so we don't start throwing errors
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        //Otherwise, toggle lerp
        s.Fade = !s.Fade;
    }

    #endregion
}
