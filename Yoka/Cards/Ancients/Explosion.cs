/*
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Cards;
using Yoka.Powers;

namespace Yoka.Cards.Ancients
{
    internal class Explosion : YokaCard, ITomeCard
    {
        public Explosion() : base(2, CardType.Power, CardRarity.Ancient, TargetType.AllEnemies)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        public bool Show =>

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ExplosionPower>(1m),
            new CalculationBaseVar(0m),
            new ExtraDamageVar(1m),
            new CalculatedDamageVar(MegaCrit.Sts2.Core.ValueProps.ValueProp.Move).WithMultiplier((CardModel card, Creature? _) =>
            {
                return Utils.TookUnblockedDamageCount(card.Owner) + GoldLostThisCombatTracker.Get(card.Owner.Creature);
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<ExplosionPower>(choiceContext, Owner.Creature, DynamicVars["ExplosionPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(1m);
            DynamicVars["ExplosionPower"].UpgradeValueBy(1m);
        }
    }
}
*/