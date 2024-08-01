using DSharpPlus.Entities;

namespace DSharpPlus.EventArgs
{
    /// <summary>
    /// Represents arguments for <see cref="DiscordClient.Ready"/> event.
    /// </summary>
    public sealed class RelationshipRemovedEventArgs : DiscordEventArgs
    {
        internal DiscordRelationship Relationship;

        internal RelationshipRemovedEventArgs(DiscordClient client) : base(client) { }
    }
}
