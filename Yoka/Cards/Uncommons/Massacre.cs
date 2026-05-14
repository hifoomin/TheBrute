using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Cards;

namespace Yoka.Cards.Uncommons
{
    internal class Massacre : YokaCard
    {
        public Massacre() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
        {
        }

        private int lastGold;

        private bool changedGoldThisTurn => Owner.Gold != lastGold;

        protected override bool ShouldGlowGoldInternal => Owner.Gold != lastGold;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(14m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var hitCount = changedGoldThisTurn ? 2 : 1;
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState).WithHitCount(hitCount)
                .WithHitFx(null /*"vfx/vfx_giant_horizontal_slash"*/)
                .Execute(choiceContext);
        }

        public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
        {
            lastGold = Owner.Gold;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(5m);
        }
    }
}