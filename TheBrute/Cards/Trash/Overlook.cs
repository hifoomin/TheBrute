#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheBrute.Character;

#endregion

namespace TheBrute.Cards.Trash
{
    [Pool(typeof(EventCardPool))]
    internal class Overlook : TheBruteCard
    {
        public Overlook() : base(2, CardType.Skill, CardRarity.Event, TargetType.Self)
        {
        }

        public override CardPoolModel VisualCardPool => ModelDb.CardPool<TheBruteCardPool>();

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(9m, ValueProp.Move),
            new BlockVar("BlockNextTurn", 9m, ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            var blockVar = (BlockVar)DynamicVars["BlockNextTurn"];
            IEnumerable<AbstractModel> modifiers;

            var blockNextTurnAmount = Hook.ModifyBlock(CombatState, Owner.Creature, blockVar.BaseValue, blockVar.Props, this, cardPlay, out modifiers);

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