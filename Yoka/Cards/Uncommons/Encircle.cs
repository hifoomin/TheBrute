using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;

namespace Yoka.Cards.Uncommons
{
    internal class Encircle : YokaCard
    {
        public Encircle() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<EncirclePower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(3m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered),
            new PowerVar<Powers.EncirclePower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<Powers.EncirclePower>(choiceContext, Owner.Creature, DynamicVars["EncirclePower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }
    }
}