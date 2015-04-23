using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Configuration;
using System.Net;
using System.Net.Mail;

using SisoDb.Sql2012;
using SisoDb.Configurations;

using CTCT;
using CTCT.Components;
using CTCT.Components.Contacts;
using CTCT.Components.AccountService;
using CTCT.Components.EmailCampaigns;
using CTCT.Exceptions;
using CTCT.Components.Tracking;
using CTCT.Services;

using HSoft.SQL;

namespace HSoft.ClientManager.Web
{

    public static partial class Tools
    {

        private static ConstantContact _constantContact = null;
        private static string _apiKey = string.Empty;
        private static string _accessToken = string.Empty;

        public static void sendSMS(String from, String to, String txtSubject, String txtSMS)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from);
            msg.To.Add(new MailAddress(to));
            msg.Subject = txtSubject;
            if (txtSMS.Length > 140) { txtSMS.Substring(0, 140); }
            msg.Body = txtSMS;

            SmtpClient smtp = new SmtpClient("smtp.uncleharry.biz", 25);
            smtp.Credentials = new NetworkCredential("sysadmin@uncleharry.biz", "Taurec86@");
            smtp.Send(msg);
            smtp.Dispose();
            msg.Dispose();
        }

        public static void sendMail(MailAddress from, MailAddress to, String txtSubject, String txtBody, Boolean isHTML = true)
        {
            MailMessage message = new MailMessage(from, to);
            message.IsBodyHtml = isHTML;
            message.Subject = txtSubject;
            message.Body = txtBody;

            SmtpClient SMTPServer = new SmtpClient("localhost");
            SMTPServer.Credentials = new System.Net.NetworkCredential(from.Address, @"Taurec86@");
            try
            {
                SMTPServer.Send(message);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception(String.Format("sendMail : {0}", ex.Message));

                String ssql = String.Empty;
                HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

                ssql = String.Format("INSERT INTO _auditt([Table], Field, OldValue) VALUES('{0}','{1}','{2}')",
                       from.Address,
                       to.Address,
                       ex.Message);
                _sql.Execute(ssql);
                _sql.Close();

                throw ex2;
            }
//            finally
//            {
//                SMTPServer.Dispose();
//            }
        }

        public static void sendMail(MailAddress _smtpfrom, String _smtppass, MailAddress from, MailAddress to, String txtSubject, String txtBody, Boolean isHTML = true)
        {
            MailMessage message = new MailMessage(from, to);
            message.IsBodyHtml = isHTML;
            message.Subject = txtSubject;
            message.Body = txtBody;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            message.Bcc.Add(_smtpfrom); 

            SmtpClient SMTPServer = new SmtpClient("localhost");
            SMTPServer.Credentials = new System.Net.NetworkCredential(_smtpfrom.Address, _smtppass);
            try
            {
                SMTPServer.Send(message);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception(String.Format("sendMail : {0}", ex.Message));

                // Determine if email address is already on file
                String ssql = String.Empty;
                HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

                ssql = String.Format("INSERT INTO _auditt([Table], Field, OldValue) VALUES('{0}','{1}','{2}')",
                       from.Address,
                       to.Address,
                       ex.Message);
                _sql.Execute(ssql);
                _sql.Close();
                
                throw ex2;
            }
        }        

        public static Boolean InitData()
        {
            Boolean bres = false;

            bres = update_Campains();
            bres = update_Campain();

            return bres;
        }
        public static Boolean update_Campain() { return update_Campain(false, null); }
        public static Boolean update_Campain(Boolean _sign, String _guser)
        {
            // update once an hour
            // if (check_hours()) { return true; } 

            String ssql = String.Empty;

            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            _apiKey = ConfigurationManager.AppSettings["APIKey"];
            _accessToken = ConfigurationManager.AppSettings["AccessToken"];
            if (_accessToken.Length != new Guid().ToString().Length)
            {
                byte[] decryptedB = Convert.FromBase64String(ConfigurationManager.AppSettings["AccessToken"]);
                _accessToken = System.Text.Encoding.UTF8.GetString(decryptedB).Replace("\0", "");
            }
            _constantContact = new ConstantContact(_apiKey, _accessToken);

            ssql = "SELECT e.Value Id" +
                   "  FROM EmailCampaignStrings e,EmailCampaignStrings e2,EmailCampaignStrings e3,EmailCampaignTracking et " +
                   " WHERE e.StructureId = e2.StructureId " +
                   "   AND e.StructureId = e3.StructureId " +
                   "   AND e.MemberPath = 'Id' " +
                   "   AND e2.MemberPath = 'Status' " +
                   "   AND e3.MemberPath = 'Name' " +
                   "   AND e2.Value = 'SENT' " +
                   "   AND e.Value = et.EmailCampaignId " +
                   "   AND et.isTracked = 1";
            DataTable dt = _sql.GetTable(ssql);
            foreach (DataRow dr in dt.Rows)
            {

                // update once an hour
                // if (check_hours()) { return true; } 

                DateTime lastupdate = DateTime.MinValue;

                _apiKey = ConfigurationManager.AppSettings["APIKey"];
                _accessToken = ConfigurationManager.AppSettings["AccessToken"];
                if (_accessToken.Length != new Guid().ToString().Length)
                {
                    byte[] decryptedB = Convert.FromBase64String(ConfigurationManager.AppSettings["AccessToken"]);
                    _accessToken = System.Text.Encoding.UTF8.GetString(decryptedB).Replace("\0", "");
                }
                _constantContact = new ConstantContact(_apiKey, _accessToken);

                DateTime.TryParse(_sql.ExecuteScalar(String.Format("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Campaign_{0}' AND isdeleted = 0", dr["Id"])).ToString(), out lastupdate);
                lastupdate.AddMinutes(1);

                int iCount = 0;
                Pagination _page = null;
                while (1 == 1)
                {
                    ResultSet<ClickActivity> _clicks = null;
                    try
                    {
                        _clicks = _constantContact.GetCampaignTrackingClicks(dr["Id"].ToString(), 500, lastupdate);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (_clicks == null)
                    {
                        Exception ex = new Exception("No results returned, possible connection failure.");
                        throw ex;
                    }
                    foreach (ClickActivity _click in _clicks.Results)
                    {
                        ssql = String.Format("SELECT COUNT(*) FROM [CampainActivity] WHERE CampaignId = {0} AND ContactId = {1} AND ActivityType = '{2}'", _click.CampaignId, _click.ContactId, _click.ActivityType);
                        Int16 i = Int16.Parse(_sql.ExecuteScalar(ssql).ToString());
                        // needs to do a datetime check because CC delivers duplicates
                        if (i != 0)
                        {
                            ssql = String.Format("DELETE FROM [CampainActivity] WHERE CampaignId = {0} AND ContactId = {1} AND ActivityType = '{2}'", _click.CampaignId, _click.ContactId, _click.ActivityType);
                            _sql.Execute(ssql);
                        }
                        if ((_sign) && (_click.ActivityType=="EMAIL_CLICK"))
                        {
                            ssql = String.Format("SELECT '<br/> ' + FORMAT( CONVERT(Date,'{2:d}'), 'MM/dd/yy', 'en-US' ) + ' ' + ect.LeadText + ' ' + e.Short " +
                                                 "  FROM [dbo].[Employee] e,[dbo].[EmailCampaignTracking] ect " +
                                                 " WHERE e.[Id] = '{0}' " +
                                                 "   AND ect.[EmailCampaignId] = {1} ", _guser, _click.CampaignId, _click.ClickDate);
                            String _ssign = _sql.ExecuteScalar(ssql).ToString();
                            ssql = String.Format("UPDATE t " +
                                                 "   SET t.SalesNote = t.SalesNote + '{0}', " +
                                                 "       t.updatedby = '{2}', " +
                                                 "       t.updatedate = GetDate() " +
                                                 "  FROM Lead_Flat t " +
                                                 " WHERE t.ConstantContactID = {1} ", _ssign, _click.ContactId, _guser);
                            _sql.Execute(String.Format("INSERT INTO _auditt([Table],[Field],OldValue,NewValue) VALUES('Lead_Flat','SalesNote','{0}','{1}')",_click.ContactId,ssql.Replace("'","''")));
                            _sql.Execute(ssql);
                        }
                        ssql = String.Format("INSERT INTO [CampainActivity](ActivityType,CampaignId,ContactId,EmailAddress,OpenDate) VALUES ('{0}',{1},{2},'{3}','{4}')",_click.ActivityType,_click.CampaignId, _click.ContactId,_click.EmailAddress,_click.ClickDate );
                        _sql.Execute(ssql);
                    }
                    iCount = iCount + _clicks.Results.Count;
                    if (_clicks.Meta.Pagination.Next == null) { break; }
                    if (_page == null) { _page = new Pagination(); }
                    _page.Next = _clicks.Meta.Pagination.Next;
                }
                Tools.SetDate(DateTime.Now, String.Format("Campaign_{0}", dr["Id"]));


            }
            _sql.Close();

            return true;
        }
        public static Boolean check_hours()
        {
//            return false;

            DateTime _lastupdate = DateTime.MinValue;
            HttpContext.Current.Application.Lock();
            {
                if (HttpContext.Current.Application["_lastupdate_Campain"] != null)
                {
                    _lastupdate = DateTime.Parse(HttpContext.Current.Application["_lastupdate_Campain"].ToString());
                }
                HttpContext.Current.Application["_lastupdate_Campain"] = DateTime.Now;
            }
            HttpContext.Current.Application.UnLock();

            // update once an hour
            if (_lastupdate > DateTime.Now.AddHours(-1)) { return true; } else { return false; }
        }
        public static Boolean update_Campains()
        {
            // update once an hour
            // if (check_hours()) { return true; } 

            DateTime lastupdate = DateTime.MinValue;

            SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db();
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            _apiKey = ConfigurationManager.AppSettings["APIKey"];
            _accessToken = ConfigurationManager.AppSettings["AccessToken"];
            if (_accessToken.Length != new Guid().ToString().Length)
            {
                byte[] decryptedB = Convert.FromBase64String(ConfigurationManager.AppSettings["AccessToken"]);
                _accessToken = System.Text.Encoding.UTF8.GetString(decryptedB).Replace("\0", "");
            }
            _constantContact = new ConstantContact(_apiKey, _accessToken);

            DateTime.TryParse(_sql.ExecuteScalar("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Campains' AND isdeleted = 0").ToString(), out lastupdate);

            int iCount = 0;
            Pagination _page = null;
            while (1 == 1)
            {
                ResultSet<EmailCampaign> _campains = null;
                try
                {
                    _campains = _constantContact.GetCampaigns(null, 50, lastupdate, _page);
                }
                catch (Exception ex) 
                { 
                    throw ex; 
                }
                if (_campains == null)
                {
                    Exception ex = new Exception("No results returned, possible connection failure.");
                    throw ex;
                }
                using (var session = _siso.BeginSession())
                {
                    foreach (EmailCampaign _campain in _campains.Results)
                    {
                        if (session.Query<EmailCampaign>().Count(o => o.Id == _campain.Id) != 0)
                        {
                            session.Update(_campain);
                        }
                        else
                        {
                            session.Insert(_campain);
                        }
                    }
                }
                iCount = iCount + _campains.Results.Count;
                if (_campains.Meta.Pagination.Next == null) { break; }
                if (_page == null) { _page = new Pagination(); }
                _page.Next = _campains.Meta.Pagination.Next;
            }
            Tools.SetDate(DateTime.Now, "Campaigns");

            _sql.Close();

            return true;
        }
        public static void SetDate(DateTime rdate, String skey)
        {
            String ssql = String.Empty;

            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            ssql = String.Format("UPDATE _CCupdates SET isdeleted = 1,updatedby='{0}',updatedate='{1}' WHERE tablename = '{2}' AND isdeleted = 0", HttpContext.Current.Session["guser"].ToString().ToUpper(), DateTime.Now, skey);
            _sql.Execute(ssql);
            ssql = String.Format("INSERT INTO _CCupdates(tablename, lastmodified, creadedby, createdate) VALUES('{3}','{2}','{0}','{1}')", HttpContext.Current.Session["guser"].ToString().ToUpper(), DateTime.Now, rdate, skey);
            _sql.Execute(ssql);
            _sql.Close();
        }
        public static void devlogincheat()
        {
            if (HttpContext.Current.Session["guser"] == null)
            {
                if (Environment.MachineName.ToUpper().StartsWith("RENE"))
                {
                    { HttpContext.Current.Session["guser"] = "7d5aa961-5478-4fa1-b5db-d6a2071ed834"; }
                }
                else
                {
                    HttpContext.Current.Response.Redirect("~/Account/Login.aspx?source=internal");
                }
            }
            if (HttpContext.Current.Session["wx"] == null) { HttpContext.Current.Session["wx"] = 1746; }
            if (HttpContext.Current.Session["wy"] == null) { HttpContext.Current.Session["wy"] = 927; }
        }
        public static Boolean invalid = false;
        public static Boolean IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", Tools.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}