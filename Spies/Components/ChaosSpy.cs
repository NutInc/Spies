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

    public class ChaosSpy : MonoBehaviour
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

            _player.Broadcast(Instance.Config.ChaosSpies.SpawningBroadcast.Duration,
                Instance.Config.ChaosSpies.SpawningBroadcast.Content);
            _player.SendConsoleMessage(Instance.Config.ChaosSpies.SpawningConsoleInfo, "yellow");

            _disguisedRoleType = _player.Role;
            _player.IsFriendlyFireEnabled = true;

            if (!Instance.Config.ChaosSpies.Inventory.Any())
                yield break;

            if (Instance.Config.ChaosSpies.Inventory.TryGetValue(_disguisedRoleType, out List<ItemType> items))
                _player.ResetInventory(items);
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

            if (Side.ChaosInsurgency == ev.Attacker.Side && _player == ev.Target)
            {
                if (Instance.Config.PreventSpyFf)
                    ev.Amount = 0;

                ev.Attacker.Broadcast(Instance.Config.ChaosSpies.SameTeamBroadcast.Duration,
                    Instance.Config.ChaosSpies.SameTeamBroadcast.Content);
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
                _player.SetRole(Instance.Config.ChaosSpies.HiddenRole, true);
                _player.Broadcast(Instance.Config.ChaosSpies.RevealBroadcast.Duration,
                    Instance.Config.ChaosSpies.RevealBroadcast.Content);
                Timing.CallDelayed(0.275f, () => { _player.CurrentItem = curItem; });
            }

            _player.IsFriendlyFireEnabled = false;
            PlayerEvents.Hurting -= OnHurting;
            Object.Destroy(this);
        }
    }
}