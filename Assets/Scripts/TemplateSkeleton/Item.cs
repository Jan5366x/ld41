using UnityEngine;

namespace TemplateSkeleton
{
    
    public enum ItemSlot
    {
        Head = 0,
        Armor = 1,
        LeftHand = 2,
        RightHand = 3,
        Pants = 4,
        Shoes = 5
    }
    
    [CreateAssetMenu(fileName = "New Item", menuName = "Template/Item")]
    public class Item : ScriptableObject {
		 public static float SellModifier = 0.75f;


        public string ItemName;
        public int BasePrice;
        public Sprite PreviewSmall;
        public Sprite PreviewLarge;
        public Effect ItemEffect;

        // TODO replace with a easier to use solution since this takes to much time and can cause issues
        public Sprite[] Sprites;
        
        public float ArmorResistence;
        
        // TODO implement usage in game
        public ItemSlot[] PossibleSlots;
    }
}
