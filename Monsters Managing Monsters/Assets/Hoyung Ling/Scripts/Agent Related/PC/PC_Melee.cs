using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Melee : MonoBehaviour
{

    [Header("Swinging")]
    public GameObject Sword_Sprite;
    public int swings;
    public KeyCode KC_Attack;
    int IN_Swings = 3;

    public bool BL_Staggered = false;

    [Header("Swing Prefab")]
    public GameObject GO_MeleeCast;

    //Restore Swings
    [Header("Restore Swing Rate")]
    float CoolDown = 0;
    public float Delay;

    // Use this for initialization
    void Start()
    {
        IN_Swings = swings;
    }

    // Update is called once per frame
    void Update()
    {
        if (!BL_Staggered)
            if (Input.GetKeyDown(KC_Attack)) Attack();

        RestoreSwings();
    }

    void RestoreSwings()
    {
        if (IN_Swings >= 3) return;

        if (Time.time > CoolDown)
        {
            CoolDown = Time.time + Delay;
            IN_Swings++;
        }
    }

    void Attack()
    {
        CoolDown = Time.time + Delay;
        IN_Swings--;
        Sword_Sprite.SetActive(false);
        StartCoroutine(SwordSpriteAnimator());
        Instantiate(GO_MeleeCast, transform.position + new Vector3(0, 1, 0), transform.rotation);
        if (IN_Swings == 0)
        {
            BL_Staggered = true;
            StartCoroutine(AttackSwingReset());
        }
    }

    IEnumerator SwordSpriteAnimator()
    {
        yield return new WaitForSeconds(0.21f);
        Sword_Sprite.SetActive(true);
    }

    IEnumerator AttackSwingReset()
    {
        yield return new WaitForSeconds(1.0f);
        ResetSwings();
    }

    public void ResetSwings()
    {
        IN_Swings = swings;
    }

    public int swingcount()
    {
        return IN_Swings;
    }
}
