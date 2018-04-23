using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewInventory : MonoBehaviour
{
    public Rect ViewRect;
    public UnitLogic unit;

    public void handleSelectSlot(int slot)
    {
    }

    private static readonly Color Black = new Color(0, 0, 0);
    private static readonly Color Red = new Color(1f, 0, 0);
    private static readonly Color Blue = new Color(0, 0, 1f);

    private Inventory _inventory;

    public Inventory Inventory
    {
        get { return _inventory.Copy(); }
        set { _inventory = value.Copy(); }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnGUI()
    {
        DrawHPBar();
        DrawManaBar();
        DrawLeftWeapon();
        DrawRightWeapon();
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