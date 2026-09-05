#region

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheBrute.Cards.Ancients;
using TheBrute.Cards.AprilFools;

#endregion

namespace TheBrute.Cards.Starters
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Steal : TheBruteCard, ITranscendenceCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Steal() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
        {
        }

        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(1m),
            new GoldVar(1),
            new DamageVar(10m, ValueProp.Move)
        ];

        public CardModel GetTranscendenceTransformedCard()
        {
            return SpecialEventManager.IsAprilFools(DateTime.Today) ? ModelDb.Card<SnakebiteUltra>() : ModelDb.Card<Usurp>();
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);

            await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);

            var result = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).Execute(choiceContext);

            AudioUtils.PlaySlash(result);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}