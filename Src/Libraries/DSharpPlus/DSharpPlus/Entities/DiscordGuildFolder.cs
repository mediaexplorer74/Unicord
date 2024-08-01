using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpPlus.Entities
{
    /// <summary>
    /// Represents a folder of <see cref="DiscordGuild"/>s
    /// </summary>
    public class DiscordGuildFolder
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("id")]
        public long? Id { get; private set; }

        [JsonProperty("guild_ids")]
        private List<ulong> _guildIds;

        [JsonIgnore]
        public IReadOnlyList<ulong> GuildIds => _guildIds;

        [JsonProperty("color")]
        private int? _color;

        [JsonIgnore]
        public DiscordColor? Color => _color.HasValue ? new DiscordColor(_color.Value) : default;
    
        public bool IsValid()
        {
            return Name != null && Id != null;
        }
    }
}
