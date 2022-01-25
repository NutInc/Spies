namespace Spies
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    /// <inheritdoc cref="IConfig"/>
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug messages should be shown.
        /// </summary>
        [Description("Whether debug messages should be shown.")]
        public bool ShowDebug { get; set; } = false;

        /// <summary>
        /// Gets or sets the chance for a spy to spawn whenever a user changes role.
        /// </summary>
        [Description("The chance for a spy to spawn whenever a user changes role.")]
        public float SpawnChance { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether a spy should be revealed after they shoot a teammate.
        /// </summary>
        [Description("If a spy should be revealed after they shoot a teammate.")]
        public bool RevealAfterShot { get; set; } = false;

        /// <summary>
        /// Gets or sets the <see cref="List{T}"/> of spies which can spawn.
        /// </summary>
        [Description("The list of spies which can spawn.")]
        public List<Spy> Spies { get; set; } = new List<Spy>
        {
            new Spy
            {
                Name = "NtfSpy",
                SpawnedRole = RoleType.ChaosMarauder,
                DisguiseRole = RoleType.NtfSergeant,
                SpawnMessage = new Broadcast("You have spawned as a <color=blue>NTF Spy!</color>\n<size=20>Other people view you as a Chaos Insurgent, assassinate your so-called teammates!</size>"),
                Inventory = new List<ItemType>
                {
                    ItemType.KeycardChaosInsurgency,
                    ItemType.GunLogicer,
                    ItemType.Medkit,
                    ItemType.Painkillers,
                },
            },
            new Spy
            {
                Name = "CiSpy",
                SpawnedRole = RoleType.NtfSergeant,
                DisguiseRole = RoleType.ChaosMarauder,
                SpawnMessage = new Broadcast("You have spawned as a <color=green>Chaos Insurgent Spy!</color>\n<size=20>Other people view you as a Lieutenant, assassinate your so-called teammates!</size>"),
                Inventory = new List<ItemType>
                {
                    ItemType.KeycardNTFLieutenant,
                    ItemType.GunE11SR,
                    ItemType.GrenadeHE,
                    ItemType.Radio,
                    ItemType.Medkit
                },
            },
        };
    }
}