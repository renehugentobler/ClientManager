using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Obout.ComboBox;
using Obout.Grid;
using OboutInc.Window;
using Obout.SuperForm;
using OboutInc.Calendar2;
using Obout.Interface;

using HSoft.SQL;

using HSoft.ClientManager.Web;

public partial class LeadSales : System.Web.UI.Page
{
    private Grid grid1;
    private Window window1;
    private SuperForm SuperForm1;

    OboutTextBox oTextBox = new OboutTextBox();
    OboutInc.Calendar2.Calendar cal1 = new OboutInc.Calendar2.Calendar();

    static DataTable _dtEmployee = new DataTable();
//    static DataTable _dtSalesEmployee = new DataTable();
    static DataTable _dtFilterPriority = new DataTable();
//    static DataTable _dtPriority = new DataTable();
    static DataTable _dtStatus = new DataTable();
    static DataTable _dtAssignedTo = new DataTable();

    void Page_load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); } 
        
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        String sassigned = Session["ghirarchy"].ToString();
        sassigned = String.Format("{0}{1}", sassigned.Replace(",", "','"), Guid.Empty);
        sdsLeadFlat.SelectCommand = String.Format("SELECT TOP 100 " +
                                                  " l.Id " +
            // " ConstantContactID," +
            // " Customer," +
                                                  " ,l.Name" +
                                                  " ,l.[EMail]" +
                                                  " ,l.Phone" +
                                                  " ,l.EntryDate" +
                                                  " ,l.CallLaterDate" +
            // " SourceId," +
            // " Source," +
                                                  " ,l.PriorityId" +
                                                  " ,l.Priority" +
                                                  " ,l.StatusId" +
                                                  " ,l.Status" +
                                                  " ,l.MsgHistory" +
                                                  " ,l.LeadNote" +
                                                  " ,l.SalesNote" +
                                                  " ,l.AssignedToId" +
                                                  " ,l.AssignedTo" +
                                                  " ,l.TimeZone" +
            // " sourcePage," +
                                                  " ,referrerUrl" +
            // " createdby," +
            // " createdate," +
            // " updatedby," +
            // " updatedate," +
            // " isdeleted " +
                                                  "  FROM _LeadPriority p, Lead_Flat l " +
                                                  " WHERE AssignedToId IN ('{0}') " +
                                                  "   AND l.PriorityId = p.Id " +
                                                  "   AND ( " +
                                                  "         p.IsLead = 1 " +
                                                  "         OR (p.Id='F3DC2498-6F4F-449E-813C-EFDA32A9D24A' AND '{1}' IN ('DCDB22C2-65F4-46E4-91D1-CC123F83DCE2','0BA4012E-5541-4A76-92BB-C7122344DC3A','7D5AA961-5478-4FA1-B5DB-D6A2071ED834')) " +
                                                  "       ) " +
                                                  "   AND l.isdeleted = 0 " +
                                                  "   AND l.CallLaterDate < '{2:d}' " +
                                                  " ORDER BY EntryDate DESC", sassigned, Session["guser"], DateTime.Now.AddDays(14));

        grid1 = new Grid();
        grid1.ID = "Grid1";
        grid1.DataSourceID = "sdsLeadFlat";
        grid1.Serialize = false;
        grid1.AutoGenerateColumns = false;
        grid1.CallbackMode = false;
        grid1.ClientSideEvents.OnBeforeClientEdit = "Grid1_ClientEdit";
        grid1.ClientSideEvents.ExposeSender = true;
        grid1.FolderStyle = "styles/grid/premiere_blue";
        grid1.PageSize = -1;
        grid1.AllowPageSizeSelection = false;
        grid1.AllowPaging = false;
        grid1.AllowAddingRecords = false;
        grid1.EnableRecordHover = true;
        grid1.AllowFiltering = false;
        grid1.AllowSorting = true;
        grid1.AllowColumnReordering = false;
        grid1.AllowColumnResizing = false;

