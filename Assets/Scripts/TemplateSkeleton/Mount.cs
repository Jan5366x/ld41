using UnityEngine;

namespace TemplateSkeleton
{
    [CreateAssetMenu(fileName = "New Mount", menuName = "Template/Mount")]
    public class Mount : ScriptableObject {
        public int Strength;
        public int Intelligence;
        public int Stamina;
        public int Agility;
    
        public float MaxSpeed;
        public float Acceleration;

        public GameObject Presentation;
    }
}
