using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Gnaw : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Gnaw() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        private decimal _extraEverythingFromGnawPlays;

        private decimal ExtraEverythingFromGnawPlays
        {
            get
            {
                return _extraEverythingFromGnawPlays;
            }
            set
            {
                AssertMutable();
                _extraEverythingFromGnawPlays = value;
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(4m, ValueProp.Move),
            new RepeatVar(2),
            new CardsVar(1),
            new DynamicVar("EverythingIncrease", 1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitCount(DynamicVars.Repeat.IntValue)
            .WithHitFx("vfx/vfx_bite", null, "blunt_attack.mp3")
            .Execute(choiceContext);

            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

            var increase = DynamicVars["EverythingIncrease"].BaseValue;
            this.BuffFromGnawPlay(increase);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Repeat.UpgradeValueBy(1);
        }

        protected override void AfterDowngraded()
        {
            base.AfterDowngraded();

            EnergyCost.AddThisCombat((int)ExtraEverythingFromGnawPlays);

            DynamicVars.Damage.BaseValue += ExtraEverythingFromGnawPlays;
            DynamicVars.Repeat.BaseValue += ExtraEverythingFromGnawPlays;

            DynamicVars.Cards.BaseValue += ExtraEverythingFromGnawPlays;
        }

        private void BuffFromGnawPlay(decimal extraDamage)
        {
            EnergyCost.AddThisCombat((int)extraDamage);

            DynamicVars.Damage.BaseValue += extraDamage;
            DynamicVars.Repeat.BaseValue += extraDamage;

            DynamicVars.Cards.BaseValue += extraDamage;

            ExtraEverythingFromGnawPlays += extraDamage;
        }
    }
}