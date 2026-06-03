using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards.Tokens;

namespace TheBrute.Cards.Uncommons
{
    internal class Nourish : TheBruteCard
    {
        public Nourish() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2m)
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.maxHpRelated
        ]);

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        private CardModel? lastAttackOrSkill;

        private CardModel? GetLastAttackOrSkill()
        {
            HashSet<PileType> fuckingGarbagePiles =
            [
                PileType.None,
                PileType.Exhaust,
                PileType.Deck
            ];

            return CombatManager.Instance.History.CardPlaysFinished
                   .Select(e => e.CardPlay.Card)
                   .LastOrDefault(c =>
                   c.Owner == Owner &&
                   c.Pile != null &&
                   !fuckingGarbagePiles.Contains(c.Pile.Type) &&
                   (c.Type == CardType.Attack || c.Type == CardType.Skill));
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips
        {
            get
            {
                var lastAttackOrSkill = GetLastAttackOrSkill();
                if (lastAttackOrSkill == null)
                {
                    return [];
                }

                // var canonical = lastAttackOrSkill.CanonicalInstance;

                List<IHoverTip> hoverTips = new();
                hoverTips.Add(HoverTipFactory.FromCard(lastAttackOrSkill, lastAttackOrSkill.IsUpgraded));
                // hoverTips.AddRange(canonical.HoverTips);

                // I'll settle for this for now, this creates confusion because it can display more than one card if lastAttackOrSkill has a FromCard hover tip as well
                // the downside of not having this is that lastAttackOrSkill's hover tips won't show but oh well, maybe I';ll revisit it  later ,, ,

                return hoverTips;
            }
        }

        protected override bool IsPlayable => GetLastAttackOrSkill() != null;

        protected override bool ShouldGlowRedInternal => !IsPlayable;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            lastAttackOrSkill = GetLastAttackOrSkill();

            if (lastAttackOrSkill == null)
            {
                return;
            }

            await CardCmd.Exhaust(choiceContext, lastAttackOrSkill);
            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.MaxHp.UpgradeValueBy(1m);
        }
    }
}