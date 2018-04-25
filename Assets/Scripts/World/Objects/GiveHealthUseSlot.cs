using System.Linq;
using UnityEngine;

public class GiveHealthUseSlot : UseSlot
{
    public float health;

    public override void OnUse(UnitLogic player)
    {
        if (player)
        {
            player.HP = Mathf.Max(player.HP + health, player.Template.MaxHealth);
        }
    }

    private void Update()
    {
        
    }
}