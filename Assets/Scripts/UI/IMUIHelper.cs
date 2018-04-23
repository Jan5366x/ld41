using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMUIHelper : MonoBehaviour
{
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