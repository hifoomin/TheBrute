#region

using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

#endregion

namespace TheBrute.Powers
{
    internal class LeveragePower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];
    }

    /*
    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Powers.ThornsPower), "BeforeDamageReceived")]
    internal class LeveragePowerBeforeDamageReceivedPatch
    {
        private static void Prefix(ref decimal amount, PlayerChoiceContext choiceContext, Creature target, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            var leveragePowerAmount = target.GetPowerAmount<LeveragePower>();

            if (dealer?.Monster?.NextMove is MoveState currentIntent && leveragePowerAmount > 0)
            {
                var hasSingleHitAttack = currentIntent.Intents.Any(intent => intent is AttackIntent && intent is not MultiAttackIntent);

                if (hasSingleHitAttack)
                {
                    Main.Logger.Warn($"Before: {amount}");

                    amount *= leveragePowerAmount / 100m;

                    Main.Logger.Warn($"After: {amount}");

                    KURWA GOWNO ZJEBANE JEBANA GRA KURWa KLASYK

    I OCZYwISCIEK URWA NIE MOZNA TEGO ZROBIC W POWER W BEFORE DAMAG ERECEIVED ANI AFTER BO NIE MA JAK ROZORNIC CZY TO DAMAGE Z THORNS BO GRA JEST KURWA ZROBIONA PRZEZ JEBANYCH ZJEBOW BEZMOZGICH KTORZY NIE KOPIOWALI O WIELE LEPSZEGO KODU Z ROR2
    KURWA NAWET JEST GOSCIU CO WYGLADA JAK LUIGI MANGIONE ALE PEWNIE MA ROOM TEMP IQ I CHUJ
                }
            }
        }
    }
    */

    [HarmonyPatch]
    internal static class ThornsPowerPatch
    {
        private static MethodBase TargetMethod()
        {
            var beforeDamageReceived = AccessTools.Method(typeof(ThornsPower), nameof(ThornsPower.BeforeDamageReceived));

            var stateMachine = beforeDamageReceived!.GetCustomAttribute<AsyncStateMachineAttribute>()!.StateMachineType;

            return AccessTools.Method(stateMachine, "MoveNext")!;
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var codes = new List<CodeInstruction>(instructions);
            var stateMachine = TargetMethod().DeclaringType!;

            var thisField = AccessTools.Field(stateMachine, "<>4__this")!;
            var dealer = AccessTools.Field(stateMachine, "dealer")!;

            var getAmount = AccessTools.Method(typeof(PowerModel), "get_Amount")!;

            var decimalImplicit = AccessTools.Method(typeof(decimal), "op_Implicit", new[] { typeof(int) })!;

            var modifyThornsDamage = AccessTools.Method(typeof(ThornsPowerPatch), nameof(ModifyThornsDamage))!;

            var amountLocal = il.DeclareLocal(typeof(decimal));

            for (var i = 0; i < codes.Count - 1; i++)
            {
                if (!codes[i].Calls(getAmount) || !codes[i + 1].Calls(decimalImplicit))
                {
                    continue;
                }

                codes.InsertRange(i + 2, new[]
                {
                    new CodeInstruction(OpCodes.Stloc, amountLocal),
                    new CodeInstruction(OpCodes.Ldloc, amountLocal),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, thisField),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, dealer),
                    new CodeInstruction(OpCodes.Call, modifyThornsDamage)
                });

                return codes;
            }

            throw new Exception("KURWA, leveragepower transpile nie zadzialal i chuj no i kurwa tyle godzin na to poswiecilem i chuj ktos zepsul xD");
        }

        private static decimal ModifyThornsDamage(decimal amount, ThornsPower thornsPower, Creature? dealer)
        {
            var leveragePowerAmount = thornsPower.Owner.GetPowerAmount<LeveragePower>();

            if (leveragePowerAmount <= 0)
            {
                return amount;
            }

            if (dealer?.Monster?.NextMove is not MoveState currentIntent)
            {
                return amount;
            }

            var hasSingleHitAttack = currentIntent.Intents.Any(intent => intent is AttackIntent && intent is not MultiAttackIntent);

            if (!hasSingleHitAttack)
            {
                return amount;
            }

            return amount * (leveragePowerAmount / 100m);
        }
    }
}