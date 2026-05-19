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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Powers
{
    internal class HysteriaPower : YokaPower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(3)
        ];

        public override decimal ModifyMaxEnergy(Player player, decimal amount)
        {
            if (player != Owner.Player)
            {
                return amount;
            }
            return amount + (decimal)Amount;
        }

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            NFireBurningVfx child = NFireBurningVfx.Create(Owner.Player.Creature, 1f, goingRight: false);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);

            for (int i = 0; i < DynamicVars.Cards.BaseValue; i++)
            {
                CardModel card = CombatState.CreateCard<Burn>(Owner.Player);
                CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, Owner.Player));
            }
            await Cmd.Wait(0.5f);
        }
    }
}