using BaseLib.Abstracts;
using BaseLib.Utils;
using Yoka.Character;

namespace Yoka.Potions;

[Pool(typeof(YokaPotionPool))]
public abstract class YokaPotion : CustomPotionModel;