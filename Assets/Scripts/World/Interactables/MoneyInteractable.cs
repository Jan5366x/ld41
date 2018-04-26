using World.Objects;

namespace World.Interactables
{
    public class MoneyInteractable : Interactable
    {
        public int Money;

        public override void Interact(UnitLogic player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            player.Money += Money;
            Destroy(gameObject);
        }
    }
}