//        grid1.ClientSideEvents.OnClientDblClick = "OnClientDblClick";
        grid1.ClientSideEvents.OnClientSelect = "onClientSelect";

        // creating the columns
        Column oCol0 = new Column();
        oCol0.DataField = "";
        oCol0.ID = "Column0";
        oCol0.HeaderText = "";
        oCol0.Width = "30";
        oCol0.AllowEdit = true;
        oCol0.AllowDelete = false;
        oCol0.Visible = false;

        Column oCol1 = new Column();
        oCol1.ID = "Column1";
        oCol1.DataField = "Id";
        oCol1.ReadOnly = true;
        oCol1.HeaderText = "ID";
        oCol1.Width = "150";
        oCol1.Visible = false;

        Column oCol2 = new Column();
        oCol2.ID = "Column2";
        oCol2.DataField = "Name";
        oCol2.HeaderText = "Name";
        oCol2.Width = "120";
        oCol2.AllowSorting=true;
        oCol2.Wrap=true;
        oCol2.AllowFilter = true;
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol3 = new Column();
        oCol3.ID = "Column3";
        oCol3.DataField = "EMail";
        oCol3.HeaderText = "EMail";
        oCol3.Width = "180";
        oCol3.AllowSorting = true;
        oCol3.Wrap = true;
        oCol3.AllowFilter = true;
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol4 = new Column();
        oCol4.ID = "Column4";
        oCol4.DataField = "Phone";
        oCol4.HeaderText = "Phone";
        oCol4.Width = "80";
        oCol4.AllowSorting = true;
        oCol4.Wrap = false;
        oCol4.AllowFilter = true;
        oCol4.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol4.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol4.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol4.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol4.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol5 = new Column();
        oCol5.ID = "Column5";
        oCol5.DataField = "TimeZone";
        oCol5.HeaderText = "Zone";
        oCol5.Width = "35";
        oCol5.AllowSorting = true;
        oCol5.Wrap = false;
        oCol5.AllowFilter = true;
        oCol5.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol5.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));

        Column oCol6 = new Column();
        oCol6.ID = "Column6";
        oCol6.DataField = "EntryDate";
        oCol6.HeaderText = "Entry Date";
        oCol6.Width = "130";
        oCol6.AllowSorting = true;
        oCol6.Wrap = false;
        oCol6.AllowFilter = true;
        oCol6.DataFormatString = "{0:MM/dd/yyyy hh:mm tt}";
        oCol6.ApplyFormatInEditMode = false;

        Column oCol7 = new Column();
        oCol7.ID = "Column7";
        oCol7.DataField = "CallLaterDate";
        oCol7.HeaderText = "Call Later";
        oCol7.Width = "70";
        oCol7.AllowSorting = true;
        oCol7.Wrap = false;
        oCol7.AllowFilter = true;
        oCol7.DataFormatString = "{0:MM/dd/yyyy}";
        oCol7.NullDisplayText = "missing!";
        oCol7.ApplyFormatInEditMode = true;

        Column oCol9 = new Column();
        oCol9.ID = "Column9";
        oCol9.DataField = "Priority";
        oCol9.HeaderText = "Priority";
        oCol9.Width = "80";
        oCol9.AllowSorting = true;
        oCol9.Wrap = false;
        oCol9.AllowFilter = true;
        oCol9.ShowFilterCriterias = false;
        oCol9.TemplateSettings.FilterTemplateId = "TemplatePriority";

        Column oCol10 = new Column();
        oCol10.ID = "Column10";
        oCol10.DataField = "AssignedTo";
        oCol10.HeaderText = "Assigned To";
        oCol10.Width = "130";
        oCol10.AllowSorting = true;
        oCol10.Wrap = false;
        oCol10.AllowFilter = true;
        oCol10.ShowFilterCriterias = false;
        oCol10.TemplateSettings.FilterTemplateId = "TemplateAssignedTo";

        Column oCol12 = new Column();
        oCol12.ID = "Column12";
        oCol12.DataField = "LeadNote";
        oCol12.HeaderText = "Lead Note";
        oCol12.Width = "300";
        oCol12.AllowSorting = false;
        oCol12.Wrap = true;
        oCol12.AllowFilter = true;
        oCol12.ParseHTML = true;
        oCol12.HtmlEncode = true;
        oCol12.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol12.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));

        Column oCol13 = new Column();
        oCol13.ID = "Column13";
        oCol13.DataField = "SalesNote";
        oCol13.HeaderText = "Sales Note";
        oCol13.Width = "300";
        oCol13.AllowSorting = false;
        oCol13.Wrap = true;
        oCol13.AllowFilter = true;
        oCol13.ParseHTML = true;
        oCol13.HtmlEncode = true;
        oCol13.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol13.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));

        Column oCol14 = new Column();
        oCol14.ID = "Column14";
        oCol14.DataField = "Status";
        oCol14.HeaderText = "Status";
        oCol14.Visible = true;
        oCol14.ReadOnly = true;
        oCol14.Width = "90";
        oCol14.AllowSorting = true;
        oCol14.AllowFilter = true;
        oCol14.ShowFilterCriterias = false;
        oCol14.TemplateSettings.FilterTemplateId = "TemplateStatus";

        Column oCol99 = new Column();
        oCol99.ID = "Column99";
        oCol99.DataField = "referrerUrl";
        oCol99.HeaderText = "Referrer";
        oCol99.Visible = true;
        oCol99.ReadOnly = true;
        oCol99.Width = "180";
        oCol14.AllowSorting = true;
        oCol14.AllowFilter = true;
        oCol14.ShowFilterCriterias = false;
        oCol12.Wrap = true;
        oCol12.ParseHTML = true;
        oCol12.HtmlEncode = true;
        
        // add the columns to the Columns collection of the grid

        grid1.Columns.Add(oCol0);
        grid1.Columns.Add(oCol1);

        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol4);
        grid1.Columns.Add(oCol5);
        grid1.Columns.Add(oCol7);
        grid1.Columns.Add(oCol9);

        grid1.Columns.Add(oCol13);
        grid1.Columns.Add(oCol12);
        grid1.Columns.Add(oCol14);

        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol6);
        grid1.Columns.Add(oCol10);

        grid1.Columns.Add(oCol99);

        GridRuntimeTemplate HeadingTemplate = new GridRuntimeTemplate();
        HeadingTemplate.ID = "HeadingTemplate1";
        HeadingTemplate.Template = new Obout.Grid.RuntimeTemplate();
        HeadingTemplate.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateHeadingTemplate);

        grid1.Templates.Add(HeadingTemplate);
        grid1.TemplateSettings.HeadingTemplateId = "HeadingTemplate1";

        // add the grid to the controls collection of the PlaceHolder        
        Grid1Container.Controls.Add(grid1);

        window1 = new Window();
        window1.ID = "Window1";
        window1.IsModal = true;
        window1.Status = "";
        window1.RelativeElementID = "WindowPositionHelper";
        window1.Top = 50;
        window1.Left = 100;
        window1.Height = 510;
        window1.Width = 1026;
        window1.VisibleOnLoad = false;
        window1.ShowCloseButton = false;
        window1.StyleFolder = "styles/wdstyles/blue";
        window1.Title = "Leads Management";

        SuperForm1Container.Controls.Add(window1);

        Literal hiddenInput = new Literal();
        hiddenInput.Text = "<input type=\"hidden\" id=\"Id\" />";
        window1.Controls.Add(hiddenInput);

        PlaceHolder superFormPlaceHolder = new PlaceHolder();
        window1.Controls.Add(superFormPlaceHolder);

        Literal div1 = new Literal();
        div1.Text = "<div class=\"super-form\" >";
        superFormPlaceHolder.Controls.Add(div1);

        SuperForm1 = new SuperForm();
        SuperForm1.ID = "SuperForm1";
        SuperForm1.Width = 1000;
        SuperForm1.AutoGenerateRows = false;
        SuperForm1.AutoGenerateInsertButton = false;
        SuperForm1.AutoGenerateEditButton = false;
        SuperForm1.AutoGenerateDeleteButton = false;
        SuperForm1.DataKeyNames = new string[] { "Id" };
        SuperForm1.DefaultMode = DetailsViewMode.Insert;

        Obout.SuperForm.BoundField field1 = new Obout.SuperForm.BoundField();
        field1.DataField = "Name";
        field1.HeaderText = "Name";
        field1.FieldSetID = "FieldSet1";
        field1.AllowEdit = false;
        field1.Enabled = false;

        Obout.SuperForm.BoundField field2 = new Obout.SuperForm.BoundField();
        field2.DataField = "EMail";
        field2.HeaderText = "EMail";
        field2.FieldSetID = "FieldSet1";
        field2.AllowEdit = false;
        field2.Enabled = false;

        Obout.SuperForm.BoundField field3 = new Obout.SuperForm.BoundField();
        field3.DataField = "Phone";
        field3.HeaderText = "Phone";
        field3.FieldSetID = "FieldSet1";
        field3.AllowEdit = false;
        field3.Enabled = false;

        Obout.SuperForm.BoundField field4 = new Obout.SuperForm.BoundField();
        field4.DataField = "TimeZone";
        field4.HeaderText = "TimeZone";
        field4.FieldSetID = "FieldSet1";
        field4.AllowEdit = false;
        field4.Enabled = false;

        Obout.SuperForm.DateField field5 = new Obout.SuperForm.DateField();
        field5.DataField = "EntryDate";
        field5.HeaderText = "Entry Date";
        field5.FieldSetID = "FieldSet1";
        field5.AllowEdit = false;
        field5.Enabled = false;
        field5.DataFormatString = "{0:MM/dd/yyyy hh:mm tt}";
        field5.ApplyFormatInEditMode = true;
        field5.NullDisplayText = "missing!";

        Obout.SuperForm.DateField field6 = new Obout.SuperForm.DateField();
        field6.DataField = "CallLaterDate";
        field6.HeaderText = "Call Later";
        field6.FieldSetID = "FieldSet1";
        field6.AllowEdit = true;
        field6.Enabled = true;
        field6.DataFormatString = "{0:MM/dd/yyyy}";
        field6.ApplyFormatInEditMode = true;
        field6.NullDisplayText = "missing!";

        Obout.SuperForm.DropDownListField field8 = new Obout.SuperForm.DropDownListField();
        field8.DataField = "Priority";
        field8.HeaderText = "Priority";
        field8.FieldSetID = "FieldSet1";
        field8.AllowEdit = true;
        field8.Enabled = true;
        field8.DataSourceID = "sdsPriority";

        Obout.SuperForm.DropDownListField field9 = new Obout.SuperForm.DropDownListField();
        field9.DataField = "AssignedTo";
        field9.HeaderText = "Assigned To";
        field9.FieldSetID = "FieldSet1";
        field9.AllowEdit = true;
        field9.Enabled = true;
        field9.DataSourceID = "sdsSalesPeople";

        Obout.SuperForm.MultiLineField field11 = new Obout.SuperForm.MultiLineField();
        field11.DataField = "LeadNote";
        field11.HeaderText = "";
        field11.FieldSetID = "FieldSet2";
        field11.AllowEdit = false;
        field11.Enabled = false;
        field11.HeaderStyle.Width = 1;

        Obout.SuperForm.MultiLineField field12 = new Obout.SuperForm.MultiLineField();
        field12.DataField = "SalesNote";
        field12.HeaderText = "";
        field12.FieldSetID = "FieldSet2";
        field12.AllowEdit = true;
        field12.Enabled = true;
        field12.HeaderStyle.Width = 1;

        Obout.SuperForm.TemplateField field0 = new Obout.SuperForm.TemplateField();
        field0.FieldSetID = "FieldSet4";
        field0.EditItemTemplate = new AddButtonsItemTemplate();

        Obout.SuperForm.FieldSetRow fieldSetRow1 = new Obout.SuperForm.FieldSetRow();
        Obout.SuperForm.FieldSet fieldSet1 = new Obout.SuperForm.FieldSet();
        fieldSet1.ID = "FieldSet1";
        fieldSet1.Title = "Information";
        fieldSet1.CssClass = "FieldSet1";

        Obout.SuperForm.FieldSet fieldSet2 = new Obout.SuperForm.FieldSet();
        fieldSet2.ID = "FieldSet2";
        fieldSet2.Title = "Notes";
        
        fieldSetRow1.Items.Add(fieldSet1);
        fieldSetRow1.Items.Add(fieldSet2);

        Obout.SuperForm.FieldSetRow fieldSetRow2 = new Obout.SuperForm.FieldSetRow();
        Obout.SuperForm.FieldSet fieldSet4 = new Obout.SuperForm.FieldSet();
        fieldSet4.ID = "FieldSet4";
        fieldSet4.ColumnSpan = 2;
        fieldSet4.CssClass = "command-row";
        fieldSetRow2.Items.Add(fieldSet4);
        SuperForm1.FieldSets.Add(fieldSetRow1);
        SuperForm1.FieldSets.Add(fieldSetRow2);

        SuperForm1.Fields.Add(field1);
        SuperForm1.Fields.Add(field2);
        SuperForm1.Fields.Add(field3);
        SuperForm1.Fields.Add(field4);
        SuperForm1.Fields.Add(field5);
        SuperForm1.Fields.Add(field6);
        SuperForm1.Fields.Add(field8);
        SuperForm1.Fields.Add(field9);

        SuperForm1.Fields.Add(field11);
        SuperForm1.Fields.Add(field12);
 
        SuperForm1.Fields.Add(field0);

        superFormPlaceHolder.Controls.Add(SuperForm1);
        Literal div2 = new Literal();
        div2.Text = "</div>";
        superFormPlaceHolder.Controls.Add(div2);

        window1.Controls.Add(hiddenInput);

        if (!Page.IsPostBack)
        {
            BindGrid();
        }

    }

    void BindGrid()
    {

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        if (_dtAssignedTo.Rows.Count == 0)
        {
            ssql = "SELECT DisplayName FROM Employee WHERE isdeleted = 0 AND isSales = 1 AND isactive = 1 AND Id!='00000000-0000-0000-0000-000000000000' ";
            _dtAssignedTo = _sql.GetTable(ssql);
        }

        if (_dtEmployee.Rows.Count == 0)
        {
            ssql = "SELECT Id,DisplayName FROM Employee WHERE isdeleted = 0 ";
            _dtEmployee = _sql.GetTable(ssql);
        }

        if (_dtFilterPriority.Rows.Count == 0)
        {
            ssql = "SELECT Name FROM _LeadPriority WHERE isdeleted = 0"; 
            _dtFilterPriority = _sql.GetTable(ssql);
        }

        if (_dtStatus.Rows.Count == 0)
        {
            ssql = "SELECT Name FROM _LeadStatus WHERE isdeleted = 0";
            _dtStatus = _sql.GetTable(ssql);
        }

        _sql.Close();
    
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
        _listComboBox[0].Items.Add(new ComboBoxItem("Lead Sales 1.0.13a"));
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
                _listComboBox[_listComboBox.Count - 1].SelectedIndex = 0;

                cbName.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
                e.Container.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
            }
        }    
    }

    public class AddButtonsItemTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            PlaceHolder templatePlaceHolder = new PlaceHolder();
            container.Controls.Add(templatePlaceHolder);

            Obout.Interface.OboutButton button1 = new Obout.Interface.OboutButton();
            button1.ID = "OboutButton1";
            button1.Text = "Save";
            button1.OnClientClick = "saveChanges(); return false;";
            button1.Width = 100;

            Obout.Interface.OboutButton button2 = new Obout.Interface.OboutButton();
            button2.ID = "OboutButton2";
            button2.Text = "Cancel";
            button2.OnClientClick = "cancelChanges(); return false;";
            button2.Width = 100;

            Obout.Interface.OboutButton button3 = new Obout.Interface.OboutButton();
            button3.ID = "OboutButton3";
            button3.Text = "Send Mail";
            button3.OnClientClick = "email(); return false;";
            button3.Width = 100;

            Obout.Interface.OboutButton button4 = new Obout.Interface.OboutButton();
            button4.ID = "OboutButton4";
            button4.Text = "Mail Client";
            button4.OnClientClick = "openemail(); return false;";
            button4.Width = 100;

            templatePlaceHolder.Controls.Add(button1);
            templatePlaceHolder.Controls.Add(button2);
            templatePlaceHolder.Controls.Add(button3);
            templatePlaceHolder.Controls.Add(button4);
        
        }
    }

}
