using Quartz;
using System;
using System.Diagnostics;
using System.IO;

namespace ServerUpdater.Jobs
{
    [DisallowConcurrentExecution]
    class UpdateJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Log.LogErrorToConsole("Update started");

            try
            {
                bool goOn = true;

                //if (!Rcon.Instance.IsConnected)
                //{
                //    for (int i = 0; i < 3; i++)
                //    {
                //        if (Rcon.Instance.Connect(Settings.Instance.ServerHost, Settings.Instance.RconPort, Settings.Instance.RconPassword))
                //            break;
                //    }
                //}

                //if (!(goOn = Rcon.Instance.IsConnected))
                //    Log.LogErrorToConsole("Rcon connection failed. Update aborted.");
                //if (goOn && !(goOn = Rcon.Instance.ExitServer()))
                //    Log.LogErrorToConsole("Could not stop the server. Update aborted.");

                Process process = ProcessUtils.GetServerProcess();
                if (process != null && !process.HasExited)
                {
                    // Send CNTL-C
                    ProcessUtils.SendStop(process).Wait(Settings.Instance.ShutdownWaitTime * 1000);
                    if (goOn && !(goOn = process.HasExited))
                        Log.LogErrorToConsole("Could not stop the server. Update aborted.");
                }
                if (goOn && !Update())
                    Log.LogErrorToConsole("Update failed.");
                if (goOn)
                    StartServer();
            }
            finally
            {
                context.Scheduler.ResumeTrigger(new TriggerKey("UpdateChecker"));
                Log.LogToConsole("UpdateChecker resumed.");
            }
        }

        private bool Update()
        {
            try
            {
                using (Process steamcmd = new Process())
                {
                    steamcmd.StartInfo.FileName = Settings.Instance.SteamCmd;
                    steamcmd.StartInfo.Arguments = $"+login anonymous +force_install_dir \"{Settings.Instance.RootPath}\" +app_update {Settings.Instance.ServerAppId} validate +quit";
                    steamcmd.StartInfo.UseShellExecute = false;
                    steamcmd.Start();
                    steamcmd.WaitForExit();
                }

                Settings.Instance.InstalledRevision = Settings.Instance.LatestFoundRevision;
                Settings.Instance.Save();

                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorToConsole(ex.ToString());
            }

            return false;
        }

        private bool StartServer()
        {
            try
            {
                using (Process game = new Process())
                {
                    game.StartInfo.FileName = Path.Combine(Settings.Instance.RootPath, Settings.Instance.ExeFolder, Settings.Instance.ExeFile);
                    game.StartInfo.WorkingDirectory = Path.Combine(Settings.Instance.RootPath, Settings.Instance.ExeFolder);
                    game.StartInfo.Arguments = Settings.Instance.StartParameters;
                    game.StartInfo.UseShellExecute = false;
                    game.Start();
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorToConsole(ex.ToString());
            }

            return false;
        }
    }
}
