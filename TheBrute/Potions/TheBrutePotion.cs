using BaseLib.Abstracts;
using BaseLib.Utils;
using TheBrute.Character;

namespace TheBrute.Potions;

[Pool(typeof(TheBrutePotionPool))]
public abstract class TheBrutePotion : CustomPotionModel;