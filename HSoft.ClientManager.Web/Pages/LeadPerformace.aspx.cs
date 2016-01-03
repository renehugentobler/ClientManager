using System;
using System.Collections.Generic;
using System.Data;
// using System.Linq;
// using System.Web;
// using System.Web.UI;
using System.Web.UI.WebControls;
// using System.Web.UI.HtmlControls;

using Obout.ComboBox;
using Obout.Grid;
//using OboutInc.Window;
//using Obout.SuperForm;
//using OboutInc.Calendar2;
//using Obout.Interface;


using HSoft.SQL;
using HSoft.ClientManager.Web;

// using System.Data.OleDb;
// using System.Data.SqlClient;

public partial class Pages_LeadPerformace : System.Web.UI.Page
{

    Grid grid1 = new Grid();

    static DataTable _dtEmployee = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        ssql = "SELECT * FROM Employee WHERE isdeleted = 0";
        _dtEmployee = _sql.GetTable(ssql);

        _sql.Close();

        String sassigned = Session["ghirarchy"].ToString();
        sassigned = String.Format("{0}{1}", sassigned.Replace(",", "','"), Guid.Empty);
        sdsLeadFlat.SelectCommand = String.Format("SELECT Lead_Flat.Id, " +
                                                  "       Lead_Flat.Name, " +
                                                  "       Lead_Flat.EMail, " +
                                                  "       Lead_Flat.Phone+'&nbsp;'+COALESCE(Lead_Flat.Timezone,'___') Phone, " +
                                                  "       Lead_Flat.EntryDate, " +
                                                  "       Lead_Flat.CallLaterDate, " +
                                                  "       Lead_Flat.Source, " +
                                                  "       Lead_Flat.Priority, " +
                                                  "       Lead_Flat.Status, " +
            //                                                  "       AssignedTo, " +
                                                  "       Employee.Short AssignedTo, " +
                                                  "       Lead_Flat.referrerUrl, " +
                                                  "       Lead_Flat.LeadNote, " +
                                                  "       Lead_Flat.SalesNote  " +
                                                  "       FROM Lead_Flat, Employee,_LeadPriority " +
                                                  " WHERE Lead_Flat.IsDeleted = 0 " +
//                                                  "   AND Lead_Flat.AssignedToId IN ('{0}') " +
                                                  "   AND Lead_Flat.PriorityId = _LeadPriority.Id " +
                                                  "   AND Lead_Flat.AssignedToId = Employee.Id " +
                                                  "   AND ( " +
                                                  "         _LeadPriority.IsLead = 1 " +
                                                  "         OR (_LeadPriority.Id='F3DC2498-6F4F-449E-813C-EFDA32A9D24A' AND '{1}' IN ('DCDB22C2-65F4-46E4-91D1-CC123F83DCE2','0BA4012E-5541-4A76-92BB-C7122344DC3A','7D5AA961-5478-4FA1-B5DB-D6A2071ED834')) " +
                                                  "       ) " +
                                                  " ORDER BY EntryDate DESC ", sassigned, Session["guser"]); ;
//        sdsLeadFlat.SelectCommand = "SELECT * FROM Lead_Flat WHERE IsDeleted = 0";

        grid1.ID = "grid1";
        grid1.CallbackMode = true;
        grid1.Serialize = true;

        grid1.AllowSorting = true;
        grid1.AllowAddingRecords = false;
        grid1.AllowColumnReordering = true;
        grid1.AllowColumnResizing = true;
        grid1.AllowDataAccessOnServer = true;
        grid1.AllowFiltering = true;
        grid1.AllowPaging = true;
        grid1.EnableRecordHover = true;
        grid1.PageSize = 100;
        grid1.ShowTotalNumberOfPages = false;
        grid1.AllowManualPaging = false;

        grid1.ScrollingSettings.FixedColumnsPosition = GridFixedColumnsPositionType.Left;
        grid1.ScrollingSettings.NumberOfFixedColumns = 4;
        grid1.ScrollingSettings.ScrollHeight = int.Parse(Session["wy"].ToString()) - 130;
        grid1.ScrollingSettings.ScrollWidth= int.Parse(Session["wx"].ToString()) - 190;

        grid1.DataSourceID = "sdsLeadFlat";
        grid1.EmbedFilterInSortExpression = true;
        grid1.AutoGenerateColumns = false;

        grid1.FolderStyle = "styles/grid/grand_gray";

        // creating the columns
        Column oCol0 = new Column();
        oCol0.DataField = "Id";
        oCol0.ID = "Id";
        oCol0.HeaderText = "";
        oCol0.Width = "0";
        oCol0.AllowEdit = false;
        oCol0.AllowDelete = false;
        oCol0.AllowFilter= false;
        oCol0.Visible = false;

        // creating the columns
        Column oCol1 = new Column();
        oCol1.DataField = "Name";
        oCol1.ReadOnly = true;
        oCol1.HeaderText = "Name";
        oCol1.Width = "140";
        oCol1.AllowSorting = true;
        oCol1.Wrap = false;
        oCol1.AllowFilter = true;

        Column oCol2 = new Column();
        oCol2.ID = "EMail";
        oCol2.DataField = "EMail";
        oCol2.HeaderText = "EMail";
        oCol2.Width = "180";
        oCol2.AllowSorting = true;
        oCol2.Wrap = false;
        oCol2.AllowFilter = true;

        Column oCol3 = new Column();
        oCol3.ID = "Phone";
        oCol3.DataField = "Phone";
        oCol3.HeaderText = "Phone";
        oCol3.Width = "140";
        oCol3.AllowSorting = false;
        oCol3.Wrap = false;
        oCol3.AllowFilter = true;
        oCol3.ParseHTML = true;

