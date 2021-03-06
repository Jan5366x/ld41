﻿using World.Objects;

namespace Effects
{
    public class BloodEffectLogic : EffectLogic
    {
        public float DamagePerTick = 3;
        public float TickRate = 0.2f;

        public override string getName()
        {
            return "Blood";
        }

        public override void apply(UnitLogic player, float duration)
        {
            player.ShowPrefab("Effects\\BloodEffect", duration);
        }
    }
}