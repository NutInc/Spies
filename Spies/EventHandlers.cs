namespace Spies
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Contains all handlers for events derived from <see cref="Exiled.Events.Handlers"/>.
    /// </summary>
    public static class EventHandlers
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsSpy(out Spy spy))
            {
                ev.Player.SessionVariables.Remove("IsSpy");
                Plugin.SendDebug($"Removed spy tag ({spy.Name}) from {ev.Player.Nickname}.");
                return;
            }

            Timing.CallDelayed(0.3f, () =>
            {
                if (ev.NewRole != ev.Player.Role
                    || Random.Range(0, 100) >= Plugin.Instance.Config.SpawnChance
                    || ev.Player.IsSpy(out _))
                    return;

                List<Spy> possibleSpies = Plugin.Instance.Config.Spies.Where(x => x.SpawnedRole == ev.NewRole).ToList();
                if (possibleSpies.IsEmpty())
                    return;

                Plugin.SendDebug($"Possible spies to spawn as: {string.Join(", ", possibleSpies.Select(x => x.Name))}");
                possibleSpies[Random.Range(0, possibleSpies.Count)].Spawn(ev.Player);
            });
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnShot(ShotEventArgs)"/>
        public static void OnShot(ShotEventArgs ev)
        {
            if (!Plugin.Instance.Config.RevealAfterShot || !(Player.Get(ev.Target) is { } target))
                return;

            if (ev.Shooter.IsSpy(out Spy spy) && target.Side == spy.SpawnedRole.GetSide())
            {
                ev.Shooter.ChangeAppearance(spy.SpawnedRole);
            }
        }
    }
}