#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Powers
{
    internal class ExplosionPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

        public override int DisplayAmount => Owner.MaxHp * Amount / 100;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner.Player != player)
            {
                return;
            }

            var hittableEnemies = CombatState.HittableEnemies;
            if (hittableEnemies != null)
            {
                Main.Audio.PlaySfx("explosion.ogg");

                foreach (var hittableEnemy in hittableEnemies)
                {
                    var child = NFireBurstVfx.Create(hittableEnemy, 0.75f);
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
                    await CreatureCmd.Damage(choiceContext, hittableEnemy, DisplayAmount, ValueProp.Unpowered, null, null);
                }

                Flash();
            }
        }
    }
}