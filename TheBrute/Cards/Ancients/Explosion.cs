using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
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
using TheBrute.Cards;
using TheBrute.Powers;

namespace TheBrute.Cards.Ancients
{
    internal class Explosion : TheBruteCard, ITomeCard
    {
        public Explosion() : base(2, CardType.Power, CardRarity.Ancient, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("ExtraAsPercent", 10m),
            new CalculationBaseVar(0m),
            new ExtraDamageVar(0.1m),
            new CalculatedDamageVar(MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered).WithMultiplier((CardModel card, Creature? _) =>
            {
                return card.Owner.Creature.MaxHp;
            }),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<ExplosionPower>(choiceContext, Owner.Creature, DynamicVars.ExtraDamage.BaseValue * 100m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(0.05m);
            DynamicVars["ExtraAsPercent"].UpgradeValueBy(5m);
        }
    }
}