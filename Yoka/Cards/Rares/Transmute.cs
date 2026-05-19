using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Cards.Commons;
using Yoka.Powers;

namespace Yoka.Cards.Rares
{
    internal class Transmute : YokaCard
    {
        public Transmute() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];

        /*
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];
        */

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new HpLossVar(4)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), null, this))
            {
                card.BaseReplayCount++;

                if (!card.EnergyCost.CostsX && card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
                {
                    var vars = AccessTools.Field(typeof(DynamicVarSet), "_vars");

                    var cardVars = (Dictionary<string, DynamicVar>)vars.GetValue(card.DynamicVars);

                    if (cardVars.TryGetValue("HpLoss", out var existingVar) && existingVar is HpLossVar existingHpLoss)
                    {
                        cardVars["HpLoss"] = new HpLossVar(existingHpLoss.BaseValue + card.BaseReplayCount);
                    }
                    else
                    {
                        cardVars["HpLoss"] = new HpLossVar(DynamicVars.HpLoss.BaseValue);

                        if (!card.hasHpLossKeyword())
                        {
                            card.AddKeyword(Yoka.Keywords.hpLossKeyword);
                        }
                    }
                }
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}