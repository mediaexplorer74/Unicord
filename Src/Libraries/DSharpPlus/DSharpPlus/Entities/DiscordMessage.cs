#pragma warning disable CS0618
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DSharpPlus.Entities
{
    /// <summary>
    /// Represents a Discord text message.
    /// </summary>
    public class DiscordMessage : SnowflakeObject, IEquatable<DiscordMessage>
    {
        private string _content;
        private string _editedTimestampRaw;
        private bool _pinned;
        private MessageType? _messageType;

        [JsonIgnore]
        internal List<DiscordChannel> _mentionedChannels;

        [JsonProperty("reactions", NullValueHandling = NullValueHandling.Ignore)]
        internal List<DiscordReaction> _reactions = new List<DiscordReaction>();

        [JsonProperty("mentions", NullValueHandling = NullValueHandling.Ignore)]
        internal List<DiscordUser> _mentionedUsers;
        [JsonProperty("mention_roles", NullValueHandling = NullValueHandling.Ignore)]
        internal List<ulong> _mentionedRoleIds;

        [JsonProperty("attachments", NullValueHandling = NullValueHandling.Ignore)]
        internal List<DiscordAttachment> _attachments = new List<DiscordAttachment>();
        [JsonProperty("embeds", NullValueHandling = NullValueHandling.Ignore)]
        internal List<DiscordEmbed> _embeds = new List<DiscordEmbed>();
        [JsonProperty("stickers", NullValueHandling = NullValueHandling.Ignore)]
        internal List<DiscordSticker> _stickers = new List<DiscordSticker>();

        [JsonProperty("components", NullValueHandling = NullValueHandling.Ignore)]
        internal DiscordComponent[] _components;

        public DiscordMessage()
        {
        }

        internal DiscordMessage(DiscordMessage other)
            : this()
        {
            this.Discord = other.Discord;

            this._attachments = other._attachments; // the attachments cannot change, thus no need to copy and reallocate.
            this._embeds = new List<DiscordEmbed>(other._embeds);

            if (other._mentionedRoleIds != null)
                this._mentionedRoleIds = new List<ulong>(other._mentionedRoleIds);
            if (other._mentionedChannels != null)
                this._mentionedChannels = new List<DiscordChannel>(other._mentionedChannels);

            this._mentionedUsers = new List<DiscordUser>(other._mentionedUsers);
            this._reactions = new List<DiscordReaction>(other._reactions);

            this.Author = other.Author;
            this.ChannelId = other.ChannelId;
            this.Content = other.Content;
            this.EditedTimestampRaw = other.EditedTimestampRaw;
            this.Id = other.Id;
            this.IsTTS = other.IsTTS;
            this.MessageType = other.MessageType;
            this.Pinned = other.Pinned;
            this.TimestampRaw = other.TimestampRaw;
            this.WebhookId = other.WebhookId;
        }

        /// <summary> 
        /// Gets the channel in which the message was sent.
        /// </summary>
        [JsonIgnore]
        public virtual DiscordChannel Channel
            => (this.Discord as DiscordClient)?.InternalGetCachedChannel(this.ChannelId);

        /// <summary>
        /// Gets the ID of the channel in which the message was sent.
        /// </summary>
        [JsonProperty("channel_id", NullValueHandling = NullValueHandling.Ignore)]
        public ulong ChannelId { get; internal set; }

        /// <summary>
        /// Gets the user or member that sent the message.
        /// </summary>
        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public virtual DiscordUser Author { get; internal set; }

        /// <summary>
        /// Gets the message's content.
        /// </summary>
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Content { get => _content; internal set => OnPropertySet(ref _content, value); }

        /// <summary>
        /// Gets the message's creation timestamp.
        /// </summary>
        [JsonIgnore]
        public virtual DateTimeOffset Timestamp
            => !string.IsNullOrWhiteSpace(this.TimestampRaw) && DateTimeOffset.TryParse(this.TimestampRaw, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dto) ?
                dto : this.CreationTimestamp;

        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        internal string TimestampRaw { get; set; }

        /// <summary>
        /// Gets the message's edit timestamp. Will be null if the message was not edited.
        /// </summary>
        [JsonIgnore]
        public DateTimeOffset? EditedTimestamp
            => !string.IsNullOrWhiteSpace(this.EditedTimestampRaw) && DateTimeOffset.TryParse(this.EditedTimestampRaw, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dto) ?
                (DateTimeOffset?)dto : null;

        [JsonProperty("edited_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        internal string EditedTimestampRaw
        {
            get => _editedTimestampRaw;
            set => OnPropertySet(ref _editedTimestampRaw, value, nameof(EditedTimestamp), nameof(IsEdited));
        }

        /// <summary>
        /// Gets whether this message was edited.
        /// </summary>
        [JsonIgnore]
        public bool IsEdited
            => !string.IsNullOrWhiteSpace(this.EditedTimestampRaw);

        /// <summary>
        /// Gets whether the message is a text-to-speech message.
        /// </summary>
        [JsonProperty("tts", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsTTS { get; internal set; }

        /// <summary>
        /// Gets whether the message mentions everyone.
        /// </summary>
        [JsonProperty("mention_everyone", NullValueHandling = NullValueHandling.Ignore)]
        public bool MentionEveryone { get; internal set; }

        /// <summary>
        /// Gets users or members mentioned by this message.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<DiscordUser> MentionedUsers
            => this._mentionedUsers;

        // TODO this will probably throw an exception in DMs since it tries to wrap around a null List...
        // this is probably low priority but need to find out a clean way to solve it...
        /// <summary>
        /// Gets roles mentioned by this message.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<ulong> MentionedRoleIds
            => this._mentionedRoleIds != null && this.Channel?.Guild != null ? _mentionedRoleIds : (IReadOnlyList<ulong>)Array.Empty<ulong>();

        /// <summary>
        /// Gets channels mentioned by this message.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<DiscordChannel> MentionedChannels
            => this._mentionedChannels ?? (IReadOnlyList<DiscordChannel>)Array.Empty<DiscordChannel>();

        /// <summary>
        /// Gets files attached to this message.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<DiscordAttachment> Attachments
            => this._attachments;

        /// <summary>
        /// Gets embeds attached to this message.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<DiscordEmbed> Embeds
            => this._embeds;

        /// <summary>
        /// Gets reactions used on this message.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<DiscordReaction> Reactions
            => this._reactions;

        /// <summary>
        /// Gets users or members mentioned by this message.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<DiscordComponent> Components
            => this._components;

        /*
        /// <summary>
        /// Gets the nonce sent with the message, if the message was sent by the client.
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? Nonce { get; internal set; }
        */

        /// <summary>
        /// Gets whether the message is pinned.
        /// </summary>
        [JsonProperty("pinned", NullValueHandling = NullValueHandling.Ignore)]
        public bool Pinned { get => _pinned; internal set => OnPropertySet(ref _pinned, value); }

        /// <summary>
        /// Gets the id of the webhook that generated this message.
        /// </summary>
        [JsonProperty("webhook_id", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? WebhookId { get; internal set; }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public MessageType? MessageType { get => _messageType; internal set => OnPropertySet(ref _messageType, value); }

        /// <summary>
        /// Gets the message activity in the Rich Presence embed.
        /// </summary>
        [JsonProperty("activity", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordMessageActivity Activity { get; internal set; }

        /// <summary>
        /// Gets the message application in the Rich Presence embed.
        /// </summary>
        [JsonProperty("application", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordMessageApplication Application { get; internal set; }

        [JsonProperty("message_reference", NullValueHandling = NullValueHandling.Ignore)]
        internal InternalDiscordMessageReference? _reference { get; set; }


        /// <summary>
        /// Gets the original message reference from the crossposted message.
        /// </summary>
        [JsonIgnore]
        public DiscordMessageReference Reference
            => (this._reference.HasValue) ? this?.InternalBuildMessageReference() : null;

        /// <summary>
        /// Gets the bitwise flags for this message.
        /// </summary>
        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public MessageFlags? Flags { get; internal set; }

        /// <summary>
        /// Gets the message this message is a response to. Only applies if <see cref="MessageType"/> == <see cref="MessageType.Reply"/>
        /// </summary>
        [JsonProperty("referenced_message", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordMessage ReferencedMessage { get; internal set; }

        /// <summary>
        /// Is this message a hit in search results?
        /// </summary>
        [JsonProperty("hit", NullValueHandling = NullValueHandling.Ignore)]
        public Optional<bool> Hit { get; internal set; }

        /// <summary>
        /// Gets reactions used on this message.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<DiscordSticker> Stickers
            => this._stickers;

        /// <summary>
        /// Gets whether the message originated from a webhook.
        /// </summary>
        [JsonIgnore]
        public bool WebhookMessage
            => this.WebhookId != null;

        /// <summary>
        /// Gets the jump link to this message.
        /// </summary>
        [JsonIgnore]
        public Uri JumpLink
        {
            get
            {
                var gid = this.Channel is DiscordDmChannel ? "@me" : this.Channel.GuildId.ToString(CultureInfo.InvariantCulture);
                var cid = this.ChannelId.ToString(CultureInfo.InvariantCulture);
                var mid = this.Id.ToString(CultureInfo.InvariantCulture);

                return new Uri($"https://discord.com/channels/{gid}/{cid}/{mid}");
            }
        }

        internal DiscordMessageReference InternalBuildMessageReference()
        {
            var client = this.Discord as DiscordClient;
            var guildId = this._reference.Value.guildId;
            var channelId = this._reference.Value.channelId;
            var messageId = this._reference.Value.messageId;

            var reference = new DiscordMessageReference();

            if (guildId.HasValue)
                if (client._guilds.TryGetValue(guildId.Value, out var g))
                    reference.Guild = g;

                else reference.Guild = new DiscordGuild
                {
                    Id = guildId.Value,
                    Discord = client
                };

            if (channelId.HasValue)
            {
                var channel = client.InternalGetCachedChannel(channelId.Value);

                if (channel == null)
                {
                    reference.Channel = new DiscordChannel
                    {
                        Id = channelId.Value,
                        Discord = client
                    };

                    if (guildId.HasValue)
                        reference.Channel.GuildId = guildId.Value;
                }
                else reference.Channel = channel;
            }

            if (client.MessageCache.TryGet(m => m.Id == messageId.Value && m.ChannelId == channelId, out var msg))
                reference.Message = msg;

            else
            {
                reference.Message = new DiscordMessage
                {
                    ChannelId = ChannelId,
                    Discord = client
                };

                if (messageId.HasValue)
                    reference.Message.Id = messageId.Value;
            }

            return reference;
        }

        /// <summary>
        /// Edits the message.
        /// </summary>
        /// <param name="content">New content.</param>
        /// <param name="embed">New embed.</param>
        /// <returns></returns>
        public Task<DiscordMessage> ModifyAsync(Optional<string> content = default, Optional<DiscordEmbed> embed = default)
            => this.Discord.ApiClient.EditMessageAsync(this.ChannelId, this.Id, content, embed);

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <returns></returns>
        public Task DeleteAsync(string reason = null)
            => this.Discord.ApiClient.DeleteMessageAsync(this.ChannelId, this.Id, reason);

        /// <summary>
        /// Modifes the visibility of embeds in this message.
        /// </summary>
        /// <param name="hideEmbeds">Whether to hide all embeds.</param>
        /// <returns></returns>
        public Task ModifyEmbedSuppressionAsync(bool hideEmbeds)
            => this.Discord.ApiClient.ModifyEmbedSuppressionAsync(hideEmbeds, this.ChannelId, this.Id);

        /// <summary>
        /// Pins the message in its channel.
        /// </summary>
        /// <returns></returns>
        public Task PinAsync()
            => this.Discord.ApiClient.PinMessageAsync(this.ChannelId, this.Id);

        /// <summary>
        /// Unpins the message in its channel.
        /// </summary>
        /// <returns></returns>
        public Task UnpinAsync()
            => this.Discord.ApiClient.UnpinMessageAsync(this.ChannelId, this.Id);

        /// <summary>
        /// Responds to the message.
        /// </summary>
        /// <param name="content">Message content to respond with.</param>
        /// <param name="tts">Whether the message is to be read using TTS.</param>
        /// <param name="embed">Embed to attach to the message.</param>
        /// <param name="mentions">Allowed mentions in the message</param>
        /// <returns>The sent message.</returns>
        public Task<DiscordMessage> RespondAsync(string content = null, bool tts = false, DiscordEmbed embed = null, IEnumerable<IMention> mentions = null, DiscordMessage replyTo = null)
            => this.Discord.ApiClient.CreateMessageAsync(this.ChannelId, content, tts, embed, mentions, replyTo.Id);

        /// <summary>
        /// Responds to the message with a file.
        /// </summary>
        /// <param name="fileName">Name of the file to be attached.</param>
        /// <param name="fileData">Stream containing the data to attach to the message as a file.</param>
        /// <param name="content">Message content to respond with.</param>
        /// <param name="tts">Whether the message is to be read using TTS.</param>
        /// <param name="embed">Embed to attach to the message.</param>
        /// <param name="mentions">Allowed mentions in the message</param>
        /// <returns>The sent message.</returns>
        public Task<DiscordMessage> RespondWithFileAsync(string fileName, Stream fileData, string content = null, bool tts = false, DiscordEmbed embed = null, IEnumerable<IMention> mentions = null)
            => this.Discord.ApiClient.UploadFileAsync(this.ChannelId, fileData, fileName, content, tts, embed, mentions);

        /// <summary>
        /// Responds to the message with a file.
        /// </summary>
        /// <param name="fileData">Stream containing the data to attach to the message as a file.</param>
        /// <param name="content">Message content to respond with.</param>
        /// <param name="tts">Whether the message is to be read using TTS.</param>
        /// <param name="embed">Embed to attach to the message.</param>
        /// <param name="mentions">Allowed mentions in the message</param>
        /// <returns>The sent message.</returns>
        public Task<DiscordMessage> RespondWithFileAsync(FileStream fileData, string content = null, bool tts = false, DiscordEmbed embed = null, IEnumerable<IMention> mentions = null)
            => this.Discord.ApiClient.UploadFileAsync(this.ChannelId, fileData, Path.GetFileName(fileData.Name), content, tts, embed, mentions);

        /// <summary>
        /// Responds to the message with a file.
        /// </summary>
        /// <param name="filePath">Path to the file to be attached to the message.</param>
        /// <param name="content">Message content to respond with.</param>
        /// <param name="tts">Whether the message is to be read using TTS.</param>
        /// <param name="embed">Embed to attach to the message.</param>
        /// <param name="mentions">Allowed mentions in the message</param>
        /// <returns>The sent message.</returns>
        public async Task<DiscordMessage> RespondWithFileAsync(string filePath, string content = null, bool tts = false, DiscordEmbed embed = null, IEnumerable<IMention> mentions = null)
        {
            using (var fs = File.OpenRead(filePath))
                return await this.Discord.ApiClient.UploadFileAsync(this.ChannelId, fs, Path.GetFileName(fs.Name), content, tts, embed, mentions).ConfigureAwait(false);
        }

        /// <summary>
        /// Responds to the message with several files.
        /// </summary>
        /// <param name="files">A filename to data stream mapping.</param>
        /// <param name="content">Message content to respond with.</param>
        /// <param name="tts">Whether the message is to be read using TTS.</param>
        /// <param name="embed">Embed to attach to the message.</param>
        /// <param name="mentions">Allowed mentions in the message</param>
        /// <returns>The sent message.</returns>
        public Task<DiscordMessage> RespondWithFilesAsync(Dictionary<string, Stream> files, string content = null, bool tts = false, DiscordEmbed embed = null, IEnumerable<IMention> mentions = null)
        {
            if (files.Count > 10)
                throw new ArgumentException("Cannot send more than 10 files with a single message.");

            return this.Discord.ApiClient.UploadFilesAsync(this.ChannelId, files, content, tts, embed, mentions);
        }

        /// <summary>
        /// Creates a reaction to this message
        /// </summary>
        /// <param name="emoji">The emoji you want to react with, either an emoji or name:id</param>
        /// <returns></returns>
        public Task CreateReactionAsync(DiscordEmoji emoji)
            => this.Discord.ApiClient.CreateReactionAsync(this.ChannelId, this.Id, emoji.ToReactionString());

        /// <summary>
        /// Deletes your own reaction
        /// </summary>
        /// <param name="emoji">Emoji for the reaction you want to remove, either an emoji or name:id</param>
        /// <returns></returns>
        public Task DeleteOwnReactionAsync(DiscordEmoji emoji)
            => this.Discord.ApiClient.DeleteOwnReactionAsync(this.ChannelId, this.Id, emoji.ToReactionString());

        /// <summary>
        /// Deletes another user's reaction.
        /// </summary>
        /// <param name="emoji">Emoji for the reaction you want to remove, either an emoji or name:id.</param>
        /// <param name="user">Member you want to remove the reaction for</param>
        /// <param name="reason">Reason for audit logs.</param>
        /// <returns></returns>
        public Task DeleteReactionAsync(DiscordEmoji emoji, DiscordUser user, string reason = null)
            => this.Discord.ApiClient.DeleteUserReactionAsync(this.ChannelId, this.Id, user.Id, emoji.ToReactionString(), reason);

        /// <summary>
        /// Gets users that reacted with this emoji.
        /// </summary>
        /// <param name="emoji">Emoji to react with.</param>
        /// <param name="limit">Limit of users to fetch.</param>
        /// <param name="after">Fetch users after this user's id.</param>
        /// <returns></returns>
        public Task<IReadOnlyList<DiscordUser>> GetReactionsAsync(DiscordEmoji emoji, int limit = 25, ulong? after = null)
            => this.GetReactionsInternalAsync(emoji, limit, after);

        /// <summary>
        /// Deletes all reactions for this message.
        /// </summary>
        /// <param name="reason">Reason for audit logs.</param>
        /// <returns></returns>
        public Task DeleteAllReactionsAsync(string reason = null)
            => this.Discord.ApiClient.DeleteAllReactionsAsync(this.ChannelId, this.Id, reason);

        /// <summary>
        /// Deletes all reactions of a specific reaction for this message.
        /// </summary>
        /// <param name="emoji">The emoji to clear, either an emoji or name:id.</param>
        /// <returns></returns>
        public Task DeleteReactionsEmojiAsync(DiscordEmoji emoji)
            => this.Discord.ApiClient.DeleteReactionsEmojiAsync(this.ChannelId, this.Id, emoji.ToReactionString());

        /// <summary>
        /// Acknowledges the message. This is available to user tokens only.
        /// </summary>
        /// <returns></returns>
        public Task AcknowledgeAsync()
        {
            if (Discord.Configuration.TokenType == TokenType.User)
            {
                return Discord.ApiClient.AcknowledgeMessageAsync(Id, Channel.Id);
            }

            throw new InvalidOperationException("ACK can only be used when logged in as regular user.");
        }

        private async Task<IReadOnlyList<DiscordUser>> GetReactionsInternalAsync(DiscordEmoji emoji, int limit = 25, ulong? after = null)
        {
            if (limit < 0)
                throw new ArgumentException("Cannot get a negative number of reactions' users.");

            if (limit == 0)
                return new DiscordUser[0];

            var users = new List<DiscordUser>(limit);
            var remaining = limit;
            var last = after;

            int lastCount;
            do
            {
                var fetchSize = remaining > 100 ? 100 : remaining;
                var fetch = await this.Discord.ApiClient.GetReactionsAsync(this.Channel.Id, this.Id, emoji.ToReactionString(), last, fetchSize).ConfigureAwait(false);

                lastCount = fetch.Count;
                remaining -= lastCount;

                users.AddRange(fetch);
                last = fetch.LastOrDefault()?.Id;
            } while (remaining > 0 && lastCount > 0);

            return new ReadOnlyCollection<DiscordUser>(users);
        }

        /// <summary>
        /// Returns a string representation of this message.
        /// </summary>
        /// <returns>String representation of this message.</returns>
        public override string ToString()
        {
            return $"Message {this.Id}; Attachment count: {this._attachments.Count}; Embed count: {this._embeds.Count}; Contents: {this.Content}";
        }

        /// <summary>
        /// Checks whether this <see cref="DiscordMessage"/> is equal to another object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>Whether the object is equal to this <see cref="DiscordMessage"/>.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as DiscordMessage);
        }

        /// <summary>
        /// Checks whether this <see cref="DiscordMessage"/> is equal to another <see cref="DiscordMessage"/>.
        /// </summary>
        /// <param name="e"><see cref="DiscordMessage"/> to compare to.</param>
        /// <returns>Whether the <see cref="DiscordMessage"/> is equal to this <see cref="DiscordMessage"/>.</returns>
        public bool Equals(DiscordMessage e)
        {
            if (e is null)
                return false;

            if (ReferenceEquals(this, e))
                return true;

            return this.Id == e.Id && this.ChannelId == e.ChannelId;
        }

        /// <summary>
        /// Gets the hash code for this <see cref="DiscordMessage"/>.
        /// </summary>
        /// <returns>The hash code for this <see cref="DiscordMessage"/>.</returns>
        public override int GetHashCode()
        {
            int hash = 13;

            hash = (hash * 7) + this.Id.GetHashCode();
            hash = (hash * 7) + this.ChannelId.GetHashCode();

            return hash;
        }

        /// <summary>
        /// Gets whether the two <see cref="DiscordMessage"/> objects are equal.
        /// </summary>
        /// <param name="e1">First message to compare.</param>
        /// <param name="e2">Second message to compare.</param>
        /// <returns>Whether the two messages are equal.</returns>
        public static bool operator ==(DiscordMessage e1, DiscordMessage e2)
        {
            var o1 = e1 as object;
            var o2 = e2 as object;

            if ((o1 == null && o2 != null) || (o1 != null && o2 == null))
                return false;

            if (o1 == null && o2 == null)
                return true;

            return e1.Id == e2.Id && e1.ChannelId == e2.ChannelId;
        }

        /// <summary>
        /// Gets whether the two <see cref="DiscordMessage"/> objects are not equal.
        /// </summary>
        /// <param name="e1">First message to compare.</param>
        /// <param name="e2">Second message to compare.</param>
        /// <returns>Whether the two messages are not equal.</returns>
        public static bool operator !=(DiscordMessage e1, DiscordMessage e2)
            => !(e1 == e2);
    }
}
