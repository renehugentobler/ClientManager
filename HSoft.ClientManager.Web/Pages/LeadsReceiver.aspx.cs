using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;

using HSoft.SQL;
using HSoft.ClientManager.Web;

using SisoDb.Sql2012;
using SisoDb.Configurations;

using CTCT;
using CTCT.Components;
using CTCT.Components.Contacts;
using CTCT.Components.AccountService;
using CTCT.Components.EmailCampaigns;
using CTCT.Exceptions;

// using Verifalia.Api;

public partial class Pages_LeadsReceiver : System.Web.UI.Page
{

    Boolean blnTestMode = false;
    public static Int32 google_conversion_id = 1072692919;
    public static String google_conversion_language = "en";
    public static String google_conversion_format = "3";
    public static String google_conversion_color = "ffffff";
    public static String google_conversion_label = "CKl7CPmIwgkQt_2__wM";
    public static String google_remarketing_only = "false";

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["target"])) { int.TryParse(Request.QueryString["target"].ToString(), out intTarget); }

        String cstext1 = String.Format(// "<script type='text/javascript'>" +
                                       "\n\tvar google_conversion_id = {1};\n" +
                                       "\tvar google_conversion_language = '{2}';\n" +
                                       "\tvar google_conversion_format = '{3}';\n" +
                                       "\tvar google_conversion_color = '{4}';\n" +
                                       "\tvar google_conversion_label = '{5}';\n" +
                                       "\tvar google_remarketing_only = {6};\n" +
//                                       "\tvar google_conversion_value =  '{0}';\n" +
                                       // "</script>"
                                       "", intTarget,google_conversion_id,google_conversion_language,google_conversion_format,google_conversion_color,google_conversion_label,google_remarketing_only);
        var _script = new HtmlGenericControl("script");
        _script.Attributes.Add("type", "text/javascript");
        _script.InnerHtml = cstext1.ToString();
        google_init.Controls.Add(_script);
