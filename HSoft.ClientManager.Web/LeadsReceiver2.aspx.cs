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
    public static MailAddress totatyana = new MailAddress("4104460158@tmomail.net", "Tatyana Neudecker");

    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
    String ssql = String.Empty;

    String _sheader = "";

    protected void sendSMS2(string sheader, string semail, string sAssignedToId)
    {
        if (_sheader.Length > 0)
        {
            // send txt to salesperson, Tatyana, me and Harry
            foreach (String _sguid in new string[] { sAssignedToId, "0BA4012E-5541-4A76-92BB-C7122344DC3A", "DCDB22C2-65F4-46E4-91D1-CC123F83DCE2", "7D5AA961-5478-4FA1-B5DB-D6A2071ED834" })
//            foreach (String _sguid in new string[] { "0BA4012E-5541-4A76-92BB-C7122344DC3A", "DCDB22C2-65F4-46E4-91D1-CC123F83DCE2", "7D5AA961-5478-4FA1-B5DB-D6A2071ED834" })
//            foreach (String _sguid in new string[] { "DCDB22C2-65F4-46E4-91D1-CC123F83DCE2", "0BA4012E-5541-4A76-92BB-C7122344DC3A" })
            {
                try
                {
                    ssql = String.Format("SELECT e.DisplayName,e.Phone,s.EMail FROM Employee e,_SMSProviders s WHERE UPPER(e.Id) = '{0}' AND e.isdeleted = 0 AND s.Id = e.Provider AND s.isdeleted = 0", _sguid.ToUpper());
                    DataRow _sms = _sql.GetRow(ssql);

                    if (semail.Length > 139 - sheader.Length) { semail = semail.Substring(0, 139 - sheader.Length); };
                    String sendto = String.Format("{0}@{1}", _sms["Phone"].ToString(), _sms["EMail"].ToString());

                    ssql = String.Format("INSERT INTO _auditt([Table], Field, OldValue, NewValue) VALUES('{0}','{1}','{2}','{3}')",
                                 sendto,
                                 _sms["DisplayName"].ToString(),
                                 semail.Replace("'", "''"), "");
                    _sql.Execute(ssql);
                    Tools.sendSMS("leadsreceiver@uncleharry.biz", sendto, _sheader, semail);
                    System.Threading.Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    try { Tools.sendSMS("leadsreceiver@uncleharry.biz", torene.Address, "Lead SMS error", String.Format("{0} {1}", ex.Message.ToString(), ssql)); }
                    catch { }
                }
            }
        }
    }
    protected void sendEmail(string sendtomail,string sendtoname, string skey)
    {
        MailAddress _smtpfrom = new MailAddress("emailer@uncleharry.biz", "Sales Uncleharry");
        MailAddress _from = new MailAddress(String.Format("{0}@uncleharry.biz", "harry"), "Sales Uncleharry");
        MailAddress _to = new MailAddress(sendtomail, sendtoname);
        MailAddress _sender = new MailAddress(String.Format("{0}@uncleharry.biz", "harry"), "Sales Uncleharry");

        DataRow _drEmail = _sql.GetTable(String.Format("SELECT * FROM Email WHERE isdeleted = 0 AND EmailDescription = '{0}'", skey)).Rows[0];

        String sEmail = String.Format(_drEmail["EmailBody"].ToString(), sendtoname, "Harry", "Harry Raker");
        String ssubject = _drEmail["EmailSubject"].ToString();

        try
        {
            Tools.sendMail(_smtpfrom, @"Taurec86@", _from, _to, ssubject, sEmail, true);
        }
        catch (Exception ex)
        {
            Exception ex2 = new Exception(String.Format("{0}", ex.Message));
            throw ex2;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        NameValueCollection nvc = Request.Form;

        String strName = String.Empty;
        String strEmail = String.Empty;
        String strPhone = String.Empty;
        String dtePostDate = String.Empty;
        String strNote = String.Empty;
        String strSource = "unknown";
        int intID = -1;


        if (!Page.IsPostBack)
        {

//            try 
//            {
//                Tools.sendSMS("leadsreceiver@uncleharry.biz", torene.Address, "Received Lead", nvc["Name"].ToString()); 
//            }
//            catch (Exception ex)
//            {
//                try { Tools.sendSMS("leadsreceiver@uncleharry.biz", torene.Address, "Lead SMS error", String.Format("*{0} {1}", ex.Message.ToString(), ssql)); }
//                catch { }
//            }

            TimeZoneInfo edtZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime timeUtc = DateTime.UtcNow;
            DateTime edtTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,edtZone);

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

            // skip over spam
            if (
                (strName.ToUpper().Contains("RAY BAN ")) 
                ||  (strEmail.Contains("*") &&  strEmail.ToUpper().Contains("GMAIL.COM")) 
                )
            {
                Response.Redirect("https://www.fcc.gov/guides/spam-unwanted-text-messages-and-email");
            }

            txtName.Text = strName;
            txtEmail.Text = strEmail;
            txtPhone.Text = strPhone;
            txtComments.Text = strNote;

            String sAssignedToId = "";
            if (AddLead(ref intID, strName, strEmail, strPhone, edtTime, strSource, strNote, out sAssignedToId))
            {
                if (String.IsNullOrEmpty(txtError.Text))
                {
                    ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                            "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "LeadReceiver", _sheader, intID, "", Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
                    _sql.Execute(ssql);
                    _sql.Close();

                    String noHtml = Regex.Replace(strNote.Trim(), @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;", " ").Trim();
                    String stext = String.Format("{3:t} {0} {1} {2} {4}", strName.Trim(), strEmail.Trim(), strPhone.Trim(), DateTime.Now, noHtml);
                    sendSMS2(_sheader, stext, sAssignedToId);
                    sendEmail(strEmail,strName,"Your Income Potential");

                    Response.Redirect("http://uncleharry.com/thank-you/");
                }
                ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                     "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "LeadReceiver", "ADD", intID, txtError.Text.Replace("'","''"), Guid.Empty);
                _sql.Execute(ssql);
                _sql.Close();
            }
            ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                     "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "LeadReceiver", "ADD", intID, txtError.Text.Replace("'", "''"), Guid.Empty);
            _sql.Execute(ssql);
            _sql.Close();

        }
    }
   
    private Boolean AddLead(ref int intID, String strName, String strEmail, String strPhone, DateTime dtePostDate, String strSource, string strNote,  out String sAssignedToId)
    {

        Boolean blnValidEntry = false;

        String sSourceId = "CC2AE91C-5286-41A6-8B75-BD566B2427F9";
        String sPriorityId = "F00563E9-B454-42C6-8257-476A702EFABF";
        String sStatusId = "D692B6D9-E95D-4435-BF10-3A5A4368758C";
        sAssignedToId = String.Empty;

        DateTime dCallLaterDate = DateTime.Now.Date.AddDays(5);
        String ssql = String.Empty;

        try
        {
            // Determine if email address is already on file
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            if (strEmail.Length > 0)
            {
                ssql = String.Format("SELECT COALESCE((SELECT Customer FROM Lead_Flat WHERE EMail='{0}' AND isdeleted=0),0)", strEmail);
                intID = int.Parse(_sql.ExecuteScalar2(ssql).ToString());
                txtId.Text = intID.ToString();
            }

            if (intID == 0)
            {
                _sheader = "New Lead";

                intID = int.Parse(_sql.ExecuteScalar2("SELECT CASE WHEN (SELECT MAX(Customer) FROM Lead_Flat WHERE isdeleted=0)<100001 THEN 100001 ELSE (SELECT MAX(Customer)+1 FROM Lead_Flat WHERE isdeleted=0) END").ToString());
                txtId.Text = intID.ToString();

                sSourceId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadSource WHERE (RTRIM(old_Source)='{0}') OR ('{0}'='Test Lead') AND isdeleted = 0", strSource.Trim())).ToString();
                sPriorityId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadPriority WHERE (Name='{0}' AND isdeleted=0)", "Undefined")).ToString();
                sStatusId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadStatus WHERE (Name='{0}' AND isdeleted=0)", "New Lead")).ToString();

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
                String strTimeZone = String.Empty;
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

                //                if (Tools.IsValidEmail(strEmail))
                if (strEmail.Length > 0)
                {
                    ssql = String.Format("SELECT COALESCE((SELECT MAX(a.Value) " +
                                         "  FROM ContactStrings a, ContactStrings b  " +
                                         " WHERE a.MemberPath = 'Id' " +
                                         "   AND a.StructureId = b.StructureId " +
                                         "   AND b.MemberPath = 'EmailAddresses.EmailAddr' " +
                                         "   AND b.Value = '{0}'),0) ", strEmail.ToLower());
                    Int32 Id = Int32.Parse(_sql.ExecuteScalar(ssql).ToString());
                    if (Id != 0)
                    {
                        ssql = String.Format("UPDATE t SET [ConstantContactID] = {0} FROM Lead_Flat t WHERE Customer = {1} AND isdeleted=0 ", Id, intID);
                        _sql.Execute(ssql);
                    }
                    else
                    { 
                        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db();
                        using (var session = _siso.BeginSession())
                        {
                            var result = session.Query<CTCT.Components.Contacts.Contact>() .Where(o => o.Id == Id.ToString());

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

                            try { _ctres = _constantContact.AddContact(_ct, false); } catch {}

                            if (_ctres == null)
                            {
                                ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                                     "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "ConstantContact", "ERROR", intID, "null value", "D0F06FCC-B87E-4F33-A3D3-D64354863F39");
                                _sql.Execute(ssql);
                            }
                            else
                            {
                                try
                                {
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
                                catch (Exception ex)
                                {
                                    ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                                         "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "ConstantContact", "ERROR", intID, ex.InnerException.ToString().Replace("'", "''"), "D0F06FCC-B87E-4F33-A3D3-D64354863F39");
                                    _sql.Execute(ssql);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ssql = String.Format("SELECT lf.*,lp.IsLead FROM Lead_Flat lf,_LeadPriority lp WHERE Customer = {0} AND lf.isdeleted=0 AND lp.isdeleted=0 AND PriorityId = lp.Id", intID);
                DataRow dr = _sql.GetTable(ssql).Rows[0];
                sSourceId = dr["SourceId"].ToString();
                sPriorityId = dr["PriorityId"].ToString();
                sStatusId = dr["StatusId"].ToString();
                sAssignedToId = dr["AssignedToId"].ToString();
                dCallLaterDate = DateTime.Now.Date.AddDays(2);

                intID = int.Parse(dr["Customer"].ToString());
                DateTime EntryDate = DateTime.Parse(dr["EntryDate"].ToString());
                String oldPhone = dr["Phone"].ToString(); 
                String soldComment = dr["LeadNote"].ToString();

                if (strName.Length > 64) { strName = strName.Substring(0, 64); }
                if (strPhone.Length > 16) { strPhone = strPhone.Substring(strPhone.Length - 16, 16); }

                if (dr["LeadNote"].ToString().Contains(strNote)==true)
                {
                    strNote = dr["LeadNote"].ToString();
                }
                else
                {
                    strNote = String.Format("{0}</br>{2:d} {1}", dr["LeadNote"], strNote, DateTime.Now);
                }

                // add new phone if not equal
                if (dr["Phone"].ToString().Contains(strPhone) == true)
                { }
                else
                {
                    strNote = String.Format("{0}</br><b>Previous phone : {1}</b>", strNote, dr["Phone"]);
                }

                // add new name if not equal
                if (dr["Name"].ToString().Contains(strName) == true)
                { }
                else
                {
                    strNote = String.Format("{0}</br><b>Previous name : {1}</b>", strNote, dr["Name"]);
                }

                EntryDate = DateTime.Parse(dr["EntryDate"].ToString());
                if (EntryDate.AddHours(24) < DateTime.Now)
                {
                }
                else
                {
                    // returning lead
                    sStatusId = "D27F9479-EF66-495C-B279-8A852047319E";
                    if (dr["IsLead"].ToString() == "0") { sPriorityId = "1CFBCA64-33E9-4F44-9BB4-8ECA57B3B539"; }
                    dCallLaterDate = DateTime.Now.Date.AddDays(2);
                    _sheader = "Returning Lead";
                }

                if (strNote.Length > 4095) { strNote = strNote.Substring(0, 4095); }

                ssql = String.Format("UPDATE t " +
                                    "   SET t.Name = '{1}', " +
                                    "       t.Phone = '{2}', " +
                                    "       t.CallLaterDate = '{3}', " +
                                    "       t.SourceId = '{4}', " +
                                    "       t.Source = (SELECT Name FROM _LeadSource WHERE Id = '{4}' AND isdeleted = 0), " +
                                    "       t.PriorityId = '{5}', " +
                                    "       t.Priority = (SELECT Name FROM _LeadPriority WHERE Id = '{5}' AND isdeleted = 0), " +
                                    "       t.StatusId = '{6}', " +
                                    "       t.Status = (SELECT Name FROM _LeadStatus WHERE Id = '{6}' AND isdeleted = 0), " +
                                    "       t.LeadNote = '{7}', " +
                                    "       t.AssignedToId = '{8}', " +
                                    "       t.AssignedTo = (SELECT DisplayName FROM Employee WHERE Upper(Id) = '{0}' AND isdeleted = 0) " +
                                    "   FROM Lead_Flat t "  +
                                    " WHERE Customer = {0} AND isdeleted = 0",
                                    intID,
                                    strName.Replace("'", "''"),
                                    strPhone,
                                    dCallLaterDate,
                                    sSourceId,
                                    sPriorityId,
                                    sStatusId,
                                    strNote.Replace("''", "'").Replace("'", "''"),
                                    sAssignedToId);
                _sql.Execute(ssql);
                strNote = dr["LeadNote"].ToString();
            }

            blnValidEntry = true;
        
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
            Tools.sendMail(from, torene, "Lead SQL Error", String.Format("{0,120}", sqlex.Message), false);
        }
        catch (Exception ex)
        {
            txtError.Text = ex.Message + '\n' + ex.StackTrace;
            txtError.Text += "\n sql: " + ssql;
            Tools.sendMail(from, torene, "Lead Error", String.Format("{0,120}", ex.Message), false);
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