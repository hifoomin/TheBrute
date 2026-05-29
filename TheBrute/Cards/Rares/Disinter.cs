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

namespace TheBrute.Cards.Rares
{
    internal class Disinter : TheBruteCard
    {
        public Disinter() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        private int currentGoldLoss = 7;
        private int currentMaxHpGain = 1;

        private int increasedGoldLoss;
        private int increasedMaxHpGain;

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

        [SavedProperty]
        public int CurrentMaxHpGain
        {
            get
            {
                return currentMaxHpGain;
            }
            set
            {
                AssertMutable();
                currentMaxHpGain = value;
                DynamicVars.MaxHp.BaseValue = currentMaxHpGain;
            }
        }

        // protected override bool ShouldGlowRedInternal => Utils.HasGold(Owner, DynamicVars.Gold.IntValue);
        // fucking piece of shit garbage trump code

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(CurrentMaxHpGain),
            new GoldVar(CurrentGoldLoss),
            new IntVar("GoldCostIncrease", 7m),
            new IntVar("MaxHpGainIncrease", 1m)
        ];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.maxHpRelated, TheBrute.Cards.Tags.goldRelated
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

        [SavedProperty]
        public int IncreasedMaxHpGain
        {
            get
            {
                return increasedMaxHpGain;
            }
            set
            {
                AssertMutable();
                increasedMaxHpGain = value;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Utils.HasGold(Owner, DynamicVars.Gold.IntValue))
            {
                await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner);

                await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);

                var goldCostIncrease = DynamicVars["GoldCostIncrease"].IntValue;
                var maxHpGainIncrease = DynamicVars["MaxHpGainIncrease"].IntValue;
                BuffFromPlay(goldCostIncrease, maxHpGainIncrease);
                (DeckVersion as Disinter)?.BuffFromPlay(goldCostIncrease, maxHpGainIncrease);
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }

        protected override void AfterDowngraded()
        {
            UpdateCurrentValues();
        }

        private void BuffFromPlay(int extraGoldLoss, int extraMaxHpGain)
        {
            IncreasedGoldLoss += extraGoldLoss;
            IncreasedMaxHpGain += extraMaxHpGain;
            UpdateCurrentValues();
        }

        private void UpdateCurrentValues()
        {
            CurrentGoldLoss = 7 + IncreasedGoldLoss;
            CurrentMaxHpGain = 1 + IncreasedMaxHpGain;
        }
    }
}