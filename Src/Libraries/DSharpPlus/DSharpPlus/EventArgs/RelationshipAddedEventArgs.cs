using DSharpPlus.Entities;

namespace DSharpPlus.EventArgs
{
    /// <summary>
    /// Represents arguments for <see cref="DiscordClient.Ready"/> event.
    /// </summary>
    public sealed class RelationshipAddedEventArgs : DiscordEventArgs
    {
        internal DiscordRelationship Relationship;

        internal RelationshipAddedEventArgs(DiscordClient client) : base(client) { }
    }
}
