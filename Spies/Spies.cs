namespace Spies
{
    using Configs;
    using Exiled.API.Features;
    using Handlers;
    using System;
    using PlayerEvents = Exiled.Events.Handlers.Player;
    using ServerEvents = Exiled.Events.Handlers.Server;

    public class Spies : Plugin<Config>
    { 
        internal readonly PlayerHandlers _playerHandlers = new PlayerHandlers();
        private readonly ServerHandlers _serverHandlers = new ServerHandlers();
        internal static Spies Instance;
        
        public override void OnEnabled()
        {
            Instance = this;
            PlayerEvents.Died += _playerHandlers.OnDied;
            ServerEvents.SendingConsoleCommand += _serverHandlers.OnConsoleCommand;
            ServerEvents.RespawningTeam += _serverHandlers.OnRespawningTeam;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerEvents.Died -= _playerHandlers.OnDied;
            ServerEvents.SendingConsoleCommand -= _serverHandlers.OnConsoleCommand;
            ServerEvents.RespawningTeam -= _serverHandlers.OnRespawningTeam;
            Instance = null;
        }

        public override string Author => "Build";
        public override string Name => "Spies";
        public override Version Version => new Version(1, 0, 1);
    }
}