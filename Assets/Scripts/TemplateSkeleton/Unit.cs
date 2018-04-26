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

        public Item BaseBody;
        public Item BaseHair;
        public GameObject DeathDrop;
    }
}
