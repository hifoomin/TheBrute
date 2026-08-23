#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Possession : TheBruteCard
    {
        public Possession() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(4m),
            new GoldVar(10),
            new PowerVar<PossessionPower>(1m)
        ];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);

            await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);

            await PowerCmd.Apply<PossessionPower>(choiceContext, Owner.Creature, DynamicVars["PossessionPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.MaxHp.UpgradeValueBy(1m);
            DynamicVars.Gold.UpgradeValueBy(3m);
        }
    }
}