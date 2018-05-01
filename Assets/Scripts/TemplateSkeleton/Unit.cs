using System.Runtime.ConstrainedExecution;
using UnityEngine;

namespace TemplateSkeleton
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "Template/Unit")]
    public class Unit : ScriptableObject
    {
        public string UnitName;
    
        public int MaxHealth;
        public int MaxMana;

        public float HPRegeneration;
        public float ManaRegeneration;
        public float StaminaRegeneration;
        public float StaminaUsage;
        public float StaminaMinUsage;
    
        public int Strength;
        public int Intelligence;
        public int Stamina;
        public int Agility;
    
        public float MaxSpeed;
        public float Acceleration;
    
        public bool IsEnemy;
        public bool IsPlayer;
        public bool IsInteractable;
        public bool IsShowName;
    
        public GameObject Presentation;
        public GameObject DeathPrefab;
        public GameObject TargetMarker;
        public float HandRange = 0.4f;
        public float FollowRange = 1f;

        public GameObject DeathDrop;
        
        
        public Texture2D BodyTexture;
        public Texture2D AttachmentTexture;

        private Sprite[] _cachedBodySprites;
        private Sprite[] _cachedAttachmentSprites;
        
        public Sprite[] GetBodySprites()
        {

           /* if (_cachedBodySprites.Length > 0)
                return _cachedBodySprites;
 */
            _cachedBodySprites = GetByTexture(BodyTexture);
            return _cachedBodySprites;

        }
        
        public Sprite[] GetAttachmentSprites()
        {
            /*if (_cachedAttachmentSprites.Length > 0)
                return _cachedAttachmentSprites; */

            _cachedAttachmentSprites = GetByTexture(AttachmentTexture);
            return _cachedAttachmentSprites;

        }




        public static Sprite[] GetByTexture(Texture2D texture2D)
        {
            var sprites = new Sprite[12];
            for (int i = 0; i < 12; i++)
            {
                sprites[i] = Sprite.Create(texture2D, new Rect(i*32, 0, 32, 32), new Vector2(0.5f, 0.5f),32);
            }

            return sprites;
        }

       
    }
}
