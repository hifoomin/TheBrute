using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Powers;
using TheBrute.Relics;
using TheBrute.Relics.Uncommons;

namespace TheBrute.Relics.Rares
{
    internal class Biocatalyst : TheBruteRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
           HoverTipFactory.FromPower<ThornsPower>()
        ];
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Powers.ThornsPower), "BeforeDamageReceived")]
    internal class BioCatalystBeforeDamageReceivedPatch
    {
        private static void Postfix(Task __result, PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            var combatState = target.CombatState;
            var biocatalyst = target.Player?.GetRelic<Biocatalyst>();
            var hittableEnemiesExceptInflictor = combatState.HittableEnemies.Where(x => x != dealer);
            if (combatState != null && biocatalyst != null)
            {
                biocatalyst.Flash();
                CreatureCmd.Damage(choiceContext, hittableEnemiesExceptInflictor, amount, props, target, cardSource, null);
            }
        }
    }
}