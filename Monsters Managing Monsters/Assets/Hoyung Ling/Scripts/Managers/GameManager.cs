using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool PixelMode = false;

    [Header("Controls")]

    public KeyCode KC_CameraLock;
    public KeyCode KC_Flashlight;

    [Header("Movement")]
    public KeyCode KC_Up;
    public KeyCode KC_Left;
    public KeyCode KC_Down;
    public KeyCode KC_Right;

    public int day;
    public int score;

    [Header("Monster Stats")]
    [SerializeField] int PL_Succ = 0;
    [SerializeField] int PL_Ogre = 0;
    [SerializeField] int PL_Goblin = 0;
    [SerializeField] int PL_Demon = 0;

    bool BL_Pause = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void PowerBoost(Attribution.Attributes attr)
    {
        if (attr == Attribution.Attributes.Succubus)
            PL_Succ++;
        if (attr == Attribution.Attributes.Ogre)
            PL_Ogre++;
        if (attr == Attribution.Attributes.Goblin)
            PL_Goblin++;
        if (attr == Attribution.Attributes.Demon)
            PL_Demon++;
    }

    #region ~ Save Related ~

    public void SaveGame()
    {
        SaveLoadHandler.instance.SaveData();
    }

    public void LoadGame()
    {
        SaveLoadHandler.instance.LoadData();
    }

    #endregion

    #region ~ Scene Related ~

    public void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public void LastScene()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        BL_Pause = !BL_Pause;

        if (BL_Pause == true)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    #endregion
}
