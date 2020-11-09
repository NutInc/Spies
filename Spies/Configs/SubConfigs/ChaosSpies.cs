namespace Spies.Configs.SubConfigs
{
    using Exiled.API.Features;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public class ChaosSpies
    {
        [Description("Percent chance a spy will spawn in a given respawn wave.")]
        public int SpawnChance { get; private set; } = 40;
        [Description("Broadcast played to the spy when they spawn.")]
        public Broadcast SpawningBroadcast { get; private set; } = new Broadcast("You are a <color=green>Chaos Spy!</color>\\nPress [`] for more info.", 10);
        [Description("Message to send to the spys console when they spawn.")]
        public string SpawningConsoleInfo { get; private set; } = "You are a Chaos Spy! You are immune to MTF for now, but as soon as you damage an insurgent, your immunity will turn off. Help Chaos win the round and kill Scientists and MTF as necessary.";
        [Description("Role the spy will reveal as after shooting a MTF or Scientist.")]
        public RoleType HiddenRole { get; private set; } = RoleType.ChaosInsurgency;
        [Description("Broadcast played to the spy when their role is revealed.")]
        public Broadcast RevealBroadcast { get; private set; } = new Broadcast("You have been revealed as a spy!", 10);
        [Description("Broadcast played when a ClassD or ChaosInsurgency attacks the spy.")]
        public Broadcast SameTeamBroadcast { get; private set; } = new Broadcast("You have attacked a spy of your own faction!", 2);
        [Description("Inventory the spy will spawn with. Leave empty for the default inventory.")]
        public Dictionary<RoleType, List<ItemType>> Inventory { get; private set; } = new Dictionary<RoleType, List<ItemType>>
        {
            { 
                RoleType.NtfCadet, new List<ItemType>
                {
                    ItemType.GunProject90,
                    ItemType.KeycardNTFLieutenant,
                    ItemType.GrenadeFrag,
                    ItemType.GrenadeFlash,
                    ItemType.Disarmer,
                    ItemType.WeaponManagerTablet
                }
            }
        };
    }
}