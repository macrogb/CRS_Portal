using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using System.Xml;
using System.Xml.Serialization;

namespace CRS_Portal.HelperMethods
{
    public class Methods
    {
        public string connString = string.Empty;

        SqlConnection sqlConn;

        DataSet _ds;
        SqlDataAdapter _da;

        DataTable _dt = new DataTable();
        DataTable _dt1 = new DataTable();
        DataTable _dt2 = new DataTable();

        DataRow _dr;

        string _fSelect, _fSelect1;

        public string StrUser = string.Empty;
        public string StrProjName = "SCV";

        void OpenConnection()
        {
            //connString=System.Configuration.ConfigurationManager.ConnectionStrings["Constr"].ToString();
            connString = Helper.LoadDatabaseByBankName();
                       
            sqlConn = new SqlConnection(connString);
            if (sqlConn.State == ConnectionState.Closed)
            {
                sqlConn.Open();
            }
        }

        void CloseConnection()
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        public bool BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            bool isSuccuss;
            tableName = "[Adhoc_" + tableName + "]";
            try
            {
                ArrayList dt_index = new ArrayList();
                List<DataRow> rowsToDelete = new List<DataRow>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string valuesarr = string.Empty;
                    List<Object> lst = new List<Object>(dataTable.Rows[i].ItemArray);

                    foreach (Object s in lst)
                    {
                        valuesarr += s.ToString();
                    }

                    if (string.IsNullOrEmpty(valuesarr))
                    {
                        rowsToDelete.Add(dataTable.Rows[i]);
                    }
                }

                foreach (DataRow row in rowsToDelete)
                {
                    dataTable.Rows.Remove(row);
                }


                OpenConnection();
                SqlConnection SqlConnectionObj = sqlConn;

