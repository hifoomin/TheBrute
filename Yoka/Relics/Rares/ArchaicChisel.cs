using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Relics;

namespace Yoka.Relics.Rares
{
    internal class ArchaicChisel : YokaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        public override async Task AfterRestSiteSmith(Player player)
        {
            if (player != Owner)
            {
                return;
            }

            var deck = player.Deck;

            for (int i = 0; i < 30; i++)
            {
                Main.Logger.Warn("trying attempt");

                var randomCard = player.PlayerRng.Rewards.NextItem(deck.Cards);
                if (randomCard == null)
                {
                    Main.Logger.Warn("random card is null");
                    continue;
                }

                var invalidIds = new HashSet<string>
                {
                    "ENCHANTMENT.DEPRECATED_ENCHANTMENT",
                    "ENCHANTMENT.MOCK_FREE_ENCHANTMENT",
                    "ENCHANTMENT.SLUMBERING_ESSENCE"
                };

                var validEnchantments = ModelDb.DebugEnchantments.Where(x => !invalidIds.Contains(x.Id.ToString()));

                var randomEnchantment = player.PlayerRng.Rewards.NextItem(ModelDb.DebugEnchantments);
                if (randomEnchantment == null)
                {
                    continue;
                }

                if (randomEnchantment.CanEnchant(randomCard))
                {
                    var mutableEnchantment = randomEnchantment.ToMutable();
                    Main.Logger.Warn("successfully enchanted");
                    Flash();
                    CardCmd.Enchant(mutableEnchantment, randomCard, 1);
                    CardCmd.Preview(randomCard, 2.4f);
                    break;
                }
            }
        }
    }
}