using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Cards
{
    public static class EmbarassTracker
    {
        public static readonly HashSet<Creature> ModifiedCreatures = new();
    }

    [HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.Damage), new Type[] { typeof(PlayerChoiceContext), typeof(IEnumerable<Creature>), typeof(decimal), typeof(ValueProp), typeof(Creature), typeof(CardModel) })]
    public static class EmbarassDamagePatch
    {
        private static bool _isDuplicating = false;

        [HarmonyPrefix]
        public static bool Prefix(PlayerChoiceContext choiceContext, IEnumerable<Creature> targets, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, ref Task<IEnumerable<DamageResult>> __result)
        {
            if (_isDuplicating) return true;
            if (dealer == null) return true;

            if (EmbarassTracker.ModifiedCreatures.Contains(dealer))
            {
                _isDuplicating = true;

                var strengthAmount = dealer.GetPowerAmount<StrengthPower>();

                var expectedModified = amount + strengthAmount;

                var targetPerHit = expectedModified / 2m;

                var newBase = targetPerHit - strengthAmount;

                int finalNewBase = Math.Max(0, (int)newBase);
                if (targetPerHit > 0 && finalNewBase <= 0)
                {
                    finalNewBase = 1;
                }

                __result = RunDuplicatedDamage(choiceContext, targets, (decimal)finalNewBase, props, dealer, cardSource);
                return false;
            }

            return true;
        }

        private static async Task<IEnumerable<DamageResult>> RunDuplicatedDamage(PlayerChoiceContext choiceContext, IEnumerable<Creature> targets, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            try
            {
                var combinedDamageResults = new List<DamageResult>();

                var damageResult1 = await CreatureCmd.Damage(choiceContext, targets, amount, props, dealer, cardSource);
                if (damageResult1 != null) combinedDamageResults.AddRange(damageResult1);

                var damageResult2 = await CreatureCmd.Damage(choiceContext, targets, amount, props, dealer, cardSource);
                if (damageResult2 != null) combinedDamageResults.AddRange(damageResult2);

                return combinedDamageResults;
            }
            finally
            {
                _isDuplicating = false;
            }
        }
    }
}