using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Powers
{
#pragma warning disable STS001 // Symbol missing localization

    internal class TemporaryThornsNextTurnPower : YokaPower
#pragma warning restore STS001 // Symbol missing localization
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player == Owner.Player && AmountOnTurnStart != 0)
            {
                await PowerCmd.Apply<TemporaryThornsPower>(choiceContext, player.Creature, Amount, player.Creature, null);
                await PowerCmd.Remove(this);
            }
        }
    }
}