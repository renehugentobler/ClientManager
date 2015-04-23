using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Data.SqlClient;

using SisoDb.Sql2012;
using SisoDb.Configurations;

using CTCT;
using CTCT.Components;
using CTCT.Components.Contacts;
using CTCT.Components.AccountService;
using CTCT.Components.EmailCampaigns;
using CTCT.Exceptions;

using Verifalia.Api;

public partial class Pages_LeadsReceiver2 : System.Web.UI.Page
{

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
                                       "", intTarget,google_conversion_id,google_conversion_language,google_conversion_format,google_conversion_color,google_conversion_label,google_remarketing_only);
        var _script = new HtmlGenericControl("script");
        _script.Attributes.Add("type", "text/javascript");
        _script.InnerHtml = cstext1.ToString();
        google_init.Controls.Add(_script);
        googleImg.Src = String.Format("//www.googleadservices.com/pagead/conversion/{0}/?label={1}&guid=ON&script=0", google_conversion_id, google_conversion_label, intTarget);
    }

    public static int intTarget = -1;

    public static MailAddress from = new MailAddress("leadsreceiver@uncleharry.biz", "Leads Receiver");
    public static MailAddress torene = new MailAddress("4438255040@tmomail.net", "René Marçel Hugentobler");
    public static MailAddress toharry = new MailAddress("4109673455@txt.att.net", "Harry Raker");
    public static MailAddress totatyana = new MailAddress("4104460158@tmomail.net", "Tatiana Drobotova");

    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection nvc = Request.Form;

        String strName = String.Empty;
        String strEmail = String.Empty;
        String strPhone = String.Empty;
        DateTime dtePostDate = DateTime.Now;
        String strNote = String.Empty;
        String strSource = "unknown";
        String strTimeZone = String.Empty;
        int intID = -1;
        String sheader = String.Empty;
        String _smail = String.Empty;
        DateTime dCallLaterDate = DateTime.Now.Date.AddDays(5);

        String sSourceId = "CC2AE91C-5286-41A6-8B75-BD566B2427F9";
        String sPriorityId = "F00563E9-B454-42C6-8257-476A702EFABF";
        String sStatusId = "D692B6D9-E95D-4435-BF10-3A5A4368758C";
        String sAssignedToId = String.Empty;

        // init db
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        // init timezone
        TimeZoneInfo edtZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        DateTime timeUtc = DateTime.UtcNow;
        DateTime edtTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, edtZone);

        if (!Page.IsPostBack)
        {

            try
            {
                // send dev email to rene
                Tools.sendMail(from, torene, "Received Lead", String.Format("'{0}'", nvc.ToString()), false);

                if (!string.IsNullOrEmpty(nvc["Name"]))
                {
                    strName = nvc["Name"];
                    // Case the Name
                    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                    strName = myTI.ToTitleCase(strName);
                }
                if (!string.IsNullOrEmpty(nvc["Email"]))
                {
                    strEmail = nvc["Email"];
                    // check email
                    // verifyemail(strEmail)
                }
                if (!string.IsNullOrEmpty(nvc["Phone"]))
                {
                    strPhone = nvc["Phone"];
                    //fix the phone#
                    ssql = string.Format("SELECT dbo.formatPhone('{0}')", strPhone);
                    strPhone = _sql.ExecuteScalar(ssql).ToString();
                }
                if (!string.IsNullOrEmpty(nvc["Comments"]))
                {
                    strNote = nvc["Comments"];
                }
                strSource = "Video Lead";

                // Determine if email address is already on file 
                ssql = String.Format("SELECT COALESCE((SELECT Customer FROM Lead_Flat WHERE EMail='{0}' AND isdeleted=0),0)", strEmail);
                intID = int.Parse(_sql.ExecuteScalar2(ssql).ToString());
                if (intID == 0)
                {
                    // new id
                    intID = int.Parse(_sql.ExecuteScalar2("SELECT CASE WHEN (SELECT MAX(Customer) FROM Lead_Flat WHERE isdeleted=0)<100001 THEN 100001 ELSE (SELECT MAX(Customer)+1 FROM Lead_Flat WHERE isdeleted=0) END").ToString());
                    sAssignedToId = "0BA4012E-5541-4A76-92BB-C7122344DC3A";

                    // get assigned code
                    try
                    {
                        ssql = String.Format("SELECT COALESCE((SELECT AssignedToId FROM _Leadassign WHERE Customer = {0} AND isdeleted=0),(SELECT Id FROM Employee WHERE (isMasterSales=1 AND isdeleted=0)))", intID);
                        sAssignedToId = _sql.ExecuteScalar(ssql).ToString();
                    }
                    catch (Exception ex)
                    {
                        Tools.sendMail(from, torene, "Lead AssignTo Error", String.Format("{0} {1}", ex.Message, String.Format("SELECT AssignedToId FROM _Leadassign WHERE Customer = {0} AND isdeleted=0", intID)), false);
                        sAssignedToId = "0BA4012E-5541-4A76-92BB-C7122344DC3A";
                    }

                    // get timezone
                    if (strPhone.Length >= 12)
                    {
                        try
                        {
                            ssql = String.Format("SELECT TimeZone FROM tblAreaCodesAndTimeZones WHERE AreaCode = '{0}'", strPhone.Substring(strPhone.Length - 12, 3));
                            strTimeZone = _sql.ExecuteScalar2(ssql).ToString();
                        }
                        catch { }
                    }

                    // don't blow out sql fields
                    if (strName.Length > 64) { strName = strName.Substring(0, 64); }
                    if (strEmail.Length > 64) { strEmail = strEmail.Substring(0, 64); }
                    if (strPhone.Length > 16) { strPhone = strPhone.Substring(strPhone.Length - 16, 16); }
                    if (strNote.Length > 4095) { strNote = strNote.Substring(0, 4095); }

                    ssql = String.Format("INSERT INTO Lead_Flat(Customer, Name, EMail, Phone, EntryDate, CallLaterDate, SourceId, Source, PriorityId, Priority, StatusId, Status, LeadNote, AssignedToId, AssignedTo, TimeZone, isdeleted) " +
                                               "SELECT '{0}','{1}','{2}','{3}','{4}','{5}',({6}),'{7}','{8}',({9}),'{10}',({11}),'{12}','{13}',({14}),'{15}',{16} " +
                                               "", 
                                               intID, 
                                               strName.Replace("'", "''"), 
                                               strEmail.Replace("'", "''"), 
                                               strPhone.Replace("'", "''"), 
                                               dtePostDate,
                                               dtePostDate.Date.AddDays(5),
                                               String.Format("SELECT Upper(Id) FROM _LeadSource WHERE Upper(Name) = '{0}' AND isdeleted = 0", strSource.ToUpper()),
                                               strSource, 
                                               sPriorityId,
                                               String.Format("SELECT Name FROM _LeadPriority WHERE Upper(Id) = '{0}' AND isdeleted = 0", sPriorityId.ToUpper()),
                                               sStatusId,
                                               String.Format("SELECT Name FROM _LeadStatus WHERE Upper(Id) = '{0}' AND isdeleted = 0", sStatusId.ToUpper()), 
                                               strNote.Replace("'", "''"), 
                                               sAssignedToId,
                                               String.Format("SELECT DisplayName FROM Employee WHERE Upper(Id) = '{0}' AND isdeleted = 0", sAssignedToId.ToUpper()),
                                               strTimeZone, 
                                               0);
                    _sql.Execute(ssql);

                    // throw (new Exception(ssql));

                    // Add to CC and SisoDB
                    SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db();
                    using (var session = _siso.BeginSession())
                    {
//                        var result = session.Query<CTCT.Components.Contacts.Contact>() .Where(o => o.Id == Id);
//                        if (result.FirstOrDefault() != null)
//                        {
//                            ssql = String.Format("UPDATE t SET [ConstantContactID] = {0} FROM Lead_Flat t WHERE Customer = {1} ", Id, intID);
//                            _sql.Execute(ssql);
//                        }
//                        else
//                        {
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
                                ssql = String.Format("UPDATE t " +
                                                     "   SET t.ConstantContactID = {0} " +
                                                     "  FROM Lead_Flat t " +
                                                     " WHERE Customer = {1}", _ctres.Id, intID);
                                _sql.Execute(ssql);
                                ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                    "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "ConstantContact", "ADD", intID, _ctres.Id, "D0F06FCC-B87E-4F33-A3D3-D64354863F39");
                                _sql.Execute(ssql);
                            }
                            catch (CTCT.Exceptions.CtctException ex)
                            {
                                ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                                     "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "ConstantContact", "ERROR", intID, ex.InnerException.ToString().Replace("'", "''"), "D0F06FCC-B87E-4F33-A3D3-D64354863F39");
                                _sql.Execute(ssql);
                            }
                        }
//                    }
                }
                else
                {
                    ssql = String.Format("SELECT * FROM Lead_Flat WHERE Customer = {0} AND isdeleted=0", intID);
                    DataRow dr = _sql.GetTable(ssql).Rows[0];
                    sSourceId = dr["SourceId"].ToString();
                    sPriorityId = dr["PriorityId"].ToString();
                    sStatusId = dr["StatusId"].ToString();
                    sAssignedToId = dr["AssignedToId"].ToString();
                    dCallLaterDate = DateTime.Parse(dr["CallLaterDate"].ToString());

                    // add new lead note
                    if (dr["LeadNote"].ToString().Contains(strNote))
                    {
                        strNote = dr["LeadNote"].ToString();
                    }
                    else
                    {
                        strNote = String.Format("{0}</br>{2:d} {1}", dr["LeadNote"], strNote, DateTime.Now);
                    }

                    // add new phone if not equal
                    if (dr["Phone"].ToString().Length == 0)
                    { }
                    else if (dr["Phone"].ToString() == strPhone)
                    { }
                    else
                    {
                        strNote = String.Format("{0}</br><b>Old phone# : {1}</b>", dr["LeadNote"], dr["Phone"]);
                    }

                    // add new name if not equal
                    if (dr["Name"].ToString().Length == 0)
                    { }
                    else if (dr["Name"].ToString() == strName)
                    { }
                    else
                    {
                        strNote = String.Format("{0}</br><b>Old name : {1}</b>", dr["LeadNote"], dr["strName"]);
                    }

                    DateTime EntryDate = DateTime.Parse(dr["EntryDate"].ToString());
                    if (EntryDate.AddHours(4) < DateTime.Now)
                    {
                    }
                    else
                    {
                        // returning lead
                        sStatusId = "D27F9479-EF66-495C-B279-8A852047319E";
                        sPriorityId = "1CFBCA64-33E9-4F44-9BB4-8ECA57B3B539";
                        dCallLaterDate = DateTime.Now.Date.AddDays(2);
                        sheader = "Returning Lead";
                        String noHtml = Regex.Replace(strNote.ToString().Trim(), @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;", string.Empty).Trim();
                        _smail = String.Format("{0} {1} {2} {3}", dr["Name"].ToString().Trim(), dr["EMail"].ToString().Trim(), dr["Phone"].ToString().Trim(), noHtml);
                        if (_smail.Length > 140) { _smail = _smail.Substring(0, 140); };
                    }

                    if (strName.Length > 64) { strName = strName.Substring(0, 64); }
                    if (strPhone.Length > 16) { strPhone = strPhone.Substring(strPhone.Length - 16, 16); }
                    if (strNote.Length > 4095) { strNote = strNote.Substring(0, 4095); }

                    ssql = String.Format("UPDATE t " +
                                       "   SET t.Name = '{1}', " +
                                       "       t.Phone = '{2}', " +
                                       "       t.CallLaterDate = '{3}' " +
                                       "       t.SourceId = '{4}', " +
                                       "       t.Source = (SELECT Name FROM _LeadSource WHERE Id = '{4}' AND isdeleted = 0), " +
                                       "       t.PriorityId = '{5}', " +
                                       "       t.Priority = (SELECT Name FROM _LeadPriority WHERE Id = '{5}' AND isdeleted = 0), " +
                                       "       t.StatusId = '{6}', " +
                                       "       t.Status = (SELECT Name FROM _LeadStatus WHERE Id = '{6}' AND isdeleted = 0), " +
                                       "       t.LeadNote = '{7}' " +
                                       "       t.AssignedToId = '{8}', " +
                                       "       t.AssignedToId = (SELECT Name FROM Employee WHERE Id = '{8}' AND isdeleted = 0), " +
                                       " WHERE Customer = {0} ABD isdeleted = 0",
                                       intID,
                                       strName.Replace("'", "''"),
                                       strPhone,
                                       dCallLaterDate,
                                       sSourceId,
                                       sPriorityId,
                                       sStatusId,
                                       strNote.Replace("'", "''"),
                                       sAssignedToId);
                    _sql.Execute(ssql);

                    // Update to CC
                    // Update SisoDb

                }
                if (_smail.Length > 0)
                {
                    ssql = String.Format("SELECT e.DisplayName,e.Phone,s.EMail FROM Employee e,_SMSProviders s WHERE e.Id = '{0}' AND e.isdeleted = 0 AND s.Id = e.Provider AND s.isdeleted = 0", sAssignedToId.ToUpper());
                    DataRow _sms = _sql.GetRow(ssql);
                    MailAddress toAssigned = new MailAddress(String.Format("{0}@{1}", _sms["Phone"], _sms["EMail"]), _sms["DisplayName"].ToString());
                    foreach (MailAddress _mail in new MailAddress[] { toAssigned, toharry, torene, totatyana })
                    {
                        txtError.Text = String.Format("{0}", from.DisplayName);
//                        txtError.Text = String.Format("{0} {1} {2} {3}", from, _mail, "Returning Lead", _smail);
                        Tools.sendMail(from, _mail, "Returning Lead", _smail, false);
                    }
                }
                _sql.Close();
                Response.Redirect("http://uncleharry.com.spiraea.arvixe.com/thank-you/");
            }
            catch (SqlException sqlex)
            {
                txtError.Text = sqlex.Message + '\n' + sqlex.StackTrace + "\n Errors: ";
                for (int i = 0; i < sqlex.Errors.Count; i++) { txtError.Text += String.Format("   Index #" + i + " Error: " + sqlex.Errors[i].ToString()); }
                txtError.Text += "\n Data: ";
                if (sqlex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in sqlex.Data) { txtError.Text += String.Format("    Key:{0,-20} Value:{1}", "'" + de.Key.ToString() + "'", de.Value); }
                }
                txtError.Text += "\n sql: " + ssql;
                Tools.sendMail(from, torene, "Lead SQL Error", String.Format("Error {0}", sqlex.Message), false);
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message + '\n' + ex.StackTrace;
                Tools.sendMail(from, torene, "Lead Error", String.Format("Error {0}", ex.Message), false);
            }
            finally
            {
                // fill form fields
                txtId.Text = intID.ToString();
                txtName.Text = strName;
                txtEmail.Text = strEmail;
                txtPhone.Text = strPhone;
                txtComments.Text = strNote;
            }
        }
    }
   

    private Boolean AddLead(out int intID, String strName, String strEmail, String strPhone, String strComments, DateTime dtePostDate, String strSource, string strNote)
    {

        Boolean blnValidEntry = false;
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        intID = -1;

        try
        {
            intID = int.Parse(_sql.ExecuteScalar2("SELECT CASE WHEN (SELECT MAX(Customer) FROM Lead_Flat)<100001 THEN 100001 ELSE (SELECT MAX(Customer)+1 FROM _test) END").ToString());

            // Determine if email address is already on file
            ssql = String.Format("SELECT COUNT(*) FROM Lead_Flat WHERE EMail='{0}' AND isdeleted=0", strEmail);
            int intContactCount = int.Parse(_sql.ExecuteScalar(ssql).ToString());

            if (intContactCount == 0)
            {
                // check if sequence is broken and send email

                ssql = String.Format("SELECT Id FROM _LeadSource WHERE (RTRIM(old_Source)='{0}') OR ('{0}'='Test Lead')", strSource.Trim());
                String sSourceId = _sql.ExecuteScalar(ssql).ToString();
                ssql = String.Format("SELECT Id FROM _LeadPriority WHERE (Name='{0}' AND isdeleted=0)", "Undefined");
                String sPriorityId = _sql.ExecuteScalar(ssql).ToString();
                ssql = String.Format("SELECT Id FROM _LeadStatus WHERE (Name='{0}' AND isdeleted=0)", "New Lead");
                String sStatusId = _sql.ExecuteScalar(ssql).ToString();
                ssql = String.Format("SELECT Id FROM Employee WHERE (isMasterSales=1 AND isdeleted=0)", "");
                String sAssignedToId = _sql.ExecuteScalar(ssql).ToString();

                // overwrite AssignedToId with entry in _Leadassign
                // strange stuff may happen if we get a deleted returning from Bill
                try
                {
                    ssql = String.Format("SELECT COALESCE((SELECT AssignedToId FROM _Leadassign WHERE Customer = {0}),'44F7B957-4AA9-466F-B5C8-8840586157B6')", intID);
                    sAssignedToId = _sql.ExecuteScalar(ssql).ToString();
                }
                catch (Exception ex)
                {
                    Tools.sendMail(from, torene, "Lead AssignTo Error", String.Format("{0} {1}", ex.Message, String.Format("SELECT AssignedToId FROM _Leadassign WHERE Customer = {0}", intID)), false);
                    ssql = String.Format("SELECT Id FROM Employee WHERE (isMasterSales=1 AND isdeleted=0)", "");
                    sAssignedToId = _sql.ExecuteScalar(ssql).ToString();
                }
                ssql = String.Format("SELECT DisplayName FROM Employee WHERE Id = '{0}' AND isdeleted = 0", sAssignedToId);
                String sAssignedToName = _sql.ExecuteScalar(ssql).ToString();

                if (!String.IsNullOrEmpty(strNote)) { strComments = String.Format("{0} {1}", strNote.Trim(), strComments.Trim()); }

                // don't blow out sql fields
                if (strName.Length > 64) { strName = strName.Substring(0, 64); }
                if (strEmail.Length > 64) { strEmail = strEmail.Substring(0, 64); }
                if (strPhone.Length > 16) { strPhone = strPhone.Substring(strPhone.Length - 16, 16); }
                if (strComments.Length > 4095) { strComments = strComments.Substring(0, 4095); }
                if (strComments.Length > 4095) { strComments = strComments.Substring(0, 4095); }

                // case stuff correctly
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
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

                if (strPhone.Length >= 12)
                {
                    strAreaCode = strPhone.Substring(strPhone.Length - 12, 3);
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
                if (strEmail.Length > 0)
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
                        strComments = String.Format("{0} {1}", soldComment, DateTime.Now, strNote, strComments);
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
                    Tools.sendMail(from, torene, sheader, stext, false);
                    Tools.sendMail(from, totatyana, sheader, stext, false);
                }
                Tools.sendMail(from, torene, sheader, String.Format("Updated as {0}", intID), false);
            }

            blnValidEntry = true;
            _sql.Close();

        }
        catch (SqlException sqlex)
        {
            txtError.Text = sqlex.Message + '\n' + sqlex.StackTrace + "\n Errors: ";
            for (int i = 0; i < sqlex.Errors.Count; i++) { txtError.Text+=String.Format("   Index #" + i + " Error: " + sqlex.Errors[i].ToString()); }
            txtError.Text += "\n Data: ";
            if (sqlex.Data.Count > 0)
            {
                foreach (DictionaryEntry de in sqlex.Data) { txtError.Text += String.Format("    Key:{0,-20} Value:{1}", "'" + de.Key.ToString() + "'", de.Value); }
            }
            txtError.Text += "\n sql: " + ssql;
            Tools.sendMail(from, torene, "Received Lead SQL Error", String.Format("Error {0}", sqlex.Message), false);
        }
        catch (Exception ex)
        {
            txtError.Text = ex.Message + '\n' + ex.StackTrace;
            Tools.sendMail(from, torene, "Received Lead Error", String.Format("Error {0}", ex.Message), false);
        }

        return blnValidEntry;

    }

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
}