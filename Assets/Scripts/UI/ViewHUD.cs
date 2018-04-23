using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHUD : MonoBehaviour
{
    public Rect ViewRect;
    public UnitLogic unit;
    private String text;
    private float ShowTextDuration;

    private static readonly Color Black = new Color(0, 0, 0);
    private static readonly Color Red = new Color(1f, 0, 0);
    private static readonly Color Green = new Color(0, 1f, 0f);
    private static readonly Color DarkGreen = new Color(0, 0.6f, 0f);
    private static readonly Color Blue = new Color(0, 0, 1f);
    private static readonly Color Brown = new Color(0.42f, 0.21f, 0.13f);
    private static readonly Color Grey = new Color(0.3f, 0.3f, 0.3f);
    private static readonly Color LightGrey = new Color(0.6f, 0.6f, 0.6f);

    public Rect getTruncatedInnerRect(int border)
    {
        int w = (int) Mathf.Min(1024, ViewRect.width - 2 * border);
        int h = (int) Mathf.Min(1024, ViewRect.height - 2 * border);

        int cx = (int) ViewRect.center.x;
        int cy = (int) ViewRect.center.y;

        return new Rect(cx - w / 2, cy - h / 2, w, h);
    }
    
    public void ShowText(String text, float duration)
    {
        this.text = text;
        ShowTextDuration = duration;
        print("UUUUUUUUUUUUUUUUUUUUUUUUUUuuuu");
    }


    private void Update()
    {
        ShowTextDuration -= Time.deltaTime;
    }

    private void OnGUI()
    {
        DrawHPBar();
        DrawManaBar();
        DrawStaminaBar();
        DrawLeftWeapon();
        DrawRightWeapon();
        DrawMoney();
        if (ShowTextDuration >= 0)
        {
            DrawText();
            print("TTTTTTTTTTTTTTTTTTTTTTTTtttttttttt");
        }
    }

    private void DrawText()
    {
        Rect rect = getTruncatedInnerRect(150);
        IMUIHelper.DrawFilledRect(
            new Rect(rect.left, rect.top, rect.width, rect.height),
            Brown
        );
        GUI.Label(
            new Rect(rect.left + 10, rect.top + 10, rect.width - 20, rect.height - 20),
            text
        );
    }

    private void DrawMoney()
    {
        GUI.Label(new Rect(ViewRect.center.x - 30, ViewRect.top + 10, 60, 30), "$" + unit.Money);
    }

    protected void DrawRightWeapon()
    {
        IMUIHelper.DrawBorderRect(
            new Rect(ViewRect.right - 100, ViewRect.bottom - 100, 100, 100),
            1,
            Black
        );
    }

    protected void DrawLeftWeapon()
    {
        IMUIHelper.DrawBorderRect(
            new Rect(ViewRect.left, ViewRect.bottom - 100, 100, 100),
            1,
            Black
        );
    }

    protected void DrawHPBar()
    {
        IMUIHelper.DrawFilledBorderRect(
            new Rect(ViewRect.left + 10, ViewRect.top + 10, 100, 10),
            1,
            unit.HP / unit.Template.MaxHealth,
            Black,
            Red
        );
    }

    protected void DrawManaBar()
    {
        IMUIHelper.DrawFilledBorderRect(
            new Rect(ViewRect.right - 110, ViewRect.top + 10, 100, 10),
            1,
            unit.Mana / unit.Template.MaxMana,
            Black,
            Blue
        );
    }

    protected void DrawStaminaBar()
    {
        IMUIHelper.DrawFilledBorderRect(
            new Rect(ViewRect.right - 110, ViewRect.top + 25, 100, 6),
            1,
            unit.Stamina / unit.Template.Stamina,
            Black,
            unit.Stamina < unit.Template.StaminaMinUsage ? DarkGreen : Green
        );
    }
}