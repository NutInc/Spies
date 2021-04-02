namespace Spies.Commands
{
#pragma warning disable SA1101
    using System;
    using System.Text;
    using CommandSystem;
    using NorthwoodLib.Pools;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpyParent : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpyParent"/> class.
        /// </summary>
        public SpyParent() => LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "spy";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc/>
        public override string Description { get; } = "Parent command for the Spies plugin.";

        /// <inheritdoc/>
        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Check());
            RegisterCommand(new Spawn());
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Please enter a valid subcommand! Available:");
            foreach (ICommand command in AllCommands)
            {
                stringBuilder.AppendLine(command.Aliases.Length > 0
                    ? $"{command.Command} | Aliases: {string.Join(", ", command.Aliases)}"
                    : command.Command);
            }

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder);
            return false;
        }
    }
}