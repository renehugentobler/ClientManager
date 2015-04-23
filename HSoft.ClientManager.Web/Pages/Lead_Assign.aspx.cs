using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using HSoft.SQL;

using Obout.ComboBox;
using Obout.Grid;
using OboutInc.Calendar2;
using Obout.Interface;

public partial class Pages_Lead_Assign : System.Web.UI.Page
{

    static DataTable _dtEmployee = new DataTable();
    static DataTable _dtSalespeople = new DataTable();
    static DataTable _dtPriority = new DataTable();
    OboutInc.Calendar2.Calendar cal1 = new OboutInc.Calendar2.Calendar();
    OboutTextBox oTextBox = new OboutTextBox();

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }
    
    protected void RebindGrid(object sender, EventArgs e)
	{
		CreateGrid();
	}

    protected void Page_Load(object sender, EventArgs e)
	{		
	    CreateGrid();			
	}

    protected void CreateGrid()
    {
        // for testing only
        if (Session["guser"] == null) { Session["guser"] = "7d5aa961-5478-4fa1-b5db-d6a2071ed834"; }
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        String ssql = String.Empty;

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        if (_dtEmployee.Rows.Count == 0)
        {
            ssql = "SELECT * FROM Employee WHERE isdeleted = 0";
            _dtEmployee = _sql.GetTable(ssql);
        }

        if (_dtSalespeople.Rows.Count == 0)
        {
            ssql = "SELECT * FROM Employee WHERE isSales=1 AND isdeleted = 0 ORDER BY DisplayName ASC";
            _dtSalespeople = _sql.GetTable(ssql);
        }

        if (_dtPriority.Rows.Count == 0)
        {
            ssql = "SELECT * FROM _LeadPriority WHERE isdeleted = 0 ORDER BY Sequence ASC";
            _dtPriority = _sql.GetTable(ssql);
        }

        sdsLead_Flat.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;
//        sdsLead_Flat.SelectCommand = String.Format("SELECT * FROM Lead_Flat WHERE AssignedToId = '{0}'", Session["guser"].ToString());
        String sassigned = Session["ghirarchy"].ToString();
        sassigned = String.Format("{0}{1}",sassigned.Replace(",","','"),Guid.Empty);
        sdsLead_Flat.SelectCommand = String.Format("SELECT * " +
                                                   "  FROM Lead_Flat " +
                                                   " WHERE AssignedToId IN ('{0}') ORDER BY EntryDate DESC ", sassigned);

        sdsPriority.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;
        sdsEmployee.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;

        List<ComboBox> _listComboBox = new List<ComboBox>();
        _listComboBox.Add(new ComboBox());

        _listComboBox[0].ID = String.Format("Header");
        _listComboBox[0].Width = Unit.Pixel(200);
        _listComboBox[0].Height = Unit.Pixel(15);
        _listComboBox[0].Mode = ComboBoxMode.TextBox;
        _listComboBox[0].Enabled = false;
        _listComboBox[0].Items.Add(new ComboBoxItem("Lead Assignment"));
        _listComboBox[0].SelectedIndex = 0;
        HeadingTemplate1.Container.Controls.Add(_listComboBox[0]);
        foreach (String sguid in Session["ghirarchy"].ToString().Split(','))
        {
            if (sguid.Length != 0)
            {
                _listComboBox.Add(new ComboBox());
                _listComboBox[_listComboBox.Count-1].ID = String.Format("History{0}", sguid);
                _listComboBox[_listComboBox.Count - 1].Width = Unit.Pixel(120);
                _listComboBox[_listComboBox.Count - 1].Height = Unit.Pixel(15);
                _listComboBox[_listComboBox.Count - 1].Mode = ComboBoxMode.TextBox;
                _listComboBox[_listComboBox.Count - 1].Enabled = false;
                _listComboBox[_listComboBox.Count - 1].Items.Add(new ComboBoxItem(_dtEmployee.Select(String.Format("Id='{0}'", sguid))[0]["DisplayName"].ToString()));
                _listComboBox[_listComboBox.Count - 1].SelectedIndex = 0;

                HeadingTemplate1.Container.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
            }
        }

        grid1.Height = int.Parse(Session["wy"].ToString()) - 20;

        // grid filter
        GridFilteringSettings gfs = new GridFilteringSettings();
        gfs.FilterPosition = GridFilterPosition.Top;
        gfs.FilterLinksPosition = GridElementPosition.Top;
        //        gfs.InitialState = GridFilterState.Visible;
        gfs.InitialState = GridFilterState.Hidden;
        grid1.FilteringSettings = gfs;
        grid1.AllowFiltering = true;

        GridRuntimeTemplate tplDatePicker = new GridRuntimeTemplate();
        tplDatePicker.ID = "tplDatePicker";
        tplDatePicker.Template = new Obout.Grid.RuntimeTemplate();
        tplDatePicker.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateDatePickerTemplate);
        tplDatePicker.ControlID = "txtlaterDate";
        tplDatePicker.ControlPropertyName = "value";
        tplDatePicker.UseQuotes = true;

        // adding the templates to the Templates collection
        grid1.Templates.Add(tplDatePicker);

    }

    protected void ComboBox1_LoadingItems(object sender, ComboBoxLoadingItemsEventArgs e)
    {
        // Getting the countries

        // Looping through the items and adding them to the "Items" collection of the ComboBox
        foreach (DataRow dr in _dtSalespeople.Rows)
        {
            (sender as ComboBox).Items.Add(new ComboBoxItem(dr["DisplayName"].ToString(), dr["DisplayName"].ToString()));
        }

        e.ItemsLoadedCount = _dtSalespeople.Rows.Count;
        e.ItemsCount = _dtSalespeople.Rows.Count;
    }

    protected void ComboBox2_LoadingItems(object sender, ComboBoxLoadingItemsEventArgs e)
    {
        // Getting the countries

        // Looping through the items and adding them to the "Items" collection of the ComboBox
        foreach (DataRow dr in _dtPriority.Rows)
        {
            (sender as ComboBox).Items.Add(new ComboBoxItem(dr["Name"].ToString(), dr["Name"].ToString()));
        }

        e.ItemsLoadedCount = _dtPriority.Rows.Count;
        e.ItemsCount = _dtPriority.Rows.Count;
    }

    public void CreateDatePickerTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPlaceHolder = new PlaceHolder();
        e.Container.Controls.Add(oPlaceHolder);
        oPlaceHolder.DataBinding += new EventHandler(DataBindDatePickerTemplate);
    }

    protected void DataBindDatePickerTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPlaceHolder = sender as PlaceHolder;
        Obout.Grid.TemplateContainer oContainer = oPlaceHolder.NamingContainer as Obout.Grid.TemplateContainer;

        Table oTable = new Table();
        oTable.CellPadding = 0;
        oTable.CellSpacing = 0;
        oTable.Attributes["width"] = "100%";

        TableRow oRow = new TableRow();

        TableCell oCell1 = new TableCell();
        TableCell oCell2 = new TableCell();
        oCell2.Attributes["width"] = "30";

        oTextBox = new OboutTextBox();
        oTextBox.ID = "txtLaterDate";
        oTextBox.FolderStyle = "styles/premiere_blue/interface/OboutTextBox";
        oTextBox.Width = Unit.Percentage(100);

        oCell1.Controls.Add(oTextBox);

        cal1.ID = "cal1";
        cal1.StyleFolder = "../calendar/styles/default";
        cal1.DatePickerMode = true;
        cal1.ShowYearSelector = false;
        cal1.DatePickerImagePath = "../calendar/styles/icon2.gif";

        oCell2.Controls.Add(cal1);

        oRow.Cells.Add(oCell1);
        oRow.Cells.Add(oCell2);

        oTable.Rows.Add(oRow);

        oPlaceHolder.Controls.Add(oTable);

    }


    public void OnItemUpdated(object sender, GridRecordEventArgs e)
    {
        String ssql = String.Empty;

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        String sPriorityId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadPriority WHERE (Name='{0}' AND isdeleted=0)", e.Record["Priority"])).ToString();
        // String sStatusId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadStatus WHERE (Name='{0}' AND isdeleted=0)", e.Record["Status"])).ToString();
        String sAssignedToId = _sql.ExecuteScalar(String.Format("SELECT Id FROM Employee WHERE (DisplayName='{0}' AND isdeleted=0)", e.Record["AssignedTo"])).ToString();

        ssql = String.Format("UPDATE [Lead_Flat] " +
                             "   SET CallLaterDate='{1}', " +
                             "       AssignedTo='{2}', " +
                             "       Priority='{3}', " +
                             "       AssignedToId='{4}', " +
                             "       PriorityId='{5}' " +
                             " WHERE Id = '{0}' " +
                             "", e.Record["Id"], e.Record["CallLaterDate"]==null?"null":String.Format("{0:d}",e.Record["CallLaterDate"]), e.Record["AssignedTo"], e.Record["Priority"], sAssignedToId, sPriorityId);

        _sql.ExecuteScalar(ssql);

    }

}