namespace Spies.Handlers
{
    using Components;
    using Exiled.Events.EventArgs;
    using Exiled.API.Features;
    using System.Linq;
    using System.Collections.Generic;
    using MEC;

    public class PlayerHandlers
    {
        public bool MtfExterminated;
        public bool ChaosExterminated;

        public void OnDied(DiedEventArgs _)
        {
            Timing.CallDelayed(0.1f, () =>
            {
                MtfExterminated = !Player.Get(Team.MTF).Any() && !Player.Get(Team.RSC).Any();
                ChaosExterminated = !Player.Get(Team.CHI).Any() && !Player.Get(Team.CDP).Any();

                if (MtfExterminated)
                    foreach (Player ply in Player.List)
                    {
                        var mtfSpy = ply.GameObject.GetComponent<MtfSpy>();
                        if (mtfSpy != null)
                            mtfSpy.Destroy();
                    }

                if (ChaosExterminated)
                    foreach (Player ply in Player.List)
                    {
                        var chaosSpy = ply.GameObject.GetComponent<ChaosSpy>();
                        if (chaosSpy != null)
                            chaosSpy.Destroy();
                    }
            });
        }
    }
}