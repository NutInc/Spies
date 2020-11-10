namespace Spies.Components
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using PlayerEvents = Exiled.Events.Handlers.Player;
    using static Spies;

    public class MtfSpy : MonoBehaviour
    {
        private Player _player;
        public Team disguisedTeam = Team.RIP;
        private RoleType _disguisedRoleType;

        private void Start()
        {
            _player = Player.Get(gameObject);
            Timing.RunCoroutine(InitSpy());
            PlayerEvents.Hurting += OnHurting;
        }

        private IEnumerator<float> InitSpy()
        {
            yield return Timing.WaitUntilFalse(() => disguisedTeam == Team.RIP);

            _player.Broadcast(Instance.Config.MtfSpies.SpawningBroadcast.Duration,
                Instance.Config.MtfSpies.SpawningBroadcast.Content);
            _player.SendConsoleMessage(Instance.Config.MtfSpies.SpawningConsoleInfo, "yellow");

            _disguisedRoleType = _player.Role;
            _player.IsFriendlyFireEnabled = true;

            if (Instance.Config.MtfSpies.Inventory.Any())
                _player.ResetInventory(Instance.Config.MtfSpies.Inventory);
        }

        private void Update()
        {
            if (_player == null || _player.Role != _disguisedRoleType)
                Destroy(false);
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker == ev.Target)
                return;

            if (Side.Mtf == ev.Attacker.Side && _player == ev.Target)
            {
                if (Instance.Config.PreventSpyFf)
                    ev.Amount = 0;

                ev.Attacker.Broadcast(Instance.Config.MtfSpies.SameTeamBroadcast.Duration,
                    Instance.Config.MtfSpies.SameTeamBroadcast.Content);
                return;
            }

            if (ev.Attacker != _player || ev.Target.Side != _player.Side)
                return;

            Destroy();
        }

        public void Destroy(bool reveal = true)
        {
            if (reveal)
            {
                var curItem = _player.CurrentItem;
                _player.SetRole(Instance.Config.MtfSpies.HiddenRole, true);
                _player.Broadcast(Instance.Config.MtfSpies.RevealBroadcast.Duration,
                    Instance.Config.MtfSpies.RevealBroadcast.Content);
                Timing.CallDelayed(0.275f, () => { _player.CurrentItem = curItem; });
            }

            _player.IsFriendlyFireEnabled = false;
            PlayerEvents.Hurting -= OnHurting;
            Object.Destroy(this);
        }
    }
}