using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves.Validation;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards;

namespace TheBrute.Powers
{
    internal class ExplosionPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

        public override int DisplayAmount => (Owner.MaxHp * Amount) / 100;

        /*
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("CalculatedDamageButGood", 0m),
            new DynamicVar("DisplayAmountWhatTheFuckSpaghettiCodeStrikesAgain", 0m)
        ];

        public void SetDisplayAmount
        */

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner.Player != player)
            {
                return;
            }

            var hittableEnemies = CombatManager.Instance._state?.HittableEnemies;
            if (hittableEnemies != null)
            {
                foreach (var hittableEnemy in hittableEnemies)
                {
                    NFireBurstVfx child = NFireBurstVfx.Create(hittableEnemy, 0.75f);
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
                }

                // recalculate here somehow maybe?
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), hittableEnemies, DisplayAmount, ValueProp.Unpowered, null, null);
            }
        }
    }
}