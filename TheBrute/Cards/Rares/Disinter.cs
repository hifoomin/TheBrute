#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Disinter : TheBruteCard
    {
        private int currentGoldLoss = 6;
        private int currentMaxHpGain = 1;

        private int increasedGoldLoss;
        private int increasedMaxHpGain;

        public Disinter() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override bool CanBeGeneratedInCombat => false;

        [SavedProperty]
        public int CurrentGoldLoss
        {
            get => currentGoldLoss;
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
            get => currentMaxHpGain;
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
            new IntVar("GoldCostIncrease", 6m),
            new IntVar("MaxHpGainIncrease", 1m)
        ];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        [SavedProperty]
        public int IncreasedGoldLoss
        {
            get => increasedGoldLoss;
            set
            {
                AssertMutable();
                increasedGoldLoss = value;
            }
        }

        [SavedProperty]
        public int IncreasedMaxHpGain
        {
            get => increasedMaxHpGain;
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
            CurrentGoldLoss = 6 + IncreasedGoldLoss;
            CurrentMaxHpGain = 1 + IncreasedMaxHpGain;
        }
    }
}