using World.Objects;

namespace World.Interactables
{
    public class BuyInteractable : Interactable
    {
        public Inventory inventory;

        public override void Interact(UnitLogic player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            player.BuyInventory(inventory);
        }
    }
}