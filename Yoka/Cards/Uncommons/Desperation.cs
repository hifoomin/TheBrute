using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.OpenXRCompositionLayer;

namespace Yoka.Cards.Uncommons
{
    internal class Desperation : YokaCard
    {
        public Desperation() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);

            IEnumerable<CardModel> enumerable = PileType.Discard.GetPile(base.Owner).Cards.Where(Filter).ToList();
            foreach (CardModel item in enumerable)
            {
                await CardPileCmd.Add(item, PileType.Hand);
            }
        }

        private bool Filter(CardModel card)
        {
            var isZeroCostCard = card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !card.EnergyCost.CostsX;
            bool what = isZeroCostCard;
            if (what)
            {
                CardType type = card.Type;
                bool what2 = (uint)(type - 1) <= 2u;
                what = what2;
            }
            return what;
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}