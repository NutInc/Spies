namespace Spies
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Extensions;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Contains all handlers for events derived from <see cref="Exiled.Events.Handlers"/>.
    /// </summary>
    public static class EventHandlers
    {
        private static Config _config = Plugin.Instance.Config;

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            var player = ev.Player;

            if (ev.Reason == SpawnReason.Escaped) return;

            if (player.IsSpy(out Spy spy))
            {
                player.SessionVariables.Remove("IsSpy");
                player.CustomInfo = string.Empty;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;
                Plugin.SendDebug($"Removed spy tag ({spy.Name}) from {player.Nickname}.");
                return;
            }

            Timing.CallDelayed(0.3f, () =>
            {
                if (ev.NewRole != player.Role
                    || Random.Range(0, 100) >= _config.SpawnChance
                    || player.IsSpy(out _))
                    return;

                List<Spy> possibleSpies = _config.Spies.Where(x => x.SpawnedRole == ev.NewRole).ToList();
                if (possibleSpies.IsEmpty())
                    return;

                Plugin.SendDebug($"Possible spies to spawn as: {string.Join(", ", possibleSpies.Select(x => x.Name))}");
                possibleSpies[Random.Range(0, possibleSpies.Count)].SpawnAsSpy(player);
            });
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnShot(ShotEventArgs)"/>
        public static void OnShot(ShotEventArgs ev)
        {

            var shooter = ev.Shooter;

            if (!(ev.Target is { } target))
                return;

            if (shooter.IsSpy(out var shooterSpy) && target.Role.Side == shooterSpy.SpawnedRole.GetSide() && _config.RevealAfterShot)
            {
                RevealSpy(shooter, shooterSpy);
                ev.CanHurt = true;
                return;
            }

            if (Plugin.Instance.spawnProtectedSpies.Contains(target))
            {
                ev.CanHurt = false;
                return;
            };

            if (!target.IsSpy(out var targetSpy)) return;

            if (shooter.Role.Side == targetSpy.SpawnedRole.GetSide() && _config.RevealBeingShot)
            {
                RevealSpy(target, targetSpy);
                ev.CanHurt = true;
            }
            else if(!targetSpy.Revealed)
            {
                ev.CanHurt = false;
            }

        }

        private static void RevealSpy(Player shooter, Spy spy)
        {
            shooter.ChangeAppearance(spy.DisguiseRole);
            shooter.CustomInfo = Extensions.GetNewSpyRoleName(shooter);

            Plugin.Instance.spawnProtectedSpies.Remove(shooter);

            shooter.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
            spy.Revealed = true;
        }
    }
}