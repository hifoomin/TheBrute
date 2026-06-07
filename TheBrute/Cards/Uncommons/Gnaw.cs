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
        public Gnaw() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        private decimal _extraDamageFromGnawPlays;

        private decimal ExtraDamageFromGnawPlays
        {
            get
            {
                return _extraDamageFromGnawPlays;
            }
            set
            {
                AssertMutable();
                _extraDamageFromGnawPlays = value;
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(1m, ValueProp.Move),
            new DynamicVar("DamageIncrease", 1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_bite", null, "blunt_attack.mp3")
            .Execute(choiceContext);

            var card = CreateClone();
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, Owner), 1.6f);

            var gnaws = Owner.PlayerCombatState.AllCards.OfType<Gnaw>();
            decimal baseValue = DynamicVars["DamageIncrease"].BaseValue;
            foreach (var gnaw in gnaws)
            {
                gnaw.BuffFromGnawPlay(baseValue);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
        }

        protected override void AfterDowngraded()
        {
            base.AfterDowngraded();
            DynamicVars.Damage.BaseValue += ExtraDamageFromGnawPlays;
        }

        private void BuffFromGnawPlay(decimal extraDamage)
        {
            DynamicVars.Damage.BaseValue += extraDamage;
            ExtraDamageFromGnawPlays += extraDamage;
        }
    }
}