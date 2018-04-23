﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellInventory : MonoBehaviour
{
    public Rect ViewRect;
    public UnitLogic unit;
    private int itemDrawOffset;
    private int itemSelectIdx;
    public int playerIdx;

    public void handleSelectSlot(int slot)
    {
        unit.Sell(slot, _market);
    }

    private static readonly Color Black = new Color(0, 0, 0);
    private static readonly Color Red = new Color(1f, 0, 0);
    private static readonly Color Green = new Color(0, 1f, 0f);
    private static readonly Color DarkGreen = new Color(0, 0.6f, 0f);
    private static readonly Color Blue = new Color(0, 0, 1f);
    private static readonly Color Brown = new Color(0.42f, 0.21f, 0.13f);
    private static readonly Color Grey = new Color(0.3f, 0.3f, 0.3f);
    private static readonly Color LightGrey = new Color(0.6f, 0.6f, 0.6f);

    private Inventory _inventory;
    private Inventory _market;

    public float MoveDelayRemaining;
    public float MoveDelay = 0.1f;
    public bool show = false;

    public Rect getTruncatedInnerRect(int border)
    {
        int w = (int) Mathf.Min(1024, ViewRect.width - 2 * border);
        int h = (int) Mathf.Min(1024, ViewRect.height - 2 * border);

        int cx = (int) ViewRect.center.x;
        int cy = (int) ViewRect.center.y;

        return new Rect(cx - w / 2, cy - h / 2, w, h);
    }

    public Inventory MyInventory
    {
        get
        {
            if (_inventory != null)
            {
                return _inventory.Copy();
            }

            return null;
        }
        set { _inventory = value.Copy(); }
    }

    // Use this for initialization
    void Start()
    {
        itemDrawOffset = 0;
        show = false;
        if (_inventory == null)
        {
            _inventory = new Inventory();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            MoveDelayRemaining -= Time.deltaTime;
            if (MoveDelayRemaining <= 0)
            {
                int drawCount = GetItemDrawCount();
                if (Input.GetAxis("Vertical" + playerIdx) < 0)
                {
                    itemSelectIdx = Math.Min(itemSelectIdx + 1, Inventory.COUNT_SLOT - 1);
                    MoveDelayRemaining = MoveDelay;
                }
                else if (Input.GetAxis("Vertical" + playerIdx) > 0)
                {
                    itemSelectIdx = Math.Max(itemSelectIdx - 1, 0);
                    MoveDelayRemaining = MoveDelay;
                }

                if (itemDrawOffset > itemSelectIdx)
                {
                    itemDrawOffset = Math.Max(0, itemSelectIdx);
                }
                else if (itemSelectIdx > itemDrawOffset + drawCount)
                {
                    itemDrawOffset = Math.Min(Math.Min(itemDrawOffset + 1, itemSelectIdx),
                        Inventory.COUNT_SLOT - 1 - drawCount);
                }

                if (Input.GetButtonDown("AttackA" + playerIdx))
                {
                    handleSelectSlot(itemSelectIdx);
                    MoveDelayRemaining = MoveDelay;
                }
            }
        }
    }

    private void OnGUI()
    {
        if (show)
        {
            DrawInventoryBorder();
            DrawInventory();
        }
    }

    private int GetItemDrawCount()
    {
        Rect rect = getTruncatedInnerRect(150);
        int offsetLeft = 10;
        int offsetTop = 10;
        int offsetBottom = 10;
        int widthPreview = 64;
        int widthInnerBorder = 10;
        int widthRight = (int) (rect.width - (2 * offsetLeft + widthPreview + widthInnerBorder));
        int heightItem = 64;
        int heightInnerBorder = 10;
        for (int idx = 0; idx < Inventory.COUNT_SLOT; idx++)
        {
            if ((offsetTop + heightItem) > (rect.height - offsetBottom))
            {
                return idx - 1;
            }

            offsetTop += heightItem + heightInnerBorder;
        }

        return 0;
    }

    private void DrawInventory()
    {
        Rect rect = getTruncatedInnerRect(150);
        int offsetLeft = 10;
        int offsetTop = 10;
        int offsetBottom = 10;
        int widthPreview = 64;
        int widthInnerBorder = 10;
        int widthRight = (int) (rect.width - (2 * offsetLeft + widthPreview + widthInnerBorder));
        int heightItem = 64;
        int heightInnerBorder = 10;
        for (int idx = itemDrawOffset; idx < Inventory.COUNT_SLOT; idx++)
        {
            if ((offsetTop + heightItem) < (rect.height - offsetBottom))
            {
                Rect previewRect = new Rect(rect.left + offsetLeft, rect.top + offsetTop, widthPreview,
                    heightItem);
                IMUIHelper.DrawFilledRect(previewRect, idx == itemSelectIdx ? LightGrey : Grey);
                if (_inventory != null)
                {
                    var previewObj = MyInventory.GetObject(idx);
                    if (previewObj)
                    {
                        var previewSpriteRenderer = previewObj.GetComponent<SpriteRenderer>();
                        if (previewSpriteRenderer)
                        {
                            GUI.DrawTexture(previewRect, previewSpriteRenderer.sprite.texture, ScaleMode.ScaleToFit);
                        }

                        GUI.Label(previewRect, "" + _inventory.GetQuantity(idx));
                    }
                }

                IMUIHelper.DrawFilledRect(
                    new Rect(rect.left + offsetLeft + widthPreview + widthInnerBorder, rect.top + offsetTop,
                        widthRight, heightItem),
                    idx == itemSelectIdx ? LightGrey : Grey);
            }

            offsetTop += heightItem + heightInnerBorder;
        }
    }

    private void DrawInventoryBorder()
    {
        Rect rect = getTruncatedInnerRect(150);
        IMUIHelper.DrawFilledRect(
            new Rect(
                rect.left, rect.top, rect.width, rect.height
            ), Brown);
    }

    public void Show(Inventory iv, Inventory market)
    {
        MyInventory = iv;
        show = true;
    }

    public Inventory Hide()
    {
        show = false;
        return MyInventory;
    }
}