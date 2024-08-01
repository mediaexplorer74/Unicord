﻿using System;
using static Unicord.Constants;

namespace Unicord.Universal.Models
{
    class SecuritySettingsModel : ViewModelBase
    {
        public bool HelloForLogin
        {
            get => App.RoamingSettings.Read(VERIFY_LOGIN, false);
            set => App.RoamingSettings.Save(VERIFY_LOGIN, value);
        }

        public bool HelloForNSFW
        {
            get => App.RoamingSettings.Read(VERIFY_NSFW, false);
            set => App.RoamingSettings.Save(VERIFY_NSFW, value);
        }

        public bool HelloForSettings
        {
            get => App.RoamingSettings.Read(VERIFY_SETTINGS, false);
            set => App.RoamingSettings.Save(VERIFY_SETTINGS, value);
        }

        public bool EnableAnalytics
        {
            get => App.RoamingSettings.Read(ENABLE_ANALYTICS, true);
            set => App.RoamingSettings.Save(ENABLE_ANALYTICS, value);
        }

        public TimeSpan AuthenticationTime
        {
            get => App.RoamingSettings.Read(AUTHENTICATION_TIME, TimeSpan.FromMinutes(5));
            set => App.RoamingSettings.Save(AUTHENTICATION_TIME, value);
        }
    }
}
