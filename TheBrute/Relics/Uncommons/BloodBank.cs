using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Powers;
using TheBrute.Relics;

namespace TheBrute.Relics.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class BloodBank : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override bool ShowCounter => true;

        private int currentCounter;

        public int CurrentCounter
        {
            get => currentCounter;
            set
            {
                if (currentCounter == value)
                {
                    return;
                }

                currentCounter = value;
                InvokeDisplayAmountChanged();
            }
        }

        public override int DisplayAmount => CurrentCounter;

        public override RelicRarity Rarity => RelicRarity.Uncommon;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(6m),
            new CardsVar(1)
        ];

        // logic is handled in MaxHpTrackerLoseMaxHpPatch
    }
}