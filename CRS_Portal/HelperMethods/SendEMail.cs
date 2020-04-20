using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SCV_Portal.HelperMethods
{
    public class EmailParam
    {
        public string PrimarySMTP { get; set; }
        public string PrimaryUserName { get; set; }
        public string PrimaryFrom { get; set; }
        public string PrimaryPassword { get; set; }
        public int PrimaryPortNo { get; set; }
        public bool PrimaryUseDefaultCredentials { get; set; }
        public bool PrimaryEnableSsl { get; set; }

        public string SecondarySMTP { get; set; }
        public string SecondaryUserName { get; set; }
        public string SecondaryFrom { get; set; }
        public string SecondaryPassword { get; set; }
        public int SecondaryPortNo { get; set; }
        public bool SecondaryUseDefaultCredentials { get; set; }
        public bool SecondaryEnableSsl { get; set; }

        public string ToEmailAddress { get; set; }
        public string CCEmailAddress { get; set; }
        public string BCCEmailAddress { get; set; }

        public string MailContent { get; set; } //with design template
        public string MailSubject { get; set; }
        public string AloneMailContent { get; set; } //with out design template

        public string ApplicationName { get; set; }

    }

    public class SendEmail
    {
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

        private MailAddressCollection GetMailAddressList(string EmailId)
        {
            MailAddressCollection objMAC = new MailAddressCollection();
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

        private string NullToSpace(object txt)
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

        public bool SendEmailUsingPrimary(EmailParam objEmail)
        {

            byte[] bArray = { };

            string P_SMTP = objEmail.PrimarySMTP;
            string P_MsgFrom = objEmail.PrimaryFrom;
            string P_MailFrmUID = objEmail.PrimaryUserName;
            string P_MailFrmPW = objEmail.PrimaryPassword;
            int P_MailFrmPort = objEmail.PrimaryPortNo;
            bool P_PrimaryUseDefaultCredentials = objEmail.PrimaryUseDefaultCredentials;
            bool P_PrimaryEnableSsl = objEmail.PrimaryEnableSsl;

            string S_SMTP = objEmail.SecondarySMTP;
            string S_MsgFrom = objEmail.SecondaryFrom;
            string S_MailFrmUID = objEmail.SecondaryUserName;
            string S_MailFrmPW = objEmail.SecondaryPassword;
            int S_MailFrmPort = objEmail.SecondaryPortNo;
            bool S_PrimaryUseDefaultCredentials = objEmail.SecondaryUseDefaultCredentials;
            bool S_PrimaryEnableSsl = objEmail.SecondaryEnableSsl;
            
            string MsgTo = string.Empty;
            string ccEmail = string.Empty;
            string bccEmail = string.Empty;
            string MsgSubject = string.Empty;
            string MsgBody = string.Empty;


            MailMessage msg = new MailMessage();
            MailAddressCollection lst_ToMAC;
            MailAddressCollection lst_CCMAC;
            MailAddressCollection lst_BCCMAC;

            bool bMailSent = false;
            try
            {
                MsgSubject = objEmail.MailSubject;

                MsgBody = objEmail.MailContent;

                msg = new MailMessage(objEmail.ApplicationName + " <" + objEmail.PrimaryFrom + ">", objEmail.ToEmailAddress, MsgSubject, MsgBody);
                msg.IsBodyHtml = true;

                lst_ToMAC = GetMailAddressList(objEmail.ToEmailAddress);

                foreach (var item in lst_ToMAC)
                {
                    msg.To.Add(item);
                }

                if (!string.IsNullOrEmpty(objEmail.CCEmailAddress))
                {
                    lst_CCMAC = GetMailAddressList(objEmail.CCEmailAddress);

                    foreach (var item in lst_CCMAC)
                    {
                        msg.CC.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(objEmail.BCCEmailAddress))
                {
                    lst_BCCMAC = GetMailAddressList(objEmail.BCCEmailAddress);
                    foreach (var item in lst_BCCMAC)
                    {
                        msg.Bcc.Add(item);
                    }
                }

                SmtpClient mailClient = new SmtpClient(P_SMTP, P_MailFrmPort);
                NetworkCredential NetCrd = new NetworkCredential(P_MsgFrom, P_MailFrmPW);
                mailClient.UseDefaultCredentials = P_PrimaryUseDefaultCredentials;
                mailClient.Credentials = NetCrd;
                mailClient.EnableSsl = P_PrimaryEnableSsl;

                mailClient.Send(msg);
                bMailSent = true;
                SaveEmailMessage(objEmail, true, true);
            }
            catch (Exception ex)
            {
                bMailSent = false;
                SaveEmailMessage(objEmail, false, true, ex);
            }
            finally
            {
                if (msg != null)
                {
                    msg.Dispose();
                    msg = null;
                }
            }
            return bMailSent;
        }

        public bool SendEmailUsingSecondary(EmailParam objEmail)
        {

            byte[] bArray = { };

            string P_SMTP = objEmail.PrimarySMTP;
            string P_MsgFrom = objEmail.PrimaryFrom;
            string P_MailFrmUID = objEmail.PrimaryUserName;
            string P_MailFrmPW = objEmail.PrimaryPassword;
            int P_MailFrmPort = objEmail.PrimaryPortNo;
            bool P_PrimaryUseDefaultCredentials = objEmail.PrimaryUseDefaultCredentials;
            bool P_PrimaryEnableSsl = objEmail.PrimaryEnableSsl;

            string S_SMTP = objEmail.SecondarySMTP;
            string S_MsgFrom = objEmail.SecondaryFrom;
            string S_MailFrmUID = objEmail.SecondaryUserName;
            string S_MailFrmPW = objEmail.SecondaryPassword;
            int S_MailFrmPort = objEmail.SecondaryPortNo;
            bool S_PrimaryUseDefaultCredentials = objEmail.SecondaryUseDefaultCredentials;
            bool S_PrimaryEnableSsl = objEmail.SecondaryEnableSsl;

            string MsgTo = string.Empty;
            string ccEmail = string.Empty;
            string bccEmail = string.Empty;
            string MsgSubject = string.Empty;
            string MsgBody = string.Empty;


            MailMessage msg = new MailMessage();
            MailAddressCollection lst_ToMAC;
            MailAddressCollection lst_CCMAC;
            MailAddressCollection lst_BCCMAC;

            bool bMailSent = false;
            try
            {
                MsgSubject = objEmail.MailSubject;

                MsgBody = objEmail.MailContent;

                msg = new MailMessage(objEmail.ApplicationName + " <" + objEmail.SecondaryFrom + ">", objEmail.ToEmailAddress, MsgSubject, MsgBody);
                msg.IsBodyHtml = true;

                lst_ToMAC = GetMailAddressList(objEmail.ToEmailAddress);

                foreach (var item in lst_ToMAC)
                {
                    msg.To.Add(item);
                }

                if (!string.IsNullOrEmpty(objEmail.CCEmailAddress))
                {
                    lst_CCMAC = GetMailAddressList(objEmail.CCEmailAddress);

                    foreach (var item in lst_CCMAC)
                    {
                        msg.CC.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(objEmail.BCCEmailAddress))
                {
                    lst_BCCMAC = GetMailAddressList(objEmail.BCCEmailAddress);
                    foreach (var item in lst_BCCMAC)
                    {
                        msg.Bcc.Add(item);
                    }
                }

                SmtpClient mailClient = new SmtpClient(S_SMTP, S_MailFrmPort);
                NetworkCredential NetCrd = new NetworkCredential(S_MsgFrom, S_MailFrmPW);
                mailClient.UseDefaultCredentials = S_PrimaryUseDefaultCredentials;
                mailClient.Credentials = NetCrd;
                mailClient.EnableSsl = S_PrimaryEnableSsl;

                mailClient.Send(msg);
                bMailSent = true;
                SaveEmailMessage(objEmail, true, false);
            }
            catch (Exception ex)
            {
                bMailSent = false;
                SaveEmailMessage(objEmail, false, false, ex);
            }
            return bMailSent;
        }

        private void SaveEmailMessage(EmailParam objEmail, bool eMailStatus, bool isPrimary, Exception ex = null)
        {
            string sqlQuery = string.Empty;

            try
            {
                string MsgBody = replacesplchr(objEmail.AloneMailContent);

                sqlQuery = " INSERT INTO EMCONTENTPF (EMDATE,EMTIME,EMFROM,EMTO,EMSUB,EMATTA,EMBODY,EMSTATUS,EMCC, EMBCC, EMERROR) VALUES " +
                                                 " (@EMDATE,@EMTIME,@EMFROM,@EMTO,@EMSUB,@EMATTA,@EMBODY,@EMSTATUS,@EMCC,@EMBCC, @EMERROR) ";

                string EMFROM = string.Empty;
                if (isPrimary)
                {
                    EMFROM = objEmail.PrimaryFrom;
                }
                else
                {
                    EMFROM = objEmail.SecondaryFrom;
                }
                string EMDATE = DateTime.Now.ToString("yyyyMMdd");
                string EMTIME = DateTime.Now.ToString("HH:mm:ss");
                string EMTO = objEmail.ToEmailAddress;
                string EMSUB = objEmail.MailSubject;
                string EMATTA = "";
                string EMBODY = MsgBody;
                string EMSTATUS = eMailStatus.ToString();
                string EMCC = objEmail.CCEmailAddress;
                string EMBCC = objEmail.BCCEmailAddress;

                sqlQuery = sqlQuery.Replace("@EMDATE", "'"+EMDATE+"'");
                sqlQuery = sqlQuery.Replace("@EMTIME", "'" + EMTIME + "'");
                sqlQuery = sqlQuery.Replace("@EMFROM", "'" + EMFROM + "'");
                sqlQuery = sqlQuery.Replace("@EMTO", "'" + EMTO + "'");

                sqlQuery = sqlQuery.Replace("@EMSUB", "'" + EMSUB + "'");
                sqlQuery = sqlQuery.Replace("@EMATTA", "'" + EMATTA + "'");
                sqlQuery = sqlQuery.Replace("@EMBODY", "'" + EMBODY + "'");
                
                sqlQuery = sqlQuery.Replace("@EMCC", "'" + EMCC + "'");
                sqlQuery = sqlQuery.Replace("@EMBCC", "'" + EMBCC + "'");
                if (EMSTATUS.ToLower() =="true")
                {
                    sqlQuery = sqlQuery.Replace("@EMSTATUS", "'1'");
                }
                else
                {
                    sqlQuery = sqlQuery.Replace("@EMSTATUS", "'0'");
                }
                if (ex != null)
                {
                    sqlQuery = sqlQuery.Replace("@EMERROR", "'" + ex.ToString() + "'");
                }
                else
                {
                    sqlQuery = sqlQuery.Replace("@EMERROR", "''");
                }
                Helper.ExecuteCommand(sqlQuery);
                
            }
            catch (Exception ex1)
            {
                throw ex1;
            }
            

        }

        public string GetEmailTemplateByBankname(string sbankName, string path)
        {
            if (sbankName == "UBI")
            {
                path = System.IO.Path.GetFullPath(path+ "NewEmailTemplate\\UBIdesign.html");
            }
            else if (sbankName == "UBL")
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\UBLdesign.html");
            }
            else if (sbankName == "ICICI")
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\ICICIdesign.html");
            }
            else if (sbankName == "RBL")
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\RBLdesign.html");
            }
            else if (sbankName == "ANB")
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\ANBdesign.html");
            }
            else if (sbankName == "BOI")
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\BOIdesign.html");
            }
            else if (sbankName == "CANARA")
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\CANARAdesign.html");
            }
            else if (sbankName == "PNB")
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\PNBdesign.html");
            }
            else if (sbankName == "SEPAH")
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\SEPAHdesign.html");
            }
            else
            {
                path = System.IO.Path.GetFullPath(path + "NewEmailTemplate\\design.html");
            }
            return path;
        }
    }
}


