using UnityEngine;
using World.Objects;

namespace World.Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void Interact(UnitLogic player);

        public virtual bool CanInteract(UnitLogic obj)
        {
            float dx = transform.position.x - obj.transform.position.x;
            float dy = transform.position.y - obj.transform.position.y;
            float d = Mathf.Sqrt(dx * dx + dy * dy);
            return d < obj.Template.HandRange;
        }
    }
}