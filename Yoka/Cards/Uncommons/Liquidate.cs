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
using Yoka.Cards;

namespace Yoka.Cards.Uncommons
{
    internal class Liquidate : YokaCard
    {
        public Liquidate() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new EnergyVar(2),
            new CardsVar(2),
            new GoldVar(12)
        ];

        protected override bool ShouldGlowRedInternal => !Utils.HasGold(Owner, 12);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Utils.HasGold(Owner, 12))
            {
                await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
                await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
                VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_coin_explosion_regular");
                await PlayerCmd.LoseGold(DynamicVars["Gold"].IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(1m);
        }
    }
}