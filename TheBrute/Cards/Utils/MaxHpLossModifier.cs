#region

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Cards
{
    public class MaxHpLossModifier : CardModifier
    {
        private const string AmountSaveKey = "Amount";

        public int Amount { get; private set; }

        public static bool Has(CardModel? card)
        {
            return GetAmount(card) > 0;
        }

        public static int GetAmount(CardModel? card)
        {
            return Get(card)?.Amount ?? 0;
        }

        public static bool CanApplyTo(CardModel? card)
        {
            return card != null;
        }

        public static MaxHpLossModifier? Get(CardModel? card)
        {
            if (card == null)
            {
                return null;
            }

            return DirectModifiers(card).OfType<MaxHpLossModifier>().FirstOrDefault();
        }

        public static bool AddTo(CardModel card, decimal amount)
        {
            if (!CanApplyTo(card))
            {
                return false;
            }

            var amountToAdd = (int)amount;

            if (amountToAdd <= 0)
            {
                return false;
            }

            var existingMaxHpLoss = Get(card);

            if (existingMaxHpLoss != null)
            {
                existingMaxHpLoss.AddAmount(amountToAdd);
                return true;
            }

            var maxHpLossModifier = (MaxHpLossModifier)Get<MaxHpLossModifier>().MutableClone();
            maxHpLossModifier.SetAmount(amountToAdd);

            AddModifier(card, maxHpLossModifier);
            return true;
        }

        public override void StoreSaveData(ModifierSave save)
        {
            save.IntProperties[AmountSaveKey] = Amount;
        }

        public override void LoadSaveData(ModifierSave save)
        {
            if (save.IntProperties.TryGetValue(AmountSaveKey, out var amount))
            {
                SetAmount(amount);
            }
        }

        public override void ModifyDescriptionPost(Creature? target, ref string description)
        {
            if (Amount <= 0)
            {
                return;
            }

            description += $"\nLose {Amount} Max HP.";
        }

        public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Amount <= 0)
            {
                return;
            }

            if (cardPlay.Card.Owner?.Creature == null)
            {
                return;
            }

            if (!cardPlay.IsLastInSeries)
            {
                return;
            }

            // if (cardPlay.Card.Pile?.Type != PileType.Hand)
            // {
            // return;
            // }

            await CreatureCmd.LoseMaxHp(choiceContext, cardPlay.Card.Owner.Creature, Amount, true);
        }

        private void SetAmount(int amount)
        {
            Amount = Math.Max(0, amount);
        }

        private void AddAmount(int amount)
        {
            SetAmount(Amount + amount);
        }
    }
}