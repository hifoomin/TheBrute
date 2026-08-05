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
using TheBrute.Powers;

namespace TheBrute.Cards.Rares
{
    internal class Fulminate : TheBruteCard
    {
        public Fulminate() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ThornsPower>(3m),
            new PowerVar<TemporaryThornsUpPower>(3m),
            new PowerVar<TemporaryThornsUpNextTurnPower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            var repeats = ResolveEnergyXValue();
            for (int i = 0; i < repeats; i++)
            {
                await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
                await PowerCmd.Apply<Powers.TemporaryThornsUpPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsUpPower"].BaseValue, Owner.Creature, this);
                await PowerCmd.Apply<Powers.TemporaryThornsUpNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsUpNextTurnPower"].BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ThornsPower"].UpgradeValueBy(1m);
            DynamicVars["TemporaryThornsUpPower"].UpgradeValueBy(1m);
            DynamicVars["TemporaryThornsUpNextTurnPower"].UpgradeValueBy(1m);
        }
    }
}