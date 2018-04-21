using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject toFollow;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float dx = toFollow.transform.position.x - transform.position.x;
        float dy = toFollow.transform.position.y - transform.position.y;
        float mag = Mathf.Sqrt(dx * dx + dy * dy);

        if (mag > 2) //s need to go faster, but still leave some room
        {
            dx *= (1 - 2f / mag);
            dy *= (1 - 2f / mag);
        }
        else
        {
            dx /= 10;
            dy /= 10;
        }

        transform.Translate(dx, dy, 0);
    }
}