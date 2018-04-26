using UnityEngine;
using World.Objects;

namespace World.Interactables
{
    public class LoggerInteractable : Interactable
    {
        public override void Interact(UnitLogic player)
        {
            if (CanInteract(player))
            {
                Debug.Log(player);
            }
        }
    }
}