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
    internal class HostileTakeover : YokaCard
    {
        public HostileTakeover() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<StrengthPower>()];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<WeakPower>(3m),
            new PowerVar<StrengthPower>(1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_flying_slash");
            foreach (Creature enemy in CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<WeakPower>(choiceContext, enemy, DynamicVars.Weak.BaseValue, Owner.Creature, this);
                await PowerCmd.Apply<StrengthPower>(choiceContext, enemy, -DynamicVars.Strength.BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Weak.UpgradeValueBy(2m);
        }
    }
}