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
using CTCT.Components.Tracking;
using CTCT.Services;
using CTCT.Exceptions;

using Obout.Grid;

public partial class TabInfo_Campain : System.Web.UI.Page
{

    private static String ssql = String.Empty;

    Grid grid1 = new Grid();

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        grid1.ID = "grid1";
        grid1.CallbackMode = false;
        grid1.Serialize = true;
        grid1.AutoGenerateColumns = false;
        grid1.PageSize = -1;
        grid1.AllowFiltering = false;
        grid1.AllowSorting = false;
        grid1.Height =  int.Parse(Session["wy"].ToString())- 120;

        grid1.FolderStyle = "styles/grid/premiere_blue";
        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        Column oColi = new Column();
        oColi.ID = "ID";
        oColi.DataField = "Id";
        oColi.Visible = false;

        Column oCol1 = new Column();
        oCol1.ID = "Name";
        oCol1.DataField = "Campaign";
        oCol1.HeaderText = "Campaign";
        oCol1.Visible = true;
        oCol1.ReadOnly = true;
        oCol1.Width = "320";
        oCol1.Wrap = true;

        Column oCol2 = new Column();
        oCol2.ID = "OpenDate";
        oCol2.DataField = "OpenDate";
        oCol2.HeaderText = "Open Date";
        oCol2.Visible = true;
        oCol2.ReadOnly = true;
        oCol2.Width = "110";
        oCol2.DataFormatString = "{0:MM/dd/yyyy HH:mm}";

        Column oCol3 = new Column();
        oCol3.ID = "Name";
        oCol3.DataField = "Name";
        oCol3.HeaderText = "Name";
        oCol3.Visible = true;
        oCol3.ReadOnly = true;
        oCol3.Wrap = true;
        oCol3.Width = "120";
        oCol3.Wrap = true;

        Column oCol4 = new Column();
        oCol4.ID = "Email";
        oCol4.DataField = "Email";
        oCol4.HeaderText = "Email";
        oCol4.Visible = true;
        oCol4.ReadOnly = true;
        oCol4.Wrap = true;
        oCol4.Width = "200";

        Column oCol5 = new Column();
        oCol5.ID = "Phone";
        oCol5.DataField = "Phone";
        oCol5.HeaderText = "Phone";
        oCol5.Visible = true;
        oCol5.ReadOnly = true;
        oCol5.Width = "90";
        oCol5.ParseHTML = true;

        Column oCol7 = new Column();
        oCol7.ID = "Priority";
        oCol7.DataField = "Priority";
        oCol7.HeaderText = "Priority";
        oCol7.Visible = true;
        oCol7.ReadOnly = true;
        oCol7.Width = "80"; 
        
        Column oCol10 = new Column();
        oCol10.ID = "LeadNote";
        oCol10.DataField = "LeadNote";
        oCol10.HeaderText = "Lead Note";
        oCol10.Visible = true;
        oCol10.ReadOnly = true;
        oCol10.Width = "300";
        oCol10.Wrap = true;
        oCol10.ParseHTML = true;

        Column oCol12 = new Column();
        oCol12.ID = "SalesNote";
        oCol12.DataField = "SalesNote";
        oCol12.HeaderText = "Sales Note";
        oCol12.Visible = true;
        oCol12.ReadOnly = true;
        oCol12.Width = "350";
        oCol12.Wrap = true;
        oCol12.AllowSorting = false;
        oCol12.ParseHTML = true;
        oCol12.AllowFilter = true;

        grid1.Columns.Add(oColi);
        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol4);
        grid1.Columns.Add(oCol5);

        grid1.Columns.Add(oCol7);

        grid1.Columns.Add(oCol10);
        grid1.Columns.Add(oCol12);

        phGrid1.Controls.Add(grid1);

        if (!IsPostBack)
        {
//            Tools.update_Campain();

            BindGrid();
        }
    }

    void RebindGrid(object sender, EventArgs e) { BindGrid(); }
    void BindGrid()
    {

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
        String sparm1 = "7";

        if (!String.IsNullOrEmpty(Request.QueryString["sparm1"])) { sparm1 = Request.QueryString["sparm1"]; }
        ssql = String.Format("WITH a AS " +
                             "(SELECT [CampaignId],[ContactId],[EmailAddress],MAX(OpenDate) OpenDate " +
                             "   FROM [CampainActivity] " +
                             "  GROUP BY [CampaignId],[ContactId],[EmailAddress]) " +
                             "SELECT REPLACE (e2.Value,'Copy of ','') Campaign,a.ContactId Id,lf.Customer CustomerId,OpenDate OpenDate,COALESCE(lf.Name,cn.Value) Name, " +
                             "       COALESCE(EMail,a.[EmailAddress]) EMail, " +
                             "       COALESCE('<b>'+lf.Phone+'</b>',(SELECT '<b>'+dbo.formatPhone(cs.HomePhone)+'</b>' FROM [SurveyCustomer] cs WHERE cs.EmailAddress = a.[EmailAddress] AND cs.SubmissionTime = (SELECT MAX(cs2.SubmissionTime) FROM [SurveyCustomer] cs2 WHERE cs2.EmailAddress = a.[EmailAddress]))) Phone, " +
                             "       COALESCE(lf.Priority,CASE WHEN (SELECT COUNT(*) FROM [SurveyCustomer] cs WHERE cs.EmailAddress = a.[EmailAddress])=0 THEN 'CC Open' ELSE 'CC Reply' END) Priority, " +
                             "       lf.LeadNote, lf.SalesNote " +
                             "  FROM a " +
                             "       JOIN EmailCampaignStrings e ON a.[CampaignId]=e.Value AND e.MemberPath = 'Id' " +
                             "       JOIN EmailCampaignStrings e2 ON e.StructureId =e2.StructureId AND e2.MemberPath = 'Name' " +
                             "  LEFT JOIN [Lead_Flat] lf ON [ContactId] = lf.ConstantContactID " +
                             "  LEFT JOIN [dbo].[ContactStrings] cn ON cn.StructureId = a.[ContactId] AND cn.MemberPath = 'FirstName' " +
//                             "  LEFT JOIN [SurveyCustomer] cs ON cs.EmailAddress = a.[EmailAddress] " +
                             " WHERE CAST(a.OpenDate AS DATE) >= '{0:d}' " +
                             "   AND (lf.PriorityId IS NULL OR (lf.PriorityId IS NOT NULL AND lf.PriorityId != 'F3DC2498-6F4F-449E-813C-EFDA32A9D24A')) " +        // Sold
                             " ORDER BY OpenDate DESC ",DateTime.Now.Date.AddDays(int.Parse(sparm1)*-1));
        DataTable _dtLeads = _sql.GetTable(ssql);

        grid1.DataSource = _dtLeads;
        grid1.DataBind();

        _sql.Close();
    }

}