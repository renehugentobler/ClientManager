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

public partial class Pages_CC_Info : System.Web.UI.Page
{
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
}