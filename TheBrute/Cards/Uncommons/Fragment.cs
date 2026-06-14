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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards;

namespace TheBrute.Cards.Uncommons
{
    internal class Fragment : TheBruteCard
    {
        public Fragment() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<PlatingPower>(),
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.thornsRelated
        ]);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<PlatingPower>(4m),
            new PowerVar<ThornsPower>(2m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars["PlatingPower"].BaseValue, Owner.Creature, this);

            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ThornsPower"].UpgradeValueBy(2m);
        }
    }
}