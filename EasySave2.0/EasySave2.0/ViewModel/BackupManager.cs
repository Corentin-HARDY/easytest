using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;
using System;
using System.Windows;
using System.Diagnostics;

namespace EasySave
{
    public class BackupManager
    {
        // Holds the current language settings 
        Languages languages;
        public Languages Language { get => languages; set => languages = value; }

        // List to store all backup jobs.
        private List<BackupJob> backupJobs = new List<BackupJob>();

        // Path to the JSON file
        private readonly string filePath = "C:\\Temp\\BacupJobs.json";

        //public BackupManager 
        public BackupManager()
        {
            LoadBackupJobs();

            this.Language = new Languages();
        }

        // Adding a BackupJob to the JSON file for persistence
        public void AddBackupJob(BackupJob backupJob)
        {
            BackupJobs.Add(backupJob);
            // Save after adding
            SaveBackupJobs();
        }

        // Save the list of BackupJobs to a JSON file
        private void SaveBackupJobs()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(BackupJobs, options);
            File.WriteAllText(filePath, jsonString);
        }

        // Load the list of BackupJobs from a JSON file
        public void LoadBackupJobs()
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                BackupJobs = JsonSerializer.Deserialize<List<BackupJob>>(jsonString) ?? new List<BackupJob>();
            }
        }

        // Retrieve the BackupJobs
        public List<BackupJob> GetBackupJobs()
        {
            return BackupJobs;
        }


        // ----Fonctions------

        // Method to remove a BackupJob
        public bool RemoveBackupJob(string jobName, string RepoCible, string RepoSource, BackupType backupType)
        {
            var jobToRemove = BackupJobs.FirstOrDefault(job => job.Name == jobName && job.Source == RepoCible && job.Target == RepoSource && job.BackupType == backupType);

            if (jobToRemove != null)
            {
                BackupJobs.Remove(jobToRemove);
                // Save after removing
                SaveBackupJobs();
                return true;
            }
            return false;
        }

        //LogFile Location
        public static string LogLocation { get; set; } = @"C:\Temp";
        public List<BackupJob> BackupJobs { get => backupJobs; set => backupJobs = value; }
        public Func<object, object, Visibility> Closed { get; internal set; }

        public static void InitializeLogDirectory()
        {
            // Cela créera le dossier s'il n'existe pas déjà
            Directory.CreateDirectory(LogLocation);
        }


        //Get the work logiciel 
        private List<string> GetLogicielFromSettings()
        {
            string settingsPath = "C:\\EasySaveSettings\\Settings.json";
            var settingsJson = File.ReadAllText(settingsPath);
            var settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(settingsJson);

            if (settings != null && settings.ContainsKey("logiciels"))
            {
                return settings["logiciels"];
            }

            return new List<string>();
        }

        //Check if logiciel with same name found
        private bool CheckIfLogicielIsRunning()
        {
            List<string> logicielsFromSettings = GetLogicielFromSettings();
            Process[] runningProcesses = Process.GetProcesses();
            List<string> runningProcessNames = runningProcesses.Select(proc => proc.ProcessName).ToList();

            foreach (string logiciel in logicielsFromSettings)
            {
                // Verify if we have a maching name
                if (runningProcessNames.Contains(logiciel, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // Execute a BackupJob
        public bool ExecuteBackupJob(string jobName, string RepoCible, string RepoSource, BackupType backupType)
        {
            var jobToExecute = BackupJobs.FirstOrDefault(job => job.Name == jobName && job.Target == RepoCible && job.Source == RepoSource && job.BackupType == backupType);
            if (jobToExecute != null)
            {
                // Vérifier si un logiciel interdit est en cours d'exécution.
                if (CheckIfLogicielIsRunning())
                {
                    // Message for Logiciel found
                    string errorMessage = this.languages.GetMessage("LogicielRunning");
                    MessageBox.Show((errorMessage), languages.GetMessage("Info"), MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }

                //WatcherChangeTypes the crypting time 
                DateTime startTime = DateTime.Now;
                TimeSpan totalCryptingTime;

                try
                {
                    switch (jobToExecute.BackupType)
                    {
                        case BackupType.FULL:
                            totalCryptingTime = ExecuteFullBackup(jobToExecute);
                            break;
                        case BackupType.DIFFERENTIAL:
                            totalCryptingTime = ExecuteDifferentialBackup(jobToExecute);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("UnsupportedBackupType");
                    }

                    // Time and date
                    DateTime endTime = DateTime.Now;
                    TimeSpan duration = endTime - startTime;
                    string elapsedTime = duration.ToString(@"hh\:mm\:ss\.fff");

                    // Calculate the total File size.
                    string totalSize = CalculateFolderSizeAsString(LogLocation);

                    // Calculate size 
                    static string CalculateFolderSizeAsString(string LogLocation)
                    {
                        long totalSize = 0;
                        string[] fileNames = Directory.GetFiles(LogLocation, "*.*", SearchOption.AllDirectories);
                        foreach (string fileName in fileNames)
                        {
                            FileInfo fileInfo = new FileInfo(fileName);
                            totalSize += fileInfo.Length;
                        }
                        // Get fileSize
                        return FormatSize(totalSize);
                    }

                    //Format the size 
                    static string FormatSize(long bytes)
                    {
                        const int scale = 1024;
                        string[] orders = new string[] { "Go", "Mo", "Ko", "octets" };
                        long max = (long)Math.Pow(scale, orders.Length - 1);

                        foreach (string order in orders)
                        {
                            if (bytes > max)
                                return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                            max /= scale;
                        }
                        return "0 octets";
                    }

                    // Format CryptingTime to include milliseconds
                    string formattedCryptingTime = totalCryptingTime.ToString(@"hh\:mm\:ss\.fff");

                    Log backupLog = new Log(jobName, jobToExecute.Source, jobToExecute.Target, totalSize, startTime.ToString(), elapsedTime, formattedCryptingTime);

                    // Write in the log File
                    backupLog.WriteToLogFile(LogLocation);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.languages.GetMessage("warning"));
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        // Execute a full backupJop
        private TimeSpan ExecuteFullBackup(BackupJob job)
        {
            TimeSpan cryptingTime = TimeSpan.Zero;

            var sourceDir = new DirectoryInfo(job.Source);
            var targetDir = new DirectoryInfo(job.Target);

            if (!targetDir.Exists)
            {
                targetDir.Create();
            }

            foreach (FileInfo file in sourceDir.GetFiles())
            {
                string targetFilePath = Path.Combine(job.Target, file.Name);

                if (job.IsCrypted)
                {
                    DateTime cryptStart = DateTime.Now;
                    job.Crypter(file.FullName, job.Target);
                    DateTime cryptEnd = DateTime.Now;

                    cryptingTime += cryptEnd - cryptStart;
                }
                else
                {
                    file.CopyTo(targetFilePath, true);
                }
            }

            return cryptingTime;
        }

        // Execute a differential backupJob
        private TimeSpan ExecuteDifferentialBackup(BackupJob job)
        {
            TimeSpan cryptingTime = TimeSpan.Zero;

            State lastFullBackupState = LoadState(job.Name);
            var sourceDir = new DirectoryInfo(job.Source);
            var targetDir = new DirectoryInfo(job.Target);

            if (!targetDir.Exists) targetDir.Create();

            DateTime lastBackupDate = lastFullBackupState?.LastBackupDate ?? DateTime.MinValue;

            foreach (FileInfo file in sourceDir.GetFiles())
            {
                if (file.LastWriteTime > lastBackupDate)
                {
                    string targetFilePath = Path.Combine(job.Target, file.Name);

                    if (job.IsCrypted)
                    {
                        DateTime cryptStart = DateTime.Now;
                        job.Crypter(file.FullName, job.Target);
                        DateTime cryptEnd = DateTime.Now;

                        cryptingTime += cryptEnd - cryptStart;
                    }
                    else
                    {
                        file.CopyTo(targetFilePath, true);
                    }
                }
            }

            return cryptingTime;
        }


        // Method to save the state of a backup
        private void SaveState(State state, string jobName)
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Etats");
            string filePath = Path.Combine(directoryPath, $"{jobName}_state.json");

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Method to load the state of a backup
        private State LoadState(string jobName)
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Etats");
            string filePath = Path.Combine(directoryPath, $"{jobName}_state.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<State>(json);
            }
            return null;
        }

    }
}