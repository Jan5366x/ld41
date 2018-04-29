using System;
using UnityEngine;
using World.Objects;

namespace UI
{
    public class ViewHUD : MonoBehaviour
    {
        public Rect ViewRect;
        public UnitLogic unit;
        private String text;
        private float ShowTextDuration;
        private bool GameOver;
        private GUIStyle GameOverStyle;

        private static readonly Color Black = new Color(0, 0, 0);
        private static readonly Color Red = new Color(1f, 0, 0);
        private static readonly Color Green = new Color(0, 1f, 0f);
        private static readonly Color DarkGreen = new Color(0, 0.6f, 0f);
        private static readonly Color Blue = new Color(0, 0, 1f);
        private static readonly Color Brown = new Color(0.42f, 0.21f, 0.13f);
        private static readonly Color Grey = new Color(0.3f, 0.3f, 0.3f, 0.5f);
        private static readonly Color LightGrey = new Color(0.6f, 0.6f, 0.6f, 0.5f);

        public Sprite backgroundSprite;

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
        }

        private void Start()
        {
            GameOverStyle = new GUIStyle();
            GameOverStyle.fontSize = 64;
            GameOverStyle.fontStyle = FontStyle.Bold;
            GameOverStyle.alignment = TextAnchor.MiddleCenter;
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
            if (GameOver)
            {
                DrawGameOver();
            }

            if (ShowTextDuration >= 0)
            {
                DrawText();
            }
        }

        private void DrawGameOver()
        {
            Rect rect = getTruncatedInnerRect(150);
            IMUIHelper.DrawFilledRect(rect, Brown);
            if (backgroundSprite != null)
            {
                GUI.DrawTexture(rect, backgroundSprite.texture, ScaleMode.ScaleAndCrop);
            }

            rect = getTruncatedInnerRect(160);
            IMUIHelper.DrawFilledRect(rect, Grey);
            rect = getTruncatedInnerRect(170);

            GUI.Label(rect, "Game Over\r\nScrore: " + unit.Money, GameOverStyle);
        }

        private void DrawText()
        {
            Rect rect = getTruncatedInnerRect(150);
            IMUIHelper.DrawFilledRect(rect, Brown);
            if (backgroundSprite != null)
            {
                GUI.DrawTexture(rect, backgroundSprite.texture, ScaleMode.ScaleAndCrop);
            }

            rect = getTruncatedInnerRect(160);
            IMUIHelper.DrawFilledRect(rect, Grey);
            rect = getTruncatedInnerRect(170);
            GUI.Label(rect, text);
        }

        private void DrawMoney()
        {
            GUI.Label(new Rect(ViewRect.center.x - 30, ViewRect.top + 10, 60, 30), "$" + unit.Money);
        }

        protected void DrawRightWeapon()
        {
            var rect = new Rect(ViewRect.right - 100, ViewRect.bottom - 100, 100, 100);
            IMUIHelper.DrawBorderRect(rect, 1, Black);
            var item = unit.Inventory.GetItem((int) ItemSlot.RightHand);
            if (item != null)
            {
                GUI.DrawTexture(rect, item.PreviewLarge.texture, ScaleMode.ScaleToFit);
            }
        }

        protected void DrawLeftWeapon()
        {
            var rect = new Rect(ViewRect.left, ViewRect.bottom - 100, 100, 100);
            IMUIHelper.DrawBorderRect(rect, 1, Black);
            var item = unit.Inventory.GetItem((int) ItemSlot.LeftHand);
            if (item != null)
            {
                GUI.DrawTexture(rect, item.PreviewLarge.texture, ScaleMode.ScaleToFit);
            }
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
}