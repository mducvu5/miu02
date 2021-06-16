using Discord.Commands;
using NadekoBot.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nadeko.Calc;
using NadekoBot.Common.Attributes;

namespace NadekoBot.Modules.Utility
{
    public partial class Utility
    {
        [Group]
        public class CalcCommands : NadekoSubmodule
        {
            private static readonly Evaluator _calc = new Evaluator();
            
            [NadekoCommand, Usage, Description, Aliases]
            public async Task Calculate([Leftover] string expression)
            {
                var (succ, err) = _calc.TryEvaluate(expression, out var result);
                if (succ)
                    await ctx.Channel.SendConfirmAsync("⚙ " + GetText("result"), result.ToString(CultureInfo.InvariantCulture));
                else
                    await ctx.Channel.SendErrorAsync("⚙ " + GetText("error"), err);
            }

            [NadekoCommand, Usage, Description, Aliases]
            public async Task CalcOps()
            {
                var funcs = _calc.GetFunctions()
                    .Select(x => x.Key)
                    .JoinWith(", ");
                
                await ctx.Channel.SendConfirmAsync(GetText("calcops", Prefix), funcs);
            }
        }

        private class MethodInfoEqualityComparer : IEqualityComparer<MethodInfo>
        {
            public bool Equals(MethodInfo x, MethodInfo y) => x.Name == y.Name;

            public int GetHashCode(MethodInfo obj) => obj.Name.GetHashCode(StringComparison.InvariantCulture);
        }
    }
}