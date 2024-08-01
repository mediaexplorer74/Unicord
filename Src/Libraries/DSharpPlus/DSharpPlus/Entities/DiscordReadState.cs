using System;
using Newtonsoft.Json;

namespace DSharpPlus.Entities
{
    public class DiscordReadState : SnowflakeObject
    {
        private int _mentionCount;
        private ulong _lastMessageId;
        private DateTimeOffset _lastPinTimestamp;

        [JsonIgnore]
        public bool Unread
        {
            get
            {
                // this shit should never happen but apparently it does sometimes, don't question it
                if (Id == 0)
                    return false;

                if (Discord == null || !(Discord is DiscordClient client) || Discord.IsDisposed)
                    return false;

                var channel = client.InternalGetCachedChannel(Id);
                if (channel == null)
                    return false;

                if (channel.Type != ChannelType.Voice && channel.Type != ChannelType.Category && channel.Type != ChannelType.Store)
                {
                    if (channel.Muted)
                        return false;

                    if (channel.Type == ChannelType.Private || channel.Type == ChannelType.Group)
                    {
                        return MentionCount > 0;
                    }

                    return (MentionCount > 0 || (channel.LastMessageId != 0 && channel.LastMessageId > _lastMessageId));
                }
                else
                {
                    return false;
                }
            }
        }

        [JsonProperty("mention_count")]
        public int MentionCount { get => _mentionCount; internal set => OnPropertySet(ref _mentionCount, value, nameof(MentionCount), nameof(NullableMentionCount), nameof(Unread)); }

        [JsonIgnore]
        public int NullableMentionCount => MentionCount == 0 ? -1 : MentionCount;

        [JsonProperty("last_message_id")]
        public ulong LastMessageId { get => _lastMessageId; internal set => OnPropertySet(ref _lastMessageId, value, nameof(LastMessageId), nameof(Unread)); }

        [JsonProperty("last_pin_timestamp")]
        public DateTimeOffset LastPinTimestamp { get => _lastPinTimestamp; internal set => OnPropertySet(ref _lastPinTimestamp, value); }
    }
}