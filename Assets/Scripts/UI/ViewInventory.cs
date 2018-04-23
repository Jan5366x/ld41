using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewInventory : MonoBehaviour
{
    public Rect ViewRect;
    public UnitLogic unit;
    private int itemDrawOffset;
    private int itemSelectIdx;
    public int playerIdx;

    public void handleSelectSlot(int slot)
    {
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
    private bool show;

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
    }

    // Update is called once per frame
    void Update()
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

    private void OnGUI()
    {
        DrawHPBar();
        DrawManaBar();
        DrawStaminaBar();
        DrawLeftWeapon();
        DrawRightWeapon();
        if (!show)
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
                DrawFilledRect(previewRect, idx == itemSelectIdx ? LightGrey : Grey);
                if (MyInventory)
                {
                    var previewObj = MyInventory.GetObject(idx);
                    if (previewObj)
                    {
                        var previewSpriteRenderer = previewObj.GetComponent<SpriteRenderer>();
                        if (previewSpriteRenderer)
                        {
                            GUI.DrawTexture(previewRect, previewSpriteRenderer.sprite.texture, ScaleMode.ScaleToFit);
                        }

                        GUI.Label(previewRect, "" + MyInventory.GetQuantity(idx));
                    }
                }

                DrawFilledRect(
                    new Rect(ViewRect.left + offsetLeft + widthPreview + widthInnerBorder, ViewRect.top + offsetTop,
                        widthRight, heightItem),
                    idx == itemSelectIdx ? LightGrey : Grey);
            }

            offsetTop += heightItem + heightInnerBorder;
        }
    }

    private void DrawInventoryBorder()
    {
        DrawFilledRect(
            new Rect(
                ViewRect.left + 150, ViewRect.top + 150, ViewRect.width - 300, ViewRect.height - 300
            ), Brown);
    }

    protected void DrawRightWeapon()
    {
        DrawBorderRect(
            new Rect(ViewRect.right - 100, ViewRect.bottom - 100, 100, 100),
            1,
            Black
        );
    }

    protected void DrawLeftWeapon()
    {
        DrawBorderRect(
            new Rect(ViewRect.left, ViewRect.bottom - 100, 100, 100),
            1,
            Black
        );
    }

    protected void DrawHPBar()
    {
        DrawFilledBorderRect(
            new Rect(ViewRect.left + 10, 10, 100, 10),
            1,
            unit.HP / unit.Template.MaxHealth,
            Black,
            Red
        );
    }

    protected void DrawManaBar()
    {
        DrawFilledBorderRect(
            new Rect(ViewRect.right - 110, 10, 100, 10),
            1,
            unit.Mana / unit.Template.MaxMana,
            Black,
            Blue
        );
    }

    protected void DrawStaminaBar()
    {
        DrawFilledBorderRect(
            new Rect(ViewRect.right - 110, 25, 100, 6),
            1,
            unit.Stamina / unit.Template.Stamina,
            Black,
            unit.Stamina < unit.Template.StaminaMinUsage ? DarkGreen : Green
        );
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

    ////////////////////////
    /// HELPER
    ////////////////////////
    static Texture2D _whiteTexture;

    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawFilledRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawBorderRect(Rect rect, float thickness, Color color)
    {
        // Top
        DrawFilledRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawFilledRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawFilledRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawFilledRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static void DrawFilledBorderRect(Rect rect, float thickness, float progress, Color border, Color content)
    {
        DrawBorderRect(rect, thickness, border);

        DrawFilledRect(
            new Rect(rect.x + thickness,
                rect.y + thickness,
                (rect.width - 2 * thickness) * progress,
                rect.height - 2 * thickness),
            content);
    }
}