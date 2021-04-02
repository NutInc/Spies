namespace Spies.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// A command which spawns a user as a spy.
    /// </summary>
    public class Spawn : ICommand
    {
        /// <inheritdoc/>
        public string Command { get; } = "spawn";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "s" };

        /// <inheritdoc/>
        public string Description { get; } = "Spawns a player as the specified type of spy.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("spy.spawn"))
            {
                response = "Insufficient permission. Required: spy.spawn";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Syntax: spy spawn {Player} {SpyType}";
                return false;
            }

            if (!(Player.Get(arguments.At(0)) is { } player))
            {
                response = "Could not find the specified player.";
                return false;
            }

            if (!Spy.TryGet(arguments.At(1), out Spy spy))
            {
                response = "Could not locate the specified spy type.";
                return false;
            }

            spy.Spawn(player);
            response = $"Spawned {player.Nickname} as a {spy.Name}.";
            return true;
        }
    }
}