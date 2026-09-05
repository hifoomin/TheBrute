#region

using System.Diagnostics;
using System.Text.RegularExpressions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Cards
{
    internal static class AutoTag
    {
        public static HashSet<ModelId> thornsRelatedCards = new();
        public static HashSet<ModelId> maxHpRelatedCards = new();
        public static HashSet<ModelId> goldRelatedCards = new();
    }

    [HarmonyPatch(typeof(ModelDb), "Init")]
    internal class AutoTagPatch
    {
        private const string englishLanguageName = "eng";
        private const string chineseLanguageName = "zhs";
        private const string russianLanguageName = "rus";

        private const string thornsVar = "ThornsPower";
        private const string maxHpVar = "MaxHp";
        private const string goldVar = "Gold";

        private const string englishThorns = "thorns";
        private const string englishGold = "Gold";

        private const string chineseThorns = "荆棘";
        private const string chineseGold = "金币";

        // private static readonly Regex englishThornsRegex = new(@"\bthorns\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex englishMaxHpRegex = new(@"\b(?:max(?:imum)?\s*hp|max\s*health)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // private static readonly Regex englishGoldRegex = new(@"\bGold\b", RegexOptions.Compiled);

        // private static readonly Regex chineseThornsRegex = new(@"荆棘");
        private static readonly Regex chineseMaxHpRegex = new(@"(?:最大生命值的|点最大生命值|点最大生命|生命上限)");
        // private static readonly Regex chineseGoldRegex = new(@"金币");

        private static readonly Regex russianThornsRegex = new(@"\bшип\p{L}*\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex russianMaxHpRegex = new(@"\b(?:макс\.?|максим\p{L}*|максимум)\s*ОЗ\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex russianGoldRegex = new(@"\bзолот\p{L}*\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static void Postfix()
        {
            var stopwatch = Stopwatch.StartNew();

            LocTableLoadCachePatch.Begin();

            try
            {
                var allTables = new Dictionary<string, Dictionary<string, LocTable>>();
                var languages = new[] { englishLanguageName, chineseLanguageName, russianLanguageName };

                foreach (var language in languages)
                {
                    var (tables, _, _) = LocManager.LoadTablesFromPath(language);

                    allTables[language] = tables;
                }

                // stopwatch.Stop();
                // Main.Logger.Info($"loading language tables took {stopwatch.Elapsed.TotalMilliseconds} ms!");
                // stopwatch.Start();

                var localizationRules = new[] { new LocalizationRule(englishLanguageName, Contains(englishThorns), Regex(englishMaxHpRegex), ContainsExact(englishGold)), new LocalizationRule(chineseLanguageName, ContainsExact(chineseThorns), Regex(chineseMaxHpRegex), ContainsExact(chineseGold)), new LocalizationRule(russianLanguageName, Regex(russianThornsRegex), Regex(russianMaxHpRegex), Regex(russianGoldRegex)) };

                foreach (var card in ModelDb.AllCards)
                {
                    var foundThorns = card.DynamicVars.ContainsKey(thornsVar);
                    var foundMaxHp = card.DynamicVars.ContainsKey(maxHpVar);
                    var foundGold = card.DynamicVars.ContainsKey(goldVar);

                    foreach (var localizationRule in localizationRules)
                    {
                        (foundThorns, foundMaxHp, foundGold) = AddRelatedCard(card, localizationRule, allTables, foundThorns, foundMaxHp, foundGold);

                        if (foundThorns && foundMaxHp && foundGold)
                        {
                            break;
                        }
                    }

                    AddRelatedCardInternal(card, foundThorns, foundMaxHp, foundGold);
                }
            }
            finally
            {
                LocTableLoadCachePatch.End();
            }

            stopwatch.Stop();
            Main.Logger.Info($"\nAutoTagged every single card in the game, we got:\n{AutoTag.thornsRelatedCards.Count} Thorns related cards,\n{AutoTag.goldRelatedCards.Count} Gold related cards,\n{AutoTag.maxHpRelatedCards.Count} Max HP related cards.\nThis took {stopwatch.Elapsed.TotalMilliseconds} ms!\n");
        }

        private static (bool thorns, bool maxHp, bool gold) AddRelatedCard(CardModel card, LocalizationRule localizationRule, Dictionary<string, Dictionary<string, LocTable>> allTables, bool foundThorns, bool foundMaxHp, bool foundGold)
        {
            if (allTables[localizationRule.languageName].TryGetValue(card.Description.LocTable, out var languageTable))
            {
                var text = languageTable.GetRawText(card.Description.LocEntryKey);

                if (!foundThorns)
                {
                    foundThorns = localizationRule.thornsMatcher.IsMatch(text);
                }

                if (!foundMaxHp)
                {
                    foundMaxHp = localizationRule.maxHpMatcher.IsMatch(text);
                }

                if (!foundGold)
                {
                    foundGold = localizationRule.goldMatcher.IsMatch(text);
                }
            }

            return (foundThorns, foundMaxHp, foundGold);
        }

        private static void AddRelatedCardInternal(CardModel card, bool foundThorns, bool foundMaxHp, bool foundGold)
        {
            if (foundThorns)
            {
                // Main.Logger.Warn("found thorns in " + card.Title);
                AutoTag.thornsRelatedCards.Add(card.Id);
            }

            if (foundMaxHp)
            {
                // Main.Logger.Warn("found max hp in " + card.Title);
                AutoTag.maxHpRelatedCards.Add(card.Id);
            }

            if (foundGold)
            {
                // Main.Logger.Warn("found gold in " + card.Title);
                AutoTag.goldRelatedCards.Add(card.Id);
            }
        }

        private static TextMatcher Contains(string value)
        {
            return new TextMatcher(text => text.Contains(value, StringComparison.OrdinalIgnoreCase));
        }

        private static TextMatcher ContainsExact(string value)
        {
            return new TextMatcher(text => text.Contains(value, StringComparison.Ordinal));
        }

        private static TextMatcher Regex(Regex regex)
        {
            return new TextMatcher(regex.IsMatch);
        }

        private record LocalizationRule(string languageName, TextMatcher thornsMatcher, TextMatcher maxHpMatcher, TextMatcher goldMatcher);

        private record TextMatcher(Func<string, bool> IsMatch);
    }

    [HarmonyPatch(typeof(LocManager), "LoadTablesFromPath")]
    internal static class LocTableLoadCachePatch
    {
        private static bool _enabled;
        private static bool _hasCachedEnglish;

        private static ( Dictionary<string, LocTable> tables, bool overridesActive, List<LocValidationError> validationErrors ) _cachedEnglish;

        internal static void Begin()
        {
            _enabled = true;
            _hasCachedEnglish = false;
        }

        internal static void End()
        {
            _enabled = false;
            _hasCachedEnglish = false;
        }

        private static bool Prefix(string language, bool allowOverride, ref ( Dictionary<string, LocTable> tables, bool overridesActive, List<LocValidationError> validationErrors ) __result)
        {
            if (!_enabled || language != "eng" || !allowOverride || !_hasCachedEnglish)
            {
                return true;
            }

            __result = _cachedEnglish;
            return false;
        }

        private static void Postfix(string language, bool allowOverride, ( Dictionary<string, LocTable> tables, bool overridesActive, List<LocValidationError> validationErrors ) __result)
        {
            if (_enabled && language == "eng" && allowOverride)
            {
                _cachedEnglish = __result;
                _hasCachedEnglish = true;
            }
        }
    }
}