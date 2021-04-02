namespace Spies
{
    using System;
    using Exiled.API.Features;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin InstanceValue = new Plugin();

        private Plugin()
        {
        }

        /// <summary>
        /// Gets a static instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; } = InstanceValue;

        /// <inheritdoc/>
        public override string Author { get; } = "Build";

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(2, 9, 4);

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(2, 0, 0);

        /// <summary>
        /// Sends a debug message if <see cref="Config.ShowDebug"/> is enabled.
        /// </summary>
        /// <param name="message">The debug message to be sent.</param>
        public static void SendDebug(object message) => Log.Debug(message, Instance.Config.ShowDebug);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            PlayerHandlers.ChangingRole += EventHandlers.OnChangingRole;
            PlayerHandlers.Shot += EventHandlers.OnShot;
            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            PlayerHandlers.ChangingRole -= EventHandlers.OnChangingRole;
            PlayerHandlers.Shot -= EventHandlers.OnShot;
            base.OnDisabled();
        }
    }
}