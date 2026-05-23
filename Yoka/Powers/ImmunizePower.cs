using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Powers
{
    internal class ImmunizePower : YokaPower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        private int lastMaxHP;

        public override async Task BeforeCardPlayed(CardPlay cardPlay)
        {
            lastMaxHP = Owner.MaxHp;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner == Owner.Player && lastMaxHP > Owner.MaxHp)
            {
                var alivePlayersExcludingPowerOwner = from c in CombatState.GetTeammatesOf(Owner)
                                                      where c != null && c.IsAlive && c.IsPlayer
                                                      && c != Owner
                                                      select c;

                foreach (Creature player in alivePlayersExcludingPowerOwner)
                {
                    await CreatureCmd.GainMaxHp(player, Amount);
                }
            }
        }
    }
}