//        googleImg.Src = String.Format("//www.googleadservices.com/pagead/conversion/{0}/?label={1}&guid=ON&script=0&value={2}", google_conversion_id, google_conversion_label, intTarget);
        googleImg.Src = String.Format("//www.googleadservices.com/pagead/conversion/{0}/?label={1}&guid=ON&script=0", google_conversion_id, google_conversion_label, intTarget);
    }

    public static int intTarget = -1;

    public static MailAddress from = new MailAddress("leadsreceiver@uncleharry.biz", "Leads Receiver");
    public static MailAddress torene = new MailAddress("4438255040@tmomail.net", "René Marçel Hugentobler");
    public static MailAddress toharry = new MailAddress("4109673455@txt.att.net", "Harry Raker");
    public static MailAddress tofrank = new MailAddress("4109676573@txt.att.net", "Frank Marchant");
    public static MailAddress totatyana = new MailAddress("4104460158@tmomail.net", "Tatyana Neudecker");

    protected void Page_Load(object sender, EventArgs e)
    {

        String strName = String.Empty;
        String strEmail = String.Empty;
        String strPhone = String.Empty;
        String strComments = String.Empty;
        String dtePostDate = String.Empty;
        String strNote = String.Empty;
        String strSource = "unknown";
        Boolean bNoSave = false ;
        int intID = -1;

        blnTestMode = false;

        if (!Page.IsPostBack)
        {

            String ssql = String.Empty;
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            // store the querystring

            Tools.sendMail(from, torene, "Received Lead", String.Format("'{0}'",Request.QueryString),false); 
            
            try
            {
                if (Request.QueryString.ToString().Length > 0)
                {
                    _sql.Execute(String.Format("INSERT INTO LeadReceived(Data) VALUES('{0}')", Request.QueryString));
                }
            }
            catch { }

            TimeZoneInfo edtZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime timeUtc = DateTime.UtcNow;
            DateTime edtTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,edtZone);

            if (blnTestMode)
            {
                strName = "Rene Hugentobler";
                strEmail = "rene.hugentobler@gmail.com";
                strPhone = "443-825-5040";
                strComments = "Test Lead with Comments";
                strSource = "Test Lead";
            }
            else
            {
                if (!String.IsNullOrEmpty(Request.QueryString["strName"])) { strName = Request.QueryString["strName"]; }
                if (!String.IsNullOrEmpty(Request.QueryString["strEmail"])) { strEmail = Request.QueryString["strEmail"]; }
                if (!String.IsNullOrEmpty(Request.QueryString["strPhone"])) { strPhone = Request.QueryString["strPhone"]; }
                if (!String.IsNullOrEmpty(Request.QueryString["strComments"])) { strComments = Request.QueryString["strComments"]; }
                if (!String.IsNullOrEmpty(Request.QueryString["dtePostDate"])) { dtePostDate = Request.QueryString["dtePostDate"]; }
                if (!String.IsNullOrEmpty(Request.QueryString["strSource"])) { strSource = Request.QueryString["strSource"]; }
                if (!String.IsNullOrEmpty(Request.QueryString["strNote"])) { strNote = Request.QueryString["strNote"]; }
                if (!String.IsNullOrEmpty(Request.QueryString["intID"])) { int.TryParse(Request.QueryString["intID"].ToString(), out intID); }
                if (!String.IsNullOrEmpty(Request.QueryString["target"])) { int.TryParse(Request.QueryString["target"].ToString(), out intTarget); }
                if (!String.IsNullOrEmpty(Request.QueryString["bNoSave"])) { bNoSave = true; }

                if (String.IsNullOrEmpty(strName)) { if (Request.Form["name"] != null) { strName = Request.Form["name"].Trim(); } }
                if (String.IsNullOrEmpty(strEmail)) { if (Request.Form["email"] != null) { strEmail = Request.Form["email"].Trim(); } }
                if (String.IsNullOrEmpty(strPhone)) { if (Request.Form["phone"] != null) { strPhone = Request.Form["phone"].Trim(); } }
                if (String.IsNullOrEmpty(strComments)) { if (Request.Form["comments"] != null) { strComments = Request.Form["comments"].Trim(); } }
            }

            txtId.Text = intID.ToString();
            txtName.Text = strName;
            txtEmail.Text = strEmail;
            txtPhone.Text = strPhone;
            txtComments.Text = strComments;

//            return;

            if (AddLead(intID, strName, strEmail, strPhone, strComments, edtTime, strSource, strNote))
            {
                if (String.IsNullOrEmpty(txtError.Text))
                {
                    if (!bNoSave)
                    {
                        ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                             "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "LeadReceiver", "ADD", intID, "", Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
                        _sql.Execute(ssql);
                    }

                    Response.Redirect("http://uncleharry.com/thank-you/");
                }
                ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                     "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "LeadReceiver", "ADD", intID, txtError.Text, Guid.Empty);
                _sql.Execute(ssql);
            }

            _sql.Close();

        }
    }
   
    protected void Pre_Load(object sender, EventArgs e)
    {
    }

    private Boolean AddLead(int intID, String strName, String strEmail, String strPhone, String strComments, DateTime dtePostDate, String strSource, string strNote)
    {

        Boolean blnValidEntry = false;

        try
        {
            // Determine if email address is already on file
            String ssql = String.Empty;
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            int intContactCount = int.Parse(_sql.ExecuteScalar(String.Format("SELECT COUNT(*) FROM Lead_Flat WHERE EMail='{0}' AND isdeleted=0", strEmail)).ToString());

            if (intContactCount == 0)
            {
                // check if sequence is broken and send email

                String sSourceId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadSource WHERE (RTRIM(old_Source)='{0}') OR ('{0}'='Test Lead')", strSource.Trim())).ToString();
                String sPriorityId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadPriority WHERE (Name='{0}' AND isdeleted=0)", "Undefined")).ToString();
                String sStatusId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadStatus WHERE (Name='{0}' AND isdeleted=0)", "New Lead")).ToString();
                String sAssignedToId = _sql.ExecuteScalar(String.Format("SELECT Id FROM Employee WHERE (isMasterSales=1 AND isdeleted=0)", "")).ToString();

                // overwrite AssignedToId with entry in _Leadassign
                // strange stuff may happen if we get a deleted returning from Bill
                try
                {
                    sAssignedToId = _sql.ExecuteScalar(String.Format("SELECT AssignedToId FROM _Leadassign WHERE Customer = {0}",intID)).ToString();
                }
                catch (Exception ex)
                {
                    Tools.sendMail(from, torene, "Lead AssignTo Error", String.Format("{0} {1}", ex.Message, String.Format("SELECT AssignedToId FROM _Leadassign WHERE Customer = {0}", intID)), false);
                    sAssignedToId = _sql.ExecuteScalar(String.Format("SELECT Id FROM Employee WHERE (isMasterSales=1 AND isdeleted=0)", "")).ToString();
                }

                String sAssignedToName = string.Empty;

                try
                {
                    sAssignedToName = _sql.ExecuteScalar(String.Format("SELECT DisplayName FROM Employee WHERE Id = '{0}' AND isdeleted = 0", sAssignedToId)).ToString();
                }
                catch
                {
                    sAssignedToId = "44F7B957-4AA9-466F-B5C8-8840586157B6";
                    sAssignedToName = _sql.ExecuteScalar(String.Format("SELECT DisplayName FROM Employee WHERE Id = '{0}' AND isdeleted = 0", sAssignedToId)).ToString();
                }

                if (!String.IsNullOrEmpty(strNote)) { strComments = String.Format("{0} {1}", strNote.Trim(), strComments.Trim()); }

                // don't blow out sql fields
                if (strName.Length>64) { strName = strName.Substring(0,64); }
                if (strEmail.Length>64) { strEmail = strEmail.Substring(0,64); }
                if (strPhone.Length > 16) { strPhone = strPhone.Substring(strPhone.Length - 16,16); }
                if (strComments.Length>4095) { strComments = strComments.Substring(0,4095); }
                if (strComments.Length>4095) { strComments = strComments.Substring(0,4095); }

                // case stuff correctly
                TextInfo myTI = new CultureInfo("en-US",false).TextInfo;
                strEmail = strEmail.ToLower();

                if (!Tools.IsValidEmail(strEmail))
                {
                    strEmail = "";
                }
//                else if (!verifyemail(strEmail))
//                {
//                    strEmail = "";
//                }

                strName = myTI.ToTitleCase(strName);
                ssql = string.Format("SELECT dbo.formatPhone('{0}')", strPhone);
                strPhone = _sql.ExecuteScalar(ssql).ToString();


                String strTimeZone = String.Empty;
                String strAreaCode = String.Empty;

                if (strPhone.Length>=12)
                {
                    strAreaCode = strPhone.Substring(strPhone.Length-12, 3);
                    try
                    {
                        strTimeZone = _sql.ExecuteScalar2(String.Format("SELECT TimeZone FROM tblAreaCodesAndTimeZones WHERE AreaCode = '{0}'", strAreaCode)).ToString();
                    }
                    catch { }
                }

                ssql = String.Format("INSERT INTO Lead_Flat(Customer, Name, EMail, Phone, EntryDate, CallLaterDate, SourceId, Source, PriorityId, Priority, StatusId, Status, LeadNote, AssignedToId, AssignedTo, TimeZone, isdeleted) " +
                                           "SELECT '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}',{16} " +
                                           "", intID, strName.Replace("'", "''"), strEmail.Replace("'", "''"), strPhone.Replace("'", "''"), dtePostDate,
                                           dtePostDate.Date.AddDays(5), sSourceId, strSource, sPriorityId, "Undefined",
                                           sStatusId, "New Lead", strComments.Replace("'", "''"), sAssignedToId, sAssignedToName, strTimeZone, 0);
                _sql.Execute(ssql);

                // send txt to salesperson, Tatyana, me and Harry
                foreach (String _sguid in new string[] { sAssignedToId, "0BA4012E-5541-4A76-92BB-C7122344DC3A", "DCDB22C2-65F4-46E4-91D1-CC123F83DCE2", "7D5AA961-5478-4FA1-B5DB-D6A2071ED834" })
                // foreach (String _sguid in new string[] { "0BA4012E-5541-4A76-92BB-C7122344DC3A", "DCDB22C2-65F4-46E4-91D1-CC123F83DCE2", "7D5AA961-5478-4FA1-B5DB-D6A2071ED834" })
                {
                    try
                    {
                        // ssql = String.Format("SELECT * FROM Employee WHERE Id = '{0}' AND isdeleted = 0 AND isactive = 1 AND IsSales=1", sAssignedToId.ToUpper());
                        ssql = String.Format("SELECT * FROM Employee WHERE Id = '{0}' AND isdeleted = 0", sAssignedToId.ToUpper());
                        DataRow _drEmployee = _sql.GetRow(ssql);
                        ssql = String.Format("SELECT * FROM _SMSProviders WHERE Id = '{0}' AND isdeleted = 0", _drEmployee["Provider"].ToString().ToUpper());
                        DataRow _drSMSProvider = _sql.GetRow(ssql);

                        String noHtml = Regex.Replace(strComments.Trim(), @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;", string.Empty).Trim();
                        // String stext = String.Format("{0} {1} {2} from {3:d}", strName.Trim(), strEmail.Trim(), strPhone.Trim(), dtePostDate, noHtml);
                        String stext = String.Format("{0} {1} {2} from {3:d} {4}", strName.Trim(), strEmail.Trim(), strPhone.Trim(), dtePostDate, noHtml);
                        if (stext.Length > 140) { stext = stext.Substring(0, 140); };
                        String sendto = String.Format("{0}@{1}", _drEmployee["Phone"].ToString(), _drSMSProvider["EMail"].ToString());

                        ssql = String.Format("INSERT INTO _auditt([Table], Field, OldValue, NewValue) VALUES('{0}','{1}','{2}','{3}')",
                                     sendto,
                                     _drEmployee["DisplayName"].ToString(),
                                     stext.Replace("'", "''"), noHtml.Replace("'", "''"));
                        _sql.Execute(ssql);

//                        Tools.sendMail(from, torene, "New Lead SMS", String.Format("{0} {1}", sendto, stext, false));
                        Tools.sendMail(from, new MailAddress(sendto, _drEmployee["DisplayName"].ToString()), "New Lead SMS", stext, false);
                    }
                    catch (Exception ex)
                    {
                        Tools.sendMail(from, torene, "New Lead SMS error", String.Format("*{0} {1}", ex.Message.ToString(), ssql), false);
                    }

                }

                // add to CC
                SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db(); 

//                if (Tools.IsValidEmail(strEmail))
                if (strEmail.Length>0)
                {
                    ssql = String.Format("SELECT a.Value " +
                                         "  FROM ContactStrings a, ContactStrings b  " +
                                         " WHERE a.MemberPath = 'Id' " +
                                         "   AND a.StructureId = b.StructureId " +
                                         "   AND b.MemberPath = 'EmailAddresses.EmailAddr' " +
                                         "   AND b.Value = '{0}' ", strEmail.ToLower());
                    String Id = _sql.ExecuteScalar(ssql).ToString();
                    using (var session = _siso.BeginSession())
                    {
                        var result = session.Query<CTCT.Components.Contacts.Contact>()
                            .Where(o => o.Id == Id);
                        if (result.FirstOrDefault() != null)
                        {
                            ssql = String.Format("UPDATE t SET [ConstantContactID] = {0} FROM Lead_Flat t WHERE Customer = {1} ", Id, intID);
                            _sql.Execute(ssql);
                        }
                        else
                        {
                            ConstantContact _constantContact = null;
                            string _apiKey = string.Empty;
                            string _accessToken = string.Empty;

                            _apiKey = ConfigurationManager.AppSettings["APIKey"];
                            _accessToken = ConfigurationManager.AppSettings["AccessToken"];
                            if (_accessToken.Length != new Guid().ToString().Length)
                            {
                                byte[] decryptedB = Convert.FromBase64String(ConfigurationManager.AppSettings["AccessToken"]);
                                _accessToken = System.Text.Encoding.UTF8.GetString(decryptedB).Replace("\0", "");
                            }
                            _constantContact = new ConstantContact(_apiKey, _accessToken);

                            IList<ContactList> _rscl = _constantContact.GetLists(null);
                            ContactList _cla = _rscl.FirstOrDefault(o => o.Name == "Added by Client Manager");
                            ContactList _cls = _rscl.FirstOrDefault(o => o.Name == "Sold Course");
                            ContactList _clq = _rscl.FirstOrDefault(o => o.Name == "Questionable Contact");

                            CTCT.Components.Contacts.Contact _ct = new CTCT.Components.Contacts.Contact();
                            CTCT.Components.Contacts.EmailAddress _em = new EmailAddress();
                            _em.EmailAddr = strEmail;
                            _em.ConfirmStatus = "NO_CONFIRMATION_REQUIRED";
                            _em.OptInSource = "ACTION_BY_OWNER";
                            _em.Status = "ACTIVE";
                            _ct.EmailAddresses.Add(_em);

                            _ct.Lists.Add(_cla);

                            _ct.HomePhone = strPhone;
                            _ct.FirstName = strName;
                            _ct.Source = "Client Manager";
                            _ct.Status = "ACTIVE";

                            CTCT.Components.Contacts.Contact _ctres = new CTCT.Components.Contacts.Contact();
                            try
                            {
                                _ctres = _constantContact.AddContact(_ct, false);
                                session.Insert(_ctres);
                                ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                    "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "ConstantContact", "ADD", intID, _ctres.Id, "D0F06FCC-B87E-4F33-A3D3-D64354863F39");
                                _sql.Execute(ssql);
                                ssql = String.Format("UPDATE t " +
                                                     "   SET t.ConstantContactID = {0} " +
                                                     "  FROM Lead_Flat t " +
                                                     " WHERE Customer = {1}", _ctres.Id, intID);
                                _sql.Execute(ssql);
                            }
                            catch (CTCT.Exceptions.CtctException ex)
                            {
                                ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                                     "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "ConstantContact", "ERROR", intID, ex.InnerException.ToString().Replace("'", "''"), "D0F06FCC-B87E-4F33-A3D3-D64354863F39");
                                _sql.Execute(ssql);
                            }
                        }                        
                    }
                }
            }
            else
            {
                intID = int.Parse(_sql.ExecuteScalar(String.Format("SELECT Customer FROM Lead_Flat WHERE EMail='{0}' AND isdeleted=0", strEmail)).ToString());
                DateTime EntryDate = DateTime.Parse(_sql.ExecuteScalar(String.Format("SELECT EntryDate FROM Lead_Flat WHERE EMail='{0}' AND isdeleted=0", strEmail)).ToString());
                String oldPhone = _sql.ExecuteScalar(String.Format("SELECT Phone FROM Lead_Flat WHERE Customer='{0}' AND isdeleted=0", intID)).ToString();
                String soldComment = _sql.ExecuteScalar(String.Format("SELECT LeadNote FROM Lead_Flat WHERE Customer='{0}' AND isdeleted=0", intID)).ToString();

                if (!String.IsNullOrEmpty(strNote) || !String.IsNullOrEmpty(strComments))
                {
                    if (!String.IsNullOrEmpty(soldComment)) 
                    {
                        if (String.Compare(soldComment, String.Format("{0} {1}", strNote, strComments)) != 0)
                        {
                            strComments = String.Format("{0}<br>{1:d} {2} {3}", soldComment, DateTime.Now, strNote, strComments);
                        }
                    }
                    else
                    {
                        strComments = String.Format("{0} {1}", soldComment,DateTime.Now,strNote,strComments);
                    }
                }
                else
                {
                    strComments = soldComment;
                }

                ssql = string.Format("SELECT dbo.formatPhone('{0}')", strPhone);
                strPhone = _sql.ExecuteScalar(ssql).ToString();
                if (strPhone != oldPhone)
                {
                    strComments = String.Format("{0}</br><b>{1}</b>", strComments, strPhone);
                }

                _sql.Execute(String.Format("UPDATE t " +
                                           "   SET t.LeadNote = '{2}', " +
                                           "       t.CallLaterDate = '{1}' " +
                                           "  FROM Lead_Flat t " +
                                           " WHERE Customer = {0} ", intID, DateTime.Now.Date.AddDays(2), strComments.Replace("'", "''")));

                String sheader = "Returning Lead";
                if (EntryDate.AddHours(4) < DateTime.Now)
                {
               
                    _sql.Execute(String.Format("UPDATE t " +
                                               "   SET t.Status = (SELECT Name FROM _LeadStatus WHERE isReturning=1 AND isdeleted = 0), " +
                                               "       t.StatusId = (SELECT Id FROM _LeadStatus WHERE isReturning=1 AND isdeleted = 0), " +
                                               "       t.updatedby = 'D0F06FCC-B87E-4F33-A3D3-D64354863F39', " +
                                               "       t.updatedate = '{1}' " +
                                               "  FROM Lead_Flat t " +
                                               " WHERE Customer = {0} AND isdeleted=0 ", intID, DateTime.Now));

                    DataRow _dr = _sql.GetTable(String.Format("SELECT * FROM Lead_Flat WHERE Customer='{0}' AND isdeleted=0", intID)).Rows[0];

                    if (_dr["PriorityId"].ToString().ToUpper() == "7C89308B-6912-4C50-B55D-2A5AE1B05E19")           // change Not Right to Interested
                    {
                        _sql.Execute(String.Format("UPDATE t " +
                                                   "   SET t.Priority = (SELECT Name FROM _LeadPriority WHERE Id = '1CFBCA64-33E9-4F44-9BB4-8ECA57B3B539' AND isdeleted = 0), " +
                                                   "       t.PriorityId = (SELECT Id FROM _LeadPriority WHERE Id = '1CFBCA64-33E9-4F44-9BB4-8ECA57B3B539' AND isdeleted = 0) " +
                                                   "  FROM Lead_Flat t " +
                                                   " WHERE Customer = {0} AND isdeleted=0 ", intID, DateTime.Now));
                    }


                    if (_dr["AssignedToId"].ToString().ToUpper() == "7D5AA961-5478-4FA1-B5DB-D6A2071ED834")           // change Harry To Frank
                    {
                        _sql.Execute(String.Format("UPDATE t " +
                                                   "   SET t.AssignedToId = (SELECT Id FROM Employee WHERE Id = '44F7B957-4AA9-466F-B5C8-8840586157B6' AND isdeleted = 0), " +
                                                   "       t.AssignedTo= (SELECT DisplayName FROM Employee WHERE Id = '44F7B957-4AA9-466F-B5C8-8840586157B6' AND isdeleted = 0) " +
                                                   "  FROM Lead_Flat t " +
                                                   " WHERE Customer = {0} AND isdeleted=0 ", intID, DateTime.Now));
                    }

                    String noHtml = Regex.Replace(_dr["LeadNote"].ToString().Trim(), @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;", string.Empty).Trim();
                    String stext = String.Format("{0} {1} {2} from {3:d} {4}", _dr["Name"].ToString().Trim(), _dr["EMail"].ToString().Trim(), _dr["Phone"].ToString().Trim(), _dr["EntryDate"].ToString(), noHtml);
                    if (stext.Length > 140) { stext = stext.Substring(0, 140); };
                    Tools.sendMail(from, toharry, sheader, stext, false);
                    Tools.sendMail(from, tofrank, sheader, stext, false);
                    Tools.sendMail(from, torene, sheader, stext, false);
                    Tools.sendMail(from, totatyana, sheader, stext, false);
                }
                Tools.sendMail(from, torene, sheader, String.Format("Updated as {0}", intID), false);
            }

            blnValidEntry = true; 
            _sql.Close();

        }
        catch (Exception ex)
        {
            txtError.Text = ex.Message;
            Tools.sendMail(from, torene, "Received Lead Error", String.Format("Error {0}", ex.Message), false);
        }

        return blnValidEntry;

    }

/*
      
    protected bool verifyemail(string email)
    {
        VerifaliaRestClient restClient = new VerifaliaRestClient("af533e54318140e39d3b0a02ccb0fc30", "6B2beQUWgSZneAf8wn0N");

        var result = restClient.EmailValidations.Submit(new[]
        {
            email
        },
        new WaitForCompletionOptions(TimeSpan.FromMinutes(1)));

        if (result != null) // Result is null if timeout expires
        {
            foreach (var entry in result.Entries)
            {
                String ssql = String.Empty;
                HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

                Console.WriteLine("Address: {0} => Result: {1}",
                    entry.InputData,
                    entry.Status);

                // check table on grass
                ssql = String.Format("INSERT INTO _verifalia SELECT '{0}'",
                    entry.EmailAddress,
                    entry.IsCatchAllFailure,
                    entry.IsDisposableEmailAddress,
                    entry.IsDnsFailure,
                    entry.IsMailboxFailure,
                    entry.IsNetworkFailure,
                    entry.IsRoleAccount,
                    entry.IsSmtpFailure,
                    entry.IsSuccess,
                    entry.IsSyntaxFailure,
                    entry.IsTimeoutFailure,
                    entry.Status);
                _sql.Close();

                if (entry.IsSyntaxFailure == true) { return false; };
                if (entry.IsSmtpFailure == true) { return false; } 
                if (entry.IsNetworkFailure == true) { return false; } 
                if (entry.IsMailboxFailure == true) { return false; } 
                if (entry.IsDnsFailure == true) { return false; } 
            }
        }

        return true;
    }
 
 */
 
}