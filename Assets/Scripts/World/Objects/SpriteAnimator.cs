using TemplateSkeleton;
using UnityEngine;

namespace World.Objects
{
    public class SpriteAnimator : MonoBehaviour
    {
        public Item item;
        public UnitLogic.MoveDirection direction;
        public int currentSpriteIndex;
        public static int spriteCount = 3;
        public SpriteRenderer renderer; // TODO fix naming since it collide with "UnityEngine.Component.renderer" !!

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            UpdateSprite();
        }

        public void SetItem(Item item)
        {
            this.item = item;
            currentSpriteIndex = 0;
            UpdateSprite();
        }

        public void SetDirection(UnitLogic.MoveDirection direction)
        {
            this.direction = direction;
            UpdateSprite();
        }

        public void NextSprite()
        {
            currentSpriteIndex = (currentSpriteIndex + 1) % spriteCount;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            if(!renderer)
                return;
        
            if (item == null)
            {
                renderer.sprite = null;
                return;
            }
        
            int idx = (int) direction * spriteCount + currentSpriteIndex;
            if (idx < item.Sprites.Length)
            {
                renderer.sprite = item.Sprites[idx];
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
            if (direction == UnitLogic.MoveDirection.Left || direction == UnitLogic.MoveDirection.Right)
                idleFrame = 1;
        
            if (currentSpriteIndex != idleFrame)
            {
                currentSpriteIndex = idleFrame;
                UpdateSprite();
            }
        }
    }
}