#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Multiplayer
{
    internal class StrikeADeal : TheBruteCard
    {
        public StrikeADeal() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
        {
        }

        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            EnergyHoverTip
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new EnergyVar(1),
            new PowerVar<PossessionPower>(1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            var aliveAllies = from c in CombatState.GetTeammatesOf(Owner.Creature)
                              where c != null && c.IsAlive && c.IsPlayer && c != Owner.Creature
                              select c;
            foreach (var player in aliveAllies)
            {
                await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, player.Player);
            }

            await PowerCmd.Apply<PossessionPower>(choiceContext, Owner.Creature, DynamicVars["PossessionPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(1m);
        }
    }
}