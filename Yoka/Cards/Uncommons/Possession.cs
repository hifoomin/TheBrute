using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;

namespace Yoka.Cards.Uncommons
{
    internal class Possession : YokaCard
    {
        public Possession() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new HealVar(4m),
            new GoldVar(12),
            new DynamicVar("VakuuNextTurn", 1m)
        ];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);

            await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);

            await PowerCmd.Apply<VakuuNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["VakuuNextTurn"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Heal.UpgradeValueBy(2m);
            DynamicVars.Gold.UpgradeValueBy(6m);
        }
    }
}