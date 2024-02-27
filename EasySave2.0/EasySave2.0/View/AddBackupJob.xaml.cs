using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for AddBackupJob.xaml
    /// </summary>
    public partial class AddBackupJob : Window
    {
        // Declare an instance of Languages and BackupManager 
        private Languages languages;
        private BackupManager backupManager = new BackupManager();

        // Constructor that takes a Languages instance
        public AddBackupJob(Languages languages)
        {
            InitializeComponent();

            this.languages = languages;

            UpdateUI();
        }

        // Event handler for the Back button click.
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow mainWindow)
                {
                    // Initialize data and show the MainWindow.
                    mainWindow.init();
                    mainWindow.Show();
                    this.Close();
                    return;
                }
            }

            // If no existing MainWindow is found, create a new one.
            MainWindow newMainWindow = new MainWindow();
            newMainWindow.init();
            newMainWindow.Show();
            // Close this window.
            this.Close();
        }

        // Event handler for the Add button click.
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            BackupType type = saveOption.Text == "Complet" ? BackupType.FULL : BackupType.DIFFERENTIAL;

            // Create a new BackupJob with the entered details.
            BackupJob newBackupJob = new BackupJob(namejob.Text, source.Text, target.Text, type, (bool)iscrypted.IsChecked);

            // Add the new BackupJob using the backup manager.
            backupManager.AddBackupJob(newBackupJob);

            // Show a success message.
            MessageBox.Show(languages.GetMessage("BackupJobAddedSuccess"), languages.GetMessage("Success"), MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear the input fields after adding the job.
            namejob.Text = "";
            source.Text = "";
            target.Text = "";
            saveOption.SelectedIndex = -1;
            iscrypted.IsChecked = false;
        }

        // Update the UI elements 
        public void UpdateUI()
        {
            // Update TextBlocks 
            Title.Text = languages.GetMessage("Title");
            Nme.Text = languages.GetMessage("Nme");
            Src.Text = languages.GetMessage("Src");
            Dest.Text = languages.GetMessage("Dest");
            Typ.Text = languages.GetMessage("Typ");

            // Update button content 
            Ajouter.Content = languages.GetMessage("Add");

            // Update ComboBox items
            typeOption1.Content = languages.GetMessage("Complete");
            typeOption2.Content = languages.GetMessage("Differential");
        }

    }
}

