#region

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

        private static void Postfix()
        {
            var allTables = new Dictionary<string, Dictionary<string, LocTable>>();
            var languages = new[] { englishLanguageName, chineseLanguageName, russianLanguageName };

            foreach (var language in languages)
            {
                var (tables, _, _) = LocManager.LoadTablesFromPath(language);
                allTables[language] = tables;
            }

            var englishThornsRegex = new Regex(@"\bthorns\b", RegexOptions.IgnoreCase);
            var englishMaxHpRegex = new Regex(@"\b(?:max(?:imum)?\s*hp|max\s*health)\b", RegexOptions.IgnoreCase);
            var englishGoldRegex = new Regex(@"\bGold\b");

            var chineseThornsRegex = new Regex(@"荆棘");
            var chineseMaxHpRegex = new Regex(@"(?:最大生命值的|点最大生命值|点最大生命|生命上限)");
            var chineseGoldRegex = new Regex(@"金币");

            var russianThornsRegex = new Regex(@"\bшип\p{L}*\b", RegexOptions.IgnoreCase);
            var russianMaxHpRegex = new Regex(@"\b(?:макс\.?|максим\p{L}*|максимум)\s*ОЗ\b", RegexOptions.IgnoreCase);
            var russianGoldRegex = new Regex(@"\bзолот\p{L}*\b", RegexOptions.IgnoreCase);

            var localizationRules = new[]
            {
                new LocalizationRule(englishLanguageName, englishThornsRegex, englishMaxHpRegex, englishGoldRegex),
                new LocalizationRule(chineseLanguageName, chineseThornsRegex, chineseMaxHpRegex, chineseGoldRegex),
                new LocalizationRule(russianLanguageName, russianThornsRegex, russianMaxHpRegex, russianGoldRegex)
            };

            foreach (var card in ModelDb.AllCards)
            {
                var foundThorns = card.DynamicVars.ContainsKey(thornsVar);
                var foundMaxHp = card.DynamicVars.ContainsKey(maxHpVar);
                var foundGold = card.DynamicVars.ContainsKey(goldVar);

                foreach (var localizationRule in localizationRules)
                {
                    (foundThorns, foundMaxHp, foundGold) = AddRelatedCard(card, localizationRule, allTables, foundThorns, foundMaxHp, foundGold);
                }

                AddRelatedCardInternal(card, foundThorns, foundMaxHp, foundGold);
            }
        }

        private static (bool thorns, bool maxHp, bool gold) AddRelatedCard(CardModel card, LocalizationRule localizationRule, Dictionary<string, Dictionary<string, LocTable>> allTables, bool foundThorns, bool foundMaxHp, bool foundGold)
        {
            if (allTables[localizationRule.languageName].TryGetValue(card.Description.LocTable, out var languageTable))
            {
                var text = languageTable.GetRawText(card.Description.LocEntryKey);

                foundThorns |= localizationRule.thornsRegex.IsMatch(text);
                foundMaxHp |= localizationRule.maxHpRegex.IsMatch(text);
                foundGold |= localizationRule.goldRegex.IsMatch(text);
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

        private record LocalizationRule(string languageName, Regex thornsRegex, Regex maxHpRegex, Regex goldRegex);
    }
}