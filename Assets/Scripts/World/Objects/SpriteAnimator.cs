using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public Item item;
    public int direction;
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

    public void SetDirection(int direction)
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
            int idx = direction * spriteCount + sprite;
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
}