using System.Linq;
using UnityEngine;
using World.Objects;

namespace TemplateSkeleton
{
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
       
        public ItemSlot[] PossibleSlots;
    }
}
