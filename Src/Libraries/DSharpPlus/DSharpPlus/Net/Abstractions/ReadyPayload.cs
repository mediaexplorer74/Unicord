using System.Collections.Generic;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DSharpPlus.Net.Abstractions
{
    /// <summary>
    /// Represents data for websocket ready event payload.
    /// </summary>
    internal class ReadyPayload
    {
        /// <summary>
        /// Gets the gateway version the client is connectected to.
        /// </summary>
        [JsonProperty("v")]
        public int GatewayVersion { get; private set; }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        [JsonProperty("user")]
        public TransportUser CurrentUser { get; private set; }

        /// <summary>
        /// Gets the private channels available for this shard.
        /// </summary>
        [JsonProperty("private_channels")]
        public IReadOnlyList<DiscordDmChannel> DmChannels { get; private set; }

        /// <summary>
        /// Gets the guilds available for this shard.
        /// </summary>
        [JsonProperty("guilds")]
        public IReadOnlyList<DiscordGuild> Guilds { get; private set; }

        /// <summary>
        /// Gets the relationships available for this shard.
        /// </summary>
        [JsonProperty("relationships", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<DiscordRelationship> Relationships { get; private set; }

        /// <summary>
        /// Gets the presences available for this shard.
        /// </summary>
        [JsonProperty("presences", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<JObject> Presences { get; private set; }

        /// <summary>
        /// Gets the relationships available for this shard.
        /// </summary>
        [JsonProperty("read_state", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<DiscordReadState> ReadStates { get; private set; }

        /// <summary>
        /// Gets the user settings for this shard
        /// </summary>
        [JsonProperty("user_settings", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordUserSettings UserSettings { get; set; }


        [JsonProperty("user_guild_settings", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<DiscordUserGuildSettings> UserGuildSettings { get; set; }

        /// <summary>
        /// Gets the current session's ID.
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; private set; }

        /// <summary>
        /// Gets debug data sent by Discord. This contains a list of servers to which the client is connected.
        /// </summary>
        [JsonProperty("_trace")]
        public IReadOnlyList<string> Trace { get; private set; }
    }
}
