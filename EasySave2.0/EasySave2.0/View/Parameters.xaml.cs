using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Logique d'interaction pour Parameters.xaml
    /// </summary>
    public partial class Parameters : Window
    {
        // Declare an instance of Languages and BackupManager 
        private Languages languages;
        private BackupManager backupManager = new BackupManager();


        public Parameters()
        {
            InitializeComponent();

            //  create an instance
            this.languages = new Languages();

            UpdateUI();

            LoadDataIntoUI();
        }

        //GetPageCompletedEventArgs settings entries in comboboxes 
        private void LoadDataIntoUI()
        {
            string settingsPath = @"C:\EasySaveSettings\Settings.json";

            if (System.IO.File.Exists(settingsPath))
            {
                var jsonData = System.IO.File.ReadAllText(settingsPath);
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);

                if (jsonObject != null)
                {
                    if (jsonObject.ContainsKey("extensions"))
                    {
                        foreach (var extension in jsonObject["extensions"])
                        {
                            UpdateComboBox(Combo1, extension);
                        }
                    }

                    if (jsonObject.ContainsKey("logiciels"))
                    {
                        foreach (var logiciel in jsonObject["logiciels"])
                        {
                            UpdateComboBox(Combo2, logiciel);
                        }
                    }
                }
            }
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


        //code to handle the AddExtenxion_Click
        private void AddExtenxion_Click(object sender, RoutedEventArgs e)
        {
            string extension = TextBoxExtension.Text;
            if (!string.IsNullOrWhiteSpace(extension))
            {
                AddValueToJsonFile("extensions", extension, Combo1, TextBoxExtension);
            }
        }

        //code to handle the AddLogiciel_Click
        private void AddLogiciel_Click(object sender, RoutedEventArgs e)
        {
            string logiciel = TextBoxLogiciel.Text;
            if (!string.IsNullOrWhiteSpace(logiciel))
            {
                AddValueToJsonFile("logiciels", logiciel, Combo2, TextBoxLogiciel);
            }
        }

        //AddBackupJob Value to Json File 
        private void AddValueToJsonFile(string key, string value, ComboBox comboBox, TextBox textBox)
        {
            string settingsPath = @"C:\EasySaveSettings\Settings.json";

            // Create the directory if not exist
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(settingsPath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(settingsPath));
            }
            if (!System.IO.File.Exists(settingsPath))
            {
                using (var stream = System.IO.File.Create(settingsPath))
                {
                    // closed
                }
            }

            // Reading Json File
            var jsonData = System.IO.File.ReadAllText(settingsPath);
            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData) ?? new Dictionary<string, List<string>>();

            //Update the list
            if (!jsonObject.ContainsKey(key))
            {
                jsonObject[key] = new List<string>();
            }
            if (!jsonObject[key].Contains(value))
            {
                jsonObject[key].Add(value);

                // Wrinting in the JsonFile
                jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(settingsPath, jsonData);

                // Update The comboBoxes
                UpdateComboBox(comboBox, value);
                textBox.Clear(); 
            }
            else
            {
                //// Pop to handle doubles 
                MessageBox.Show("La valeur \"" + value + "\" est déjà présente dans la liste.", "Doublon détecté", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Update Comboboxes
        private void UpdateComboBox(ComboBox comboBox, string value)
        {
            if (!comboBox.Items.OfType<string>().Any(item => item.Equals(value)))
            {
                comboBox.Items.Add(value);
            }
        }

        //Code to handle the DeleteExtenxion_Click
        private void DeleteExtenxion_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = Combo1.SelectedItem as string;
            if (selectedItem != null)
            {
                RemoveValueFromJsonFile("extensions", selectedItem, Combo1);
            }
        }

        // Code to handle the DeleteLogiciel_Click
        private void DeleteLogiciel_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = Combo2.SelectedItem as string;
            if (selectedItem != null)
            {
                RemoveValueFromJsonFile("logiciels", selectedItem, Combo2);
            }
        }

        // Supress value from JsonFile
        private void RemoveValueFromJsonFile(string key, string value, ComboBox comboBox)
        {
            string settingsPath = @"C:\EasySaveSettings\Settings.json";

            // Read the file
            var jsonData = System.IO.File.ReadAllText(settingsPath);
            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData) ?? new Dictionary<string, List<string>>();

            // Dekete the value in the list
            if (jsonObject.ContainsKey(key) && jsonObject[key].Contains(value))
            {
                jsonObject[key].Remove(value);

                // Rewrite the file
                jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(settingsPath, jsonData);

                // Update the comboboxes
                comboBox.Items.Remove(value);
            }
        }

        //code to handle the Save_Click
        private void Save_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Les changements ont été pris en compte", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

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

        // Update the UI elements 
        public void UpdateUI()
        {
            // Update TextBlocks 
            Titlep.Text = languages.GetMessage("Titlep");
            Extenxion.Text = languages.GetMessage("Extenxion");
            Logiciel.Text = languages.GetMessage("Logiciel");
            TypeFile.Text = languages.GetMessage("TypeFile");


            // Update button content 
            Enregistrer.Content = languages.GetMessage("Enregistrer");

        }

    }
}

