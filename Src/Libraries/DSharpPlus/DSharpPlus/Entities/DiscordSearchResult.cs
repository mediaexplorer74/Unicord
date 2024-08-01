using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DSharpPlus.Entities
{
    [Flags]
    public enum DiscordSearchFlags
    {
        None,
        Image = 1,
        Sound = 2,
        Video = 4,
        Embed = 8,
        Link = 16,
        File = 32
    }

    public class DiscordSearchResult
    {
        public bool IsIndexed { get; internal set; } = true;

        public int TotalResults { get; internal set; }

        public IReadOnlyList<IReadOnlyList<DiscordMessage>> Messages { get; internal set; }
    }
}
