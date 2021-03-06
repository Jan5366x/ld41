﻿using System;
using UnityEngine;
using World.Objects;

namespace UI
{
    public class ViewInventory : MonoBehaviour
    {
        public Rect ViewRect;
        public UnitLogic unit;
        private int itemDrawOffset;
        private int itemSelectIdx;
        public int playerIdx;

        public void handleSelectSlot(int slot)
        {
            switch ((ItemSlot) slot)
            {
                case ItemSlot.Head:
                case ItemSlot.Armor:
                case ItemSlot.Pants:
                case ItemSlot.Shoes:
                case ItemSlot.LeftHand:
                case ItemSlot.RightHand:
                    _inventory.Unequip(slot);
                    break;
                default:
                    _inventory.Equip(slot, unit);
                    break;
            }
        }

        private static readonly Color Black = new Color(0, 0, 0);
        private static readonly Color Red = new Color(1f, 0, 0);
        private static readonly Color Green = new Color(0, 1f, 0f);
        private static readonly Color DarkGreen = new Color(0, 0.6f, 0f);
        private static readonly Color Blue = new Color(0, 0, 1f);
        private static readonly Color Brown = new Color(0.42f, 0.21f, 0.13f);
        private static readonly Color Grey = new Color(0.3f, 0.3f, 0.3f, 0.5f);
        private static readonly Color LightGrey = new Color(0.6f, 0.6f, 0.6f, 0.5f);

        private Inventory _inventory;

        public float MoveDelayRemaining;
        public float MoveDelay = 0.1f;
        public bool show = false;

        public Sprite[] reservedSlotsPreview;
        public Sprite backgroundSprite;

        public Rect getTruncatedInnerRect(int border)
        {
            int w = (int) Mathf.Min(1024, ViewRect.width - 2 * border);
            int h = (int) Mathf.Min(1024, ViewRect.height - 2 * border);

            int cx = (int) ViewRect.center.x;
            int cy = (int) ViewRect.center.y;

            return new Rect(cx - w / 2, cy - h / 2, w, h);
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
                    var item = _inventory.GetItem(idx);
                    if (item != null)
                    {
                    }

                    Rect previewRect = new Rect(rect.left + offsetLeft, rect.top + offsetTop, widthPreview,
                        heightItem);
                    IMUIHelper.DrawFilledRect(previewRect, idx == itemSelectIdx ? LightGrey : Grey);

                    if (_inventory != null)
                    {
                        bool didDrawPreview = false;
                        var previewItem = _inventory.GetItem(idx);
                        if (previewItem)
                        {
                            var previewSprite = previewItem.PreviewSmall;
                            if (previewSprite)
                            {
                                GUI.DrawTexture(previewRect, previewSprite.texture, ScaleMode.ScaleAndCrop);
                                didDrawPreview = true;
                            }
                        }

                        if (!didDrawPreview)
                        {
                            if (idx < Inventory.OFFSET_SLOT && idx < reservedSlotsPreview.Length)
                            {
                                GUI.DrawTexture(previewRect, reservedSlotsPreview[idx].texture, ScaleMode.ScaleAndCrop);
                            }
                        }

                        if (previewItem)
                        {
                            GUI.Label(previewRect, "" + _inventory.GetQuantity(idx));
                        }
                    }

                    var infoRect = new Rect(rect.left + offsetLeft + widthPreview + widthInnerBorder, rect.top + offsetTop,
                        widthRight, heightItem);
                    IMUIHelper.DrawFilledRect(infoRect, idx == itemSelectIdx ? LightGrey : Grey);

                    infoRect = new Rect(rect.left + offsetLeft + widthPreview + widthInnerBorder + 10, rect.top + offsetTop,
                        widthRight - 20, heightItem);

                    if (item)
                    {
                        string text = "" + item.ItemName;
                        var obj = _inventory.GetObject((ItemSlot) idx);
                        if (obj)
                        {
                            var weapon = obj.GetComponent<Weapon>();
                            if (weapon)
                            {
                                text += "\r\nType: ";
                                text += weapon.Magic ? "Magic" : "Meele";
                                text += "\tDamage:";
                                text += (int) weapon.Damage;
                                text += "\tRange:";
                                text += (int) weapon.Range;
                                text += "\tCooldown:";
                                text += (int) weapon.CoolDown;
                                if (weapon.Effect)
                                {
                                    text += "\tEffect:";
                                    text += weapon.Effect.getName();
                                }
                            }
                            else if (item.ArmorResistence > 1)
                            {
                                text += "\r\nType: Armor";
                                text += "\r\nArmorPoints: ";
                                text += (int) item.ArmorResistence;
                            }
                        }

                        GUI.Label(infoRect, text);
                    }
                }

                offsetTop += heightItem + heightInnerBorder;
            }
        }

        private void DrawInventoryBorder()
        {
            Rect rect = getTruncatedInnerRect(150);
            IMUIHelper.DrawFilledRect(rect, Brown);
            if (backgroundSprite != null)
            {
                GUI.DrawTexture(rect, backgroundSprite.texture, ScaleMode.ScaleAndCrop);
            }
        }

        public void Show(Inventory iv)
        {
            _inventory = iv;
            show = true;
        }

        public Inventory Hide()
        {
            show = false;
            return _inventory;
        }
    }
}