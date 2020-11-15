using MadMilkman.Ini;
using System;
using System.IO;
using System.Reflection;

namespace ServerUpdater
{
    class Settings
    {
        #region Singleton

        private static readonly Lazy<Settings> _instance = new Lazy<Settings>(() => new Settings());

        public static Settings Instance
        {
            get { return _instance.Value; }
        }

        #endregion // Singleton

        private static readonly string ApplicationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private static readonly string IniFile = Path.Combine(ApplicationPath, "ServerUpdater.ini");
        private readonly IniFile ini;

        public int CheckInterval
        {
            get
            {
                string sVal = ini.Sections["UpdateChecker"].Keys["CheckInterval"].Value;
                int iVal = string.IsNullOrEmpty(sVal) ? 10 : int.Parse(sVal);
                return (iVal >= 5 && iVal <= 60) ? iVal : 10;
            }
            set
            {
                if (value >= 5 && value <= 60)
                    ini.Sections["UpdateChecker"].Keys["CheckInterval"].Value = value.ToString();
            }
        }

        public string ServerHost
        {
            get { return ini.Sections["UpdateChecker"].Keys["ServerHost"].Value; }
            set { ini.Sections["UpdateChecker"].Keys["ServerHost"].Value = value; }
        }

        public int QueryPort
        {
            get
            {
                string sVal = ini.Sections["UpdateChecker"].Keys["QueryPort"].Value;
                int iVal = string.IsNullOrEmpty(sVal) ? 27015 : int.Parse(sVal);
                return (iVal >= 1 && iVal <= 65535) ? iVal : 27015;
            }
            set
            {
                if (value >= 1 && value <= 65535)
                    ini.Sections["UpdateChecker"].Keys["QueryPort"].Value = value.ToString();
            }
        }

        public bool RconEnabled
        {
            get
            {
                string sVal = ini.Sections["UpdateChecker"].Keys["RconEnabled"].Value;
                bool bVal = string.IsNullOrEmpty(sVal) ? false : bool.Parse(sVal);
                return bVal;
            }
            set
            {
                ini.Sections["UpdateChecker"].Keys["RconEnabled"].Value = value.ToString();
            }
        }

        public int RconPort
        {
            get
            {
                string sVal = ini.Sections["UpdateChecker"].Keys["RconPort"].Value;
                int iVal = string.IsNullOrEmpty(sVal) ? 27020 : int.Parse(sVal);
                return (iVal >= 1 && iVal <= 65535) ? iVal : 27020;
            }
            set
            {
                if (value >= 1 && value <= 65535)
                    ini.Sections["UpdateChecker"].Keys["RconPort"].Value = value.ToString();
            }
        }

        public string RconPassword
        {
            get { return ini.Sections["UpdateChecker"].Keys["RconPassword"].Value; }
            set { ini.Sections["UpdateChecker"].Keys["RconPassword"].Value = value; }
        }

        public string ServerAppId
        {
            get { return ini.Sections["UpdateChecker"].Keys["ServerAppId"].Value; }
            set { ini.Sections["UpdateChecker"].Keys["ServerAppId"].Value = value; }
        }

        public long InstalledRevision
        {
            get
            {
                string sVal = ini.Sections["UpdateChecker"].Keys["InstalledRevision"].Value;
                long iVal = string.IsNullOrEmpty(sVal) ? 0 : long.Parse(sVal);
                return iVal;
            }
            set
            {
                if (value >= 0)
                    ini.Sections["UpdateChecker"].Keys["InstalledRevision"].Value = value.ToString();
            }
        }

        public long LatestFoundRevision { get; set; }

        public int UpdateType
        {
            get
            {
                string val = ini.Sections["Updater"].Keys["UpdateType"].Value;
                return (val == "1" || val == "2" || val == "3") ? int.Parse(val) : 2;
            }
            set
            {
                if (value >= 1 && value <= 3)
                    ini.Sections["Updater"].Keys["Updatetype"].Value = value.ToString();
            }
        }

        public int WaitTime
        {
            get
            {
                string sVal = ini.Sections["Updater"].Keys["WaitTime"].Value;
                int iVal = string.IsNullOrEmpty(sVal) ? 10 : int.Parse(sVal);
                return (iVal >= 5 && iVal <= 360) ? iVal : 10;
            }
            set
            {
                if (value >= 5 && value <= 360)
                    ini.Sections["Updater"].Keys["WaitTime"].Value = value.ToString();
            }
        }

        public int MaxUserWaitTime
        {
            get
            {
                string sVal = ini.Sections["Updater"].Keys["MaxUserWaitTime"].Value;
                int iVal = string.IsNullOrEmpty(sVal) ? 60 : int.Parse(sVal);
                return (iVal >= 5 && iVal <= 360) ? iVal : 60;
            }
            set
            {
                if (value >= 5 && value <= 360)
                    ini.Sections["Updater"].Keys["MaxUserWaitTime"].Value = value.ToString();
            }
        }

