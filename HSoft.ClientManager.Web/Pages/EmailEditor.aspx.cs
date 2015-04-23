using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using HSoft.SQL;

using Obout.Ajax.UI.HTMLEditor;
using Obout.Grid;

public partial class Pages_EmailEditor : System.Web.UI.Page
{
    public void Page_load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CreateGrid();
        }

        EditorID.Value = ((Editor)(grid1.Templates[0].Container.FindControl("Editor"))).ClientID;
    }

    private DataTable _dtEmail = new DataTable();
    public void CreateGrid()
    {
        String ssql = String.Empty;

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = "SELECT * FROM Email WHERE isdeleted = 0";
        _dtEmail = _sql.GetTable(ssql);
        _sql.Close();

        grid1.DataSource = _dtEmail;
        grid1.DataBind();
    }

    public void DeleteRecord(object sender, GridRecordEventArgs e)
    {
    }

    public void UpdateRecord(object sender, GridRecordEventArgs e)
    {
    }

    public void InsertRecord(object sender, GridRecordEventArgs e)
    {
    }

    public void RebindGrid(object sender, EventArgs e)
    {
        CreateGrid();
    }
}