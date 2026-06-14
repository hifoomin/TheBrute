using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Cards.Rares
{
    internal class Malice : TheBruteCard
    {
        public Malice() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        private int currentDamage = 16;

        private int damageDecrease = 4;

        [SavedProperty]
        public int CurrentDamage
        {
            get
            {
                return currentDamage;
            }
            set
            {
                AssertMutable();
                currentDamage = value;
                DynamicVars.Damage.BaseValue = currentDamage;
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1),
            new DamageVar(CurrentDamage, ValueProp.Move),
            new IntVar("DamageDecrease", 4m),
        ];

        [SavedProperty]
        public int DamageDecrease
        {
            get
            {
                return damageDecrease;
            }
            set
            {
                AssertMutable();
                damageDecrease = value;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
                .Execute(choiceContext);

            var damageDecreaseVar = DynamicVars["DamageDecrease"].IntValue;
            NerfFromPlay(damageDecreaseVar);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(6m);
        }

        protected override void AfterDowngraded()
        {
            UpdateCurrentValues();
        }

        private void NerfFromPlay(int damageDecreaseVar)
        {
            DamageDecrease += damageDecreaseVar;
            UpdateCurrentValues();
        }

        private void UpdateCurrentValues()
        {
            CurrentDamage = 16 - DamageDecrease;
        }
    }
}