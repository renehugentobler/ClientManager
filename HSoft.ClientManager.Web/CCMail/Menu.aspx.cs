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

public partial class CCMail_Menu : System.Web.UI.Page
{

    public static string sDB = "ClientManagerSP";
    
    protected void Page_Load(object sender, EventArgs e)
    {

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings[sDB].ConnectionString);

        LastDateContacts.Text = _sql.ExecuteScalar("SELECT [TimeStamp] FROM CC_TimeStamps WHERE [Name] = 'LastDateContacts' AND isdeleted = 0").ToString();
        try { RecordCountContacts.Text = _sql.ExecuteScalar("SELECT count(*) FROM [dbo].[ContactStructure]").ToString(); } catch { RecordCountContacts.Text = "-"; }

        LastDateContactLists.Text = _sql.ExecuteScalar("SELECT [TimeStamp] FROM CC_TimeStamps WHERE [Name] = 'LastDateContactLists' AND isdeleted = 0").ToString();
        try { RecordCountContactLists.Text = _sql.ExecuteScalar("SELECT count(*) FROM [dbo].[ContactListStructure]").ToString(); } catch { RecordCountContactLists.Text = "-"; }

        LastDateEmailCampaigns.Text = _sql.ExecuteScalar("SELECT [TimeStamp] FROM CC_TimeStamps WHERE [Name] = 'LastDateEmailCampaigns' AND isdeleted = 0").ToString();
        try { RecordCountEmailCampaigns.Text = _sql.ExecuteScalar("SELECT count(*) FROM [dbo].[EmailCampaignStructure]").ToString(); } catch { RecordCountEmailCampaigns.Text = "-"; }

        LastDateOpenActivities.Text = _sql.ExecuteScalar("SELECT [TimeStamp] FROM CC_TimeStamps WHERE [Name] = 'LastDateOpenActivities' AND isdeleted = 0").ToString();
        try { RecordCountOpenActivities.Text = _sql.ExecuteScalar("SELECT count(*) FROM [dbo].[ContactListStructure]").ToString(); } catch { RecordCountOpenActivities.Text = "-"; }

