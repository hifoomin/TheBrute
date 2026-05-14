using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Cards.Uncommons
{
    internal class Overlook : YokaCard
    {
        public Overlook() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new BlockVar("BlockNextTurn", 8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            BlockVar blockVar = (BlockVar)DynamicVars["BlockNextTurn"];
            IEnumerable<AbstractModel> modifiers;

            decimal blockNextTurnAmount = Hook.ModifyBlock(CombatState, Owner.Creature, blockVar.BaseValue, blockVar.Props, this, cardPlay, out modifiers);

            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, Owner.Creature, blockNextTurnAmount, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(2m);
            DynamicVars["BlockNextTurn"].UpgradeValueBy(2m);
        }
    }
}