using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards;

namespace TheBrute.Cards.Uncommons
{
    internal class Liquidate : TheBruteCard
    {
        public Liquidate() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        /*
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];
        */

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<EnergyNextTurnPower>(2m),
            new GoldVar(8)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            EnergyHoverTip
        ];

        protected override bool ShouldGlowRedInternal => !Utils.HasGold(Owner, DynamicVars.Gold.IntValue);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Utils.HasGold(Owner, DynamicVars.Gold.IntValue))
            {
                await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

                await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["EnergyNextTurnPower"].BaseValue, Owner.Creature, this);

                VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_coin_explosion_regular");

                await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["EnergyNextTurnPower"].UpgradeValueBy(1m);
        }
    }
}