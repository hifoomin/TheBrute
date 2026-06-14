using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards.Ancients;
using TheBrute.Cards.Starters;
using TheBrute.Relics.Uncommons;

namespace TheBrute.Cards
{
    public static class Utils
    {
        public static bool HasGold(Player owner, int amount)
        {
            return owner.Gold >= amount;
        }
    }
}