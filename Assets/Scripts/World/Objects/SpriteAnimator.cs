using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public Item item;
    public UnitLogic.MoveDirection direction;
    public int sprite;
    public static int spriteCount = 3;
    public SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        sprite = 0;
        UpdateSprite();
    }

    public void SetDirection(UnitLogic.MoveDirection direction)
    {
        this.direction = direction;
        UpdateSprite();
    }

    public void NextSprite()
    {
        sprite = (sprite + 1) % spriteCount;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (item != null)
        {
            int idx = (int) direction * spriteCount + sprite;
            if (idx < item.Sprites.Length)
            {
                renderer.sprite = item.Sprites[idx];
            }
            else
            {
                renderer.sprite = null;
            }
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
        
        if (sprite != idleFrame)
        {
            sprite = idleFrame;
            UpdateSprite();
        }
    }
}