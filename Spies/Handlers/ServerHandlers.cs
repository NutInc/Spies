namespace Spies.Handlers
{
    using Components;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Respawning;
    using System;
    using static Spies;
    
    public class ServerHandlers
    {
        private readonly Random _random = new Random();
        
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (_random.Next(101) > Instance.Config.SpawnChance)
                return;

            Player ply = ev.Players[_random.Next(ev.Players.Count)];
            var spy = ply.GameObject.AddComponent<Spy>();
            spy.team = ev.NextKnownTeam != SpawnableTeamType.ChaosInsurgency ? Team.CHI : Team.MTF;
        }
    }
}