        Column oCol6 = new Column();
        oCol6.ID = "EntryDate";
        oCol6.DataField = "EntryDate";
        oCol6.HeaderText = "Entry Date";
        oCol6.Width = "150";
        oCol6.AllowSorting = true;
        oCol6.Wrap = false;
        oCol6.AllowFilter = false;
        oCol6.DataFormatString = "{0:MM/dd/yyyy hh:mm tt}";
        oCol6.ApplyFormatInEditMode = false;

        Column oCol7 = new Column();
        oCol7.ID = "CallLaterDate";
        oCol7.DataField = "CallLaterDate";
        oCol7.HeaderText = "Call Later";
        oCol7.Width = "90";
        oCol7.AllowSorting = true;
        oCol7.Wrap = false;
        oCol7.AllowFilter = false;
        oCol7.DataFormatString = "{0:MM/dd/yyyy}";
        oCol7.NullDisplayText = "missing!";
        oCol7.ApplyFormatInEditMode = true;

        Column oCol8 = new Column();
        oCol8.ID = "Source";
        oCol8.DataField = "Source";
        oCol8.HeaderText = "Source";
        oCol8.Width = "90";
        oCol8.AllowSorting = false;
        oCol8.Wrap = false;
        oCol8.AllowFilter = false;

        Column oCol9 = new Column();
        oCol9.ID = "Priority";
        oCol9.DataField = "Priority";
        oCol9.HeaderText = "Priority";
        oCol9.Width = "100";
        oCol9.AllowSorting = true;
        oCol9.Wrap = false;
        oCol9.AllowFilter = true;

        Column oCol10 = new Column();
        oCol10.ID = "AssignedTo";
        oCol10.DataField = "AssignedTo";
        oCol10.HeaderText = "To";
        oCol10.Width = "50";
        oCol10.AllowSorting = true;
        oCol10.Wrap = false;
        oCol10.AllowFilter = true;

        Column oCol12 = new Column();
        oCol12.ID = "LeadNote";
        oCol12.DataField = "LeadNote";
        oCol12.HeaderText = "Lead Note";
        oCol12.Width = "300";
        oCol12.AllowSorting = false;
        oCol12.Wrap = true;
        oCol12.AllowFilter = true;
        oCol12.ParseHTML = true;
        oCol12.HtmlEncode = true;

        Column oCol13 = new Column();
        oCol13.ID = "SalesNote";
        oCol13.DataField = "SalesNote";
        oCol13.HeaderText = "Sales Note";
        oCol13.Width = "300";
        oCol13.AllowSorting = false;
        oCol13.Wrap = true;
        oCol13.AllowFilter = true;
        oCol13.ParseHTML = true;
        oCol13.HtmlEncode = true;

        // add the columns to the Columns collection of the grid
        grid1.Columns.Add(oCol0);
        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol6);
        grid1.Columns.Add(oCol7);
        grid1.Columns.Add(oCol8);
        grid1.Columns.Add(oCol9);
        grid1.Columns.Add(oCol10);
        grid1.Columns.Add(oCol12);
        grid1.Columns.Add(oCol13);

        GridRuntimeTemplate HeadingTemplate = new GridRuntimeTemplate();
        HeadingTemplate.ID = "HeadingTemplate1";
        HeadingTemplate.Template = new Obout.Grid.RuntimeTemplate();
        HeadingTemplate.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateHeadingTemplate);

        grid1.Templates.Add(HeadingTemplate);
        grid1.TemplateSettings.HeadingTemplateId = "HeadingTemplate1";

        // add the grid to the controls collection of the PlaceHolder
        phGrid1.Controls.Add(grid1);
    
    }

    public void CreateHeadingTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        List<ComboBox> _listComboBox = new List<ComboBox>();
        _listComboBox.Add(new ComboBox());

        _listComboBox[0].ID = String.Format("Header");
        _listComboBox[0].Width = Unit.Pixel(150);
        _listComboBox[0].Height = Unit.Pixel(15);
        _listComboBox[0].Mode = ComboBoxMode.TextBox;
        _listComboBox[0].Enabled = false;
        _listComboBox[0].Items.Add(new ComboBoxItem("Lead Sales 1.1.00"));
        _listComboBox[0].SelectedIndex = 0;
        e.Container.Controls.Add(_listComboBox[0]);
        foreach (String sguid in Session["ghirarchy"].ToString().Split(','))
        {
            if (sguid.Length != 0)
            {
                _listComboBox.Add(new ComboBox());
                _listComboBox[_listComboBox.Count - 1].ID = String.Format("History{0}", sguid);
                _listComboBox[_listComboBox.Count - 1].Width = Unit.Pixel(120);
                _listComboBox[_listComboBox.Count - 1].Height = Unit.Pixel(15);
                _listComboBox[_listComboBox.Count - 1].Mode = ComboBoxMode.TextBox;
                _listComboBox[_listComboBox.Count - 1].Enabled = false;
                _listComboBox[_listComboBox.Count - 1].Items.Add(new ComboBoxItem(_dtEmployee.Select(String.Format("Id='{0}'", sguid))[0]["DisplayName"].ToString()));
                // _listComboBox[_listComboBox.Count - 1].Items.Add(new ComboBoxItem(sguid.ToString()));
                _listComboBox[_listComboBox.Count - 1].SelectedIndex = 0;

                cbName.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
                e.Container.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
            }
        }
    }


}