using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
using TheBrute.Cards;
using TheBrute.Powers;

namespace TheBrute.Cards.Rares
{
    internal class FirstClass : TheBruteCard
    {
        public FirstClass() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.goldRelated
        ]);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(0m),
            new ExtraDamageVar(1m),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, Creature? _)
            {
                var goldChangedThisCombat = GoldTracker.GetTotalChangedGoldThisCombat(card.Owner.Creature);

                return goldChangedThisCombat;
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx(null /*"vfx/vfx_attack_slash"*/)
            .Execute(choiceContext);

            VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_coin_explosion_regular");
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}