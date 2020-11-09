namespace Spies.Components
{
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using PlayerEvents = Exiled.Events.Handlers.Player;
    using ServerEvents = Exiled.Events.Handlers.Server;
    using static Spies;

    public class Spy : MonoBehaviour
    {
        private Player _player;
        public Team team = Team.RIP;
        private RoleType _disguisedRole;

        private void Awake()
        {
            _player = Player.Get(gameObject);
            Timing.RunCoroutine(InitSpy());
            _player.IsFriendlyFireEnabled = true;
        }

        private void Update()
        {
            if (_player == null || _player.Role != _disguisedRole)
                Destroy(false);
        }
        
        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker == ev.Target)
                return;

            if (team.GetSide() == ev.Attacker.Side)
            {
                if (Instance.Config.PreventSpyFf)
                    ev.Amount = 0;
                    
                switch (team)
                {
                    case Team.CHI:
                        ev.Attacker.Broadcast(Instance.Config.ChaosSpies.SameTeamBroadcast.Duration, Instance.Config.ChaosSpies.SameTeamBroadcast.Content);
                        return;
                    case Team.MTF:
                        ev.Attacker.Broadcast(Instance.Config.MtfSpies.SameTeamBroadcast.Duration, Instance.Config.MtfSpies.SameTeamBroadcast.Content);
                        return;
                }
            }

            if (ev.Attacker != _player || ev.Target.Side != _player.Side)
                return;

            Destroy();
        }

        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (ev.Name.ToLower() == "reveal")
            {
                Destroy();
                ev.ReturnMessage = "Revealing your role..";
                ev.Color = "white";
            }
        }

        public void Destroy(bool reveal = true)
        {
            if (reveal)
            {
                switch (team)
                {
                    case Team.CHI:
                        _player.SetRole(Instance.Config.ChaosSpies.HiddenRole, true);
                        _player.Broadcast(Instance.Config.ChaosSpies.RevealBroadcast.Duration, Instance.Config.ChaosSpies.RevealBroadcast.Content);
                        break;
                    case Team.MTF:
                        _player.SetRole(Instance.Config.MtfSpies.HiddenRole, true);
                        _player.Broadcast(Instance.Config.MtfSpies.RevealBroadcast.Duration, Instance.Config.MtfSpies.RevealBroadcast.Content);
                        break;
                }
            }

            _player.IsFriendlyFireEnabled = false;
            UnsubscribeEvents();
            Object.Destroy(this);
        }

        private IEnumerator<float> InitSpy()
        {
            yield return Timing.WaitUntilFalse(() => team == Team.RIP);
            switch (team)
            {
                case Team.CHI:
                    _player.Broadcast(Instance.Config.ChaosSpies.SpawningBroadcast.Duration, Instance.Config.ChaosSpies.SpawningBroadcast.Content);
                    _player.SendConsoleMessage(Instance.Config.ChaosSpies.SpawningConsoleInfo, "yellow");
                    if (Instance.Config.ChaosSpies.Inventory.Any())
                        _player.ResetInventory(Instance.Config.ChaosSpies.Inventory);
                    break;
                case Team.MTF:
                    _player.Broadcast(Instance.Config.MtfSpies.SpawningBroadcast.Duration, Instance.Config.MtfSpies.SpawningBroadcast.Content);
                    _player.SendConsoleMessage(Instance.Config.MtfSpies.SpawningConsoleInfo, "yellow");
                    if (Instance.Config.MtfSpies.Inventory.Any())
                        _player.ResetInventory(Instance.Config.MtfSpies.Inventory);
                    break;
            }
            
            SubscribeEvents();
            _disguisedRole = _player.Role;
        }
        
        private void SubscribeEvents()
        {
            PlayerEvents.Hurting += OnHurting;
            ServerEvents.SendingConsoleCommand += OnConsoleCommand;
        }

        private void UnsubscribeEvents()
        {
            PlayerEvents.Hurting -= OnHurting;
            ServerEvents.SendingConsoleCommand -= OnConsoleCommand;
        }
    }
}