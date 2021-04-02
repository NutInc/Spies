namespace Spies.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// Command to check if a player is a spy.
    /// </summary>
    public class Check : ICommand
    {
        /// <inheritdoc/>
        public string Command { get; } = "check";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "c" };

        /// <inheritdoc/>
        public string Description { get; } = "Checks if a player is a spy.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("spy.check"))
            {
                response = "Insufficient permission! Required: spy.check";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Syntax: spy check {Player}";
                return false;
            }

            if (!(Player.Get(arguments.At(0)) is { } player))
            {
                response = "Could not find the specified player.";
                return false;
            }

            response = player.IsSpy(out Spy spy) ? $"{player.Nickname} is a spy with the name of \"{spy.Name}\"." : $"{player.Nickname} is not a spy!";
            return true;
        }
    }
}