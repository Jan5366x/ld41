using System.Linq;
using UnityEngine;

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

    private void Update()
    {
        
    }
}