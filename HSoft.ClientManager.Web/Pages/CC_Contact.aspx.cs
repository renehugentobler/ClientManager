using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Data.OleDb;

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

using Obout.Interface;

public partial class Pages_CC_Contact : System.Web.UI.Page
{

    private static String ssql = String.Empty;

    public ConstantContact _constantContact = null;
    private string _apiKey = string.Empty;
    private string _accessToken = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Tools.devlogincheat();
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

            LastUpdate.Text = _sql.ExecuteScalar("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Contact' AND isdeleted = 0").ToString();
            CountContacts.Text = _sql.ExecuteScalar("SELECT COUNT(*) FROM ContactStrings WHERE MemberPath = 'Status'").ToString();
            CountActive.Text = _sql.ExecuteScalar("SELECT COUNT(*) FROM ContactStrings WHERE MemberPath = 'Status' AND LOWER(Value) ='active'").ToString();
            CountLeads.Text = _sql.ExecuteScalar("SELECT COUNT(*) FROM Lead_Flat WHERE isdeleted = 0 ").ToString();
            Linked.Text = _sql.ExecuteScalar("SELECT COUNT(*) FROM Lead_Flat WHERE ConstantContactID >0 AND isdeleted = 0 ").ToString();
            _sql.Close();
        }

    }

    public int UpdateContacts(DateTime lastupdate)
    {
        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db();
//        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManagerA"].ConnectionString.CreateSql2012Db();
//        lastupdate = DateTime.Parse("1980-01-01");

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


        int iCount = 0;
        Pagination _page = null;
        DateTime _last = lastupdate;
        while (1 == 1)
        {
            ResultSet<CTCT.Components.Contacts.Contact> _contacts = null;
            try
            {
                _contacts = _constantContact.GetContacts(lastupdate, _page);
            }
            catch (Exception ex) 
            {
                Exception ex2 = new Exception(String.Format("GetContacts {0}",ex.Message));
                throw ex; 
            }
            if (_contacts == null)
            {
                Exception ex = new Exception("No results returned, possible connection failure.");
                throw ex;
            }
            using (var session = _siso.BeginSession())
            {
                foreach (CTCT.Components.Contacts.Contact _contact in _contacts.Results)
                {
                    if (session.Query<CTCT.Components.Contacts.Contact>().Count(o => o.Id == _contact.Id) != 0)
                    {
                        session.Update(_contact);
                    }
                    else
                    {
                        session.Insert(_contact);
                        _last = DateTime.Parse(_contact.DateModified);
                    }
                }
            }
            if (_last != lastupdate) { Tools.SetDate(_last, "Contact"); }
            iCount = iCount + _contacts.Results.Count;
            if (_contacts.Meta.Pagination.Next == null) { break; }
            if (_page == null) { _page = new Pagination(); }
            _page.Next = _contacts.Meta.Pagination.Next;
        }

        return iCount;
    }

    public Boolean upLoadToCC()
    {
        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db(); 
        HSoft.SQL.SqlServer _sql3 = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        _apiKey = ConfigurationManager.AppSettings["APIKey"];
        _accessToken = ConfigurationManager.AppSettings["AccessToken"];
        if (_accessToken.Length != new Guid().ToString().Length)
        {
            byte[] decryptedB = Convert.FromBase64String(ConfigurationManager.AppSettings["AccessToken"]);
            _accessToken = System.Text.Encoding.UTF8.GetString(decryptedB).Replace("\0", "");
        }
        _constantContact = new ConstantContact(_apiKey, _accessToken);

        using (var session = _siso.BeginSession())
        {
            IList<ContactList> _rscl = null;
            try
            {
                _rscl = _constantContact.GetLists(null);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception(String.Format("GetContacts {0}", ex.Message));
                throw ex;
            }

            ContactList _cla = _rscl.FirstOrDefault(o => o.Name == "Added by Client Manager");
            ContactList _cls = _rscl.FirstOrDefault(o => o.Name == "Sold Course");
            ContactList _clq = _rscl.FirstOrDefault(o => o.Name == "Questionable Contact");

            DataTable dt = _sql3.GetTable("SELECT * FROM Lead_Flat WHERE ConstantContactID IS NULL AND isdeleted = 0 ORDER BY Customer");
            foreach (DataRow dr in dt.Rows)
            {
                if (Tools.IsValidEmail(dr["EMail"].ToString()))
                {
                    String ssql = String.Format("SELECT a.Value " +
                                                "  FROM ContactStrings a, ContactStrings b  " +
                                                " WHERE a.MemberPath = 'Id' " +
                                                "   AND a.StructureId = b.StructureId " +
                                                "   AND b.MemberPath = 'EmailAddresses.EmailAddr' " +
                                                "   AND b.Value = '{0}' ", dr["EMail"].ToString().ToLower());
                    String Id = _sql3.ExecuteScalar(ssql).ToString();
                    var result = session.Query<CTCT.Components.Contacts.Contact>()
                        .Where(o => o.Id == Id);
                    if (result.FirstOrDefault() != null)
                    {
                        continue;
                    }

                    CTCT.Components.Contacts.Contact _ct = new CTCT.Components.Contacts.Contact();
                    CTCT.Components.Contacts.EmailAddress _em = new EmailAddress();
                    _em.EmailAddr = dr["EMail"].ToString();
                    _em.ConfirmStatus = "NO_CONFIRMATION_REQUIRED";
                    _em.OptInSource = "ACTION_BY_OWNER";
                    _em.Status = "ACTIVE";
                    _ct.EmailAddresses.Add(_em);

                    _ct.Lists.Add(_cla);
                    if (dr["PriorityId"].ToString().ToLower() == "F3DC2498-6F4F-449E-813C-EFDA32A9D24A".ToLower())
                    {
                        _ct.Lists.Add(_cls);
                    }
                    if ((dr["PriorityId"].ToString().ToLower() == "7c89308b-6912-4c50-b55d-2a5ae1b05e19".ToLower()) ||
                        (dr["PriorityId"].ToString().ToLower() == "3f9bcf7c-fb91-4b57-b2bf-cbe462e07cf2".ToLower()) ||
                        (dr["PriorityId"].ToString().ToLower() == "9e256177-a401-4282-8959-92e5f1ef4268".ToLower()) ||
                        (dr["PriorityId"].ToString().ToLower() == "9491f7a8-086d-4293-87f1-7a897208e325".ToLower()))
                    {
                        _ct.Lists.Add(_clq);
                    }

                    _ct.HomePhone = dr["Phone"].ToString();
                    _ct.FirstName = dr["Name"].ToString();
                    _ct.Source = "Client Manager";
                    _ct.Status = "ACTIVE";

                    CTCT.Components.Contacts.Contact _ctres = new CTCT.Components.Contacts.Contact();
                    try
                    {
                        _ctres = _constantContact.AddContact(_ct, false);
                        session.Insert(_ctres);
                        ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                            "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "ConstantContact", "ADD", dr["Customer"], _ctres.Id, "D0F06FCC-B87E-4F33-A3D3-D64354863F39");
                        _sql3.Execute(ssql);
                        ssql = String.Format("UPDATE t " +
                                             "   SET t.ConstantContactID = {0} " +
                                             "  FROM Lead_Flat t " +
                                             " WHERE Customer = {1}", _ctres.Id, dr["Customer"]);
                        _sql3.Execute(ssql);
                    }
                    catch (CTCT.Exceptions.CtctException ex)
                    {
                        ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                             "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "ConstantContact", "ERROR", dr["Customer"], ex.InnerException, "D0F06FCC-B87E-4F33-A3D3-D64354863F39");
                        _sql3.Execute(ssql);
                    }
                }
                else
                {
                    ssql = String.Format("UPDATE t " +
                     "   SET t.ConstantContactID = {0} " +
                     "  FROM Lead_Flat t " +
                     " WHERE Customer = {1}", -1, dr["Customer"]);
                    _sql3.Execute(ssql);
                }
            }
        }
        _sql3.Close();
        return true;
    }


    protected void Btn_Command(object sender, CommandEventArgs e)
    {
        CC_Command(e.CommandName);
    }

    protected void CC_Command(string scmd)
    {

        switch (scmd)
        {
            case "ResetLoadDate" :
                {
                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
                    DateTime _lastmod = DateTime.Parse(_sql.ExecuteScalar("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Contact' AND isdeleted = 0").ToString());
                    Tools.SetDate(DateTime.Parse("1900-01-01"),"Contact");
                    _sql.Close();
                }
                break;
            case "ResetDatabase":
                {
                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
                    SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db(); 
                    _siso.CreateIfNotExists();
                    // attach MSSQLDB

                    using (var session = _siso.BeginSession())
                    {
                        session.DeleteByQuery<CTCT.Components.Contacts.Contact>(o => o.Id != "");
                    }
                    _sql.Execute("UPDATE t SET ConstantContactID = null FROM Lead_Flat t");
                    _sql.Close();
                    Tools.SetDate(DateTime.Parse("1900-01-01"), "Contact");
                }
                break;
            case "UpdateDatabase":
                {
                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
                    DateTime _lastmod = DateTime.Parse(_sql.ExecuteScalar("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Contact' AND isdeleted = 0").ToString());
                    UpdateContacts(_lastmod);
                    _sql.Close();
                }
                break;
            case "RelinkDatabases":
                {
                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
                    _sql.Execute("UPDATE t SET ConstantContactID = null FROM Lead_Flat t");
                    ssql = String.Format("UPDATE t " +
                                         "   SET t.ConstantContactID = cs2.Value " +
                                         "  FROM Lead_Flat t, ContactStrings cs, ContactStrings cs2 " +
                                         " WHERE LOWER(t.EMail) = LOWER(cs.Value) " +
                                         "   AND cs.StructureId = cs2.StructureId " +
                                         "   AND cs2.MemberPath = 'Id' " +
                                         "   AND cs.MemberPath = 'EmailAddresses.EmailAddr' ", "");
                    _sql.Execute(ssql);
                    _sql.Close();
                }
                break;
            case "UpdateConstantContact":
                {
                    CC_Command("UpdateDatabase");
                    CC_Command("RelinkDatabases");
                    upLoadToCC();
                    CC_Command("UpdateDatabase");
                }
                break;
            case "Refresh" :
                {
                }
                break;
        }

        HSoft.SQL.SqlServer _sql2 = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        LastUpdate.Text = _sql2.ExecuteScalar("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Contact' AND isdeleted = 0").ToString();
        CountContacts.Text = _sql2.ExecuteScalar("SELECT COUNT(*) FROM ContactStrings WHERE MemberPath = 'Status'").ToString();
        CountActive.Text = _sql2.ExecuteScalar("SELECT COUNT(*) FROM ContactStrings WHERE MemberPath = 'Status' AND LOWER(Value) ='active'").ToString();
        CountLeads.Text = _sql2.ExecuteScalar("SELECT COUNT(*) FROM Lead_Flat WHERE isdeleted = 0 ").ToString();
        Linked.Text =_sql2.ExecuteScalar("SELECT COUNT(*) FROM Lead_Flat WHERE ConstantContactID >0  AND isdeleted = 0 ").ToString();

        _sql2.Close();
    }

}