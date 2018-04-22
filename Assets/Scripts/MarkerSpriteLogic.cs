using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSpriteLogic : MonoBehaviour
{
    public float time = 0;

    public const float range = 0.001f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dt = Time.deltaTime;
        time += dt;

        float dy = Mathf.Sin(time*10) * range;
        transform.Translate(0, dy, 0);
    }
}