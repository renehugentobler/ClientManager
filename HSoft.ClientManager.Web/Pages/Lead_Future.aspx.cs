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

using System.Net.Mail;

using HSoft.SQL;

using Obout.ComboBox;
using Obout.Grid;

using HSoft.ClientManager.Web;

public partial class Pages_Lead_Future : System.Web.UI.Page
{
    Grid grid1 = new Grid();
    static DataTable _dtSalesEmployee = new DataTable();

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }

    void Page_load(object sender, EventArgs e)		
	{	

        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        if (_dtSalesEmployee.Rows.Count == 0)
        {
            String ssql = String.Empty;
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            ssql = "SELECT * FROM Employee WHERE isdeleted = 0 AND isSales = 1 AND isactive = 1 AND Id!='00000000-0000-0000-0000-000000000000'";
            _dtSalesEmployee = _sql.GetTable(ssql);

            _sql.Close();
        }


        grid1.ID = "grid1";
		grid1.CallbackMode = true;
		grid1.Serialize = true;
		grid1.AutoGenerateColumns = false;
        
        grid1.AllowAddingRecords = false;
        grid1.AllowRecordSelection = false;
        grid1.ShowLoadingMessage = false;

        grid1.PageSize = -1;
        grid1.Height = int.Parse(Session["wy"].ToString()) - 40;
        grid1.EnableRecordHover = true;
        grid1.AllowFiltering = false;
        grid1.AllowPaging = false;
        grid1.AllowSorting = false;
        grid1.AllowPageSizeSelection = false;
            
        grid1.FolderStyle = "styles/grid/premiere_blue";

        // setting the event handlers
		grid1.UpdateCommand += new Obout.Grid.Grid.EventHandler(UpdateRecord);
		grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);    
    
		// creating the Templates
        GridRuntimeTemplate TemplateAssignedTo = new GridRuntimeTemplate();
        TemplateAssignedTo.ID = "tplAssignedTo";
        TemplateAssignedTo.Template = new Obout.Grid.RuntimeTemplate();
        TemplateAssignedTo.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateAssignedToTemplate);
		
        grid1.Templates.Add(TemplateAssignedTo);
        
        Column oColi = new Column();
        oColi.ID = "ID";
        oColi.DataField = "Id";
        oColi.Visible = false;

        Column oCol1 = new Column();
        oCol1.ID = "AssignToId";
        oCol1.DataField = "AssignToId";
        oCol1.Visible = false;

        Column oCol2 = new Column();
        oCol2.ID = "DisplayName";
        oCol2.DataField = "DisplayName";
        oCol2.HeaderText = "Assigned To";
        oCol2.Visible = true;
        oCol2.ReadOnly = false;
        oCol2.TemplateSettings.TemplateId = "tplAssignedTo";

        Column oCol3 = new Column();
        oCol3.ID = "Customer";
        oCol3.DataField = "Customer";
        oCol3.HeaderText = "Lead #";
        oCol3.Visible = true;
        oCol3.ReadOnly = true;
        oCol3.Width = "60";
    
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

    public void CreateAssignedToTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
	{
		Literal oLiteral = new Literal();
		e.Container.Controls.Add(oLiteral);
		oLiteral.DataBinding += new EventHandler(DataBindEditAssignedToTemplate);
	}
    protected void DataBindEditAssignedToTemplate(Object sender, EventArgs e)
	{
		Literal oLiteral = sender as Literal;
		Obout.Grid.TemplateContainer oContainer = oLiteral.NamingContainer as Obout.Grid.TemplateContainer;

        if (_dtSalesEmployee.Rows.Count == 0)
        {
            String ssql = String.Empty;
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            ssql = "SELECT * FROM Employee WHERE isdeleted = 0 AND isSales = 1 AND isactive = 1 AND Id!='00000000-0000-0000-0000-000000000000'";
            _dtSalesEmployee = _sql.GetTable(ssql);

            _sql.Close();
        }

        oLiteral.Text = "<select class=\"ob_gEC\" onchange=\"updateAssignedTo(this.value, " + oContainer.PageRecordIndex.ToString() + ")\">";
        foreach (DataRow dr in _dtSalesEmployee.Rows)
        {
            oLiteral.Text = String.Format("{0}<option value=\"{1}\" {3}>{2}</option>", oLiteral.Text, dr["Id"], dr["DisplayName"], oContainer.Value == dr["DisplayName"].ToString() ? " selected='selected'" : "");

        }
        oLiteral.Text = String.Format("{0}{1}",oLiteral.Text,"</select>");
	}

    void BindGrid()
    {
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = "SELECT TOP 100";

        ssql = String.Format("{0} * " +
                             "  FROM _Leadassign la, Employee e" +
                             " WHERE la.isdeleted=0 " +
                             "   AND e.isdeleted=0 " +
                             "   AND e.isactive = 1 " +
                             "   AND e.isSales= 1 " +
                             "   AND e.Id = la.AssignedToId " +
                             "   AND la.Customer > (SELECT MAX(Customer) FROM Lead_Flat) " +
                             " ORDER BY Customer ", ssql);
        DataTable _dtFutureLeads = _sql.GetTable(ssql);

        grid1.DataSource = _dtFutureLeads;
        grid1.DataBind();

        _sql.Close();
    }

    void UpdateRecord(object sender, GridRecordEventArgs e)
    {
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = String.Format("UPDATE la "+
                             "  SET la.AssignedToId = '{0}' " +
                             " FROM _Leadassign la " +
                             " WHERE Id = '{1}'", e.Record["AssignedToId"], e.Record["Id"]);
        _sql.Execute(ssql);
        _sql.Close();
    }

    void RebindGrid(object sender, EventArgs e) { BindGrid(); }

}


