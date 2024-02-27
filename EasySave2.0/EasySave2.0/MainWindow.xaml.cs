using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace EasySave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Declare an instance of Languages and BackupManager 
        Languages languages;
        BackupManager backupManager = new BackupManager();

        public MainWindow() 
        {
            InitializeComponent();

            this.Language = new Languages();

            init();

            if (!Directory.Exists("C:\\Temp"))
            {
                Directory.CreateDirectory("C:\\Temp");
            }

            //create setting.Json dans EasySaveSettings

            if (!Directory.Exists("C:\\EasySaveSettings"))
            {
                Directory.CreateDirectory("C:\\EasySaveSettings");
            }

            if (!File.Exists("C:\\EasySaveSettings\\Settings.json"))
            {
                File.Create("C:\\EasySaveSettings\\Settings.json");
            }
        }

        public Languages Language { get => languages; set => languages = value; }


        // Event handler for the Add button click.
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddBackupJob addBackupJobWindow = new AddBackupJob(languages);

            // Hide the window
            this.Visibility = Visibility.Collapsed;

            // Show the new window
            addBackupJobWindow.Closed += (s, args) => this.Visibility = Visibility.Visible;
            addBackupJobWindow.Show();
        }

        // Event handler for the Remove button click.
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var selectedJobs = datagrid.SelectedItems.Cast<BackupJob>().ToList();
            if (selectedJobs.Count > 0)
            {
                foreach (var job in selectedJobs)
                {
                    backupManager.RemoveBackupJob(job.Name, job.Target, job.Source, job.BackupType);
                }

                string message = string.Format(languages.GetMessage("BackupJobsDeletedSuccess"), selectedJobs.Count);
                string title = languages.GetMessage("DeletionSuccessful");

                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                init();
            }
            else
            {
                // If no BackupJob is selected
                MessageBox.Show(languages.GetMessage("NoBackupJobSelected"), languages.GetMessage("Error"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Event handler for the Select All button click.
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            datagrid.SelectAll();
        }

        // Event handler for the Execute button click.
        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            var selectedJobs = datagrid.SelectedItems.Cast<BackupJob>().ToList();

            if (selectedJobs.Count == 0)
            {
                MessageBox.Show(this.Language.GetMessage("NoBackupJobSelected"), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<string> successfulJobNames = new List<string>();

            foreach (var job in selectedJobs)
            {
                bool success = backupManager.ExecuteBackupJob(job.Name, job.Target, job.Source, job.BackupType);
                if (success)
                {
                    successfulJobNames.Add(job.Name);
                }
            }

            // Successfully execute message
            if (successfulJobNames.Count > 0)
            {
                string joinedNames = string.Join(", ", successfulJobNames);
                MessageBox.Show(string.Format(languages.GetMessage("BackupJobsExecutedSuccess"), joinedNames), languages.GetMessage("ExecutionSuccess"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Failure message
                MessageBox.Show(languages.GetMessage("NoBackupJobsExecuted"), languages.GetMessage("ExecutionFailure"), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            init(); 
        }

        // Event handler for the French flag.
        private void OnFrenchFlagClicked(object sender, MouseButtonEventArgs e)
        {
            SetLanguage(ChooseLangue.Fr, "fr-FR");
        }

        // Event handler for the English flag.
        private void OnEnglishFlagClicked(object sender, MouseButtonEventArgs e)
        {
            SetLanguage(ChooseLangue.En, "en-US");
        }

        // Set the language from choice 
        private void SetLanguage(ChooseLangue newLang, string cultureCode)
        {
            languages.CurrentLangue = newLang;
            languages.LoadPhrases();

            // Update the application language
            CultureInfo cultureInfo = new CultureInfo(cultureCode);
            CultureInfo.CurrentUICulture = cultureInfo;
            CultureInfo.CurrentCulture = cultureInfo;

            // Show that the language is change
            string message;
            if (newLang == ChooseLangue.Fr)
            {
                message = "La langue a été changée en Français.";
            }
            else
            {
                message = "Language has been changed to English.";
            }
            MessageBox.Show(message);

            // Update yhe Interface
            UpdateUI();

            // Update all the other view
            foreach (Window window in Application.Current.Windows)
            {
                if (window is AddBackupJob addBackupJobWindow)
                {
                    addBackupJobWindow.UpdateUI();
                }

                else if (window is Parameters parametersWindow)
                {
                    parametersWindow.UpdateUI();
                }
            }
        }

        //Update the interface to chosed language
        private void UpdateUI()
        {
            Select.Content = languages.GetMessage("Select");
            Execute.Content = languages.GetMessage("Execute");
            Name.Header = languages.GetMessage("Name");
            Source.Header = languages.GetMessage("Source");
            Target.Header = languages.GetMessage("Target");
            Type.Header = languages.GetMessage("Type");
        }

        //Initialising the backupJob list 
        public void init()
        {
            datagrid.ItemsSource = null;
            backupManager.LoadBackupJobs();
            List<BackupJob> jobs = backupManager.GetBackupJobs();
            datagrid.ItemsSource = jobs;
        }

        //Event handler for the settings
        private void OnSettingsClicked(object sender, MouseButtonEventArgs e)
        {
            Parameters parameterWindow = new Parameters();

            // Hide the window
            this.Visibility = Visibility.Collapsed;

            // Show the new window
            parameterWindow.Closed += (s, args) => this.Visibility = Visibility.Visible;
            parameterWindow.Show();
        }

    }
}
