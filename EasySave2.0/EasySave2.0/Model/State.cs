using System;
using System.Text.Json;
using System.IO;

namespace EasySave

{
    //Initialize State class
    public class State
    {
        // --- Attributes ---
        public int TotalFile { get; set; }
        public long TotalSize { get; set; }
        public int Progress { get; set; }
        public int NbFileLeft { get; set; }
        public long LeftSize { get; set; }
        public string CurrentPathSrc { get; set; }
        public string CurrentPathDest { get; set; }
        public DateTime LastBackupDate { get; set; }


        // --- Contructors ---
        public State() { }

        // Constructor()
        public State(int _TotalFile, long _TotalSize, string _CurrentPathSrc, string _CurrentPathDest, DateTime _LastBackupDate)
        {
            this.Progress = 0;
            this.TotalFile = _TotalFile;
            this.TotalSize = _TotalSize;
            this.CurrentPathSrc = _CurrentPathSrc;
            this.CurrentPathDest = _CurrentPathDest;
            this.LastBackupDate = _LastBackupDate;
        }


        // --- Methods ---
        public void UpdateState(int _Progress, int _NbFileLeft, long _LeftSize, string _CurrSrcPath, string _CurrDestPath)
        {
            this.Progress = _Progress;
            this.NbFileLeft = _NbFileLeft;
            this.LeftSize = _LeftSize;
            this.CurrentPathSrc = _CurrSrcPath;
            this.CurrentPathDest = _CurrDestPath;
        }


    }
}