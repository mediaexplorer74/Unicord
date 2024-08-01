using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpPlus.Entities.Mock
{
    internal class MockUser : DiscordUser
    {
        internal MockUser(DiscordClient discord, string username, string discriminator)
        {
            Username = username;
            Discriminator = discriminator;
            Discord = discord;
        }

        public override string Username { get; internal set; }
        public override string Discriminator { get; internal set; }
        public override string AvatarUrl => "ms-appx:///Assets/example-avatar.png";
        public override string NonAnimatedAvatarUrl => "ms-appx:///Assets/example-avatar.png";
    }

    internal class MockChannel : DiscordChannel
    {
        internal MockChannel(DiscordClient discord, string name, ChannelType type, string topic)
        {
            Name = name;
            Type = type;
            Topic = topic;
            Discord = discord;
        }

        public override string Name { get; internal set; }
        public override ChannelType Type { get; internal set; }
        public override string Topic { get; internal set; }
    }

    internal class MockMessage : DiscordMessage
    {
        internal MockMessage(DiscordClient discord, string content, DiscordUser author, DiscordChannel channel = null, DateTimeOffset timestamp = default)
        {
            Content = content;
            Author = author;
            Channel = channel;
            Timestamp = timestamp;
            Discord = discord;
        }

        public override DiscordUser Author { get; internal set; }
        public override string Content { get; internal set; }

        public override DiscordChannel Channel { get; }
        public override DateTimeOffset Timestamp { get; }

        internal void NotifyAllChanged()
        {
            InvokePropertyChanged("");
            InvokePropertyChanged(nameof(Author));
            InvokePropertyChanged(nameof(Content));
            InvokePropertyChanged(nameof(Channel));
            InvokePropertyChanged(nameof(Timestamp));
        }
    }
}
