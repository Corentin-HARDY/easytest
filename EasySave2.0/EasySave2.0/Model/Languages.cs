using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System;

namespace EasySave
{
    public class Languages
    {
        public ChooseLangue CurrentLangue { get => currentLangue; set => currentLangue = value; }

        private List<Phrase> phrases;
        string jsonFilePath = "";
        ChooseLangue currentLangue = ChooseLangue.Fr;

        // Load sentences
        public Languages()
        {
            LoadPhrases();
        }

        // Load Phrases
        public void LoadPhrases()
        {
            // Load sentences from JSON
            switch (currentLangue)
            {
                case ChooseLangue.En:
                    if (phrases != null)
                    {
                        phrases.Clear();
                    }
                    jsonFilePath = "../../../Data/en-US.json";
                    break;
                case ChooseLangue.Fr:
                    if (phrases != null)
                    {
                        phrases.Clear();
                    }
                    jsonFilePath = "../../../Data/fr-FR.json";
                    break;

            }

            var jsonContent = File.ReadAllText(jsonFilePath);
            phrases = JsonSerializer.Deserialize<List<Phrase>>(jsonContent);
        }

        //Get message from JSON file 
        public string GetMessage(string keyword)
        {
            
                //Lookup for sentence with keyword 
                var phrase = phrases.FirstOrDefault(p => p.Keyword == keyword);

                // return null
                return phrase != null ? phrase.Sentence : string.Empty;
            
        }

    }

}

