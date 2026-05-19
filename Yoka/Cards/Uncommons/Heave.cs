using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;

namespace Yoka.Cards.Uncommons
{
    internal class Heave : YokaCard
    {
        public Heave() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1),
            new DamageVar(3, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var repeats = ResolveEnergyXValue();
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue * repeats, Owner);

            VfxCmd.PlayOnCreatureCenters(CombatState.HittableEnemies, "vfx/vfx_attack_slash");
            SfxCmd.Play("slash_attack.mp3");

            var cardsInHand = PileType.Hand.GetPile(Owner).Cards.Count;
            for (int i = 0; i < cardsInHand; i++)
            {
                var hittableEnemies = CombatManager.Instance._state?.HittableEnemies;
                if (hittableEnemies == null || hittableEnemies.Count <= 0)
                {
                    continue;
                }

                var randomEnemy = Owner.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                if (randomEnemy == null)
                {
                    continue;
                }

                await CreatureCmd.Damage(choiceContext, randomEnemy, DynamicVars.Damage.BaseValue, ValueProp.Move, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
        }
    }
}