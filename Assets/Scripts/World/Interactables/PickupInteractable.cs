using UnityEngine;
using World.Objects;

namespace World.Interactables
{
    public class PickupInteractable : Interactable
    {
        public GameObject obj;
        public override void Interact(UnitLogic player)
        {
            if (!CanInteract(player))
            {
                return;
            }
        
            player.Inventory.Take(obj);
            Destroy(gameObject); //.SetActive(false));
        }
    }
}