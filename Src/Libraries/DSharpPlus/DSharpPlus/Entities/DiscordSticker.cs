using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DSharpPlus.Entities
{
    public class DiscordSticker : SnowflakeObject
    {
        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("description")]
        public string Description { get; internal set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public string Tags { get; internal set; }

        [JsonProperty("pack_id")]
        public ulong PackId { get; internal set; }

        [JsonProperty("asset")]
        public string AssetHash { get; internal set; }
        
        [JsonProperty("preview_asset")]
        public string PreviewAssetHash { get; internal set; }

        [JsonProperty("format_type")]
        public DiscordStickerType StickerType { get; set; }
    
        public string GetAssetUrl()
        {
            string extension;
            switch (StickerType)
            {
                case DiscordStickerType.APNG:
                case DiscordStickerType.PNG:
                    extension = ".png";
                    break;

                case DiscordStickerType.Lottie:
                    extension = ".json";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return $"https://discord.com/stickers/{Id}/{AssetHash}{extension}";
        } 
    }

    public enum DiscordStickerType
    {
        PNG = 1, APNG, Lottie
    }
}
