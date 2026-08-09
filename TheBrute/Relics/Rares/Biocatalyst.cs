#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

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

    [HarmonyPatch(typeof(ThornsPower), "BeforeDamageReceived")]
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