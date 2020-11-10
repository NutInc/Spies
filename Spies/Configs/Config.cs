namespace Spies.Configs
{
    using Exiled.API.Interfaces;
    using SubConfigs;
    using System.ComponentModel;

    public class Config : IConfig
    {
        [Description("Prevents spys from taking damage from the faction they are undercover from.")]
        public bool PreventSpyFf { get; private set; } = false;

        public ChaosSpies ChaosSpies { get; private set; } = new ChaosSpies();
        public MtfSpies MtfSpies { get; private set; } = new MtfSpies();
        public bool IsEnabled { get; set; } = true;
    }
}