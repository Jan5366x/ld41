using System;
using World.Objects;

namespace World.Interactables
{
    public class MessageInteractable : Interactable
    {
        public String firstInteraction;
        public String laterInteraction;

        public int interactionCount = 0;

        public override void Interact(UnitLogic player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            if (interactionCount == 0)
            {
                player.ShowMessage(firstInteraction, 10);
            }
            else
            {
                player.ShowMessage(firstInteraction, 10);
            }

            interactionCount++;
        }
    }
}