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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Cards.Uncommons
{
    internal class Sacrifice : TheBruteCard
    {
        public Sacrifice() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
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

        protected override bool IsPlayable => CombatManager.Instance.History.CardPlaysFinished.LastOrDefault(delegate (CardPlayFinishedEntry e)
        {
            HashSet<PileType> fuckingGarbagePiles = [PileType.None, PileType.Exhaust, PileType.Deck];
            var isValid = e.CardPlay.Card.Owner == Owner && !fuckingGarbagePiles.Contains(e.CardPlay.Card.Pile.Type);
            bool what = isValid;
            if (what)
            {
                var cardType = e.CardPlay.Card.Type;
                var isAttackOrSkill = (uint)(cardType - 1) <= 1u;
                what = isAttackOrSkill;
            }
            return what;
        })?.CardPlay.Card != null;

        protected override bool ShouldGlowRedInternal => !IsPlayable;

        // holy fuck lol

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.maxHpRelated
        ]);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var lastUsedAttackOrSkill = CombatManager.Instance.History.CardPlaysFinished.LastOrDefault(delegate (CardPlayFinishedEntry e)
            {
                HashSet<PileType> fuckingGarbagePiles = [PileType.None, PileType.Exhaust, PileType.Deck];
                var isValid = e.CardPlay.Card.Owner == Owner && !fuckingGarbagePiles.Contains(e.CardPlay.Card.Pile.Type);
                bool what = isValid;
                if (what)
                {
                    var cardType = e.CardPlay.Card.Type;
                    var isAttackOrSkill = (uint)(cardType - 1) <= 1u;
                    what = isAttackOrSkill;
                }
                return what;
            })?.CardPlay.Card;

            if (lastUsedAttackOrSkill != null)
            {
                await CardCmd.Exhaust(choiceContext, lastUsedAttackOrSkill);
                await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.MaxHp.UpgradeValueBy(1m);
        }
    }
}