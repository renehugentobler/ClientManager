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

using HSoft.SQL;

using Obout.Grid;

using HSoft.ClientManager.Web;

public partial class Pages_TabInfo_Sold : System.Web.UI.Page
{
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
        grid1.Height = int.Parse(Session["wy"].ToString()) - 75;

        grid1.FolderStyle = "styles/grid/premiere_blue";
        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        Column oColi = new Column();
        oColi.ID = "ID";
        oColi.DataField = "Id";
        oColi.Visible = false;

        Column oCol1 = new Column();
        oCol1.ID = "Name";
        oCol1.DataField = "Name";
        oCol1.HeaderText = "Name";
        oCol1.Visible = true;
        oCol1.ReadOnly = true;
        oCol1.Width = "180";
        oCol1.Wrap = true;

        Column oCol2 = new Column();
        oCol2.ID = "Email";
        oCol2.DataField = "Email";
        oCol2.HeaderText = "Email";
        oCol2.Visible = true;
        oCol2.ReadOnly = true;
        oCol2.Width = "180";

        Column oCol3 = new Column();
        oCol3.ID = "Phone";
        oCol3.DataField = "Phone";
        oCol3.HeaderText = "Phone";
        oCol3.Visible = true;
        oCol3.ReadOnly = true;
        oCol3.Width = "90";

        Column oCol4 = new Column();
        oCol4.ID = "Sold";
        oCol4.DataField = "updatedate";
        oCol4.HeaderText = "LastUpdate";
        oCol4.Visible = true;
        oCol4.ReadOnly = true;
        oCol4.Width = "80";
        oCol4.DataFormatString = "{0:MM/dd/yyyy}";

        Column oCol5 = new Column();
        oCol5.ID = "EntryDate";
        oCol5.DataField = "EntryDate";
        oCol5.HeaderText = "Entry Date";
        oCol5.Visible = true;
        oCol5.ReadOnly = true;
        oCol5.Width = "80";
        oCol5.DataFormatString = "{0:MM/dd/yyyy}";

        Column oCol7 = new Column();
        oCol7.ID = "Source";
        oCol7.DataField = "Source";
        oCol7.HeaderText = "Source";
        oCol7.Visible = true;
        oCol7.ReadOnly = true;
        oCol7.Width = "80";

        Column oCol10 = new Column();
        oCol10.ID = "AssignedTo";
        oCol10.DataField = "AssignedTo";
        oCol10.HeaderText = "AssignedTo";
        oCol10.Visible = true;
        oCol10.ReadOnly = false;
        oCol10.Width = "140";

        Column oCol12 = new Column();
        oCol12.ID = "SalesNote";
        oCol12.DataField = "SalesNote";
        oCol12.HeaderText = "Sales Note";
        oCol12.Visible = true;
        oCol12.ReadOnly = true;
        oCol12.Width = "450";
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

        if (!Page.IsPostBack)
        {
            BindGrid();
        }
    }

    void RebindGrid(object sender, EventArgs e) { BindGrid(); }
    void BindGrid()
    {

        String sPriorityId = "F3DC2498-6F4F-449E-813C-EFDA32A9D24A";
        String sparm1 = "7";
        if (!String.IsNullOrEmpty(Request.QueryString["sPriorityId"])) { sPriorityId = Request.QueryString["sPriorityId"]; }
        if (!String.IsNullOrEmpty(Request.QueryString["sparm1"])) { sparm1 = Request.QueryString["sparm1"]; }

        if (sPriorityId == "F3DC2498-6F4F-449E-813C-EFDA32A9D24A")
        {
            grid1.Columns[4].HeaderText = "Sold";
            grid1.Columns[8].Visible= false;
        }

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = String.Format("SELECT TOP 50 lf.Id, lf.ConstantContactID, lf.Customer, lf.Name, lf.EMail, lf.Phone, lf.EntryDate, lf.CallLaterDate, lf.SourceId, lf.Source, lf.PriorityId, lf.Priority, lf.StatusId, lf.Status, lf.MsgHistory, lf.LeadNote, dbo.fromHDR(lf.SalesNote) SalesNote, lf.AssignedToId, lf.AssignedTo, lf.TimeZone, lf.createdby, lf.createdate, lf.updatedby, lf.updatedate, lf.isdeleted " +
                             "  FROM Lead_Flat lf, EmployeeHirarchy eh " +
                             " WHERE lf.PriorityId = '{2}' " +
                             "   AND lf.AssignedToId = eh.SubEmployeeId " +
                             "   AND eh.EmployeeId = '{0}' " +
                             "   AND lf.IsDeleted = 0 " +
                             "   {1} " +
                             " ORDER BY lf.updatedate DESC, EntryDate DESC ", Session["guser"].ToString(), 
                             sparm1=="-1"?"":String.Format("AND CAST(lf.updatedate AS DATE) >= '{0:d}' ",DateTime.Now.Date.AddDays(int.Parse(sparm1)*-1)), 
                             sPriorityId);
        DataTable _dtLeads = _sql.GetTable(ssql);

        grid1.DataSource = _dtLeads;
        grid1.DataBind();

        _sql.Close();
    }

}