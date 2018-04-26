using UnityEngine;

namespace World.Objects
{
    public abstract class UseSlot : MonoBehaviour
    {
        public abstract void OnUse(UnitLogic player);
    }
}