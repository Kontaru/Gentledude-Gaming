using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfficeEntrance : MonoBehaviour {

    public static OfficeEntrance instance;
    public GameObject player;
    public GameObject fadeObject;
    public Image fadeImage;
    public Transform spawnPoint;
    public AutoDoors elevatorDoors;

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

    private void Start()
    {
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnSequence(3, fadeImage));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) RespawnPlayer();
    }

    IEnumerator RespawnSequence(float t, Image i)
    {
        player.transform.position = spawnPoint.position;
        player.SetActive(false);

        fadeObject.SetActive(true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        fadeObject.SetActive(false);

        elevatorDoors.OpenDoors(true);
        Scene_Controller.instance.dayAlert.SetActive(false);
        yield return new WaitForSeconds(1);
        player.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        elevatorDoors.OpenDoors(false);        

    }
}
