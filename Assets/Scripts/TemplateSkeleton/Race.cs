using UnityEngine;

namespace TemplateSkeleton
{
    [CreateAssetMenu(fileName = "New Race", menuName = "Template/Race")]
    public class Race : ScriptableObject
    {
        public GameObject StartPoint;
        public float TimeLimit;
    }
}
