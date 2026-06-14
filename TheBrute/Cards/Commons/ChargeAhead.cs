using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheBrute.Cards;
using TheBrute.Powers;

namespace TheBrute.Cards.Commons
{
    internal class ChargeAhead : TheBruteCard
    {
        public ChargeAhead() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(10m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new PowerVar<TemporaryThornsUpNextTurnPower>(3m)
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.thornsRelated
        ]);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx(null)
                .Execute(choiceContext);

            await PowerCmd.Apply<TemporaryThornsUpNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsUpNextTurnPower"].IntValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
            DynamicVars["TemporaryThornsUpNextTurnPower"].UpgradeValueBy(2m);
        }
    }
}