                using (var bulkCopy = new SqlBulkCopy(connString, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.BulkCopyTimeout = 0;
                    string createTable = "IF OBJECT_ID('" + tableName + "', 'U') IS NULL " + "CREATE TABLE " + tableName + "( ID INT IDENTITY(1,1) ,ADHOC_REMARKS NVARCHAR(MAX), ";
                    string dropTable = "IF OBJECT_ID('" + tableName + "', 'U') IS NOT NULL " + " DROP TABLE " + tableName;

                    ExecuteCommand(dropTable);

                    foreach (DataColumn col in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        createTable += "[" + col.ColumnName.ToString() + "]" + " NVARCHAR(MAX),";
                    }
                    createTable = createTable.TrimEnd(',');
                    createTable = createTable + ")";

                    ExecuteCommand(createTable);

                    //bulkCopy.BulkCopyTimeout = 600;
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dataTable);
                }
                isSuccuss = true;
            }
            catch (Exception ex)
            {
                isSuccuss = false;
            }
            finally
            {
                CloseConnection();
            }
            return isSuccuss;
        }

        public string CTODate(string sText)
        {
            if (!string.IsNullOrEmpty(sText) && sText.Length > 7)
            {
                sText = sText.Substring(6, 2) + "-" + sText.Substring(4, 2) + "-" + sText.Substring(0, 4);
                return sText;
            }
            else
            {
                //string sToday = DateTime.Now.ToString("dd/MM/yyyy");
                //return sToday;
                return string.Empty;
            }
        }

        public string CTOTime(string sText)
        {
            if (!string.IsNullOrEmpty(sText) && sText.Length > 5)
            {
                sText = sText.Substring(0, 2) + ":" + sText.Substring(2, 2) + ":" + sText.Substring(4, 2);
                return sText;
            }
            else
            {
                //string sToday = DateTime.Now.ToString("dd/MM/yyyy");
                //return sToday;
                return string.Empty;
            }
        }

        public DataTable GetData(string strCmd)
        {
            try
            {

                OpenConnection();

                SqlCommand sqlComm;
                sqlComm = new System.Data.SqlClient.SqlCommand(strCmd, sqlConn);
                sqlComm.CommandTimeout = 0;
                _da = new SqlDataAdapter(sqlComm);
                _ds = new DataSet();
                _da.Fill(_ds);
                _dt = _ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }

            return _dt;
        }

        public void ExecuteCommand(string command)
        {
            try
            {
                OpenConnection();

                SqlCommand strcmd;
                strcmd = new SqlCommand(command, sqlConn);
                strcmd.CommandType = CommandType.Text;
                strcmd.CommandTimeout = 0;
                strcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public string RetPolValue(string pmod, string ptype)
        {
            string strVal = string.Empty;
            string sqlCmd = "select POLDET from POLMTPF where pmodule='" + pmod.ToString().Trim() + "'" + " and ptype ='" + ptype.ToString().Trim() + "'";
            _dt = GetData(sqlCmd);

            if (_dt.Rows.Count > 0)
            {
                _dr = _dt.Rows[0];
                strVal = _dr["POLDET"].ToString().Trim();
            }

            return strVal;
        }

        public void UpdatePolValue(String pmod, String ptype, String PDet)
        {
            string strVal = string.Empty;

            try
            {
                string sqlCmd = "Update POLMTPF set POLDET='" + PDet + "' where pmodule='" + pmod.ToString().Trim() + "' and ptype ='" + ptype.ToString().Trim() + "'";
                ExecuteCommand(sqlCmd);
            }

            catch (Exception ex)
            { throw ex; }
        }

        public string NullToSpace(object txt)
        {
            string functionReturnValue = null;

            if (txt == null)
            {
                functionReturnValue = "";

            }
            else if (txt == null)
            {
                functionReturnValue = "";

            }
            else if (string.IsNullOrEmpty(txt.ToString()) | txt.ToString() == " ")
            {
                functionReturnValue = "";
            }
            else
            {
                functionReturnValue = txt.ToString().Trim();
            }
            return functionReturnValue;
        }

        public void StoreEvent(string strCode, string strFind, string strRplc, string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                StrUser = "System";
            }
            else
            {
                StrUser = UserName;
            }
            
            StoreEvent(strCode, strFind, strRplc);
        }

        public void StoreEvent(string strCode, string strFind, string strRplc)
        {           
            string Desc = string.Empty;
            if (strCode == "99999")
            {
                Desc = strRplc;
            }
            else if (strCode == "99998")
            {
                Desc = strRplc;
            }
            else if (strCode == "77777") // DP Error Code - Sucess Error Code
            {
                Desc = strRplc;
            }
            else if (strCode == "77778") // DP Error Code - Failure Error Code
            {
                Desc = strRplc;
            }
            else
            {
                Desc = GetAuditDesc(strCode).Replace(strFind, strRplc.Trim());
            }

            if (string.IsNullOrEmpty(Desc) == false)
            {
                Desc = replaceSnglQote(Desc);
            }

            _fSelect = "Insert into AUDITREPF([Date],[Time],[Category],[Description],[User])Values('" + DateTime.Today.ToString("yyyyMMdd") 
                + "','" + DateTime.Now.ToString("HHmmss") + "','" + strCode + "', '" + Desc + "','" + StrUser + "')";
            ExecuteCommand(_fSelect);
        }

        string GetAuditDesc(string strCode)
        {
            string auditDesc = string.Empty;
            _fSelect1 = "SELECT DESCRIPTION FROM  AUDITMTPF WHERE CODE='" + strCode + "'";
            _dt1 = GetData(_fSelect1);

            if (_dt1.Rows.Count > 0)
            {
                auditDesc = _dt1.Rows[0]["DESCRIPTION"].ToString().Trim();
            }
            return auditDesc;
        }

        public string replaceSnglQote(string strTxt)
        {
            string retTxt = string.Empty;
            if (strTxt != null && strTxt.Length > 0)
            {
                retTxt = strTxt.Replace("'", " ");
            }
            else
            {
                retTxt = string.Empty;
            }
            return retTxt;
        }

        public void SendEmailMessage(string pMessage, string pSubject, string pAttachment)
        {
            try
            {
                //string updsubQuery = "update POLMTPF set POLDET='Canara Bank, London – SCV Process – Data Validation & Mismatch Report for " + DateTime.Today.ToString("dd/MM/yyyy") + "' where PMODULE='APCNFG' and PTYPE='ApplicationName'";
                //ExecuteCommand(updsubQuery);

                AlertEmail objEmail = new AlertEmail();
                string _Provider = RetPolValue("APCNFG", "Provider");
                string _constr = connString;
                //string _ApplnNm = RetPolValue("APCNFG", "ApplicationName");
                string _BankNm = RetPolValue("APCNFG", "BANKNAME");
                string _ApplnNm = RetPolValue("APCNFG", "ExceptionName");
                string _EmailTo = RetPolValue("APCNFG", "DefaultEmailTo");
                string _SMTP = RetPolValue("APCNFG", "DefaultSMTP");
                string _UserId = RetPolValue("APCNFG", "DefaultEmUserId");
                string _Password = RetPolValue("APCNFG", "DefaultEmPassword");
                string _HdrBgClr = RetPolValue("APCNFG", "HeadBGColor");
                string _HdrFntClr = RetPolValue("APCNFG", "HeaderFontColor");
                //string _strPrefix = RetPolValue("APCNFG", "BANKNAME");

                _ApplnNm = _BankNm + " - " + _ApplnNm + ' ' + DateTime.Today.ToString("dd/MM/yyyy");
                //objEmail.SendEmail("1", "2", "3", "4", "5", "6", "7", "8", "9", "10");

                int iRetryCount = 0;
                for (int iCount = 0; iCount < 3; iCount++)
                {
                    iRetryCount = iRetryCount + 1;
                    try
                    {
                        objEmail.SendEmail(_Provider, _constr, _ApplnNm, _EmailTo, _SMTP, _UserId, _Password, pMessage, pSubject, pAttachment, _HdrBgClr, _HdrFntClr);

                        // Email successfully sent. kill for loop
                        iRetryCount = 4;
                        iCount = 4;
                    }
                    catch (Exception exRetry)
                    {
                        if (iRetryCount == 3)
                        {
                            throw exRetry;
                        }

                        // Sleep for 5 min if any network or internet change
                        System.Threading.Thread.Sleep(300000);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] GetFiles(string SourceFolder, string Filter, System.IO.SearchOption searchOption)
        {
            // ArrayList will hold all file names
            ArrayList alFiles = new ArrayList();

            // Create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            // for each filter find matching file names
            foreach (string FileFilter in MultipleFilters)
            {
                // add found file names to array list
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }

            // returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        }

        public void SendExceptionEmailMsg(string pMessage, string pSubject, string pAttachment)
        {
            try
            {
                //if (pSubject == null || pSubject == string.Empty || pSubject == "")
                //{
                //    pSubject = ConfigurationManager.AppSettings["ApplnNm"].ToString() + " - Exception";
                //}
                //ExceptionEmail MailObj = new ExceptionEmail();
                //MailObj.SendEmail(ConfigurationManager.AppSettings["Provider"], ConfigurationManager.AppSettings["constr"], ConfigurationManager.AppSettings["ApplnNm"], ConfigurationManager.AppSettings["EmailTo"], ConfigurationManager.AppSettings["SMTP"], ConfigurationManager.AppSettings["UserId"], ConfigurationManager.AppSettings["Password"], pMessage, pSubject, pAttachment, ConfigurationManager.AppSettings["HdrBgClr"], ConfigurationManager.AppSettings["HdrFntClr"]);

            }
            catch (Exception ex)
            {
                // Exception email thrown exception
                StoreEvent("99999", "@@@", "SCV automation - Exception email thrown an exception as " + ex.ToString().Replace("'", ""));
            }
        }

        public void WriteExceptionLog(string _SCVOutputFolder, Exception ex)
        {
            // Write Error details  ***************************************************************************************************
            string errorLogFileName = "SCV_FSCS_Validation_ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            string errorLogFilePath = _SCVOutputFolder + "Log\\" + errorLogFileName;

            if (!System.IO.Directory.Exists(_SCVOutputFolder + "Log\\"))
            {
                System.IO.Directory.CreateDirectory(_SCVOutputFolder + "Log\\");
            }

            if (!System.IO.File.Exists(errorLogFilePath))
            {
                System.IO.File.Create(errorLogFilePath).Dispose();
            }

            System.IO.StreamWriter objWriter = new System.IO.StreamWriter(errorLogFilePath, true);
            objWriter.WriteLine(DateTime.Today.ToString("dd/MM/yyyy") + "      " + DateTime.Now.ToString("HH:mm:ss") + "      " + " Exception Caught");
            objWriter.WriteLine(ex.Message);
            objWriter.WriteLine();
            objWriter.WriteLine(ex.ToString());
            objWriter.WriteLine();
            objWriter.WriteLine();
            objWriter.Close();
            // Write Error details  ***************************************************************************************************

        }

        public void WriteEventLog(string _SCVOutputFolder, string strEvent)
        {
            // Write Event Log ***************************************************************************************************
            // string logpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName);

         

            string errorLogFileName = "SCV_FSCS_Validation_ErrorLog_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            string errorLogFilePath = _SCVOutputFolder + "Log\\" + errorLogFileName;


            if (!System.IO.Directory.Exists(_SCVOutputFolder + "Log\\"))
            {
                string path = _SCVOutputFolder + "Log\\";
                System.IO.Directory.CreateDirectory(path);
            }

            if (!System.IO.File.Exists(errorLogFilePath))
            {
                System.IO.File.Create(errorLogFilePath).Dispose();
            }

            System.IO.StreamWriter objWriter = new System.IO.StreamWriter(errorLogFilePath, true);
            objWriter.WriteLine(DateTime.Today.ToString("dd/MM/yyyy") + "      " + DateTime.Now.ToString("HH:mm:ss") + "      " + strEvent);
            objWriter.Close();
            // Write Event Log ***************************************************************************************************
        }

        public string replaceFSCSSplChars(string strTxt)
        {
            string[] strSplChars = { "!", "$", "%", "*", "+", ";", "<", "=", ">", "?", "@", "^", "_", "|", "~", "\"", "'" };

            string retTxt = strTxt;
            if (retTxt != null && retTxt.Length > 0)
            {
                foreach (string x in strSplChars)
                {
                    retTxt = retTxt.Replace(x, " ");
                }
            }
            else
            {
                retTxt = string.Empty;
            }
            retTxt = retTxt.Trim();

            return retTxt;
        }

        public static List<string> SplitCustomerName(string strCustomerFullName, int totalPartitions)
        {
            List<string> list = new List<string>(strCustomerFullName.Split(' '));

            if (list == null)
                throw new ArgumentNullException("list");

            if (totalPartitions < 1)
                throw new ArgumentOutOfRangeException("totalPartitions");

            List<string>[] partitions = new List<string>[totalPartitions];

            List<string> partitionsFinal = new List<string>();
            string strTemp = null;

            int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            int k = 0;

            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<string>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                    strTemp += list[j] + " ";
                }
                k += maxSize;

                partitionsFinal.Add(strTemp.Trim());
                strTemp = "";
            }

            // Re-order the splitter names - Forename and surname should be present if there are more name parts available.
            if (partitionsFinal[0].ToString().Trim() != "")
            {
                if (partitionsFinal[3].ToString().Trim() == "")
                {
                    if (partitionsFinal[2].ToString().Trim() != "")
                    {
                        partitionsFinal[3] = partitionsFinal[2];
                        partitionsFinal[2] = "";
                    }
                    else
                    {
                        partitionsFinal[3] = partitionsFinal[1];
                        partitionsFinal[1] = "";
                    }
                }
            }
            //return partitions;
            return partitionsFinal;
        }

        public int CheckNum(int KeyVal)
        {
            int _CheckNum = 0;
            try
            {
                if ((KeyVal == 8) || ((KeyVal >= 48) && (KeyVal <= 57)))
                {
                    _CheckNum = KeyVal;
                }
                else
                {
                    _CheckNum = 0;
                }
                return (_CheckNum);
            }
            catch (Exception ex) { throw ex; }
        }

        public int CheckSpecial(int KeyVal)
        {
            int _CheckNum = 0;
            try
            {
                if ((KeyVal == 8) || ((KeyVal >= 33) && (KeyVal <= 47)) || ((KeyVal >= 58) && (KeyVal <= 64)) || ((KeyVal >= 91) && (KeyVal <= 96)) || ((KeyVal >= 123) && (KeyVal <= 126)))
                {
                    _CheckNum = KeyVal;
                }
                else
                {
                    _CheckNum = 0;
                }
                return (_CheckNum);
            }
            catch (Exception ex) { throw ex; }
        }

        public int Checkalphanum(int KeyVal)
        {
            int _CheckNum = 0;
            try
            {
                if ((KeyVal == 8) || ((KeyVal >= 48) && (KeyVal <= 57)) || ((KeyVal >= 65) && (KeyVal <= 90)) || ((KeyVal >= 97) && (KeyVal <= 122)))
                {
                    _CheckNum = KeyVal;
                }
                else
                {
                    _CheckNum = 0;
                }
                return (_CheckNum);
            }
            catch (Exception ex) { throw ex; }
        }

        public int Checkalphanum_keys(int KeyVal)
        {
            int _CheckNum = 0;
            try
            {
                if ((KeyVal == 8) || (KeyVal == 32) || (KeyVal == 44) || ((KeyVal >= 48) && (KeyVal <= 57)) || ((KeyVal >= 65) && (KeyVal <= 90)) || ((KeyVal >= 97) && (KeyVal <= 122)))
                {
                    _CheckNum = KeyVal;
                }
                else
                {
                    _CheckNum = 0;
                }
                return (_CheckNum);
            }
            catch (Exception ex) { throw ex; }
        }

        public string EncryPass(string sA)
        {
            string sEncryPass = "";

            for (int nIntI = 0; nIntI <= sA.Length - 1; nIntI++)
            {
                int i = Convert.ToInt32(System.Convert.ToChar(sA.Substring(nIntI, 1)));
                char c = Convert.ToChar(i + 5);
                sEncryPass = sEncryPass + c;
            }

            return sEncryPass;
        }

        public string DecryPass(string sA)
        {
            string sDecryPass = "";

            for (int nIntI = 0; nIntI <= sA.Length - 1; nIntI++)
            {
                int i = Convert.ToInt32(System.Convert.ToChar(sA.Substring(nIntI, 1)));
                char c = Convert.ToChar(i - 5);
                sDecryPass = sDecryPass + c;
            }

            return sDecryPass;
        }

        public void NewMailNotification(string _EmailTo, string strMailSubject, string msgbdy)
        {
            DataLayer objDataLayer = new DataLayer();
            string _ApplnNm = string.Empty;
            string mailFrom = string.Empty;
            string _UserId = string.Empty;
            string _Password = string.Empty;

            if (strMailSubject.Contains("License Exp")) // this is used for License module
            {
                _ApplnNm = RetPolValue("LICMAIL", "LicenseApplicationName");
                mailFrom = RetPolValue("LICMAIL", "LicenseFROM");
                _UserId = RetPolValue("LICMAIL", "LicenseDefaultEmUserId");
                _Password = RetPolValue("LICMAIL", "LicenseDefaultEmPassword");
            }
            else
            {
                _ApplnNm = RetPolValue("APCNFG", "ApplicationName");
                 mailFrom = RetPolValue("MAIL", "FROM");
                _UserId = RetPolValue("APCNFG", "DefaultEmUserId");
                _Password = RetPolValue("APCNFG", "DefaultEmPassword");
            }

            string headBgColor = RetPolValue("APCNFG", "HeadBGColor");
            string headFontColor = RetPolValue("APCNFG", "HeaderFontColor");
          
            string mBody = GetMessageBody(_ApplnNm, msgbdy, headBgColor, headFontColor);
            string _Provider = RetPolValue("APCNFG", "Provider");
            string _constr = connString;


            string _SMTP = RetPolValue("APCNFG", "DefaultSMTP");
            string _HdrBgClr = RetPolValue("APCNFG", "HeadBGColor");
            string _HdrFntClr = RetPolValue("APCNFG", "HeaderFontColor");
            
            bool enableSSL = bool.Parse(RetPolValue("MAIL", "ENABLESSL"));
            int port = int.Parse(RetPolValue("NET", "SMTPPORT"));
            string fpath = "";
            string sMsgTo = _EmailTo.IndexOf(";") > 0 ? _EmailTo.Substring(0, _EmailTo.IndexOf(";")) : _EmailTo;

            MailMessage msg = new MailMessage(_ApplnNm + " <" + mailFrom + ">", sMsgTo, strMailSubject, mBody);
            
            try
            {
                AlertEmail objEmail = new AlertEmail();
                objEmail.SendEmail_2020(_Provider, _constr, _EmailTo, _SMTP, _UserId, _Password, fpath, msg, enableSSL, port);
            }
            catch (Exception ex)
            {
                objDataLayer.WriteErrorLog("Mathods.NewMailNotification", _EmailTo, ex.ToString(), "");
            }
        }

        /* This function is used to develop the Email Message Body */
        private string GetMessageBody(string pApplnName, string pMessage, string _headerBgColor, string _headerFontColor)
        {
            string MsgBody = pMessage;

            //MsgBody = " <html> " +
            //           " <head><style type='text/css'>.bodytext{font-family: Trebuchet MS, Segoe UI, Verdana,  Arial, sans-serif,  Helvetica;font-size: 14px;color: #000000; padding: 1px 1px 1px 2px;} .bodytextbold{font-family: Trebuchet MS, Segoe UI, Verdana,  Arial, sans-serif,  Helvetica;font-size: 12px;font-weight: bold;color: #000000;	padding: 1px 1px 1px 2px;}</style> " +
            //           " </head> " +
            //           " <body> " +
            //           "    <table width='760' cellspacing='5' cellpadding='5' border='2' style='border-collapse:collapse;' bordercolor='" + _headerBgColor + "' " +
            //           "        bgcolor='#F9F8F0' align='center'> " +
            //           "        <tr> " +
            //           "            <td bgcolor='" + _headerBgColor + "' height='80' align='left' style='font-size:1.5em;color:" + _headerFontColor + ";Padding-left: 20px;' class='bodytext'> " +
            //                            pApplnName +
            //           "            </td> " +
            //           "        </tr> " +
            //           "        <tr> " +
            //           "            <td align='left' style='font-family: Arial; font-size: 12px; color: #000000;'> " +
            //           "                <table align='center' width='700'> ";
            //MsgBody += //"                    <tr>" +
            //           //"                         <td height='30' class='bodytext'> This Email message is sent from " +
            //           //"                            <span style='font-weight:bold;'>" + pApplnName + "</span> application." +
            //           //"                         </td> " +
            //           //"                    </tr> " +
            //           //"                    <tr> " +
            //           //"                         <td height='30' class='bodytext'> " +
            //           //"                            <span >Dated on " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToLongTimeString() + "</span> " +
            //           //"                         </td> " +
            //           //"                    </tr> " +
            //           "                    <tr> " +
            //           "                         <td height='120' class='bodytext'> " +
            //           "                            <p align='justify' style='font-weight:bold;'>  " + pMessage +
            //           "                            </p> " +
            //           "                         </td> " +
            //           "                    </tr> ";

            //MsgBody += "                    <tr> " +
            //            "                        <td height='30' class='bodytext'> " +
            //            "                            (This is an auto generated email. Please do not reply to this.) " +
            //            "                        </td> " +
            //            "                    </tr> " +
            //            "                </table> " +
            //            "            </td> " +
            //            "        </tr> " +
            //            "    </table> " +
            //            " </body> " +
            //            " </html>";
            return MsgBody;
        }

        public string ReplaceText(string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

        public Object XMLToObject(string XMLString, Object oObject)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(oObject.GetType());
            oObject = oXmlSerializer.Deserialize(new StringReader(XMLString));
            return oObject;
        }

    }
}