        _sql.Close();

    }

    protected void Btn_Command(object sender, CommandEventArgs e)
    {
        CC_Command(e.CommandName);
        Response.Redirect("/CCMail/Menu.aspx");
    }

    protected void CC_Command(string scmd)
    {
        switch (scmd)
        {
            case "ResetLoadDate":
                {
                }
                break;
            case "ResetDatabase":
                {
                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings[sDB].ConnectionString);
                    SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings[sDB].ConnectionString.CreateSql2012Db();
                    _siso.CreateIfNotExists();

                    using (var session = _siso.BeginSession())
                    {
                        session.DeleteByQuery<CTCT.Components.Contacts.Contact>(o => o.Id != "");
                        session.DeleteByQuery<CTCT.Components.Contacts.ContactList>(o => o.Id != "");
                        session.DeleteByQuery<CTCT.Components.EmailCampaigns.EmailCampaign>(o => o.Id != "");
//                        session.DeleteByQuery<CTCT.Components.Tracking.OpenActivity>(o => o.CampaignId != "");
                    }

                    _sql.Execute("UPDATE CC_TimeStamps SET [TimeStamp] = '1901-01-01' WHERE [Name] = 'LastDateContacts' AND isdeleted = 0");
                    _sql.Execute("UPDATE CC_TimeStamps SET [TimeStamp] = '1901-01-01' WHERE [Name] = 'LastDateContactLists' AND isdeleted = 0");
                    _sql.Execute("UPDATE CC_TimeStamps SET [TimeStamp] = '1901-01-01' WHERE [Name] = 'LastDateEmailCampaigns' AND isdeleted = 0");
                    _sql.Execute("UPDATE CC_TimeStamps SET [TimeStamp] = '1901-01-01' WHERE [Name] = 'LastDateOpenActivities' AND isdeleted = 0");
                    _sql.Close();
                }
                break;
            case "UpdateDatabase":
                {
                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings[sDB].ConnectionString);

                    DateTime _lastmod;

                    _lastmod = DateTime.Parse(_sql.ExecuteScalar("SELECT [TimeStamp] FROM CC_TimeStamps WHERE [Name] = 'LastDateEmailCampaigns' AND isdeleted = 0").ToString());
                    UpdateEmailCampaigns(_lastmod);

                    _lastmod = DateTime.Parse(_sql.ExecuteScalar("SELECT [TimeStamp] FROM CC_TimeStamps WHERE [Name] = 'LastDateOpenActivities' AND isdeleted = 0").ToString());
                    UpdateEmailCampaignOpens(_lastmod);

                    _lastmod = DateTime.Parse(_sql.ExecuteScalar("SELECT [TimeStamp] FROM CC_TimeStamps WHERE [Name] = 'LastDateContacts' AND isdeleted = 0").ToString());
                    UpdateContacts(_lastmod);

                    _lastmod = DateTime.Parse(_sql.ExecuteScalar("SELECT [TimeStamp] FROM CC_TimeStamps WHERE [Name] = 'LastDateContactLists' AND isdeleted = 0").ToString());
                    UpdateContactLists(_lastmod);

                    _sql.Close();
                }
                break;
        }
    }

    public void UpdateEmailCampaignOpens(DateTime lastupdate)
    {
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings[sDB].ConnectionString);
        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings[sDB].ConnectionString.CreateSql2012Db();

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

        Pagination _page = null;
        DateTime _last = lastupdate;
        int? _limit = 500;
        while (1 == 1)
        {
            ResultSet<CTCT.Components.Tracking.OpenActivity> _openactivities = null;
            try
            {
                _openactivities = _constantContact.GetCampaignTrackingOpens(_limit, lastupdate, _page);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception(String.Format("GetCampaignTrackingOpens {0}", ex.Message));
                throw ex;
            }

            if (_openactivities.Meta.Pagination.Next == null) { break; }
            if (_page == null) { _page = new Pagination(); }
            _page.Next = _openactivities.Meta.Pagination.Next;
        }

        _sql.Execute(String.Format("UPDATE CC_TimeStamps SET [TimeStamp] = '{0}' WHERE [Name] = 'LastDateOpenActivities' AND isdeleted = 0", _last));

    }

    public void UpdateEmailCampaigns(DateTime lastupdate)
    {
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings[sDB].ConnectionString);
        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings[sDB].ConnectionString.CreateSql2012Db();

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


        Pagination _page = null;
        DateTime _last = lastupdate;
        int? _limit = 50;
        while (1 == 1)
        {
            ResultSet<CTCT.Components.EmailCampaigns.EmailCampaign> _emailcampaigns = null;
            try
            {
                _emailcampaigns = _constantContact.GetCampaigns(CampaignStatus.SENT, _limit, lastupdate, _page);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception(String.Format("GetCampaigns {0}", ex.Message));
                throw ex;
            }

            using (var session = _siso.BeginSession())
            {
                foreach (CTCT.Components.EmailCampaigns.EmailCampaign _emailcampaign in _emailcampaigns.Results)
                {
                    if (session.Query<CTCT.Components.EmailCampaigns.EmailCampaign>().Count(o => o.Id == _emailcampaign.Id) != 0)
                    {
                        session.Update(_emailcampaign);
                    }
                    else
                    {
                        session.Insert(_emailcampaign);
                        if (_emailcampaign.ModifiedDate > _last) { _last = (DateTime)_emailcampaign.ModifiedDate; }
                    }
                }
            }

            if (_emailcampaigns.Meta.Pagination.Next == null) { break; }
            if (_page == null) { _page = new Pagination(); }
            _page.Next = _emailcampaigns.Meta.Pagination.Next;
        }
        _sql.Execute(String.Format("UPDATE CC_TimeStamps SET [TimeStamp] = '{0}' WHERE [Name] = 'LastDateEmailCampaigns' AND isdeleted = 0", _last));

        _sql.Close();
    }

    public void UpdateContactLists(DateTime lastupdate)
    {
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings[sDB].ConnectionString);
        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings[sDB].ConnectionString.CreateSql2012Db();

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


        DateTime _last = lastupdate;
        IList<ContactList> _contactlists = null;
        try
        {
            _contactlists = _constantContact.GetLists(lastupdate);
        }
        catch (Exception ex)
        {
            Exception ex2 = new Exception(String.Format("GetContactLists {0}", ex.Message));
            throw ex;
        }
        using (var session = _siso.BeginSession())
        {
            foreach (ContactList _contactlist in _contactlists)
            {
                if (session.Query<CTCT.Components.Contacts.ContactList>().Count(o => o.Id == _contactlist.Id) != 0)
                {
                    session.Update(_contactlist);
                }
                else
                {
                    session.Insert(_contactlist);
                    if (DateTime.Parse(_contactlist.DateModified) > _last) { _last = DateTime.Parse(_contactlist.DateModified); }
                }
            }
        }

        _sql.Execute(String.Format("UPDATE CC_TimeStamps SET [TimeStamp] = '{0}' WHERE [Name] = 'LastDateContactLists' AND isdeleted = 0", _last));

        _sql.Close();
    }

    public void UpdateContacts(DateTime lastupdate)
    {
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings[sDB].ConnectionString);
        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings[sDB].ConnectionString.CreateSql2012Db();

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

        Pagination _page = null;
        DateTime _last = lastupdate;
        int? _limit = 500;
        while (1 == 1)
        {
            ResultSet<CTCT.Components.Contacts.Contact> _contacts = null;
            try
            {
                _contacts = _constantContact.GetContacts(lastupdate,_limit, _page);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception(String.Format("GetContacts {0}", ex.Message));
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
                        if (DateTime.Parse(_contact.DateModified) > _last) { _last = DateTime.Parse(_contact.DateModified); }
                    }
                }
            }
            if (_contacts.Meta.Pagination.Next == null) { break; }
            if (_page == null) { _page = new Pagination(); }
            _page.Next = _contacts.Meta.Pagination.Next;
        }

        _sql.Execute(String.Format("UPDATE CC_TimeStamps SET [TimeStamp] = '{0}' WHERE [Name] = 'LastDateContacts' AND isdeleted = 0", _last));

        _sql.Close();
    }
}