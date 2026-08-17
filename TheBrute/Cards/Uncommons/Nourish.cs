#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Nourish : TheBruteCard
    {
        private CardModel? lastAttackOrSkill;

        public Nourish() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2m)
        ];

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

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

        // protected override bool IsPlayable => GetLastAttackOrSkill() != null;

        protected override bool ShouldGlowRedInternal => !IsPlayable;

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

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);

            lastAttackOrSkill = GetLastAttackOrSkill();

            if (lastAttackOrSkill == null)
            {
                return;
            }

            await CardCmd.Exhaust(choiceContext, lastAttackOrSkill);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.MaxHp.UpgradeValueBy(1m);
        }
    }
}