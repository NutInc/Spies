using Exiled.API.Enums;

namespace Spies
{
#pragma warning disable SA1101
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Features;
    using MEC;

    /// <summary>
    /// Class to save the spy's config options in a clean matter.
    /// </summary>
    public class Spy
    {
        /// <summary>
        /// Gets or sets the name of the spy variant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the role that the user will spawn as.
        /// </summary>
        [Description("The role that the user will spawn as.")]
        public RoleType SpawnedRole { get; set; }

        /// <summary>
        /// Gets or sets the role that the user will be seen as.
        /// </summary>
        [Description("The role that the user will be seen as.")]
        public RoleType DisguiseRole { get; set; }

        /// <summary>
        /// Gets or sets the message to be sent to a player when they spawn.
        /// </summary>
        [Description("The message to be sent to a player when they spawn.")]
        public Broadcast SpawnMessage { get; set; }

        /// <summary>
        /// Gets or sets the contents of a users inventory when they spawn.
        /// </summary>
        public List<ItemType> Inventory { get; set; }

        /// <summary>
        /// Returns if the spy has been revealed
        /// </summary>
        public bool Revealed { get; set; } = false;

        /// <summary>
        /// Attempts to get a <see cref="Spy"/> by its name.
        /// </summary>
        /// <param name="name">The name of the <see cref="Spy"/>.</param>
        /// <param name="spy">The returned <see cref="Spy"/>.</param>
        /// <returns>A value indicating whether the returned <see cref="Spy"/> is not null.</returns>
        public static bool TryGet(string name, out Spy spy)
        {
            spy = null;
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            spy = Plugin.Instance.Config.Spies.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            return spy != null;
        }

        /// <summary>
        /// Spawns the specified player as a spy.
        /// </summary>
        /// <param name="player">The player to spawn.</param>
        public void SpawnAsSpy(Player player)
        {
            player.SetRole(DisguiseRole, SpawnReason.None, true);

            Plugin.Instance.spawnProtectedSpies.Add(player);
            Timing.CallDelayed(10f, () => Plugin.Instance.spawnProtectedSpies.Remove(player));

            // Delay fixes the FakeSyncVar not working
            Timing.CallDelayed(.1f, () => player.ChangeAppearance(SpawnedRole));

            player.Broadcast(SpawnMessage);

            player.SessionVariables["IsSpy"] = this;

            Plugin.SendDebug($"Spawned {player.Nickname} as a {Name}.");

            Timing.CallDelayed(.5f, () => player.ResetInventory(Inventory));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Name} {SpawnedRole} {DisguiseRole}";
        }
    }
}