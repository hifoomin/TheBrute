/*
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Localization.Formatters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards.Uncommons;
using TheBrute.Relics;

// fops
namespace TheBrute.Relics.Shops
{
#pragma warning disable STS001 // Symbol missing localization

    internal class FesteringHunger : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Shop;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [

        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(2)
        ];

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner.Creature))
            {
                CardModel[] cards = [ModelDb.Card<Gnaw>(), ModelDb.Card<Gnaw>()];
                await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner);
            }
        }
    }
}
*/