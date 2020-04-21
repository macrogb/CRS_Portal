using CRS_Portal.Entity;
using CRS_Portal.Models;
using System;
using System.IO;

namespace CRS_Portal.HelperMethods
{
    static class Log
    {
        static string  logFileLocation = CRSMacroSettings.LogFilePath;

        public async static void WriteEventLogwithParam(string controller, string method, string type, string position,  string message, string userID)
        {
            string _message = string.Format("Controller : {0} || Method : {1} || Type : {2} || Position : {3} || Message : {4}  || Action Performed By : {5}",
                                    controller, method, type, position, message, userID);
            //WriteEventLog(_message);
            AddEntryToLogTable(_message, userID, 1);
        }

        public async static void WriteEventErrorLogwithParam(string controller, string method, string type, string position, string message,  string userID, Exception ex)
        {
            string _message = string.Format("Controller : {0} || Method : {1} || Type : {2} || Position : {3} || Message : {4}  || Action Performed By : {5} || ErrorMessage : {6}",
                                    controller, method, type, position, message, userID, ex.ToString());
            //WriteExceptionLog(_message, ex);
            AddEntryToLogTable(_message, userID, 2);
        }

        public async static void AddEntryToLogTable(string logMessage, string userID, int logID)
        {
            using (SCVAuditLogDbContext objSCVAuditLogDbContext = new SCVAuditLogDbContext())
            {
                SCV_AuditLog model = new SCV_AuditLog();
                if (logID == 1)
                {
                    model.LogType = "Info";
                }
                else
                {
                    model.LogType = "Error";
                }

                model.LogMessage = logMessage;
                model.CreatedBy = userID;
                objSCVAuditLogDbContext.Add(model);
                objSCVAuditLogDbContext.SaveChanges();
            }
        }

        private async static void WriteExceptionLog(string message, Exception ex)
        {
            message = message + " " + ex.Message + " " + ex.ToString();
            string errorLogFileName = "SCV_Portal_Log_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string errorLogFilePath = logFileLocation + "Log\\" + errorLogFileName;

            CreateDirectory4Log();
            string archivePath = CreateDirectory4Archive();
            MoveOldFilesToArchive(archivePath, errorLogFileName, logFileLocation);

            if (!File.Exists(errorLogFilePath))
            {
                File.Create(errorLogFilePath).Dispose();
            }
            using (FileStream fs = new FileStream(errorLogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamWriter objWriter = new StreamWriter(fs))
            {
                objWriter.WriteLine(DateTime.Today.ToString("dd/MM/yyyy") + "      " + DateTime.Now.ToString("HH:mm:ss") + "      " + " Exception Caught");
                objWriter.WriteLine(message);
                //objWriter.WriteLine();
                //objWriter.WriteLine(ex.ToString());
                //objWriter.WriteLine();
                //objWriter.WriteLine();
                objWriter.Close();
            }
            
            //StreamWriter objWriter = new StreamWriter(errorLogFilePath, true);
            
        }

        private async static void WriteEventLog(string message)
        {
            string errorLogFileName = "SCV_Portal_Log_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string errorLogFilePath = logFileLocation + "Log\\" + errorLogFileName;

            CreateDirectory4Log();
            string archivePath = CreateDirectory4Archive();
            MoveOldFilesToArchive(archivePath, errorLogFileName, logFileLocation);

            if (!File.Exists(errorLogFilePath))
            {
                File.Create(errorLogFilePath).Dispose();
            }

            using (FileStream fs = new FileStream(errorLogFilePath, FileMode.Append))
            using (StreamWriter objWriter = new StreamWriter(fs))
            {
                objWriter.WriteLine(DateTime.Today.ToString("dd/MM/yyyy") + "      " + DateTime.Now.ToString("HH:mm:ss") + "      " + message);
                objWriter.Close();
            }
        }

        private static string CreateDirectory4Log()
        {
            string _logpath = string.Empty;
            if (!Directory.Exists(logFileLocation + "Log\\"))
            {
                _logpath = logFileLocation + "Log\\";
                Directory.CreateDirectory(_logpath);
            }
            return _logpath;
        }

        private static string CreateDirectory4Archive()
        {
            string _archivepath = string.Empty;
            if (!Directory.Exists(logFileLocation + "Log\\Archive\\"))
            {
                _archivepath = logFileLocation + "Log\\Archive\\";
                Directory.CreateDirectory(_archivepath);
            }
            else
            {
                _archivepath = logFileLocation + "Log\\Archive\\";
            }
            return _archivepath;
        }

        private async static void MoveOldFilesToArchive(string archivepath, string thisFileName, string logFileLocation)
        {
            if (Directory.Exists(archivepath))
            {
                foreach (var file in new DirectoryInfo(logFileLocation + "Log\\").GetFiles())
                {
                    if (file.Name != thisFileName)
                    {
                        if (!File.Exists(archivepath + file.Name))
                            file.MoveTo(archivepath + file.Name);
                        else
                        {
                            file.MoveTo(archivepath + DateTime.Now.ToString("YYYYMMDDHHMMSS") + "_" + file.Name);
                        }
                    }
                }
            }
        }

    }
}
