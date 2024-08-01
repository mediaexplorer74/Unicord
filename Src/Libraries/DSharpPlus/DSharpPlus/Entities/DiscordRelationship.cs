using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Net.Abstractions;
using Newtonsoft.Json;
using Unicord;

namespace DSharpPlus.Entities
{
    public class DiscordRelationship : NotifyPropertyChangeImpl, IComparable
    {
        private DiscordRelationshipType _relationshipType;

        [JsonIgnore]
        internal DiscordClient Discord { get; set; }

        [JsonProperty("user")]
        internal TransportUser InternalUser { get; set; }

        /// <summary>
        /// The Id of the user this relationship is associated with
        /// </summary>
        [JsonProperty("id")]
        public ulong Id { get; internal set; }

        /// <summary>
        /// This is hard to explain.
        /// </summary>
        [JsonIgnore]
        public DiscordUser User
            => Discord.TryGetCachedUserInternal(Id, out var user) ? user : null;

        [JsonProperty("type")]
        public DiscordRelationshipType RelationshipType { get => _relationshipType; internal set => OnPropertySet(ref _relationshipType, value); }

        int IComparable.CompareTo(object obj) => (InternalUser?.Username as IComparable).CompareTo(obj is DiscordRelationship r ? r.InternalUser?.Username : obj);
    }
}
