using World.Objects;

namespace World.Interactables
{
    public class SellInteractable : Interactable
    {
        public Inventory inventory;

        public override void Interact(UnitLogic player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            player.SellInventory(inventory);
        }
    }
}