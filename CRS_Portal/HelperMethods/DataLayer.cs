using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using SCV_Portal.Models;
using SCV_Portal.Entity;

namespace SCV_Portal.HelperMethods
{
    public class DataLayer
    {
        
        Methods obj = new Methods();

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public void WriteErrorLog(string MethodName, string ParameterValue, string ErrorMessage, string UserID)
        {            
            ErrorLogDetails model = new ErrorLogDetails();
            model.UserID = UserID;
            model.MethodName = MethodName;
            model.ParameterValue = ParameterValue;
            model.ErrorMessage = ErrorMessage;
           
            string data = obj.ToXML(model);

            //string auditMessage = "Method Name : " + MethodName + " || Parameter Value " + ParameterValue + " || Error Message : " + ErrorMessage;

            obj.StoreEvent("77778", "@@@", data, UserID);
        }

        public void WriteLog(string MethodName, string ParameterValue, string Message, string UserID)
        {
            LogDetails model = new LogDetails();
            model.UserID = UserID;
            model.MethodName = MethodName;
            model.ParameterValue = ParameterValue;
            model.Message = Message;
            string data = obj.ToXML(model);

            //string auditMessage = "Method Name : " + MethodName + " || Parameter Value " + ParameterValue + " || Message : " + Message;

            obj.StoreEvent("77777", "@@@", data, UserID);
        }

        public string GetEmailTemplateByBankname()
        {
            string path = string.Empty;
            string sbankName = obj.RetPolValue("APCNFG", "BANKNAME");
            if (sbankName == "UBI")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\UBI_design.html");
            }
            else if (sbankName == "UBL")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\UBL_design.html");
            }
            else if (sbankName == "SBI")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\SBI_design.html");
            }
            else if (sbankName == "ICICI")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\ICICI_design.html");
            }
            else if (sbankName == "BFC Bank")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\BFC_design.html");
            }
            else if (sbankName == "Bank of India UK")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\BOI_design.html");
            }
            else if (sbankName == "BOB")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\BOB_design.html");
            }
            else
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\design.html");
            }
            return path;
        }

    }
}