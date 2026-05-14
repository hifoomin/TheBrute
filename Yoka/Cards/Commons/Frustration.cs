using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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

namespace Yoka.Cards.Commons
{
    internal class Frustration : YokaCard
    {
        public Frustration() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
        {
        }

        protected override bool ShouldGlowGoldInternal => Utils.TookUnblockedDamageLastTurn(Owner);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(8m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new PowerVar<StrengthPower>(1m),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
                .WithHitFx(null /*"vfx/vfx_giant_horizontal_slash"*/)
                .Execute(choiceContext);

            if (Utils.TookUnblockedDamageLastTurn(Owner))
            {
                await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}