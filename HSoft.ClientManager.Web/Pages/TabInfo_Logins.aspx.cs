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

public partial class Pages_TabInfo_Logins : System.Web.UI.Page
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
        grid1.PageSize = 20;
        grid1.AllowFiltering = false;
        grid1.AllowSorting = false;
        grid1.AllowManualPaging = false;
        grid1.ShowColumnsFooter = false;
        grid1.ShowFooter = false;
        grid1.ShowTotalNumberOfPages = false;

        grid1.FolderStyle = "styles/grid/premiere_blue";
        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        Column oColi = new Column();
        oColi.ID = "ID";
        oColi.DataField = "Id";
        oColi.Visible = false;

        Column oCol1 = new Column();
        oCol1.ID = "Name";
        oCol1.DataField = "DisplayName";
        oCol1.HeaderText = "Name";
        oCol1.Visible = true;
        oCol1.ReadOnly = true;
        oCol1.Width = "180";
        oCol1.Wrap = true;

        Column oCol2 = new Column();
        oCol2.ID = "LoginDate";
        oCol2.DataField = "LoginDate";
        oCol2.HeaderText = "Last Login";
        oCol2.Visible = true;
        oCol2.ReadOnly = true;
        oCol2.Width = "180";
        oCol2.Wrap = true;
        oCol2.DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}";

        Column oCol3 = new Column();
        oCol3.ID = "UpdateDate";
        oCol3.DataField = "UpdateDate";
        oCol3.HeaderText = "Last Update";
        oCol3.Visible = true;
        oCol3.ReadOnly = true;
        oCol3.Width = "180";
        oCol3.Wrap = true;
        oCol3.DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}";
        
        grid1.Columns.Add(oColi);
        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol3);

        phGrid1.Controls.Add(grid1);

        if (!Page.IsPostBack)
        {
            BindGrid();
        }
    }

    void RebindGrid(object sender, EventArgs e) { BindGrid(); }
    void BindGrid()
    {

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        DateTimeOffset now = DateTimeOffset.UtcNow;
        TimeZoneInfo _server = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone.StandardName);
        TimeZoneInfo _here = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        TimeSpan difference = _here.GetUtcOffset(now) - _server.GetUtcOffset(now);
        ssql = String.Format("SELECT e.DisplayName, " +
                             "      (SELECT TOP 1 DateAdd(hour,{2},createdate) FROM _auditt a WHERE a.[Table] = 'LOGIN' AND a.[Field]='Id' AND LEN(a.NewValue)=0 AND a.[Key]=e.Id ORDER BY createdate DESC) LoginDate, " +
                             "      (SELECT TOP 1 DateAdd(hour,{2},updatedate) FROM Lead_Flat lf WHERE lf.AssignedToId = e.Id ORDER BY updatedate DESC) UpdateDate " +
                             "  FROM Employee e, EmployeeHirarchy eh " +
                             " WHERE e.Id = eh.SubEmployeeId " +
                             "   AND eh.EmployeeId = '{0}' " +
                             "   AND e.Id != '{1}' " +
                             "   AND e.isactive = 1 " +
                             " ORDER BY e.DisplayName ", Session["guser"].ToString(), Guid.Empty, difference.Hours);
        DataTable _dtLeads = _sql.GetTable(ssql);

        // txTimeZone.Text = String.Format("{0} {1}",TimeZone.CurrentTimeZone.StandardName,TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));

        grid1.DataSource = _dtLeads;
        grid1.DataBind();

        _sql.Close();
    }
}