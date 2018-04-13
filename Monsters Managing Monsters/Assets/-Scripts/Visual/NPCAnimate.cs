using UnityEngine;
using System.Collections;

public class NPCAnimate : MonoBehaviour
{

    public bool BL_canAmimate;
    public Sprite[] animSteps;
    public float delay;

    private SpriteRenderer SR;
    private bool isAnimating;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        StartCoroutine(Animate(delay));
    }

    private void Update()
    {
        if (BL_canAmimate && !isAnimating) StartCoroutine(Animate(delay));
    }

    IEnumerator Animate(float delay)
    {
        isAnimating = true;

        for (int i = 0; i < animSteps.Length; i++)
        {
            SR.sprite = animSteps[i];
            yield return new WaitForSeconds(delay);
        }

        isAnimating = false;
    }
}
