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

            var logic = GetComponent<UnitLogic>();
            if (Mathf.Abs(horizontal) > 1e-6 || Mathf.Abs(vertical) > 1e-6)
            {
                logic.Move(horizontal, vertical, sprint);
            }
            else
            {
                logic.StopMovement();
            }

            var useTool = Input.GetButtonDown("UseTool" + playerid);
            var attackA = Input.GetButtonDown("AttackA" + playerid);
            var attackB = Input.GetButtonDown("AttackB" + playerid);
            if (useTool)
            {
                logic.Interact();
            } else if (attackA)
            {
                logic.AttackLeft();
            } else if (attackB)
            {
                logic.AttackRight();
            }
        }
    }
}