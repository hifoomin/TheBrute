#region

using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Random;

#endregion

namespace TheBrute.Cards
{
    public static class AudioUtils
    {
        public static void PlaySlash(AttackCommand result)
        {
            if (result == null)
            {
                Main.Audio.PlaySfx("attack_light.ogg");
                return;
            }

            var highestDamageDone = result.Results
                .SelectMany(hit => hit)
                .GroupBy(r => r.Receiver)
                .Select(g => g.Sum(r => r.TotalDamage + r.OverkillDamage))
                .DefaultIfEmpty(0)
                .Max();

            var soundToPlay = highestDamageDone switch
            {
                >= 20 => SpecialEventManager.IsAprilFools(DateTime.Today) ? "attack_heavy_april_fools.ogg" : "attack_heavy.ogg",
                >= 13 => "attack_medium.ogg",
                _ => "attack_light.ogg"
            };

            Main.Audio.PlaySfx(soundToPlay);
        }

        // yes I know I could consolidate these methods, but I'm lazy for now lol
        public static void PlayAoeSlash(AttackCommand result)
        {
            if (result == null)
            {
                Main.Audio.PlaySfx("aoe_attack_light.ogg");
                return;
            }

            var highestDamageDone = result.Results
                .SelectMany(hit => hit)
                .GroupBy(r => r.Receiver)
                .Select(g => g.Sum(r => r.TotalDamage))
                .DefaultIfEmpty(0)
                .Max();

            var soundToPlay = highestDamageDone switch
            {
                // >= 20 => "attack_heavy.ogg", missing for now
                >= 13 => "aoe_attack_medium.ogg",
                _ => "aoe_attack_light.ogg"
            };

            Main.Audio.PlaySfx(soundToPlay);
        }

        public static void PlayPunch()
        {
            var maxExclusive = SpecialEventManager.IsAprilFools(DateTime.Now) ? 3 : 2;
            var soundToPlay = Rng.Chaotic.NextInt(0, maxExclusive) switch
            {
                2 => "punch_april_fools.ogg",
                1 => "punch_2.ogg",
                _ => "punch_1.ogg"
            };

            Main.Audio.PlaySfx(soundToPlay);
        }

        public static void PlayBite()
        {
            var maxExclusive = SpecialEventManager.IsAprilFools(DateTime.Now) ? 3 : 2;

            var soundToPlay = Rng.Chaotic.NextInt(0, maxExclusive) switch
            {
                2 => "bite_april_fools.ogg",
                1 => "bite_2.ogg",
                _ => "bite_1.ogg"
            };

            Main.Audio.PlaySfx(soundToPlay);
        }
    }
}