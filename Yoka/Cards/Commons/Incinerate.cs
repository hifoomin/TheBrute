using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;
using Yoka.Cards;

namespace Yoka.Cards.Commons
{
    internal class Incinerate : YokaCard
    {
        public Incinerate() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(14m, ValueProp.Move),
            new GoldVar(5)
        ];

        protected override bool ShouldGlowRedInternal => !Utils.HasGold(Owner, 5);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            if (Utils.HasGold(Owner, 5))
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx(null /*"vfx/vfx_attack_slash"*/)
                .Execute(choiceContext);
                VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_coin_explosion_regular");
                await PlayerCmd.LoseGold(DynamicVars["Gold"].IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(7m);
        }
    }
}