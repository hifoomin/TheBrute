using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Cards;
using Yoka.Powers;

namespace Yoka.Cards.Uncommons
{
    internal class BloodCoagulant : YokaCard
    {
        public BloodCoagulant() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>(),
            HoverTipFactory.FromPower<TemporaryThornsPower>(),
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new HpLossVar(1m),
            new PowerVar<ThornsPower>(3m),
            new PowerVar<TemporaryThornsPower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<TemporaryThornsPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ThornsPower"].UpgradeValueBy(1m);
            DynamicVars["TemporaryThornsPower"].UpgradeValueBy(1m);
        }
    }
}