using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public string[] controllers;
    public GameObject[] panes;
    public GameObject panelPrefab;
    public GameObject playerPrefab;
    private bool isInitialized;

    // Use this for initialization
    void Start()
    {
        panes = new GameObject[4] {null, null, null, null};
        isInitialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Start"))
        {
            if (!isInitialized)
            {
                isInitialized = true;
                SetupPanes();
            }
        }
    }

    void SetupPanes()
    {
        controllers = Input.GetJoystickNames();
        SetupPanes(Mathf.Max(1, controllers.Length));
    }

    void SetupPanes(int numPanes)
    {
        int width = Screen.width;
        int height = Screen.height;


        //numPanes = 1
        // => Alles an 1
        //numPanes = 2
        // => Split X
        //numPanes = 3
        // => Split Y
        // => Split upper X
        //numPanes = 4
        // => Split Y
        // => Split X (upper/lower)

        int idx = 0;
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                GameObject player;
                if (idx < numPanes && panes[idx] == null)
                {
                    panes[idx] = Instantiate(panelPrefab, transform);
                    player = Instantiate(playerPrefab, panes[idx].transform);
                    Debug.Log("+++++");
                    Debug.Log(panes);
                    Debug.Log(player);
                    panes[idx].GetComponentInChildren<FollowCamera>().toFollow = player;
                    panes[idx].GetComponentInChildren<PlayerMovement>().playerid = idx;
                }

                if (panes[idx] != null)
                {
                    float xx = 0;
                    float yy = 0;
                    float ww = 0;
                    float hh = 0;
                    switch (numPanes)
                    {
                        case 1:
                            xx = 0;
                            yy = 0;
                            ww = 1;
                            hh = 1;
                            break;
                        case 2:
                            ww = 0.5f;
                            hh = 1;
                            xx = x * ww;
                            yy = 0;
                            break;
                        case 3:
                            ww = idx < 2 ? 0.5f : 1f;
                            hh = 0.5f;
                            xx = x * ww;
                            yy = y * hh;
                            break;
                        case 4:
                            ww = 0.5f;
                            hh = 0.5f;
                            xx = x * ww;
                            yy = y * hh;
                            break;
                    }

                    //panes[idx].transform.localPosition.Set(xx, yy, -1);
                    var paneCamera = panes[idx].GetComponentInChildren<Camera>();
                    Debug.Log(xx + " " + yy + " " + ww + " " + hh);
                    paneCamera.rect = new Rect(xx, yy, ww, hh);
                }

                idx++;
            }
        }
    }
}