using TemplateSkeleton;
using UnityEngine;

namespace World.Objects
{
    public class SpriteAnimator : MonoBehaviour
    {
        public Item Item;
        public UnitLogic.MoveDirection Direction;
        public int CurrentSpriteIndex;
        public static int SpriteCount = 3;
        public SpriteRenderer renderer; // TODO fix naming since it collide with "UnityEngine.Component.renderer" !!

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            UpdateSprite();
        }

        public void SetItem(Item item)
        {
            this.Item = item;
            CurrentSpriteIndex = 0;
            UpdateSprite();
        }

        public void SetDirection(UnitLogic.MoveDirection direction)
        {
            this.Direction = direction;
            UpdateSprite();
        }

        public void NextSprite()
        {
            CurrentSpriteIndex = (CurrentSpriteIndex + 1) % SpriteCount;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            if(!renderer)
                return;
        
            if (Item == null)
            {
                renderer.sprite = null;
                return;
            }
        
            int idx = (int) Direction * SpriteCount + CurrentSpriteIndex;
            if (idx < Item.GetSprites().Length)
            {
                renderer.sprite = Item.GetSprites()[idx];
            }
            else
            {
                renderer.sprite = null;
            }
        }

        public void Idle()
        {
            int idleFrame = 0;

            // select idle frame based of dicection
            if (Direction == UnitLogic.MoveDirection.Left || Direction == UnitLogic.MoveDirection.Right)
                idleFrame = 1;
        
            if (CurrentSpriteIndex != idleFrame)
            {
                CurrentSpriteIndex = idleFrame;
                UpdateSprite();
            }
        }
    }
}