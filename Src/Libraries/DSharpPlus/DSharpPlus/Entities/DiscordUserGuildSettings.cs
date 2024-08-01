using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Net.Serialization;
using Newtonsoft.Json;
using Unicord;

namespace DSharpPlus.Entities
{
    public enum DiscordMessageNotifications
    {
        All,
        OnlyAtMentions,
        Nothing
    }

    public class DiscordUserChannelSettings
    {
        [JsonProperty("channel_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong ChannelId { get; internal set; }

        [JsonProperty("muted")]
        public bool Muted { get; internal set; }

        [JsonProperty("mute_config")]
        public DiscordMuteConfig MuteConfig { get; internal set; }

        [JsonProperty("message_notifications")]
        public DiscordMessageNotifications MessageNotifications { get; internal set; }
    }

    public class DiscordUserGuildSettings : NotifyPropertyChangeImpl
    {
        [JsonProperty("version")]
        public int Version { get; internal set; }

        [JsonProperty("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; internal set; }

        [JsonProperty("suppress_roles")]
        public bool SuppressRoles { get; internal set; }

        [JsonProperty("suppress_everyone")]
        public bool SuppressEveryone { get; internal set; }

        [JsonProperty("muted")]
        public bool Muted { get; internal set; }

        [JsonProperty("mute_config")]
        public DiscordMuteConfig MuteConfig { get; internal set; }

        [JsonProperty("message_notifications")]
        public DiscordMessageNotifications MessageNotifications { get; internal set; }

        [JsonProperty("mobile_push")]
        public bool MobilePushNotifications { get; internal set; }

        [JsonProperty("channel_overrides")]
        public List<DiscordUserChannelSettings> ChannelOverrides { get; internal set; }
            = new List<DiscordUserChannelSettings>();
    }

    public class DiscordMuteConfig
    {
        [JsonProperty("selected_time_window", NullValueHandling = NullValueHandling.Include)]
        public int? SelectedTimeWindow { get; internal set; }

        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public Optional<DateTimeOffset> EndTime { get; internal set; }
    }
}
