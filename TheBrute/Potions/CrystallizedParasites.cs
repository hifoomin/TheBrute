#region

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using TheBrute.Powers;

#endregion

namespace TheBrute.Potions
{
    internal class CrystallizedParasites : TheBrutePotion
    {
        public override PotionRarity Rarity => PotionRarity.Uncommon;

        public override PotionUsage Usage => PotionUsage.CombatOnly;

        public override TargetType TargetType => TargetType.AnyPlayer;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ThornsPower>(8m),
            new PowerVar<TemporaryThornsUpPower>(8m)
        ];

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            AssertValidForTargetedPotion(target);
            NCombatRoom.Instance?.PlaySplashVfx(target, new Color("19be73"));

            await PowerCmd.Apply<ThornsPower>(choiceContext, target, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, null);
            await PowerCmd.Apply<TemporaryThornsUpPower>(choiceContext, target, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, null);
        }
    }
}