        public int ShutdownWaitTime
        {
            get
            {
                string sVal = ini.Sections["Updater"].Keys["ShutdownWaitTime"].Value;
                int iVal = string.IsNullOrEmpty(sVal) ? 60 : int.Parse(sVal);
                return (iVal >= 10 && iVal <= 360) ? iVal : 60;
            }
            set
            {
                if (value >= 10 && value <= 360)
                    ini.Sections["Updater"].Keys["ShutdownWaitTime"].Value = value.ToString();
            }
        }

        public string StartParameters
        {
            get { return ini.Sections["Updater"].Keys["StartParameters"].Value; }
            set { ini.Sections["Updater"].Keys["StartParameters"].Value = value; }
        }

        public string SteamCmd
        {
            get { return ini.Sections["Paths"].Keys["SteamCmd"].Value; }
            set { ini.Sections["Paths"].Keys["SteamCmd"].Value = value; }
        }

        public string RootPath
        {
            get { return ini.Sections["Paths"].Keys["RootPath"].Value; }
            set { ini.Sections["Paths"].Keys["RootPath"].Value = value; }
        }

        public string ExeFolder
        {
            get { return ini.Sections["Paths"].Keys["ExeFolder"].Value; }
            set { ini.Sections["Paths"].Keys["ExeFolder"].Value = value; }
        }

        public string ExeFile
        {
            get { return ini.Sections["Paths"].Keys["ExeFile"].Value; }
            set { ini.Sections["Paths"].Keys["ExeFile"].Value = value; }
        }

        public string UpdateFoundMessage
        {
            get { return ini.Sections["Messages"].Keys["UpdateFound"].Value; }
            set { ini.Sections["Messages"].Keys["UpdateFound"].Value = value; }
        }

        public string TenMinutesMessage
        {
            get { return ini.Sections["Messages"].Keys["TenMinutes"].Value; }
            set { ini.Sections["Messages"].Keys["TenMinutes"].Value = value; }
        }

        public string OneMinuteMessage
        {
            get { return ini.Sections["Messages"].Keys["OneMinute"].Value; }
            set { ini.Sections["Messages"].Keys["OneMinute"].Value = value; }
        }

        public bool UseDiscordBot
        {
            get
            {
                string sVal = ini.Sections["Notifications"].Keys["UseDiscordBot"].Value;
                bool bVal = string.IsNullOrEmpty(sVal) ? false : bool.Parse(sVal);
                return bVal;
            }
            set
            {
                ini.Sections["Notifications"].Keys["UseDiscordBot"].Value = value.ToString();
            }
        }

        public string DiscordWebHookURL
        {
            get { return ini.Sections["Notifications"].Keys["DiscordWebHookURL"].Value; }
            set { ini.Sections["Notifications"].Keys["DiscordWebHookURL"].Value = value; }
        }

        public string DiscordBotName
        {
            get { return ini.Sections["Notifications"].Keys["DiscordBotName"].Value; }
            set { ini.Sections["Notifications"].Keys["DiscordBotName"].Value = value; }
        }

        public bool DiscordBotUseTTS
        {
            get
            {
                string sVal = ini.Sections["Notifications"].Keys["DiscordBotUseTTS"].Value;
                bool bVal = string.IsNullOrEmpty(sVal) ? false : bool.Parse(sVal);
                return bVal;
            }
            set
            {
                ini.Sections["Notifications"].Keys["DiscordBotUseTTS"].Value = value.ToString();
            }
        }


        public Settings()
        {
            ini = new IniFile();
            Load();
        }

