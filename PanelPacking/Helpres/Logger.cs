using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PanelPacking.Helpres
{
    public class Logger
    {
        public string folderPath;
        public string fileName;
        public Logger()
        {
            this.folderPath = Environment.CurrentDirectory + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(this.folderPath)) 
            {
                Directory.CreateDirectory(this.folderPath);
            }
            this.fileName = this.folderPath + "\\LOG-" + DateTime.Now.ToString("yyyy-MM-dd");
            if(!File.Exists(this.fileName)) 
            {
                File.Create(this.fileName);
            }
        }
        public void writeLogError(string message) 
        {
            message = "ERR" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogErrorScanner(string message)
        {
            message = "ERRSCANNER" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogScan(string message)
        {
            message = "SCAN" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogScanNo(string message)
        {
            message = "SCANNO" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogScanYes(string message)
        {
            message = "SCANYES" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogRemoveToday(string message)
        {
            message = "RMTODAY" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogAddToPack(string message)
        {
            message = "ADDTOPACK" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogRemoveFromPack(string message)
        {
            message = "RMFROMPACK" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogGoToEdit(string message)
        {
            message = "GOEDIT" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogPack(string message)
        {
            message = "PACK" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogPackEdit(string message)
        {
            message = "PACKEDIT" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogPackEditReverse(string message)
        {
            message = "PACKEDITREVERSE" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogPackEditPanel(string message)
        {
            message = "PACKEDITPANEL" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogLogin(string message)
        {
            message = "LOGIN" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }

        public void writeLogClose(string message)
        {
            message = "CLOSEAPP" + "\t" + DateTime.Now.ToString() + "\t" + message;
            File.AppendAllText(this.fileName, message + Environment.NewLine);
        }
    }
}
