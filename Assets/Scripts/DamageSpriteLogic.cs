using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpriteLogic : MonoBehaviour
{
    public float Iterations = 20;
    public float delta = 0.01f;
    public float wait = 0.1f;


    // Use this for initialization
    void Start()
    {
        StartCoroutine("MoveUp");
        StartCoroutine("Fade");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator MoveUp()
    {
        for (int y = 0; y < Iterations; y++)
        {
            transform.Translate(0, delta, 0);
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(Iterations / 2 * wait);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        for (int y = 0; y < Iterations / 2; y++)
        {
            renderer.color = new Color(1f, 1f, 1f, (1f / y));
            yield return new WaitForSeconds(wait);
        }

        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}