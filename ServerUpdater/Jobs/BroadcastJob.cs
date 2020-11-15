using Quartz;

namespace ServerUpdater.Jobs
{
    [DisallowConcurrentExecution]
    class BroadcastJob : IJob
    {
        public string Message { get; set; }

        public void Execute(IJobExecutionContext context)
        {
            if (string.IsNullOrWhiteSpace(Message))
                return;

            if (Settings.Instance.RconEnabled)
            {
                Log.LogToConsole($"RCON Broadcast: {Message}");

                if (!Rcon.Instance.IsConnected)
                {
                    for(int i = 0; i < 3; i++)
                    {
                        if (Rcon.Instance.Connect(Settings.Instance.ServerHost, Settings.Instance.RconPort, Settings.Instance.RconPassword))
                            break;
                    }
                }

                if (Rcon.Instance.IsConnected)
                {
                    Rcon.Instance.Broadcast(Message);
                    Rcon.Instance.Disconnect();
                }
            }

            if (Settings.Instance.UseDiscordBot)
            {
                Log.LogToConsole($"Discord Broadcast: {Message}");

                if (!string.IsNullOrWhiteSpace(Settings.Instance.DiscordWebHookURL))
                {
                    Discord.Instance.Broadcast(Message);
                }
            }
        }
    }
}
