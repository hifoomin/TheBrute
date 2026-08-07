using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Powers
{
    internal class HysteriaPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.ForEnergy(this)
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(0m),
            new GoldVar(0)
        ];

        private bool procLossesNextTurn = false;

        public void SetLossAmounts(decimal maxHpLoss, int goldLoss)
        {
            AssertMutable();
            DynamicVars.MaxHp.BaseValue = maxHpLoss;
            DynamicVars.Gold.BaseValue = goldLoss;
            // Main.Logger.Warn("SETTING HP LOSS AMOUNT TO " + maxHpLoss + " AND GOLD LOSS AMOUNT TO " + goldLoss);
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != Owner.Player)
            {
                // Main.Logger.Warn("PLAYER DOESNT HAVE POWER");
                return;
            }

            if (player.PlayerCombatState == null || player.PlayerCombatState.TurnNumber <= 1)
            {
                // Main.Logger.Warn("PLAYER COMBAT STATE IS NULL OR TURN NUMBER IS <= 1");
                return;
            }

            if (procLossesNextTurn)
            {
                // Main.Logger.Warn("PROC LOSSES IS TRUE, REMOVING MAX HP AND GOLD ! !!! THEN SETTING IT TO FALSE");
                Flash();
                await CreatureCmd.LoseMaxHp(choiceContext, Owner, DynamicVars.MaxHp.BaseValue, true);
                await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner.Player);
                procLossesNextTurn = false;
            }
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (participants.Contains(Owner))
            {
                procLossesNextTurn = false;
                if (Owner.Player != null && Owner.Player.PlayerCombatState != null && Owner.Player.PlayerCombatState.Energy <= 0)
                {
                    // Main.Logger.Warn("THIS MOTHERFUCKER JUST SPEND ALL THEIR ENERGY, SETTING PROC LOSSES NEXT TURN TO TRUE");
                    Flash();
                    procLossesNextTurn = true;
                }
            }
        }

        public override decimal ModifyMaxEnergy(Player player, decimal amount)
        {
            if (player != Owner.Player)
            {
                return amount;
            }
            return amount + (decimal)Amount;
        }
    }
}