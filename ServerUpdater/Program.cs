using Quartz;
using Quartz.Impl;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ServerUpdater
{
    class Program
    {
        private static readonly StringBuilder Sb = new StringBuilder();

        static void Main(string[] args)
        {
            // Check Settings
            FileInfo fi = new FileInfo(Settings.Instance.SteamCmd);
            if (!fi.Exists || (fi.Name != "steamcmd.exe" && fi.Name != "steamcmd.sh"))
            {
                Log.LogErrorToConsole("SteamCmd path incorrect");
                Log.LogErrorToConsole("Press any key to exit");
                Console.ReadKey();
                return;
            }

            DirectoryInfo gameServerPath = new DirectoryInfo(Path.Combine(Settings.Instance.RootPath, Settings.Instance.ExeFolder));
            if (!gameServerPath.Exists)
            {
                Log.LogErrorToConsole("Game Server folders incorrect");
                Log.LogErrorToConsole("Press any key to exit");
                Console.ReadKey();
                return;
            }
            FileInfo gameServerFile = new FileInfo(Path.Combine(gameServerPath.FullName, Settings.Instance.ExeFile));
            if (!gameServerFile.Exists)
            {
                Log.LogErrorToConsole("Game Server executable incorrect");
                Log.LogErrorToConsole("Press any key to exit");
                Console.ReadKey();
                return;
            }
            if (Settings.Instance.RconEnabled)
            {
                if (!Rcon.Instance.Connect(Settings.Instance.ServerHost, Settings.Instance.RconPort, Settings.Instance.RconPassword))
                {
                    Log.LogErrorToConsole("Could not connect to the server using RCON");
                    Log.LogErrorToConsole("Press any key to exit");
                    Console.ReadKey();
                    return;
                }

                Rcon.Instance.Disconnect();
            }

            // Start updater
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail updateJob = JobBuilder.Create<Jobs.UpdateJob>()
                .WithIdentity("Update")
                .StoreDurably(true)
                .Build();

            scheduler.AddJob(updateJob, true);

            IJobDetail checkForPlayersJob = JobBuilder.Create<Jobs.CheckForPlayersJob>()
                .WithIdentity("CheckForPlayers")
                .StoreDurably(true)
                .Build();

            scheduler.AddJob(checkForPlayersJob, true);

            IJobDetail broadcastJob = JobBuilder.Create<Jobs.BroadcastJob>()
                .WithIdentity("Broadcast")
                .StoreDurably(true)
                .Build();

            scheduler.AddJob(broadcastJob, true);

            IJobDetail updateCheckJob = JobBuilder.Create<Jobs.UpdateCheckJob>()
                .WithIdentity("UpdateChecker")
                .UsingJobData(new JobDataMap { { "UpdateJobKey", updateJob.Key } })
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("UpdateChecker")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(Settings.Instance.CheckInterval)
                    .RepeatForever())
                .ForJob(updateCheckJob)
                .Build();

            scheduler.ScheduleJob(updateCheckJob, trigger);

            while (Console.ReadLine() != "quit") { }

            scheduler.Shutdown();
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Sb.AppendLine(e.Data);
        }
    }
}
