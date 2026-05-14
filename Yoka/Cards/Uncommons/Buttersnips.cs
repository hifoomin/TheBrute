using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;

namespace Yoka.Cards.Uncommons
{
    internal class Buttersnips : YokaCard
    {
        public Buttersnips() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ThornsPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            // 5 at base + 2 per each thorns, so 9 at base with starting relic, 17 with bristle (pretty goog but no block)
            // upgraded, it does 10 at base + 2 per each thorns, so 14 at base with starting relic, 22 with bristle, extremely good
            new CalculationBaseVar(5m),
            new ExtraDamageVar(2m),
            new CalculatedDamageVar(MegaCrit.Sts2.Core.ValueProps.ValueProp.Move).WithMultiplier((CardModel card, Creature? _) =>
            {
                /*
                int totalThornsPowers = 0;
                var cachedUniqueThornsPowers = card.Owner.Creature.Powers.Where(x => x.Description.)
                for (int i = 0; i < )
                totalThornsPowers += card.Owner.Creature.GetPowerAmount<>
                // wanted to get all powers with a description tolower that contains thorns for mod compatiblity, but maybe not for now lol (also idk how to filter it by language and such)
                */
                return card.Owner.Creature.GetPowerAmount<ThornsPower>() + card.Owner.Creature.GetPowerAmount<TemporaryThornsPower>();
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).TargetingAllOpponents(CombatState)
                .WithHitFx(null /*"vfx/vfx_giant_horizontal_slash"*/)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationBase.UpgradeValueBy(5m);
        }
    }
}