namespace Spies.Configs.SubConfigs
{
    using Exiled.API.Features;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class MtfSpies
    {
        [Description("Percent chance a spy will spawn in a given respawn wave.")]
        public int SpawnChance { get; private set; } = 40;

        [Description("Broadcast played to the spy when they spawn.")]
        public Broadcast SpawningBroadcast { get; private set; } =
            new Broadcast("You are an <color=blue>MTF Spy!</color>\\nPress [`] for more info.", 10);

        [Description("Message to send to the spys console when they spawn.")]
        public string SpawningConsoleInfo { get; private set; } =
            "You are a MTF Spy! You are immune to chaos for now, but as soon as you damage an insurgent, your immunity will turn off. Help MTF win the round and kill ClassD and Insurgents as necessary.";

        [Description("Role the spy will reveal as after shooting a ClassD or ChaosInsurgency.")]
        public RoleType HiddenRole { get; private set; } = RoleType.NtfScientist;

        [Description("Broadcast played to the spy when their role is revealed.")]
        public Broadcast RevealBroadcast { get; private set; } = new Broadcast("You have been revealed as a spy!", 10);

        [Description("Broadcast played when a MTF or Scientist attacks the spy.")]
        public Broadcast SameTeamBroadcast { get; private set; } =
            new Broadcast("You have attacked a spy of your own faction!", 2);

        [Description("Inventory the spy will spawn with. Leave empty for the default inventory.")]
        public List<ItemType> Inventory { get; private set; } = new List<ItemType>();
    }
}