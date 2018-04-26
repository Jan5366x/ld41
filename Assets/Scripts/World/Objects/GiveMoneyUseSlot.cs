namespace World.Objects
{
    public class GiveMoneyUseSlot : UseSlot
    {
        public int money;

        public override void OnUse(UnitLogic player)
        {
            if (player)
            {
                player.Money += money;
            }
        }
    }
}