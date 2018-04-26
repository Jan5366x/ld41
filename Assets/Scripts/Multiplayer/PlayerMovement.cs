using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using World.Objects;

public class PlayerMovement : MonoBehaviour
{
    public int playerid;
    public PauseScreen pauseScreen;


    // Use this for initialization
    void Start()
    {
        pauseScreen = GameObject.FindGameObjectWithTag("Finish").GetComponent<PauseScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerid >= 0)
        {
            var logic = GetComponent<UnitLogic>();
            var isMenu = logic.AnyInventoryShown;

            var horizontal = Input.GetAxis("Horizontal" + playerid);
            var vertical = Input.GetAxis("Vertical" + playerid);
            var sprint = Input.GetButton("Sprint" + playerid);
            var useTool = Input.GetButtonDown("UseTool" + playerid);
            var attackA = Input.GetButtonDown("AttackA" + playerid);
            var attackB = Input.GetButtonDown("AttackB" + playerid);

            if (!isMenu && !pauseScreen.IsShow)
            {
                if (Mathf.Abs(horizontal) > 1e-6 || Mathf.Abs(vertical) > 1e-6)
                {
                    logic.Move(horizontal, vertical, sprint);
               
                }
                else
                {
                    logic.StopMovement();
                }

                if (useTool)
                {
                    logic.Interact();
                }
                else if (attackA)
                {
                    logic.AttackLeft();
                    /*logic.ShowMessage(
                        "Hallo ich bin ein Text\r\nUnd ich kann auch über mehrere Zeilen gehen\r\n\t Leider kann ich keine Bilder, aber das ist okay.",
                        10);*/
                }
                else if (attackB)
                {
                    logic.AttackRight();
                    
                    // TODO fix that !
                    logic.ShowInventory();
                }
            }
            else
            {
                if (attackB)
                {
                    logic.CloseInventories();
                }
            }
        }
    }
}