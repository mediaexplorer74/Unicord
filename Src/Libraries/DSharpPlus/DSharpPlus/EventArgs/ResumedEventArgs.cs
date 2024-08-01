namespace DSharpPlus.EventArgs
{
    /// <summary>
    /// Represents arguments for <see cref="DiscordClient.Ready"/> event.
    /// </summary>
    public sealed class ResumedEventArgs : DiscordEventArgs
    {
        internal ResumedEventArgs(DiscordClient client) : base(client) { }
    }
}
