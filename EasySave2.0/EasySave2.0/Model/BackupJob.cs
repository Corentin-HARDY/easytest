using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace EasySave
{
    //Create enum BackupType
    public enum BackupType
    {
        FULL,
        DIFFERENTIAL
    }

    //Initiate BackupJob class
    public class BackupJob
    {
        // --- Attributes ---
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public BackupType BackupType { get; set; } 
        public State State { get; set; }
        public bool IsCrypted { get; set; }

        // Prepare options to indent JSON Files
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        // --- Constructors ---
        public BackupJob() { }

        // Constructor used to get BackupJob data from user()
        public BackupJob(string name, string source, string target, BackupType backupType, bool iscrypted)
        {
            Name = name;
            Source = source;
            Target = target;
            this.BackupType = backupType;
            IsCrypted = iscrypted;
            State = null;
        }


        // Méthode de génération de clé aléatoire
        private byte[] GenerateRandomKey(int keySize)
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var randomKey = new byte[keySize];
                rng.GetBytes(randomKey);
                return randomKey;
            }
        }

        //Method to to crypt 
        public void Crypter(string fichierSource, string repertoireDestination)
        {
            // Récupérer la liste des extensions autorisées à partir des paramètres utilisateur.
            List<string> extensionsAutorisees = GetAllowedExtensionsFromSettings();

            // Vérifier que le fichier a une des extensions autorisées.
            if (extensionsAutorisees.Contains(Path.GetExtension(fichierSource)))
            {
                // Génération d'une clé aléatoire
                byte[] cle = GenerateRandomKey(16);

                // Lire le contenu du fichier source
                byte[] contenuSource = File.ReadAllBytes(fichierSource);

                // Chiffrement du contenu en utilisant la clé générée
                for (int i = 0; i < contenuSource.Length; i++)
                {
                    contenuSource[i] = (byte)(contenuSource[i] ^ cle[i % cle.Length]);
                }

                // Construction du chemin de destination et écriture du fichier chiffré
                string nomFichier = Path.GetFileName(fichierSource);
                string cheminDestination = Path.Combine(repertoireDestination, nomFichier);
                File.WriteAllBytes(cheminDestination, contenuSource);

            }
            else
            {
                // Par exemple, pour le copier sans le chiffrer :
                File.Copy(fichierSource, Path.Combine(repertoireDestination, Path.GetFileName(fichierSource)), overwrite: true);
            }
        }

        //Get the extensions to crypt
        private List<string> GetAllowedExtensionsFromSettings()
        {
            string settingsPath = "C:\\EasySaveSettings\\Settings.json";
            var settingsJson = File.ReadAllText(settingsPath);
            var settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(settingsJson);

            if (settings != null && settings.ContainsKey("extensions"))
            {
                return settings["extensions"];
            }

            return new List<string>(); 
        }



    }
}

    
