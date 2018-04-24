using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public static OpenDoor instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    Animation anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animation>();
	}
	
	public void Open()
    {
        anim.Play();
    }

    public void Close()
    {
        anim.Rewind();
    }
}
