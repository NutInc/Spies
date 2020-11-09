namespace Spies.Handlers
{
    using Components;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Respawning;
    using System;
    using System.Linq;
    using static Spies;
    
    public class ServerHandlers
    {
        private readonly Random _random = new Random();
        
        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (ev.Name.ToLower() != "reveal") 
                return;
            
            var chaosSpy = ev.Player.GameObject.GetComponent<ChaosSpy>();
            var mtfSpy = ev.Player.GameObject.GetComponent<MtfSpy>();
            if (chaosSpy != null)
                chaosSpy.Destroy();
            else if (mtfSpy != null)
                mtfSpy.Destroy();
            else
                return;

            ev.IsAllowed = false;
            ev.ReturnMessage = "Revealing your role..";
            ev.Color = "white";
        }
        
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            Instance._playerHandlers.MtfExterminated = !Player.Get(Team.MTF).Any() && !Player.Get(Team.RSC).Any();
            Instance._playerHandlers.ChaosExterminated = !Player.Get(Team.CHI).Any() && !Player.Get(Team.CDP).Any();


            Player ply = ev.Players[_random.Next(ev.Players.Count)];
            switch (ev.NextKnownTeam)
            {
                case SpawnableTeamType.ChaosInsurgency:
                    if (_random.Next(101) > Instance.Config.MtfSpies.SpawnChance || Instance._playerHandlers.MtfExterminated)
                        break;
                    
                    var mtfSpy = ply.GameObject.AddComponent<MtfSpy>();
                    mtfSpy.disguisedTeam = Team.CHI;
                    break;
                case SpawnableTeamType.NineTailedFox:
                    if (_random.Next(101) > Instance.Config.ChaosSpies.SpawnChance || Instance._playerHandlers.ChaosExterminated)
                        break;
                        
                    var chaosSpy = ply.GameObject.AddComponent<ChaosSpy>();
                    chaosSpy.disguisedTeam = Team.MTF;
                    break;
            }
        }
    }
}