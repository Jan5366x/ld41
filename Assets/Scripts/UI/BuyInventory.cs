using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyInventory : MonoBehaviour
{
    public Rect ViewRect;
    public UnitLogic unit;
    private int itemDrawOffset;
    private int itemSelectIdx;
    public int playerIdx;

    public void handleSelectSlot(int slot)
    {
        unit.Buy(_inventory.Items[slot]);
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

    public float moveDelayRemaining;
    public float moveDelay = 0.0001f;
    public bool show = false;

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
            moveDelayRemaining -= Time.deltaTime;
            if (moveDelayRemaining <= 0)
            {
                int drawCount = GetItemDrawCount();
                if (Input.GetAxis("Vertical" + playerIdx) < 0)
                {
                    itemSelectIdx = Math.Min(itemSelectIdx + 1, Inventory.COUNT_SLOT - 1);
                    moveDelayRemaining = moveDelay;
                }
                else if (Input.GetAxis("Vertical" + playerIdx) > 0)
                {
                    itemSelectIdx = Math.Max(itemSelectIdx - 1, 0);
                    moveDelayRemaining = moveDelay;
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
            }

            if (Input.GetButtonDown("UseTool" + playerIdx))
            {
                handleSelectSlot(itemSelectIdx);
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
        int offsetLeft = 160;
        int offsetTop = 160;
        int offsetBottom = 160;
        int widthPreview = 64;
        int widthInnerBorder = 10;
        int widthRight = (int) (ViewRect.width - (2 * offsetLeft + widthPreview + widthInnerBorder));
        int heightItem = 64;
        int heightInnerBorder = 10;
        for (int idx = 0; idx < Inventory.COUNT_SLOT; idx++)
        {
            if ((offsetTop + heightItem) > (ViewRect.bottom - offsetBottom))
            {
                return idx - 1;
            }

            offsetTop += heightItem + heightInnerBorder;
        }

        return 0;
    }

    private void DrawInventory()
    {
        int offsetLeft = 160;
        int offsetTop = 160;
        int offsetBottom = 160;
        int widthPreview = 64;
        int widthInnerBorder = 10;
        int widthRight = (int) (ViewRect.width - (2 * offsetLeft + widthPreview + widthInnerBorder));
        int heightItem = 64;
        int heightInnerBorder = 10;
        for (int idx = itemDrawOffset; idx < Inventory.COUNT_SLOT; idx++)
        {
            if ((offsetTop + heightItem) < (ViewRect.bottom - offsetBottom))
            {
                Rect previewRect = new Rect(ViewRect.left + offsetLeft, ViewRect.top + offsetTop, widthPreview,
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
                    new Rect(ViewRect.left + offsetLeft + widthPreview + widthInnerBorder, ViewRect.top + offsetTop,
                        widthRight, heightItem),
                    idx == itemSelectIdx ? LightGrey : Grey);
            }

            offsetTop += heightItem + heightInnerBorder;
        }
    }

    private void DrawInventoryBorder()
    {
        IMUIHelper.DrawFilledRect(
            new Rect(
                ViewRect.left + 150, ViewRect.top + 150, ViewRect.width - 300, ViewRect.height - 300
            ), Brown);
    }

    public void Show(Inventory iv)
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