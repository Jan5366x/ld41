using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int playerid;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playerid >= 0)
        {
            var horizontal = Input.GetAxis("Horizontal" + playerid);
            var vertical = Input.GetAxis("Vertical" + playerid);
            var sprint = Input.GetButton("Sprint" + playerid);
            
            GetComponent<UnitLogic>().move(horizontal, vertical, sprint);
        }
    }
}