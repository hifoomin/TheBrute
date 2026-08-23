#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Cards.Multiplayer
{
    internal class ClaviculaNox : TheBruteCard
    {
        public ClaviculaNox() : base(2, CardType.Power, CardRarity.Rare, TargetType.AllAllies)
        {
        }

        public override bool CanBeGeneratedInCombat => false;

        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<PlatingPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<PlatingPower>(7m),
            new GoldVar(5)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            var alivePlayers = from c in CombatState!.GetTeammatesOf(Owner.Creature)
                               where c != null && c.IsAlive && c.IsPlayer
                               select c;
            foreach (var player in alivePlayers)
            {
                if (player.Player == null)
                {
                    continue;
                }

                await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, player.Player);
                await PowerCmd.Apply<PlatingPower>(choiceContext, player, DynamicVars["PlatingPower"].BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["PlatingPower"].UpgradeValueBy(2m);
            DynamicVars.Gold.UpgradeValueBy(2m);
        }
    }
}