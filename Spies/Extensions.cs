namespace Spies
{
    using System.Linq;
    using Exiled.API.Extensions;
    using Exiled.API.Features;

    /// <summary>
    /// Various extension methods for aid in handling spies.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Changes the <see cref="Player"/>'s appearance to all other <see cref="Player"/>s.
        /// Nabbed from <see cref="Exiled.API.Extensions.MirrorExtensions"/> and edited to ignore users without a UserId.
        /// </summary>
        /// <param name="player">The player to change the appearance of.</param>
        /// <param name="roleType">The role for the player to be viewed as.</param>
        public static void ChangeAppearance(this Player player, RoleType roleType)
        {
            foreach (Player target in Player.List.Where(x => x != player && !string.IsNullOrEmpty(x.UserId)))
            {
                target.SendFakeSyncVar(player.ReferenceHub.networkIdentity, typeof(CharacterClassManager), nameof(CharacterClassManager.NetworkCurClass), (sbyte)roleType);
            }
        }

        /// <summary>
        /// Checks if the specified <see cref="Player"/> is a Spy.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to check.</param>
        /// <param name="spy">The type of <see cref="Spy"/> that the user is, if they are one.</param>
        /// <returns>If the user is a spy.</returns>
        public static bool IsSpy(this Player player, out Spy spy)
        {
            bool isSpy = player.SessionVariables.TryGetValue("IsSpy", out object spyType);

            if (isSpy && spyType is Spy type)
            {
                spy = type;
            }
            else
            {
                spy = null;
            }

            return isSpy && spy != null;
        }
    }
}