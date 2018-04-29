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

    
        
        public float ArmorResistence;
       
        public ItemSlot[] PossibleSlots;
        
        
        public Texture2D Texture;


        private Sprite[] _cachedSprites;

        public Sprite[] GetSprites()
        {

            if (_cachedSprites != null)
                return _cachedSprites;

            _cachedSprites = Resources.LoadAll<Sprite>(Texture.name);
            return _cachedSprites;

        }
        
    }
}
