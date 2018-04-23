using System.Linq;
using UnityEngine;

public class GiveManaUseSlot : UseSlot
{
    public float mana;
    public override void OnUse(UnitLogic player)
    {
        if (player)
        {
            player.Mana = Mathf.Max(player.Mana + mana, player.Template.MaxMana);
        }
    }
}