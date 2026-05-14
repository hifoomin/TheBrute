using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;

namespace Yoka.Cards.Rares
{
    internal class Fulminate : YokaCard
    {
        public Fulminate() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<TemporaryThornsPower>(),
            HoverTipFactory.FromPower<TemporaryThornsNextTurnPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<TemporaryThornsPower>(3m),
            new PowerVar<TemporaryThornsNextTurnPower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            var repeats = ResolveEnergyXValue();
            await PowerCmd.Apply<Powers.TemporaryThornsPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsPower"].BaseValue * repeats, Owner.Creature, this);
            await PowerCmd.Apply<Powers.TemporaryThornsNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsNextTurnPower"].BaseValue * repeats, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["TemporaryThornsPower"].UpgradeValueBy(1m);
            DynamicVars["TemporaryThornsNextTurnPower"].UpgradeValueBy(1m);
        }
    }
}