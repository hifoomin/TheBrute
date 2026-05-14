using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yoka.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class RecklessStrike : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public RecklessStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override HashSet<CardTag> CanonicalTags => new([CardTag.Strike]);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(14m, ValueProp.Move),
            new GoldVar(2)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            var allCards = Owner.PlayerCombatState.AllCards;
            var card = Owner.RunState.Rng.CombatCardSelection.NextItem(allCards);
            if (card != null && (!card.DynamicVars.TryGetValue("Gold", out var gold) || gold.BaseValue <= 0))
            {
                // CardCmd.ApplyKeyword(item, CardKeyword.Exhaust);
                // item.DynamicVars.HpLoss.BaseValue += DynamicVars.HpLoss.BaseValue;
                var vars = AccessTools.Field(typeof(DynamicVarSet), "_vars");

                var cardVars = (Dictionary<string, DynamicVar>)vars.GetValue(card.DynamicVars);

                cardVars["Gold"] = new GoldVar(DynamicVars.Gold.IntValue);
                // Main.Logger.Warn("reckless strike onplay: trying to add keyword and gold");
                card.AddKeyword(Yoka.Keywords.goldLossKeyword);
                // Main.Logger.Warn("reckless strike onplay: cards new gold value is " + vars["Gold"].BaseValue);
            }

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx(null /*"vfx/vfx_attack_slash"*/)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}