        public void Load()
        {
            // Read Settings
            if (File.Exists(IniFile))
                ini.Load(IniFile);

            if (!ini.Sections.Contains("UpdateChecker"))
                ini.Sections.Add("UpdateChecker");

            IniSection section = ini.Sections["UpdateChecker"];

            if (!section.Keys.Contains("CheckInterval"))
                section.Keys.Add("CheckInterval", "10").TrailingComment.Text = "Interval in minutes to check for updates (Min. 5, Max. 60)";
            if (!section.Keys.Contains("ServerHost"))
                section.Keys.Add("ServerHost", "127.0.0.1").TrailingComment.Text = "Hostname or IP of the server";
            if (!section.Keys.Contains("QueryPort"))
                section.Keys.Add("QueryPort", "27015").TrailingComment.Text = "Query port of the server";
            if (!section.Keys.Contains("RconEnabled"))
                section.Keys.Add("RconEnabled", "true").TrailingComment.Text = "Is Rcon enabled on the server";
            if (!section.Keys.Contains("RconPort"))
                section.Keys.Add("RconPort", "27020").TrailingComment.Text = "Rcon port of the server";
            if (!section.Keys.Contains("RconPassword"))
                section.Keys.Add("RconPassword", "").TrailingComment.Text = "Rcon password of the server";
            if (!section.Keys.Contains("ServerAppId"))
                section.Keys.Add("ServerAppId", "376030").TrailingComment.Text = "AppId of the Game Server.";
            if (!section.Keys.Contains("InstalledRevision"))
                section.Keys.Add("InstalledRevision", "0").TrailingComment.Text = "Currently installed revision of Game Server" + Environment.NewLine + "DO NOT CHANGE THIS! THIS IS NOT EQUIVALENT TO THE VERSION!";

            if (!ini.Sections.Contains("Updater"))
                ini.Sections.Add("Updater");

            section = ini.Sections["Updater"];
            section.LeadingComment.EmptyLinesBefore = 1;

            if (!section.Keys.Contains("UpdateType"))
                section.Keys.Add("UpdateType", "2").TrailingComment.Text = "Type to define when to update if an update occurs" + Environment.NewLine + "1 = Immediately (message to use, wait 1 min. update)" + Environment.NewLine + "2 = After a given time" + Environment.NewLine + "3 = When the last user left";
            if (!section.Keys.Contains("WaitTime"))
                section.Keys.Add("WaitTime", "10").TrailingComment.Text = "Only if UpdateType = 2" + Environment.NewLine + "Time to wait until the server updates in minutes (Min. 5, Max. 360)";
            if (!section.Keys.Contains("MaxUserWaitTime"))
                section.Keys.Add("MaxUserWaitTime", "10").TrailingComment.Text = "Only if UpdateType = 3" + Environment.NewLine + "Time to wait until the server updates if still not all users have left the server (Min. 5, Max. 360)";
            if (!section.Keys.Contains("ShutdownWaitTime"))
                section.Keys.Add("ShutdownWaitTime", "60").TrailingComment.Text = "The number of seconds to wait for the server to shutdown (Min. 10, Max. 360)";
            if (!section.Keys.Contains("StartParameters"))
                section.Keys.Add("StartParameters", "TheIsland?QueryPort=27015?Port=7777?listen").TrailingComment.Text = "Enter the parameters you use to start the server here." + Environment.NewLine + "These will be used to start the server after an update.";

            if (!ini.Sections.Contains("Paths"))
                ini.Sections.Add("Paths");

            section = ini.Sections["Paths"];
            section.LeadingComment.EmptyLinesBefore = 1;

            if (!section.Keys.Contains("SteamCmd"))
                section.Keys.Add("SteamCmd", "C:\\Steam\\steamcmd.exe").TrailingComment.Text = "Path to your steamcmd.exe";
            if (!section.Keys.Contains("RootPath"))
                section.Keys.Add("RootPath", "C:\\ARK").TrailingComment.Text = "Path to your Game Server root directory.";
            if (!section.Keys.Contains("ExeFolder"))
                section.Keys.Add("ExeFolder", "ShooterGame\\Binaries\\Win64").TrailingComment.Text = "Full path to your Game Server executable from the root path.";
            if (!section.Keys.Contains("ExeFile"))
                section.Keys.Add("ExeFile", "ShooterGameServer.exe").TrailingComment.Text = "The name of the Game Server executable.";

            if (!ini.Sections.Contains("Messages"))
                ini.Sections.Add("Messages");

            section = ini.Sections["Messages"];
            section.LeadingComment.EmptyLinesBefore = 1;

            if (!section.Keys.Contains("UpdateFound"))
                section.Keys.Add("UpdateFound", "Update found. Automatic update will start in %minutes% minutes!").TrailingComment.Text = "Message to be sent when an update is found.";
            if (!section.Keys.Contains("TenMinutes"))
                section.Keys.Add("TenMinutes", "Automatic update will start in %minutes% minutes!").TrailingComment.Text = "Message to be sent when the 10 minute mark is reached.";
            if (!section.Keys.Contains("OneMinute"))
                section.Keys.Add("OneMinute", "Automatic update will start any minute!").TrailingComment.Text = "Message to be sent when the 1 minute mark is reached.";

            if (!ini.Sections.Contains("Notifications"))
                ini.Sections.Add("Notifications");

            section = ini.Sections["Notifications"];
            section.LeadingComment.EmptyLinesBefore = 1;

            if (!section.Keys.Contains("UseDiscordBot"))
                section.Keys.Add("UseDiscordBot", "false").TrailingComment.Text = "Use Discord Bot to Send Message Before Restart?";
            if (!section.Keys.Contains("DiscordWebHookURL"))
                section.Keys.Add("DiscordWebHookURL", "").TrailingComment.Text = "This is your Webhook Url provided by discord.";
            if (!section.Keys.Contains("DiscordBotName"))
                section.Keys.Add("DiscordBotName", "").TrailingComment.Text = "This will override the name you setup in your webhook. Leave blank to use default.";
            if (!section.Keys.Contains("DiscordBotUseTTS"))
                section.Keys.Add("DiscordBotUseTTS", "false").TrailingComment.Text = "If enabled, will make the bot announce with Text to Speech, otherwise will turn off Text to Speech.";

            Save();
        }

        public void Save()
        {
            try
            {
                ini.Save(IniFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Settings could not be saved.");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
