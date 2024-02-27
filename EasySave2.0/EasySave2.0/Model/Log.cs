using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace EasySave
{
    public class Log
    {
        // --- Attributes ---
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Size { get; set; }
        public string StartTime { get; set; }
        public string ElapsedTime { get; set; }
        public string CryptingTime { get; set; }

        // --- Constructors ---
        public Log() { }

        public Log(string name, string source, string target, string size, string startTime, string elapsedTime, string cryptingTime)
        //public Log(string name, string source, string target, string size, string startTime, string elapsedTime)

        {
            Name = name;
            Source = source;
            Target = target;
            Size = size;
            StartTime = startTime;
            ElapsedTime = elapsedTime;
            CryptingTime = cryptingTime;
        }

        // Write Log file
        public void WriteToLogFile(string logLocation)
        {
            WriteToJsonFile(logLocation);
            WriteToXmlFile(logLocation);
        }

        private void WriteToJsonFile(string logLocation)
        {
            string filePath = Path.Combine(logLocation, "log.json");
            string jsonContent = ToString() + Environment.NewLine;
            File.AppendAllText(filePath, jsonContent);
        }

        private void WriteToXmlFile(string logLocation)
        {
            string filePath = Path.Combine(logLocation, "log.xml");
            var logEntry = new LogEntry
            {
                Name = this.Name,
                FileSource = this.Source,
                FileTarget = this.Target,
                FileSize = this.Size,
                FileTransferTime = this.ElapsedTime,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };

            // If the file does not exist create it 
            if (!File.Exists(filePath))
            {
                var newDoc = new System.Xml.Linq.XDocument(
                    new System.Xml.Linq.XElement("LogEntries",
                        new System.Xml.Linq.XElement("LogEntry", new System.Xml.Linq.XElement("Name", logEntry.Name),
                            new System.Xml.Linq.XElement("FileSource", logEntry.FileSource),
                            new System.Xml.Linq.XElement("FileTarget", logEntry.FileTarget),
                            new System.Xml.Linq.XElement("FileSize", logEntry.FileSize),
                            new System.Xml.Linq.XElement("FileTransferTime", logEntry.FileTransferTime),
                            new System.Xml.Linq.XElement("Time", logEntry.Time)
                        )
                    )
                );
                newDoc.Save(filePath);
            }
            else
            {
                var doc = System.Xml.Linq.XDocument.Load(filePath);
                doc.Root.Add(
                    new System.Xml.Linq.XElement("LogEntry", new System.Xml.Linq.XElement("Name", logEntry.Name),
                        new System.Xml.Linq.XElement("FileSource", logEntry.FileSource),
                        new System.Xml.Linq.XElement("FileTarget", logEntry.FileTarget),
                        new System.Xml.Linq.XElement("FileSize", logEntry.FileSize),
                        new System.Xml.Linq.XElement("FileTransferTime", logEntry.FileTransferTime),
                        new System.Xml.Linq.XElement("Time", logEntry.Time)
                    )
                );
                doc.Save(filePath);
            }
        }

        // Format JSON file
        public override string ToString()
        {
            var logEntry = new
            {
                Name,
                FileSource = Source,
                FileTarget = Target,
                FileSize = Size,
                FileTransferTime = ElapsedTime,
                CryptingTime, // Ajouté ici
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };

            return JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
        }


        // For serilization XML
        public class LogEntry
        {
            public string Name { get; set; }
            public string FileSource { get; set; }
            public string FileTarget { get; set; }
            public string FileSize { get; set; }
            public string FileTransferTime { get; set; }
            public string Time { get; set; }
            public string CryptingTime { get; set; }

        }
    }
}