//begin Tran
//INSERT INTO POLMTPF VALUES('MAILPrimary','PrimarySMTP','Primary SMTP Details','mail.macroinfotech.co.uk','Y')
//INSERT INTO POLMTPF VALUES('MAILPrimary','PrimaryUserName','Primary User name','fpay@macroinfotech.co.uk','Y')
//INSERT INTO POLMTPF VALUES('MAILPrimary','PrimaryFrom','Primary From Address','fpay@macroinfotech.co.uk','Y')
//INSERT INTO POLMTPF VALUES('MAILPrimary','PrimaryPassword','Primary Password','Apex1234','Y')
//INSERT INTO POLMTPF VALUES('MAILPrimary','PrimaryPortNo','Primary Port no','25','Y')
//INSERT INTO POLMTPF VALUES('MAILPrimary','PrimaryUseDefaultCredentials','Primary UseDefaultCredentials','true','Y')
//INSERT INTO POLMTPF VALUES('MAILPrimary','PrimaryEnableSsl','Primary EnableSSL','false','Y')

//INSERT INTO POLMTPF VALUES('MAILSecondary','SecondarySMTP','Secondary SMTP Details','mail.macroinfotech.co.uk','Y')
//INSERT INTO POLMTPF VALUES('MAILSecondary','SecondaryUserName','Secondary User name','scvadmingroup@macroinfotech.co.uk','Y')
//INSERT INTO POLMTPF VALUES('MAILSecondary','SecondaryFrom','Secondary From Address','scvadmingroup@macroinfotech.co.uk','Y')
//INSERT INTO POLMTPF VALUES('MAILSecondary','SecondaryPassword','Secondary Password','Apex9899*','Y')
//INSERT INTO POLMTPF VALUES('MAILSecondary','SecondaryPortNo','Secondary Port no','25','Y')
//INSERT INTO POLMTPF VALUES('MAILSecondary','SecondaryUseDefaultCredentials','Secondary UseDefaultCredentials','true','Y')
//INSERT INTO POLMTPF VALUES('MAILSecondary','SecondaryEnableSsl','Secondary EnableSSL','false','Y')

//INSERT INTO POLMTPF VALUES('MAILSend','MailTo', 'Mail to address', 'arunkumar.s@macroinfotech.co.uk','Y')
//INSERT INTO POLMTPF VALUES('MAILSend','MailCC', 'Mail to CC address','arunkumar.s@macroinfotech.co.uk','Y')
//INSERT INTO POLMTPF VALUES('MAILSend','MailBCC','Mail to BCC address','arunkumar.s@macroinfotech.co.uk','Y')

//Rollback