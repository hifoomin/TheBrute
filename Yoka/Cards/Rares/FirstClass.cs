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

namespace Yoka.Cards.Rares
{
    internal class FirstClass : YokaCard
    {
        public FirstClass() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            Yoka.Cards.Tags.goldRelated
        ]);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(0m, ValueProp.Move),
            new CalculationBaseVar(0m),
            new CalculationExtraVar(0.12m),
            new CalculatedVar("GoldDamage").WithMultiplier((card, _) =>
            {
                // return Math.Ceiling(card.Owner.Gold * 0.15m);
                return card.Owner.Gold;
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            var damage = DynamicVars.Damage.BaseValue + ((CalculatedVar)DynamicVars["GoldDamage"]).Calculate(Owner.Creature);
            await DamageCmd.Attack(damage).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx(null /*"vfx/vfx_attack_slash"*/)
            .Execute(choiceContext);
            VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_coin_explosion_regular");
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationExtra.UpgradeValueBy(0.04m);
        }
    }
}