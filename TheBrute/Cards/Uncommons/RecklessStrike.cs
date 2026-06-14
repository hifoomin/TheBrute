using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Reflection.Metadata.Ecma335;

namespace TheBrute.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class RecklessStrike : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public RecklessStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override HashSet<CardTag> CanonicalTags => new([CardTag.Strike, TheBrute.Cards.Tags.goldRelated]);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1),
            new DamageVar(13m, ValueProp.Move),
            new GoldVar(3),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_flying_slash")
                .Execute(choiceContext);

            static bool filter(CardModel c) => c.IsUpgradable;
            var cards = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars.Cards.IntValue), context: choiceContext, player: Owner, filter: filter, source: this));
            foreach (var card in cards)
            {
                var vars = AccessTools.Field(typeof(DynamicVarSet), "_vars");

                var cardVars = (Dictionary<string, DynamicVar>)vars.GetValue(card.DynamicVars);

                cardVars["Gold"] = new GoldVar(DynamicVars.Gold.IntValue);
                // Main.Logger.Warn("reckless strike onplay: trying to add keyword and gold");
                card.AddKeyword(TheBrute.Cards.Keywords.goldLossKeyword);

                CardCmd.Upgrade(card);
            }

            // (!randomCard.DynamicVars.TryGetValue("Gold", out var gold) || gold.BaseValue <= 0)
            // keep just in case some mod pops up that has infinite upgrades and stacking upgrades with reckless strike breaks the gold cost or whatever
            // well im sure it would but im just lazy to fix this rn
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}