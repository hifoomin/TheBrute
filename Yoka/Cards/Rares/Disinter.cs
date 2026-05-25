using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Cards.Rares
{
    internal class Disinter : YokaCard
    {
        public Disinter() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        private int currentGoldLoss = 1;

        private int increasedGoldLoss;

        [SavedProperty]
        public int CurrentGoldLoss
        {
            get
            {
                return currentGoldLoss;
            }
            set
            {
                AssertMutable();
                currentGoldLoss = value;
                DynamicVars.Gold.BaseValue = currentGoldLoss;
            }
        }

        protected override bool ShouldGlowRedInternal => Utils.HasGold(Owner, DynamicVars.Gold.IntValue);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(1m),
            new GoldVar(CurrentGoldLoss),
            new IntVar("Increase", 2m)
        ];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            Yoka.Cards.Tags.maxHpRelated, Yoka.Cards.Tags.goldRelated
        ]);

        [SavedProperty]
        public int IncreasedGoldLoss
        {
            get
            {
                return increasedGoldLoss;
            }
            set
            {
                AssertMutable();
                increasedGoldLoss = value;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Utils.HasGold(Owner, DynamicVars.Gold.IntValue))
            {
                await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner);

                int intValue = base.DynamicVars["Increase"].IntValue;
                BuffFromPlay(intValue);
                (base.DeckVersion as Disinter)?.BuffFromPlay(intValue);

                CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }

        protected override void AfterDowngraded()
        {
            UpdateGoldLoss();
        }

        private void BuffFromPlay(int extraGoldLoss)
        {
            IncreasedGoldLoss += extraGoldLoss;
            UpdateGoldLoss();
        }

        private void UpdateGoldLoss()
        {
            CurrentGoldLoss = 1 + IncreasedGoldLoss;
        }
    }
}