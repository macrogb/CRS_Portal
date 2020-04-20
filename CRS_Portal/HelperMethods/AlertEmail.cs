using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SCV_Portal.HelperMethods
{
    public sealed class AlertEmail
    {
        #region "Instance Variables"

        //SQL Server Connection Declaration
        SqlConnection sqlcon;
        SqlCommand strcmd;
        SqlDataAdapter da;

        //MS Access Connection Declaration
        //OleDbConnection oledbcon;
        //OleDbCommand stroledbcmd;
        //OleDbDataAdapter daacc;

        MailAddressCollection objMAC;

        #endregion

        #region "Private Variables"

        private string _provider;
        private string _conStr;

        private string _headerBgColor;
        private string _headerFontColor;

        private string fselect;

        private bool isValidCon = false;

        #endregion

        #region "Constructor"

        public AlertEmail()
        {
        }

        #endregion

        #region "Functions"

        /* Initialize the connection */
        private void Connection()
        {
            switch (_provider)
            {
                case "SQLSERVER":
                    sqlcon = new SqlConnection();
                    break;
                //case "MSACCESS":
                //    oledbcon = new OleDbConnection();
                //    break;
            }
        }

        /* Open the connection */
        private void OpenConnection()
        {
            if (_provider == "SQLSERVER")
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.ConnectionString = _conStr;
                    sqlcon.Open();
                }
            }
            //else if (_provider == "MSACCESS")
            //{
            //    if (oledbcon.State == ConnectionState.Closed)
            //    {
            //        oledbcon.ConnectionString = _conStr;
            //        oledbcon.Open();
            //    }
            //}
            else { }
        }

        /* Close the connection */
        private void CloseConnection()
        {
            if (_provider == "SQLSERVER")
            {
                if (sqlcon.State == ConnectionState.Open)
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
            //else if (_provider == "MSACCESS")
            //{
            //    if (oledbcon.State == ConnectionState.Open)
            //    {
            //        oledbcon.Close();
            //        oledbcon.Dispose();
            //    }
            //}
            else { }
        }

        /* Execute the query */
        private void ExecuteCommand(string command)
        {
            try
            {
                Connection();
                OpenConnection();
                if (_provider == "SQLSERVER")
                {
                    strcmd = new SqlCommand(command, sqlcon);
                    strcmd.CommandType = CommandType.Text;
                    strcmd.ExecuteNonQuery();
                }
                //else if (_provider == "MSACCESS")
                //{
                //    stroledbcmd = new OleDbCommand(command, oledbcon);
                //    stroledbcmd.CommandType = CommandType.Text;
                //    stroledbcmd.ExecuteNonQuery();
                //}
                else { }

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

        /* Execute the query and return as DataTable*/
        private DataTable ExecuteData(string fetch)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                Connection();

                OpenConnection();

                if (_provider == "SQLSERVER")
                {
                    strcmd = new SqlCommand(fetch, sqlcon);
                    da = new SqlDataAdapter(strcmd);
                    da.Fill(ds);
                }
                //else if (_provider == "MSACCESS")
                //{
                //    stroledbcmd = new OleDbCommand(fetch, oledbcon);
                //    daacc = new OleDbDataAdapter(stroledbcmd);
                //    daacc.Fill(ds);
                //}
                else
                {
                }

                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }

            return dt;
        }

        /* Test the Connection using the connection string*/
        private void IsValidConnection()
        {
            try
            {
                if ((!string.IsNullOrEmpty(_provider)) && (_provider == "SQLSERVER" || _provider == "MSACCESS"))
                {
                    Connection();
                    OpenConnection();
                    CloseConnection();
                    isValidCon = true;
                }
                else
                {
                    isValidCon = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                isValidCon = false;
                throw ex;
            }
        }

        /* This function is used to create EMCONTENTPF table only if EMCONTENTPF table is not exists.  */
        private void CreateTable()
        {
            string sqlQuery = string.Empty;
            try
            {
                if (_provider == "SQLSERVER")
                {
                    sqlQuery = " IF OBJECT_ID('EMCONTENTPF', 'U') IS NOT NULL " + //Check the table EMCONTENTPF is Exists.
                               " BEGIN  " +
                               "    IF COL_LENGTH('EMCONTENTPF','EMCC') IS NULL " + // Check the EMCC column is Exists in EMCONTENTPF table.
                               "    BEGIN " +
                               "        ALTER TABLE EMCONTENTPF " + // Add EMCC column in EMCONTENTPF table.
                               "        ADD " +
                               "        EMCC ntext NULL " +
                               "    END  " +
                               " END " +
                               " ELSE " +
                               " BEGIN " +
                               "    CREATE TABLE EMCONTENTPF( " + // If EMCONTENTPF table is not exists, Create EMCONTENTPF table.
                               "    EMDATE	    nvarchar(8)		Not null, " +
                               "    EMTIME	    nvarchar(8)		Not null, " +
                               "    EMFROM	    nvarchar(70)	Not null, " +
                               "    EMTO	    nvarchar(70)	Not null, " +
                               "    EMSUB	    nvarchar(120)	Not null, " +
                               "    EMATTA	    Image			Null, " +
                               "    EMBODY	    nText			Not null, " +
                               "    EMSTATUS   nvarchar(1)	    Not null, " +
                               "    EMCC	    nText	        Null) " +
                               " END ";
                    ExecuteCommand(sqlQuery);
                }
                else if (_provider == "MSACCESS")
                {

                    if (IsTableExist("EMCONTENTPF"))
                    {
                        if (!IsFieldExist("EMCONTENTPF", "EMCC"))
                        {
                            sqlQuery = " ALTER TABLE EMCONTENTPF ADD COLUMN EMCC MEMO NULL ";
                            ExecuteCommand(sqlQuery);
                        }
                    }
                    else
                    {
                        sqlQuery = " CREATE TABLE EMCONTENTPF( " +
                                   " EMDATE      varchar(8) 		Not null, " +
                                   " EMTIME	    varchar(8)		Not null,  " +
                                   " EMFROM	    varchar(70) 	Not null,  " +
                                   " EMTO	    varchar(70)	    Not null,  " +
                                   " EMSUB	    varchar(120)	Not null,  " +
                                   " EMATTA	    OLEObject 		Null,  " +
                                   " EMBODY	    Text	 		Not null,  " +
                                   " EMSTATUS    varchar(1)	    Not null,  " +
                                   " EMCC	    Text	        Null)  ";
                        ExecuteCommand(sqlQuery);

                    }
                }
                else
                {
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /* This function is used Check the table EMCONTENTPF already exists or not in MSACCESS database */
        private bool IsTableExist(string tblName)
        {
            bool bStatus = false;
            DataTable dbTbl = new DataTable();
            string[] restrictions = new string[4];

            try
            {
                Connection();
                OpenConnection();

                restrictions[2] = tblName;
                //dbTbl = oledbcon.GetSchema("Tables", restrictions);

                if (dbTbl.Rows.Count > 0)   //Table does not exist           
                    bStatus = true;
                else
                    bStatus = false;        //Table exists       
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { CloseConnection(); }
            return bStatus;
        }

        /* This function is used Check the field EMCC in the table EMCONTENTPF already exists or not in MSACCESS database */
        private bool IsFieldExist(string tblName, string fldName)
        {
            bool bStatus = false;
            DataTable dbTbl = new DataTable();
            try
            {
                Connection();
                OpenConnection();
                // Get the table definition loaded in a table
                string strSql = "Select TOP 1 * from " + tblName;
                //daacc = new OleDbDataAdapter(strSql, oledbcon);
                //daacc.Fill(dbTbl);
                //daacc.Dispose();
                // Get the index of the field name
                int i = dbTbl.Columns.IndexOf(fldName);

                if (i == -1) //Field is missing                
                    bStatus = false;
                else //Field is there                
                    bStatus = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return bStatus;

        }

        /* This function is used to convert to string from object */
        public string NullToSpace(object txt)
        {
            string strRetValue = string.Empty;

            if (txt == DBNull.Value)
                strRetValue = "";
            else if (txt == null)
                strRetValue = "";
            else if ((txt.ToString() == "") || (txt.ToString() == " "))
                strRetValue = "";
            else
                strRetValue = Convert.ToString(txt).Trim();

            return strRetValue;
        }

        /* This function is used to avoid the single quotes */
        private string replacesplchr(string txt)
        {
            if ((txt != null) && (txt != string.Empty) && (txt != ""))
            {
                txt = txt.Replace("'", "").Replace(",", "");
                txt = txt.Trim();
            }
            else
            {
                txt = string.Empty;
            }

            return txt;
        }

        /* This function is used to retrieve Module details */
        private string GetDetailValue(string pmod, string ptype)
        {
            string DetailValue = string.Empty;
            try
            {
                fselect = "select POLDET from POLMTPF where pmodule='" + pmod.Trim() + "'" + " and ptype ='" + ptype.Trim() + "'";

                DataTable dt = ExecuteData(fselect);

                if (dt.Rows.Count > 0)
                    DetailValue = NullToSpace(dt.Rows[0]["POLDET"]);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return DetailValue;
        }

        /* This function is used to retrieve mail addresses */
        private MailAddressCollection GetMailAddressList(string pModuleNm, string pType)
        {
            objMAC = new MailAddressCollection();
            string MailAddresses = string.Empty;

            fselect = "select POLDET from POLMTPF where pmodule='" + pModuleNm.Trim() + "'" + " and ptype ='" + pType.Trim() + "'";

            DataTable dt = ExecuteData(fselect);

            if (dt.Rows.Count > 0)
            {
                MailAddresses = NullToSpace(dt.Rows[0]["POLDET"]);
                if (string.IsNullOrEmpty(MailAddresses))
                {
                    return objMAC;
                }
                string[] mailIdCollections = MailAddresses.Split(';');

                if (mailIdCollections.Count() > 0)
                {
                    foreach (string mailId in mailIdCollections)
                    {
                        if (mailId != string.Empty)
                            objMAC.Add(new MailAddress(mailId));
                    }
                }
                else
                {
                    if (MailAddresses != string.Empty)
                        objMAC.Add(new MailAddress(MailAddresses));
                }
            }
            return objMAC;
        }

        private MailAddressCollection GetMailAddressList(string EmailId)
        {
            objMAC = new MailAddressCollection();
            string MailAddresses = string.Empty;


            MailAddresses = NullToSpace(EmailId);

            string[] mailIdCollections = MailAddresses.Split(';');

            if (mailIdCollections.Count() > 0)
            {
                foreach (string mailId in mailIdCollections)
                {
                    if (mailId != string.Empty)
                        objMAC.Add(new MailAddress(mailId));
                }
            }
            else
            {
                if (MailAddresses != string.Empty)
                    objMAC.Add(new MailAddress(MailAddresses));
            }

            return objMAC;
        }



        /****************************************************************************************************/
        /*                                  This is the Main function                                       */
        /*                 Sending Alert Emails and Store the Email message in database                     */
        /****************************************************************************************************/

        public void SendEmail(string pProvider, string pConnnectionString, string pApplicationName, string pMailTo, string pSmtpHost, string pUserId, string pPassword,
            string pMessage, string pSubject, string pFullAttachmentPath, string pHeaderBgColor, string pHeaderFontColor)
        {
            //MessageBox.Show("I am here!!");
            _provider = pProvider;
            _conStr = pConnnectionString;

            _headerBgColor = pHeaderBgColor;
            _headerFontColor = pHeaderFontColor;

            byte[] bArray = { };

            string SMTP = string.Empty;
            string MsgFrom = string.Empty;
            string MailFrmPW = string.Empty;
            string MailFrmUID = string.Empty;
            string MsgTo = string.Empty;
            string MsgSubject = string.Empty;
            string MsgBody = string.Empty;
            string ccEmail = string.Empty;

            MailMessage msg = null;
            FileStream readStream = null;
            BinaryReader readBinary = null;
            MailAddressCollection lstMAC;
            MailAddressCollection bcclstMAC;

            IsValidConnection();
            if (isValidCon)
            {
                bool bMailSent = false;
                try
                {

                    MsgFrom = GetDetailValue("MAIL", "FROM");

                    MailFrmUID = GetDetailValue("MAIL", "FRUID");
                    MailFrmPW = GetDetailValue("MAIL", "FRPASS");

                    SMTP = GetDetailValue("NET", "BACKSMTP");

                    //MessageBox.Show("From : " + MsgFrom);
                    //MessageBox.Show("From Uid: " + MailFrmUID);
                    //MessageBox.Show("Password : " + MailFrmPW);
                    //MessageBox.Show("SMTP : " + SMTP);


                    /* Save the email message */
                    try
                    {
                        if (pFullAttachmentPath.Trim().Length > 0)
                        {
                            FileInfo finfo = new FileInfo(pFullAttachmentPath);
                            if (finfo.Exists)
                            {
                                readStream = new FileStream(pFullAttachmentPath, FileMode.Open, FileAccess.Read);
                                readBinary = new BinaryReader(readStream);
                                bArray = readBinary.ReadBytes(Convert.ToInt32(finfo.Length));
                            }
                            finfo = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (readBinary != null)
                        {
                            readBinary.Close();
                        }
                        if (readStream != null)
                        {
                            readStream.Close();
                            readStream.Dispose();
                        }
                    }

                    /* Get mail address to send Email*/
                    lstMAC = GetMailAddressList("MAIL", "EMAILERRNOT");

                    /* If the SMTP mail server has no value, use the alternative smtp.  */
                    if (MsgFrom == string.Empty || MailFrmPW == string.Empty || SMTP == string.Empty)
                    {
                        MsgFrom = pUserId;
                        MailFrmPW = pPassword;
                        SMTP = pSmtpHost;
                    }

                    if (lstMAC.Count > 0)
                    {
                        // Assign the first item to EmailTo address
                        MsgTo = NullToSpace(lstMAC[0]);

                        // Remove the first item in the collection, because the first item is used as To Address.
                        lstMAC.RemoveAt(0);
                    }
                    else
                    {
                        MsgTo = pMailTo; // If the To address has no value, use the alternative address.
                    }

                    MsgSubject = pSubject;

                    MsgBody = GetMessageBody(pApplicationName, pMessage);

                    msg = new MailMessage(pApplicationName + " <" + MsgFrom + ">", MsgTo, MsgSubject, MsgBody);

                    //MessageBox.Show("To : " + MsgTo);
                    //MessageBox.Show("Subject : " + MsgSubject);
                    //MessageBox.Show("Body : " + MsgBody);

                    if (pFullAttachmentPath.Trim().Length > 0)
                    {
                        FileInfo fi = new FileInfo(pFullAttachmentPath);
                        if (fi.Exists)
                        {
                            msg.Attachments.Add(new Attachment(pFullAttachmentPath));
                        }
                        fi = null;
                    }

                    foreach (MailAddress mAdd in lstMAC)
                    {
                        msg.CC.Add(mAdd);

                        /* Converting list of lstMAC into string */
                        string mailid = NullToSpace(mAdd);
                        if (mailid != string.Empty)
                            ccEmail += mailid + ";";
                    }

                    if (ccEmail.Length > 0)
                    {
                        ccEmail = ccEmail.Substring(0, ccEmail.Length - 1);
                    }

                    bcclstMAC = GetMailAddressList("MAIL", "BCCEMAIL");
                    foreach (MailAddress mAdd in bcclstMAC)
                    {
                        msg.Bcc.Add(mAdd);                      
                        string mailid = NullToSpace(mAdd);
                        if (mailid != string.Empty)
                            ccEmail += mailid + ";";
                    }

                    msg.IsBodyHtml = true;

                    SmtpClient mailClient = new SmtpClient(SMTP, 25);
                    //NetworkCredential NetCrd = new NetworkCredential(MailFrmUID, MailFrmPW);
                    mailClient.UseDefaultCredentials = true;
                    //mailClient.Credentials = NetCrd;

                    mailClient.Port = int.Parse(GetDetailValue("NET", "SMTPPORT"));
                    mailClient.EnableSsl = bool.Parse(GetDetailValue("MAIL", "ENABLESSL"));

                    // Added By Rose - 17/10/2013
                    if (pFullAttachmentPath.Trim().Length > 0)
                    {
                        FileInfo fi = new FileInfo(pFullAttachmentPath);
                        if (fi.Exists)
                        {
                            mailClient.Timeout = Convert.ToInt32(fi.Length / 1024) * 10000;
                        }
                        fi = null;
                    }

                    mailClient.Send(msg);
                    bMailSent = true;

                    SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "1", ccEmail);

                }
                catch (Exception ex)
                {
                    if (bMailSent)
                    {
                        /* Save the email message. Mail sent, but could not save the file into database */
                        SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "1", ccEmail);
                    }
                    else
                    {
                        /* Save the email message. This function is execute only the database connection is valid */
                        SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "0", ccEmail);
                    }

                    throw ex;
                }
                finally
                {
                    if (msg != null)
                    {
                        msg.Dispose();
                        msg = null;
                    }
                }
            }
            else
            {
                SendStaticEmail(pApplicationName, pMailTo, pSmtpHost, pUserId, pPassword, pSubject, pMessage, pFullAttachmentPath);
            }
        }

        public void SendEmail_2020(string pProvider, string pConnnectionString, string pMailTo, string pSmtpHost, string pUserId, 
            string pPassword, string pFullAttachmentPath, MailMessage msg, bool enableSSL, int port)
        {
            //MessageBox.Show("I am here!!");
            _provider = pProvider;
            _conStr = pConnnectionString;

            byte[] bArray = { };

            string SMTP = string.Empty;
            string MsgFrom = string.Empty;
            string MailFrmPW = string.Empty;
            string MailFrmUID = string.Empty;
            string MsgTo = string.Empty;
            string MsgSubject = string.Empty;
            string ccEmail = string.Empty;

            FileStream readStream = null;
            BinaryReader readBinary = null;
            MailAddressCollection lstMAC;
            MailAddressCollection bcclstMAC;
            IsValidConnection();
            if (isValidCon)
            {
                bool bMailSent = false;
                try
                {
                    MsgFrom = GetDetailValue("MAIL", "FROM");

                    MailFrmUID = pUserId;
                    MailFrmPW = pPassword;

                    SMTP = pSmtpHost;

                    /* Save the email message */
                    try
                    {
                        if (pFullAttachmentPath.Trim().Length > 0)
                        {
                            FileInfo finfo = new FileInfo(pFullAttachmentPath);
                            if (finfo.Exists)
                            {
                                readStream = new FileStream(pFullAttachmentPath, FileMode.Open, FileAccess.Read);
                                readBinary = new BinaryReader(readStream);
                                bArray = readBinary.ReadBytes(Convert.ToInt32(finfo.Length));
                            }
                            finfo = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (readBinary != null)
                        {
                            readBinary.Close();
                        }
                        if (readStream != null)
                        {
                            readStream.Close();
                            readStream.Dispose();
                        }
                    }


                    if (pMailTo == "")
                        lstMAC = GetMailAddressList("APCNFG", "DefaultEmailTo");
                    else
                        lstMAC = GetMailAddressList(pMailTo);

                    /* If the SMTP mail server has no value, use the alternative smtp.  */
                    if (MsgFrom == string.Empty || MailFrmPW == string.Empty || SMTP == string.Empty)
                    {
                        MsgFrom = pUserId;
                        MailFrmPW = pPassword;
                        SMTP = pSmtpHost;
                    }

                    if (lstMAC.Count > 0)
                    {
                        // Assign the first item to EmailTo address
                        MsgTo = NullToSpace(lstMAC[0]);

                        // Remove the first item in the collection, because the first item is used as To Address.
                        lstMAC.RemoveAt(0);
                    }

                    SmtpClient mailClient = new SmtpClient(SMTP, port);


                    if (pFullAttachmentPath.Trim().Length > 0)
                    {
                        FileInfo fi = new FileInfo(pFullAttachmentPath);
                        if (fi.Exists)
                        {
                            msg.Attachments.Add(new Attachment(pFullAttachmentPath));
                            mailClient.Timeout += Convert.ToInt32(fi.Length / 1024) * 1000;
                        }
                        fi = null;
                    }

                    foreach (MailAddress mAdd in lstMAC)
                    {
                        msg.CC.Add(mAdd);

                        /* Converting list of lstMAC into string */
                        string mailid = NullToSpace(mAdd);
                        if (mailid != string.Empty)
                            ccEmail += mailid + ";";
                    }

                    bcclstMAC = GetMailAddressList("MAIL", "BCCEMAIL");

                    foreach (MailAddress mAdd in bcclstMAC)
                    {
                        msg.Bcc.Add(mAdd);

                        /* Converting list of lstMAC into string */
                        string mailid = NullToSpace(mAdd);
                        if (mailid != string.Empty)
                            ccEmail += mailid + ";";
                    }

                    if (ccEmail.Length > 0)
                    {
                        ccEmail = ccEmail.Substring(0, ccEmail.Length - 1);
                    }

                    msg.IsBodyHtml = true;

                    NetworkCredential NetCrd = new NetworkCredential(MailFrmUID, MailFrmPW);
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = NetCrd;
                    mailClient.EnableSsl = enableSSL;



                    mailClient.Send(msg);
                    bMailSent = true;

                    SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, msg.Body, bArray, "1", ccEmail);

                }
                catch (Exception ex)
                {
                    Methods obj = new Methods();
                    // obj.StoreEvent("99999", "", ex.ToString());
                    if (bMailSent)
                    {

                        /* Save the email message. Mail sent, but could not save the file into database */
                        SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, msg.Body, bArray, "1", ccEmail);
                    }
                    else
                    {
                        /* Save the email message. This function is execute only the database connection is valid */
                        SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, msg.Body, bArray, "0", ccEmail);
                    }

                    throw ex;
                }
                finally
                {
                    if (msg != null)
                    {
                        msg.Dispose();
                        msg = null;
                    }
                }
            }
            else
            {
                //SendStaticEmail(pApplicationName, pMailTo, pSmtpHost, pUserId, pPassword, pSubject, pMessage, pFullAttachmentPath);
            }
        }

        public void SendEmail_withMultiAttach_CRS(string pProvider, string pConnnectionString, string pMailTo, string pSmtpHost, string pUserId, string pPassword, string[] pFullAttachmentPath, MailMessage msg, bool enableSSL, int port)
        {
            //MessageBox.Show("I am here!!");
            _provider = pProvider;
            _conStr = pConnnectionString;

            // _headerBgColor = pHeaderBgColor;
            //_headerFontColor = pHeaderFontColor;

            byte[] bArray = { };

            string SMTP = string.Empty;
            string MsgFrom = string.Empty;
            string MailFrmPW = string.Empty;
            string MailFrmUID = string.Empty;
            string MsgTo = string.Empty;
            string MsgSubject = string.Empty;
            string MsgBody = string.Empty;
            string ccEmail = string.Empty;

            //MailMessage msg = null;
            FileStream readStream = null;
            BinaryReader readBinary = null;
            MailAddressCollection lstMAC;

            IsValidConnection();
            if (isValidCon)
            {
                bool bMailSent = false;
                try
                {

                    //MsgFrom = GetDetailValue("MAIL", "FROM");

                    //MailFrmUID = GetDetailValue("MAIL", "FRUID");
                    //MailFrmPW = GetDetailValue("MAIL", "FRPASS");

                    //SMTP = GetDetailValue("NET", "BACKSMTP");


                    MsgFrom = GetDetailValue("MAIL", "FROM");

                    MailFrmUID = pUserId;
                    MailFrmPW = pPassword;

                    SMTP = pSmtpHost;

                    //MessageBox.Show("From : " + MsgFrom);
                    //MessageBox.Show("From Uid: " + MailFrmUID);
                    //MessageBox.Show("Password : " + MailFrmPW);
                    //MessageBox.Show("SMTP : " + SMTP);


                    /* Save the email message */
                    try
                    {
                        if (pFullAttachmentPath[0].Trim().Length > 0)
                        {
                            FileInfo finfo = new FileInfo(pFullAttachmentPath[0]);
                            if (finfo.Exists)
                            {
                                readStream = new FileStream(pFullAttachmentPath[0], FileMode.Open, FileAccess.Read);
                                readBinary = new BinaryReader(readStream);
                                bArray = readBinary.ReadBytes(Convert.ToInt32(finfo.Length));
                            }
                            finfo = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (readBinary != null)
                        {
                            readBinary.Close();
                        }
                        if (readStream != null)
                        {
                            readStream.Close();
                            readStream.Dispose();
                        }
                    }

                    /* Get mail address to send Email*/
                    //lstMAC = GetMailAddressList("MAIL", "EMAILERRNOT");

                    if (pMailTo == "")
                        lstMAC = GetMailAddressList("APCNFG", "DefaultEmailTo");
                    else
                        lstMAC = GetMailAddressList(pMailTo);




                    /* If the SMTP mail server has no value, use the alternative smtp.  */
                    if (MsgFrom == string.Empty || MailFrmPW == string.Empty || SMTP == string.Empty)
                    {
                        MsgFrom = pUserId;
                        MailFrmPW = pPassword;
                        SMTP = pSmtpHost;
                    }

                    if (lstMAC.Count > 0)
                    {
                        // Assign the first item to EmailTo address
                        MsgTo = NullToSpace(lstMAC[0]);

                        // Remove the first item in the collection, because the first item is used as To Address.
                        lstMAC.RemoveAt(0);
                    }

                    //MsgSubject = pSubject;

                    //MsgBody = GetMessageBody(pApplicationName, pMessage);
                    //MsgBody = mBody;

                    //msg = new MailMessage(pApplicationName + " <" + MsgFrom + ">", MsgTo, MsgSubject, MsgBody);

                    //MessageBox.Show("To : " + MsgTo);
                    //MessageBox.Show("Subject : " + MsgSubject);
                    //MessageBox.Show("Body : " + MsgBody);
                    SmtpClient mailClient = new SmtpClient(SMTP, port);

                    foreach (string filePath in pFullAttachmentPath)
                    {
                        if (filePath.Trim().Length > 0)
                        {
                            FileInfo fi = new FileInfo(filePath);
                            if (fi.Exists)
                            {
                                msg.Attachments.Add(new Attachment(filePath));
                                mailClient.Timeout += Convert.ToInt32(fi.Length / 1024) * 1000;
                            }
                            fi = null;
                        }
                    }


                    foreach (MailAddress mAdd in lstMAC)
                    {
                        msg.CC.Add(mAdd);

                        /* Converting list of lstMAC into string */
                        string mailid = NullToSpace(mAdd);
                        if (mailid != string.Empty)
                            ccEmail += mailid + ";";
                    }


                    if (ccEmail.Length > 0)
                    {
                        ccEmail = ccEmail.Substring(0, ccEmail.Length - 1);
                    }

                    msg.IsBodyHtml = true;
                    //msg.

                    //SmtpClient mailClient = new SmtpClient(SMTP, 25);

                    NetworkCredential NetCrd = new NetworkCredential(MailFrmUID, MailFrmPW);
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = NetCrd;
                    mailClient.EnableSsl = enableSSL;


                    mailClient.Send(msg);
                    bMailSent = true;

                    SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "1", ccEmail);

                }
                catch (Exception ex)
                {
                    if (bMailSent)
                    {
                        /* Save the email message. Mail sent, but could not save the file into database */
                        SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "1", ccEmail);
                    }
                    else
                    {
                        /* Save the email message. This function is execute only the database connection is valid */
                        SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "0", ccEmail);
                    }

                    throw ex;
                }
                finally
                {
                    if (msg != null)
                    {
                        msg.Dispose();
                        msg = null;
                    }
                }
            }
            else
            {
                //SendStaticEmail(pApplicationName, pMailTo, pSmtpHost, pUserId, pPassword, pSubject, pMessage, pFullAttachmentPath);
            }
        }


        public void SendEmail(string pProvider, string pConnnectionString, string pApplicationName, string pMailTo, string pSmtpHost, string pUserId, string pPassword, string pMessage, string pSubject, string pFullAttachmentPath, string pHeaderBgColor, string pHeaderFontColor, string imgLocation)
        {
            //MessageBox.Show("I am here!!");
            _provider = pProvider;
            _conStr = pConnnectionString;

            _headerBgColor = pHeaderBgColor;
            _headerFontColor = pHeaderFontColor;

            byte[] bArray = { };

            string SMTP = string.Empty;
            string MsgFrom = string.Empty;
            string MailFrmPW = string.Empty;
            string MailFrmUID = string.Empty;
            string MsgTo = string.Empty;
            string MsgSubject = string.Empty;
            string MsgBody = string.Empty;
            string ccEmail = string.Empty;

            MailMessage msg = null;
            FileStream readStream = null;
            BinaryReader readBinary = null;
            MailAddressCollection lstMAC;

            IsValidConnection();
            if (isValidCon)
            {
                bool bMailSent = false;
                try
                {

                    MsgFrom = GetDetailValue("MAIL", "FROM");

                    MailFrmUID = GetDetailValue("MAIL", "FRUID");
                    MailFrmPW = GetDetailValue("MAIL", "FRPASS");

                    SMTP = GetDetailValue("NET", "BACKSMTP");

                    //MessageBox.Show("From : " + MsgFrom);
                    //MessageBox.Show("From Uid: " + MailFrmUID);
                    //MessageBox.Show("Password : " + MailFrmPW);
                    //MessageBox.Show("SMTP : " + SMTP);


                    /* Save the email message */
                    try
                    {
                        if (pFullAttachmentPath.Trim().Length > 0)
                        {
                            FileInfo finfo = new FileInfo(pFullAttachmentPath);
                            if (finfo.Exists)
                            {
                                readStream = new FileStream(pFullAttachmentPath, FileMode.Open, FileAccess.Read);
                                readBinary = new BinaryReader(readStream);
                                bArray = readBinary.ReadBytes(Convert.ToInt32(finfo.Length));
                            }
                            finfo = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (readBinary != null)
                        {
                            readBinary.Close();
                        }
                        if (readStream != null)
                        {
                            readStream.Close();
                            readStream.Dispose();
                        }
                    }

                    /* Get mail address to send Email*/
                    lstMAC = GetMailAddressList("MAIL", "EMAILERRNOT");

                    /* If the SMTP mail server has no value, use the alternative smtp.  */
                    if (MsgFrom == string.Empty || MailFrmPW == string.Empty || SMTP == string.Empty)
                    {
                        MsgFrom = pUserId;
                        MailFrmPW = pPassword;
                        SMTP = pSmtpHost;
                    }

                    if (lstMAC.Count > 0)
                    {
                        // Assign the first item to EmailTo address
                        MsgTo = NullToSpace(lstMAC[0]);

                        // Remove the first item in the collection, because the first item is used as To Address.
                        lstMAC.RemoveAt(0);
                    }
                    else
                    {
                        MsgTo = pMailTo; // If the To address has no value, use the alternative address.
                    }

                    MsgSubject = pSubject;

                    MsgBody = GetMessageBody(pApplicationName, pMessage, imgLocation);

                    AlternateView alHTML = AlternateView.CreateAlternateViewFromString(MsgBody, null, MediaTypeNames.Text.Html);
                    string logo = imgLocation + "logo.jpg";
                    LinkedResource lkLogo = new LinkedResource(logo, MediaTypeNames.Image.Jpeg);
                    lkLogo.ContentId = "Logo";
                    alHTML.LinkedResources.Add(lkLogo);

                    string scvimg1 = imgLocation + "fscs_scv.jpg";
                    LinkedResource lkscv1 = new LinkedResource(scvimg1, MediaTypeNames.Image.Jpeg);
                    lkscv1.ContentId = "scv1";
                    alHTML.LinkedResources.Add(lkscv1);

                    string scvimg2 = imgLocation + "fscs_scvg.jpg";
                    LinkedResource lkscv2 = new LinkedResource(scvimg2, MediaTypeNames.Image.Jpeg);
                    lkscv2.ContentId = "scv2";
                    alHTML.LinkedResources.Add(lkscv2);

                    string scvimg3 = imgLocation + "pra-&-fsa.jpg";
                    LinkedResource lkscv3 = new LinkedResource(scvimg3, MediaTypeNames.Image.Jpeg);
                    lkscv3.ContentId = "scv3";
                    alHTML.LinkedResources.Add(lkscv3);

                    string dotimg = imgLocation + "dot.jpg";
                    LinkedResource lkdot = new LinkedResource(dotimg, MediaTypeNames.Image.Jpeg);
                    lkdot.ContentId = "dot";
                    alHTML.LinkedResources.Add(lkdot);

                    msg = new MailMessage(pApplicationName + " <" + MsgFrom + ">", MsgTo);
                    msg.Subject = MsgSubject;
                    msg.AlternateViews.Add(alHTML);

                    //MessageBox.Show("To : " + MsgTo);
                    //MessageBox.Show("Subject : " + MsgSubject);
                    //MessageBox.Show("Body : " + MsgBody);

                    if (pFullAttachmentPath.Trim().Length > 0)
                    {
                        FileInfo fi = new FileInfo(pFullAttachmentPath);
                        if (fi.Exists)
                        {
                            msg.Attachments.Add(new Attachment(pFullAttachmentPath));
                        }
                        fi = null;
                    }

                    foreach (MailAddress mAdd in lstMAC)
                    {
                        msg.CC.Add(mAdd);

                        /* Converting list of lstMAC into string */
                        string mailid = NullToSpace(mAdd);
                        if (mailid != string.Empty)
                            ccEmail += mailid + ";";
                    }

                    if (ccEmail.Length > 0)
                    {
                        ccEmail = ccEmail.Substring(0, ccEmail.Length - 1);
                    }

                    msg.IsBodyHtml = true;

                    SmtpClient mailClient = new SmtpClient(SMTP, 25);
                    //NetworkCredential NetCrd = new NetworkCredential(MailFrmUID, MailFrmPW);
                    mailClient.UseDefaultCredentials = true;
                    //mailClient.Credentials = NetCrd;

                    // Added By Rose - 17/10/2013
                    if (pFullAttachmentPath.Trim().Length > 0)
                    {
                        FileInfo fi = new FileInfo(pFullAttachmentPath);
                        if (fi.Exists)
                        {
                            mailClient.Timeout = Convert.ToInt32(fi.Length / 1024) * 1000;
                        }
                        fi = null;
                    }

                    mailClient.Send(msg);
                    bMailSent = true;

                    SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "1", ccEmail);

                }
                catch (Exception ex)
                {
                    if (bMailSent)
                    {
                        /* Save the email message. Mail sent, but could not save the file into database */
                        SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "1", ccEmail);
                    }
                    else
                    {
                        /* Save the email message. This function is execute only the database connection is valid */
                        SaveErrorMessage(MsgFrom, MsgTo, MsgSubject, MsgBody, bArray, "0", ccEmail);
                    }

                    throw ex;
                }
                finally
                {
                    if (msg != null)
                    {
                        msg.Dispose();
                        msg = null;
                    }
                }
            }
            else
            {
                SendStaticEmail(pApplicationName, pMailTo, pSmtpHost, pUserId, pPassword, pSubject, pMessage, pFullAttachmentPath, imgLocation);
            }
        }




        /* This function is used to send the email, Only when the database could not connect to db. */
        private void SendStaticEmail(string pApplicationName, string pMailTo, string pSmtpHost, string pUserId, string pPassword, string pSubject, string pMessage, string pAttachmentPath, string imgLocation)
        {
            MailMessage msg = null;
            try
            {
                string MsgFrom = pApplicationName + " <" + pUserId + ">";
                string MsgSubject = pSubject;
                string MsgBody = GetMessageBody(pApplicationName, pMessage, imgLocation);

                msg = new MailMessage(MsgFrom, pMailTo, MsgSubject, MsgBody);
                msg.IsBodyHtml = true;

                if (pAttachmentPath.Trim().Length > 0)
                {
                    FileInfo fi = new FileInfo(pAttachmentPath);
                    if (fi.Exists)
                    {
                        msg.Attachments.Add(new Attachment(pAttachmentPath));
                    }
                    fi = null;
                }

                SmtpClient mailClient = new SmtpClient(pSmtpHost, 25);
                //NetworkCredential NetCrd = new NetworkCredential(pUserId, pPassword);
                mailClient.UseDefaultCredentials = true;
                //mailClient.Credentials = NetCrd;

                mailClient.Send(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (msg != null)
                {
                    msg.Dispose();
                    msg = null;
                }
            }
        }

        private void SendStaticEmail(string pApplicationName, string pMailTo, string pSmtpHost, string pUserId, string pPassword, string pSubject, string pMessage, string pAttachmentPath)
        {
            MailMessage msg = null;
            try
            {
                string MsgFrom = pApplicationName + " <" + pUserId + ">";
                string MsgSubject = pSubject;
                string MsgBody = GetMessageBody(pApplicationName, pMessage);

                msg = new MailMessage(MsgFrom, pMailTo, MsgSubject, MsgBody);
                msg.IsBodyHtml = true;

                if (pAttachmentPath.Trim().Length > 0)
                {
                    FileInfo fi = new FileInfo(pAttachmentPath);
                    if (fi.Exists)
                    {
                        msg.Attachments.Add(new Attachment(pAttachmentPath));
                    }
                    fi = null;
                }

                SmtpClient mailClient = new SmtpClient(pSmtpHost, 25);
                //NetworkCredential NetCrd = new NetworkCredential(pUserId, pPassword);
                mailClient.UseDefaultCredentials = true;
                //mailClient.Credentials = NetCrd;

                mailClient.Send(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (msg != null)
                {
                    msg.Dispose();
                    msg = null;
                }
            }
        }


        /* This function is used to develop the Email Message Body */
        private string GetMessageBody(string pApplnName, string pMessage)
        {
            string MsgBody = string.Empty;

            MsgBody = " <html> " +
                       " <head><style type='text/css'>.bodytext{font-family: Trebuchet MS, Segoe UI, Verdana,  Arial, sans-serif,  Helvetica;font-size: 14px;color: #000000; padding: 1px 1px 1px 2px;} .bodytextbold{font-family: Trebuchet MS, Segoe UI, Verdana,  Arial, sans-serif,  Helvetica;font-size: 12px;font-weight: bold;color: #000000;	padding: 1px 1px 1px 2px;}</style> " +
                       " </head> " +
                       " <body> " +
                       "    <table width='760' cellspacing='5' cellpadding='5' border='2' style='border-collapse:collapse;' bordercolor='" + _headerBgColor + "' " +
                       "        bgcolor='#F9F8F0' align='center'> " +
                       "        <tr> " +
                       "            <td bgcolor='" + _headerBgColor + "' height='80' align='left' style='font-size:1.5em;color:" + _headerFontColor + ";Padding-left: 20px;' class='bodytext'> " +
                                        pApplnName +
                       "            </td> " +
                       "        </tr> " +
                       "        <tr> " +
                       "            <td align='left' style='font-family: Arial; font-size: 12px; color: #000000;'> " +
                       "                <table align='center' width='700'> ";
            MsgBody += "                    <tr>" +
                       "                         <td height='30' class='bodytext'> This Email message is sent from " +
                       "                            <span style='font-weight:bold;'>" + pApplnName + "</span> application." +
                       "                         </td> " +
                       "                    </tr> " +
                       "                    <tr> " +
                       "                         <td height='30' class='bodytext'> " +
                       "                            <span >Dated on " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToLongTimeString() + "</span> " +
                       "                         </td> " +
                       "                    </tr> " +
                       "                    <tr> " +
                       "                         <td height='120' class='bodytext'> " +
                       "                            <p align='justify' style='font-weight:bold;'>  " + pMessage +
                       "                            </p> " +
                       "                         </td> " +
                       "                    </tr> ";

            MsgBody += "                    <tr> " +
                        "                        <td height='30' class='bodytext'> " +
                        "                            (This is an auto generated email. Please do not reply to this.) " +
                        "                        </td> " +
                        "                    </tr> " +
                        "                </table> " +
                        "            </td> " +
                        "        </tr> " +
                        "    </table> " +
                        " </body> " +
                        " </html>";
            return MsgBody;
        }
        /* This function is used to develop the Email Message Body */
        private string GetMessageBody(string pApplnName, string pMessage, string imgLocation)
        {
            string MsgBody = string.Empty;

            MsgBody = "<html> " +
                        "<body> " +
                        "<table  align='center' style='width: 60%;height: 100%; border:0px;border:none;'  border='0'   cellspacing='0' cellpadding='0'> " +
                        "			<tr> " +
                        "				<td height='90'  style='background-color:#348bcd;padding-left:25px;' colspan='3'> " +
                        "				<img  src=\"cid:Logo\" >				 " +
                        "				</td> " +
                        "			</tr> " +
                        "			<tr style='font:Arial;font-size:14px;color:#333333;background-color: #f7f7f7;padding-left:25px;'> " +
                        "				<td style='padding-left:10px;border:10px solid #FFFFFF;'  colspan='3'>					 " +
                        "          				<div  style=' border:1px solid #cccccc;height:99%;width:99%;padding:5px;'> " +
                        "  					<div style='height:20px;'></div>                    " +
                        "					<p>This Email message is sent from <B>BANK SEPAH – SCV Reporting with FSCS format(ABD & C) / Excel Format for 29/09/2016</B> application.</p>" +
                       "<p>Dated on " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToLongTimeString() + "</p>" +
                       "" + pMessage;

            MsgBody += "</div></td> " +
                        "</tr> " +
                        "<tr align='center'   > " +
                        "			<td  style='background-color:#348bcd;' height='5' colspan='3'></td> " +
                        "       	</tr> " +
                        "			<tr align='center' style='background-color:#e5e5e5; border:0px'  height='110'> " +
                        "		    <td><img src=\"cid:scv1\"></td> " +
                        "            <td><img  src=\"cid:scv2\"></td> " +
                        " 			<td><img s src=\"cid:scv3\"></td> " +
                        "			</tr>			 " +
                        "</table> " +
                        "</body> " +
                        "</html>  ";

            return MsgBody;
        }

        /* This function is used to save the error message */
        private void SaveErrorMessage(string MsgFrom, string MsgTo, string MsgSubject, string MsgBody, byte[] bArray, string MsgStatus, string CCemail)
        {
            string sqlQuery = string.Empty;

            try
            {

                /* Create table, if EMCONTENTPF table is not exists */
                CreateTable();

                MsgBody = replacesplchr(MsgBody);

                sqlQuery = " Insert into EMCONTENTPF (EMDATE,EMTIME,EMFROM,EMTO,EMSUB,EMATTA,EMBODY,EMSTATUS,EMCC) values " +
                                                 "(@EMDATE,@EMTIME,@EMFROM,@EMTO,@EMSUB,@EMATTA,@EMBODY,@EMSTATUS,@EMCC)";


                Connection();
                OpenConnection();

                if (_provider == "SQLSERVER")
                {
                    SqlCommand objCom = new SqlCommand(sqlQuery, sqlcon);
                    objCom.Parameters.Add("@EMDATE", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyyMMdd");
                    objCom.Parameters.Add("@EMTIME", SqlDbType.NVarChar).Value = DateTime.Now.ToString("HH:mm:ss");
                    objCom.Parameters.Add("@EMFROM", SqlDbType.NVarChar).Value = MsgFrom.Trim();
                    objCom.Parameters.Add("@EMTO", SqlDbType.NVarChar).Value = MsgTo.Trim();
                    objCom.Parameters.Add("@EMSUB", SqlDbType.NVarChar).Value = MsgSubject.Trim();
                    objCom.Parameters.Add("@EMATTA", SqlDbType.Image).Value = bArray;
                    objCom.Parameters.Add("@EMBODY", SqlDbType.NText).Value = MsgBody.Trim();
                    objCom.Parameters.Add("@EMSTATUS", SqlDbType.NVarChar).Value = MsgStatus;
                    objCom.Parameters.Add("@EMCC", SqlDbType.NVarChar).Value = CCemail;
                    objCom.ExecuteNonQuery();
                }
                //else if (_provider == "MSACCESS")
                //{
                //    OleDbCommand objCom = new OleDbCommand(sqlQuery, oledbcon);
                //    objCom.Parameters.AddWithValue("@EMDATE", DateTime.Now.ToString("yyyyMMdd"));
                //    objCom.Parameters.AddWithValue("@EMTIME", DateTime.Now.ToString("HH:mm:ss"));
                //    objCom.Parameters.AddWithValue("@EMFROM", MsgFrom.Trim());
                //    objCom.Parameters.AddWithValue("@EMTO", MsgTo.Trim());
                //    objCom.Parameters.AddWithValue("@EMSUB", MsgSubject.Trim());
                //    objCom.Parameters.AddWithValue("@EMATTA", bArray);
                //    objCom.Parameters.AddWithValue("@EMBODY", MsgBody.Trim());
                //    objCom.Parameters.AddWithValue("@EMSTATUS", MsgStatus);
                //    objCom.Parameters.AddWithValue("@EMCC", CCemail);
                //    objCom.ExecuteNonQuery();
                //}
                else
                {
                }

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

        #endregion
    }
}
