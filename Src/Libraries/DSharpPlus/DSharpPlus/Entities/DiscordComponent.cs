using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DSharpPlus.Entities
{
    public enum DiscordInteractionType
    {
        ActionRow = 1,
        Button
    }

    public enum DiscordButtonStyle
    {
        Primary = 1,
        Secondary,
        Success,
        Danger,
        Link
    }

    public class DiscordComponent
    {
        [JsonProperty("type")]
        public DiscordInteractionType Type { get; set; }

        [JsonProperty("components", NullValueHandling = NullValueHandling.Include)]
        public DiscordComponent[] Components { get; set; }


        [JsonProperty("style", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordButtonStyle? ButtonStyle { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty("emoji", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordEmoji Emoji { get; set; }

        [JsonProperty("custom_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CustomId { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("disabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Disabled { get; set; }
    }
}
