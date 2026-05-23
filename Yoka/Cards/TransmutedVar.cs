using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Cards
{
    public sealed class TransmutedVar : DynamicVar
    {
        public const string DefaultName = "Transmuted";

        public TransmutedVar(decimal amount) : base(DefaultName, amount)
        {
            this.WithTooltip();
        }

        public TransmutedVar(string name, decimal amount) : base(name, amount)
        {
            this.WithTooltip();
        }
    }

    public static class TransmutedVarDynamicVarSetExtensions
    {
        public static DynamicVar Transmuted(this DynamicVarSet dynamicVars)
        {
            return dynamicVars[TransmutedVar.DefaultName];
        }
    }
}