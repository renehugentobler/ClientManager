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

public partial class ltest : System.Web.UI.Page
{
    private Grid grid1;
    private Window window1;
    private SuperForm SuperForm1;

    OboutTextBox oTextBox = new OboutTextBox();
    OboutInc.Calendar2.Calendar cal1 = new OboutInc.Calendar2.Calendar();

    static DataTable _dtEmployee = new DataTable();
    static DataTable _dtSalesEmployee = new DataTable();
    static DataTable _dtFilterPriority = new DataTable();
    static DataTable _dtPriority = new DataTable();
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
        sdsLeadFlat.SelectCommand = String.Format("SELECT DISTINCT l.*, " +
                                                  " CASE (SELECT COUNT(*) FROM CampainActivity ca WHERE l.ConstantContactID = ca.ContactId AND ca.isdeleted = 0) " +
                                                  "   WHEN 0 THEN " +
                                                  "     CASE WHEN sc.Respondent IS NULL AND sc2.EmailAddress IS NULL THEN '' " +
                                                  "     ELSE " +
                                                  "       '<button onclick=''return false;''>S</button>' " +
                                                  "     END " +
                                                  "   ELSE " +
                                                  "     CASE WHEN sc.Respondent IS NULL AND sc2.EmailAddress IS NULL THEN  " +
                                                  "        '<font color=red size=2>C</font>' " +
                                                  "     ELSE " +
                                                  "       '<button onclick=''return false;''><font color=red>S</font></button>' " +
                                                  "     END " +
                                                  " END Survey " +
                                                  "  FROM _LeadPriority p, Lead_Flat l " +
                                                  "    LEFT JOIN SurveyCustomer sc ON l.EMail = sc.Respondent AND sc.isdeleted = 0 " +
                                                  "    LEFT JOIN SurveyCustomer sc2 ON l.EMail = sc2.EmailAddress AND sc2.isdeleted = 0 " +
                                                  " WHERE AssignedToId IN ('{0}') " +
                                                  "   AND l.PriorityId = p.Id "  +
                                                  "   AND ( " +
                                                  "         p.IsLead = 1 " +
                                                  "         OR (p.Id='F3DC2498-6F4F-449E-813C-EFDA32A9D24A' AND '{1}' IN ('DCDB22C2-65F4-46E4-91D1-CC123F83DCE2','0BA4012E-5541-4A76-92BB-C7122344DC3A','7D5AA961-5478-4FA1-B5DB-D6A2071ED834')) " +
                                                  "       ) " +
                                                  "   AND l.isdeleted = 0 " +
                                                  " ORDER BY EntryDate DESC", sassigned, Session["guser"]);

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
        grid1.AllowFiltering = true;
//        grid1.ShowFooter = false;
        grid1.EnableRecordHover = true;

        grid1.ClientSideEvents.OnClientSelect = "onClientSelect";

        GridFilteringSettings gfs = new GridFilteringSettings();
        gfs.FilterPosition = GridFilterPosition.Top;
        gfs.FilterLinksPosition = GridElementPosition.Top;
        //        gfs.InitialState = GridFilterState.Visible;
        gfs.InitialState = GridFilterState.Hidden;
        grid1.FilteringSettings = gfs;
        grid1.AllowFiltering = true;

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
        oCol3.AllowSorting=true;
        oCol3.Wrap=true;
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
        oCol4.Width = "90";
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
        oCol5.Width = "50";
        oCol5.AllowSorting = true;
        oCol5.Wrap = false;
        oCol5.AllowFilter = true;
        oCol5.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol5.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));

        Column oCol6 = new Column();
        oCol6.ID = "Column6";
        oCol6.DataField = "EntryDate";
        oCol6.HeaderText = "Entry Date";
        oCol6.Width = "170";
        oCol6.AllowSorting = true;
        oCol6.Wrap = false;
        oCol6.AllowFilter = true;
        oCol6.DataFormatString = "{0:MM/dd/yyyy hh:mm tt}";
        oCol6.ApplyFormatInEditMode = false;

        Column oCol7 = new Column();
        oCol7.ID = "Column7";
        oCol7.DataField = "CallLaterDate";
        oCol7.HeaderText = "Call Later";
        oCol7.Width = "170";
        oCol7.AllowSorting = true;
        oCol7.Wrap = false;
        oCol7.AllowFilter = true;
        oCol7.DataFormatString = "{0:MM/dd/yyyy}";
        oCol7.NullDisplayText = "missing!";
        oCol7.ApplyFormatInEditMode = true;

        Column oCol8 = new Column();
        oCol8.ID = "Column8";
        oCol8.DataField = "Source";
        oCol8.HeaderText = "Source";
        oCol8.Width = "80";
        oCol8.AllowSorting = true;
        oCol8.Wrap = false;
        oCol8.AllowFilter = true;

        Column oCol9 = new Column();
        oCol9.ID = "Column9";
        oCol9.DataField = "Priority";
        oCol9.HeaderText = "Priority";
        oCol9.Width = "120";
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

        Column oCol11 = new Column();
        oCol11.ID = "Column11";
        oCol11.DataField = "MsgHistory";
        oCol11.HeaderText = "History";
        oCol11.Width = "60";
        oCol11.AllowSorting = false;
        oCol11.Wrap = true;
        oCol11.AllowFilter = false;

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
        oCol14.Width = "125";
        oCol14.AllowSorting = true;
        oCol14.AllowFilter = true;
        oCol14.ShowFilterCriterias = false;
        oCol14.TemplateSettings.FilterTemplateId = "TemplateStatus";

        Column oCol15 = new Column();
        oCol15.ID = "Column15";
        oCol15.DataField = "Survey";
        oCol15.HeaderText = " ";
        oCol15.Visible = true;
        oCol15.ReadOnly = true;
        oCol15.Width = "35";
        oCol15.AllowSorting = true;
        oCol15.AllowFilter = true;
        oCol15.ParseHTML = true;
        oCol15.HtmlEncode = true;
        oCol15.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol15.FilterOptions.Add(new FilterOption(FilterOptionType.IsEmpty));
        oCol15.FilterOptions.Add(new FilterOption(FilterOptionType.IsNotEmpty));

        // add the columns to the Columns collection of the grid
        grid1.Columns.Add(oCol0);
        grid1.Columns.Add(oCol1);

        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol4);
        grid1.Columns.Add(oCol5);
        grid1.Columns.Add(oCol7);
        grid1.Columns.Add(oCol9);

        grid1.Columns.Add(oCol15);

        grid1.Columns.Add(oCol13);
        grid1.Columns.Add(oCol12);
        grid1.Columns.Add(oCol14);

        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol11);
        grid1.Columns.Add(oCol6);
        grid1.Columns.Add(oCol8);
        grid1.Columns.Add(oCol10);

        GridRuntimeTemplate HeadingTemplate = new GridRuntimeTemplate();
        HeadingTemplate.ID = "HeadingTemplate1";
        HeadingTemplate.Template = new Obout.Grid.RuntimeTemplate();
        HeadingTemplate.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateHeadingTemplate);

        GridRuntimeTemplate TemplatePriority = new GridRuntimeTemplate();
        TemplatePriority.ID = "TemplatePriority";
        TemplatePriority.ControlID = "ComboBoxPriority";
        TemplatePriority.Template = new Obout.Grid.RuntimeTemplate();
        TemplatePriority.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateTemplatePriority);

        GridRuntimeTemplate TemplateStatus = new GridRuntimeTemplate();
        TemplateStatus.ID = "TemplateStatus";
        TemplateStatus.ControlID = "ComboBoxStatus";
        TemplateStatus.Template = new Obout.Grid.RuntimeTemplate();
        TemplateStatus.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateTemplateStatus);

        GridRuntimeTemplate TemplateAssignedTo = new GridRuntimeTemplate();
        TemplateAssignedTo.ID = "TemplateAssignedTo";
        TemplateAssignedTo.ControlID = "ComboBoxAssignedTo";
        TemplateAssignedTo.Template = new Obout.Grid.RuntimeTemplate();
        TemplateAssignedTo.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateTemplateAssignedTo);

        grid1.Templates.Add(TemplatePriority);
        grid1.Templates.Add(TemplateStatus);
        grid1.Templates.Add(TemplateAssignedTo);
        grid1.Templates.Add(HeadingTemplate);
        grid1.TemplateSettings.HeadingTemplateId = "HeadingTemplate1";

        CustomFilterOption Between_EntryDate = new CustomFilterOption();
        Between_EntryDate.ID = "Between_EntryDate";
        Between_EntryDate.Text = "Between";
        Between_EntryDate.IsDefault = false;
        Between_EntryDate.TemplateSettings.FilterControlsIds = "StartDate_EntryDate,EndDate_EntryDate";
        Between_EntryDate.TemplateSettings.FilterControlsPropertyNames = "value,value";
        Between_EntryDate.TemplateSettings.FilterTemplateId = "EntryDateBetweenFilter";

        CustomFilterOption Last7Days_EntryDate = new CustomFilterOption();
        Last7Days_EntryDate.ID = "Last7Days_EntryDate";
        Last7Days_EntryDate.Text = "Last 7 Days";
        Last7Days_EntryDate.IsDefault = false;
        Last7Days_EntryDate.TemplateSettings.FilterControlsIds = "StartDateP7_EntryDate,EndDateP7_EntryDate";
        Last7Days_EntryDate.TemplateSettings.FilterControlsPropertyNames = "value,value";
        Last7Days_EntryDate.TemplateSettings.FilterTemplateId = "EntryDateLast7DaysFilter";

        CustomFilterOption Between_CallLaterDate = new CustomFilterOption();
        Between_CallLaterDate.ID = "Between_CallLaterDate";
        Between_CallLaterDate.Text = "Between";
        Between_CallLaterDate.IsDefault = false;
        Between_CallLaterDate.TemplateSettings.FilterControlsIds = "StartDate_CallLaterDate,EndDate_CallLaterDate";
        Between_CallLaterDate.TemplateSettings.FilterControlsPropertyNames = "value,value";
        Between_CallLaterDate.TemplateSettings.FilterTemplateId = "CallLaterDateBetweenFilter";

        CustomFilterOption Next7Days_CallLaterDate = new CustomFilterOption();
        Next7Days_CallLaterDate.ID = "Next7Days_CallLaterDate";
        Next7Days_CallLaterDate.Text = "Next 7 Days";
        Next7Days_CallLaterDate.IsDefault = false;
        Next7Days_CallLaterDate.TemplateSettings.FilterControlsIds = "StartDateN7_CallLaterDate,EndDateN7_CallLaterDate";
        Next7Days_CallLaterDate.TemplateSettings.FilterControlsPropertyNames = "value,value";
        Next7Days_CallLaterDate.TemplateSettings.FilterTemplateId = "CallLaterDateNext7DaysFilter";

        CustomFilterOption ByYearAndMonth_EntryDate = new CustomFilterOption();
        ByYearAndMonth_EntryDate.ID = "ByYearAndMonth_EntryDate";
        ByYearAndMonth_EntryDate.Text = "Filter By Year And Month";
        ByYearAndMonth_EntryDate.TemplateSettings.FilterTemplateId = "EntryDateByYearAndMonthFilter";
        ByYearAndMonth_EntryDate.TemplateSettings.FilterControlsIds = "YearMonth_Year_EntryDate,YearMonth_Month_EntryDate";
        ByYearAndMonth_EntryDate.TemplateSettings.FilterControlsPropertyNames = "value,value";

        CustomFilterOption ByYearAndMonth_CallLaterDate = new CustomFilterOption();
        ByYearAndMonth_CallLaterDate.ID = "ByYearAndMonth_CallLaterDate";
        ByYearAndMonth_CallLaterDate.Text = "Filter By Year And Month";
        ByYearAndMonth_CallLaterDate.TemplateSettings.FilterTemplateId = "CallLaterDateByYearAndMonthFilter";
        ByYearAndMonth_CallLaterDate.TemplateSettings.FilterControlsIds = "YearMonth_Year_CallLaterDate,YearMonth_Month_CallLaterDate";
        ByYearAndMonth_CallLaterDate.TemplateSettings.FilterControlsPropertyNames = "value,value";

        oCol6.FilterCriteria.Option.Type = FilterOptionType.Custom;
        oCol6.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol6.FilterOptions.Add(Last7Days_EntryDate);
        oCol6.FilterOptions.Add(Between_EntryDate);
        oCol6.FilterOptions.Add(ByYearAndMonth_EntryDate);

        oCol7.FilterCriteria.Option.Type = FilterOptionType.Custom;
        oCol7.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol7.FilterOptions.Add(new FilterOption(FilterOptionType.IsNull));
        oCol7.FilterOptions.Add(Next7Days_CallLaterDate);
        oCol7.FilterOptions.Add(Between_CallLaterDate);
        oCol7.FilterOptions.Add(ByYearAndMonth_CallLaterDate);

        GridRuntimeTemplate EntryDateBetweenFilter = new GridRuntimeTemplate();
        EntryDateBetweenFilter.ID = "EntryDateBetweenFilter";
        EntryDateBetweenFilter.Template = new Obout.Grid.RuntimeTemplate();
        EntryDateBetweenFilter.Template.CreateTemplate += new GridRuntimeTemplateEventHandler(CreateEntryDateBetweenFilterTemplate);

        GridRuntimeTemplate CallLaterDateBetweenFilter = new GridRuntimeTemplate();
        CallLaterDateBetweenFilter.ID = "CallLaterDateBetweenFilter";
        CallLaterDateBetweenFilter.Template = new Obout.Grid.RuntimeTemplate();
        CallLaterDateBetweenFilter.Template.CreateTemplate += new GridRuntimeTemplateEventHandler(CreateCallLaterDateBetweenFilterTemplate);

        GridRuntimeTemplate CallLaterDateNext7DaysFilter = new GridRuntimeTemplate();
        CallLaterDateNext7DaysFilter.ID = "CallLaterDateNext7DaysFilter";
        CallLaterDateNext7DaysFilter.Template = new Obout.Grid.RuntimeTemplate();
        CallLaterDateNext7DaysFilter.Template.CreateTemplate += new GridRuntimeTemplateEventHandler(CreateCallLaterDateNext7DaysFilterTemplate);

        GridRuntimeTemplate EntryDateByYearAndMonthFilter = new GridRuntimeTemplate();
        EntryDateByYearAndMonthFilter.ID = "EntryDateByYearAndMonthFilter";
        EntryDateByYearAndMonthFilter.Template = new Obout.Grid.RuntimeTemplate();
        EntryDateByYearAndMonthFilter.Template.CreateTemplate += new GridRuntimeTemplateEventHandler(CreateEntryDateByYearAndMonthFilterTemplate);

        GridRuntimeTemplate CallLaterDateByYearAndMonthFilter = new GridRuntimeTemplate();
        CallLaterDateByYearAndMonthFilter.ID = "CallLaterDateByYearAndMonthFilter";
        CallLaterDateByYearAndMonthFilter.Template = new Obout.Grid.RuntimeTemplate();
        CallLaterDateByYearAndMonthFilter.Template.CreateTemplate += new GridRuntimeTemplateEventHandler(CreateCallLaterDateByYearAndMonthFilterTemplate);

        GridRuntimeTemplate EntryDateLast7DaysFilter = new GridRuntimeTemplate();
        EntryDateLast7DaysFilter.ID = "EntryDateLast7DaysFilter";
        EntryDateLast7DaysFilter.Template = new Obout.Grid.RuntimeTemplate();
        EntryDateLast7DaysFilter.Template.CreateTemplate += new GridRuntimeTemplateEventHandler(CreateEntryDateLast7DaysFilterTemplate);

        grid1.Templates.Add(EntryDateBetweenFilter);
        grid1.Templates.Add(EntryDateByYearAndMonthFilter);
        grid1.Templates.Add(CallLaterDateBetweenFilter);
        grid1.Templates.Add(CallLaterDateByYearAndMonthFilter);
        grid1.Templates.Add(CallLaterDateNext7DaysFilter);
        grid1.Templates.Add(EntryDateLast7DaysFilter);
        grid1.Filtering += new EventHandler(grid1_Filtering);

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
//        window1.StyleFolder = "styles/blue/window";
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

        Obout.SuperForm.BoundField field7 = new Obout.SuperForm.BoundField();
        field7.DataField = "Source";
        field7.HeaderText = "Source";
        field7.FieldSetID = "FieldSet1";
        field7.AllowEdit = false;
        field7.Enabled = false;

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

        Obout.SuperForm.BoundField field10 = new Obout.SuperForm.BoundField();
        field10.DataField = "MsgHistory";
        field10.HeaderText = "History";
        field10.FieldSetID = "FieldSet1";
        field10.AllowEdit = false;
        field10.Enabled = false;

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
        SuperForm1.Fields.Add(field7);
        SuperForm1.Fields.Add(field8);
        SuperForm1.Fields.Add(field9);
        SuperForm1.Fields.Add(field10);

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

//        if (_dtAssignedTo.Rows.Count == 0)
//        {
            ssql = "SELECT * FROM Employee WHERE isdeleted = 0 AND isSales = 1 AND isactive = 1 AND Id!='00000000-0000-0000-0000-000000000000' ";
            _dtAssignedTo = _sql.GetTable(ssql);
//        }

//        if (_dtEmployee.Rows.Count == 0)
//        {
            ssql = "SELECT * FROM Employee WHERE isdeleted = 0";
            _dtEmployee = _sql.GetTable(ssql);
//        }

//        if (_dtPriority.Rows.Count == 0)
//        {
            ssql = "SELECT * FROM _LeadPriority WHERE isdeleted = 0 AND Short!='Undefined'"; 
            _dtPriority = _sql.GetTable(ssql);
//        }

//        if (_dtFilterPriority.Rows.Count == 0)
//        {
            ssql = "SELECT * FROM _LeadPriority WHERE isdeleted = 0"; 
            _dtFilterPriority = _sql.GetTable(ssql);
//        }

//        if (_dtStatus.Rows.Count == 0)
//        {
            ssql = "SELECT * FROM _LeadStatus WHERE isdeleted = 0";
            _dtStatus = _sql.GetTable(ssql);
//        }

//        if (_dtSalesEmployee.Rows.Count == 0)
//        {
            ssql = String.Format("SELECT eh.Id,EmployeeId,SubEmployeeId,DisplayName,LastName " +
                             "  FROM EmployeeHirarchy eh,Employee e  " +
                             "  WHERE eh.isdeleted = 0  " +
                             "  AND e.isdeleted=0  " +
                             "  AND e.Id = eh.SubEmployeeId  " +
                             "  AND eh.EmployeeId = '{0}'  " +
                             "  AND e.isSales=1 " +
                             "  AND eh.SubEmployeeId != '{1}' " +
                             "  ORDER BY LastName", Session["guser"].ToString(), Guid.Empty);
            ssql = "SELECT * FROM Employee WHERE isdeleted = 0 AND isSales = 1 AND isactive = 1 AND Id!='00000000-0000-0000-0000-000000000000'";
            _dtSalesEmployee = _sql.GetTable(ssql);
//        }

        _sql.Close();
    
    }
    protected void grid1_Filtering(object sender, EventArgs e)
    {
        Column entryDateColumn = grid1.Columns.GetColumnByDataField("EntryDate"); 
        Column CallLaterDateColumn = grid1.Columns.GetColumnByDataField("CallLaterDate");
        Column statusColumn = grid1.Columns.GetColumnByDataField("Status"); 
        Column priorityColumn = grid1.Columns.GetColumnByDataField("Priority"); 
        Column assignedToColumn = grid1.Columns.GetColumnByDataField("AssignedTo"); 

        if ((entryDateColumn.FilterCriteria.Option is CustomFilterOption) ||
            (CallLaterDateColumn.FilterCriteria.Option is CustomFilterOption))
        {
            CustomFilterOption filterOption = new CustomFilterOption();
            if (entryDateColumn.FilterCriteria.Option is CustomFilterOption) { filterOption = entryDateColumn.FilterCriteria.Option as CustomFilterOption; }
            if (CallLaterDateColumn.FilterCriteria.Option is CustomFilterOption) { filterOption = CallLaterDateColumn.FilterCriteria.Option as CustomFilterOption; }
            switch (filterOption.ID)
            {
                case "Last7Days_EntryDate":
                    {
                        string startDate = entryDateColumn.FilterCriteria.Values["StartDateP7_EntryDate"].ToString();
                        string endDate = entryDateColumn.FilterCriteria.Values["EndDateP7_EntryDate"].ToString();

                        if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                        {
                            // we filter between start date at 12:00AM and end date at 11:59PM
                            entryDateColumn.FilterCriteria.FilterExpression = "(" + entryDateColumn.DataField + " >= #" + startDate + " 12:00:00 AM# AND " + entryDateColumn.DataField + " <= #" + endDate + " 11:59:59 PM#)";
                        }
                    }
                    break;
                case "Between_EntryDate":
                    {
                        string startDate = entryDateColumn.FilterCriteria.Values["StartDate_EntryDate"].ToString();
                        string endDate = entryDateColumn.FilterCriteria.Values["EndDate_EntryDate"].ToString();

                        if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                        {
                            // we filter between start date at 12:00AM and end date at 11:59PM
                            entryDateColumn.FilterCriteria.FilterExpression = "(" + entryDateColumn.DataField + " >= #" + startDate + " 12:00:00 AM# AND " + entryDateColumn.DataField + " <= #" + endDate + " 11:59:59 PM#)";
                        }
                    }
                    break;
                case "ByYearAndMonth_EntryDate":
                    {
                        string year2 = entryDateColumn.FilterCriteria.Values["YearMonth_Year_EntryDate"].ToString();
                        string month2 = entryDateColumn.FilterCriteria.Values["YearMonth_Month_EntryDate"].ToString();
                        entryDateColumn.FilterCriteria.FilterExpression = "(CONVERT(" + entryDateColumn.DataField + ", 'System.String') LIKE '" + month2 + "/%') AND (CONVERT(" + entryDateColumn.DataField + ", 'System.String') LIKE '%/" + year2 + " %')";
                        break;
                    }
                case "Between_CallLaterDate":
                    {
                        string startDate = CallLaterDateColumn.FilterCriteria.Values["StartDate_CallLaterDate"].ToString();
                        string endDate = CallLaterDateColumn.FilterCriteria.Values["EndDate_CallLaterDate"].ToString();

                        if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                        {
                            // we filter between start date at 12:00AM and end date at 11:59PM
                            CallLaterDateColumn.FilterCriteria.FilterExpression = "(" + CallLaterDateColumn.DataField + " >= #" + startDate + " 12:00:00 AM# AND " + CallLaterDateColumn.DataField + " <= #" + endDate + " 11:59:59 PM#)";
                        }
                    }
                    break;
                case "Next7Days_CallLaterDate":
                    {
                        string startDate = CallLaterDateColumn.FilterCriteria.Values["StartDateN7_CallLaterDate"].ToString();
                        string endDate = CallLaterDateColumn.FilterCriteria.Values["EndDateN7_CallLaterDate"].ToString();

                        if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                        {
                            // we filter between start date at 12:00AM and end date at 11:59PM
                            CallLaterDateColumn.FilterCriteria.FilterExpression = "(" + CallLaterDateColumn.DataField + " >= #" + startDate + " 12:00:00 AM# AND " + CallLaterDateColumn.DataField + " <= #" + endDate + " 11:59:59 PM#)";
                        }
                    }
                    break;
                case "ByYearAndMonth_CallLaterDate":
                    {
                        string year2 = CallLaterDateColumn.FilterCriteria.Values["YearMonth_Year_CallLaterDate"].ToString();
                        string month2 = CallLaterDateColumn.FilterCriteria.Values["YearMonth_Month_CallLaterDate"].ToString();
                        CallLaterDateColumn.FilterCriteria.FilterExpression = "(CONVERT(" + CallLaterDateColumn.DataField + ", 'System.String') LIKE '" + month2 + "/%') AND (CONVERT(" + CallLaterDateColumn.DataField + ", 'System.String') LIKE '%/" + year2 + " %')";
                    }
                    break;
            }
        }

    }
    public void CreateTemplatePriority(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder ph1 = new PlaceHolder();
        e.Container.Controls.Add(ph1);

        ComboBox comboboxPriority = new ComboBox();
        comboboxPriority.ID = "ComboBoxPriority";
        comboboxPriority.Width = Unit.Percentage(100);
        comboboxPriority.SelectionMode = ListSelectionMode.Multiple;
        comboboxPriority.DataTextField = "Priority";
        comboboxPriority.DataValueField = "Priority";
        comboboxPriority.FolderStyle = "styles/premiere_blue/combobox";

        comboboxPriority.Items.Add(new ComboBoxItem("ALL", ""));
        comboboxPriority.SelectedIndex = 0;
        foreach (DataRow dr in _dtFilterPriority.Rows)
        {
            comboboxPriority.Items.Add(new ComboBoxItem(dr["Name"].ToString(), dr["Name"].ToString()));
        }

        ph1.Controls.Add(comboboxPriority);
    }
    public void CreateTemplateAssignedTo(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder ph1 = new PlaceHolder();
        e.Container.Controls.Add(ph1);

        ComboBox comboboxAssignedTo = new ComboBox();
        comboboxAssignedTo.ID = "ComboBoxAssignedTo";
        comboboxAssignedTo.Width = Unit.Percentage(100);
        comboboxAssignedTo.SelectionMode = ListSelectionMode.Multiple;
        comboboxAssignedTo.DataTextField = "AssignedTo";
        comboboxAssignedTo.DataValueField = "AssignedTo";
        comboboxAssignedTo.FolderStyle = "styles/premiere_blue/combobox";

        comboboxAssignedTo.Items.Add(new ComboBoxItem("ALL", ""));
        comboboxAssignedTo.SelectedIndex = 0;
        foreach (DataRow dr in _dtAssignedTo.Rows)
        {
            comboboxAssignedTo.Items.Add(new ComboBoxItem(dr["DisplayName"].ToString(), dr["DisplayName"].ToString()));
        }

        ph1.Controls.Add(comboboxAssignedTo);
    }
    public void CreateTemplateStatus(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder ph1 = new PlaceHolder();
        e.Container.Controls.Add(ph1);

        ComboBox comboboxStatus = new ComboBox();
        comboboxStatus.ID = "ComboBoxStatus";
        comboboxStatus.Width = Unit.Percentage(100);
        comboboxStatus.SelectionMode = ListSelectionMode.Multiple;
        comboboxStatus.DataTextField = "Status";
        comboboxStatus.DataValueField = "Status";
        comboboxStatus.FolderStyle = "styles/premiere_blue/combobox";

        comboboxStatus.Items.Add(new ComboBoxItem("ALL", ""));
        comboboxStatus.SelectedIndex = 0;
        foreach (DataRow dr in _dtStatus.Rows)
        {
            comboboxStatus.Items.Add(new ComboBoxItem(dr["Name"].ToString(), dr["Name"].ToString()));
        }

        ph1.Controls.Add(comboboxStatus);
    }
    public void CreateEntryDateBetweenFilterTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPhOdate = new PlaceHolder();
        e.Container.Controls.Add(oPhOdate);
        oPhOdate.DataBinding += new EventHandler(DataBindEntryDateBetweenFilterTemplate);
    }
    public void CreateEntryDateByYearAndMonthFilterTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPhOdate = new PlaceHolder();
        e.Container.Controls.Add(oPhOdate);
        oPhOdate.DataBinding += new EventHandler(DataBindEntryDateByYearAndMonthFilterTemplate);
    }
    protected void DataBindEntryDateBetweenFilterTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPhOdateBetween = sender as PlaceHolder;
        HtmlGenericControl divContainer = CreateDiv("tdText", "");
        divContainer.Controls.Add(CreateTextBox("StartDate_EntryDate", TextBoxMode.SingleLine, 45));
        divContainer.Controls.Add(CreateDiv("separator", "-"));
        divContainer.Controls.Add(CreateTextBox("EndDate_EntryDate", TextBoxMode.SingleLine, 45));

        oPhOdateBetween.Controls.Add(divContainer);
    }
    protected void DataBindEntryDateByYearAndMonthFilterTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPhOdateByYear = sender as PlaceHolder;
        HtmlGenericControl divContainer = CreateDiv("", "");
        DropDownList ddlYear = CreateYearDDL("YearMonth_Year_EntryDate", 45);
        DropDownList ddlMonth = CreateMonthDDL("YearMonth_Month_EntryDate", 53);
        ddlMonth.SelectedIndex = DateTime.Now.Month - 1;
        divContainer.Controls.Add(ddlYear);
        divContainer.Controls.Add(ddlMonth);
        oPhOdateByYear.Controls.Add(divContainer);
    }
    public void CreateEntryDateLast7DaysFilterTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPhOdate = new PlaceHolder();
        e.Container.Controls.Add(oPhOdate);
        oPhOdate.DataBinding += new EventHandler(DataBindEntryDateLast7DaysFilterTemplate);
    }
    public void CreateCallLaterDateNext7DaysFilterTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPhOdate = new PlaceHolder();
        e.Container.Controls.Add(oPhOdate);
        oPhOdate.DataBinding += new EventHandler(DataBindCallLaterDateNext7DaysFilterTemplate);
    }
    public void CreateCallLaterDateBetweenFilterTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPhOdate = new PlaceHolder();
        e.Container.Controls.Add(oPhOdate);
        oPhOdate.DataBinding += new EventHandler(DataBindCallLaterDateBetweenFilterTemplate);
    }
    public void CreateCallLaterDateByYearAndMonthFilterTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPhOdate = new PlaceHolder();
        e.Container.Controls.Add(oPhOdate);
        oPhOdate.DataBinding += new EventHandler(DataBindCallLaterDateByYearAndMonthFilterTemplate);
    }
    protected void DataBindCallLaterDateNext7DaysFilterTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPhOdateBetween = sender as PlaceHolder;
        HtmlGenericControl divContainer = CreateDiv("", "");
        TextBox tbStartDate7 = CreateTextBox("StartDateN7_CallLaterDate", TextBoxMode.SingleLine, 45);
        tbStartDate7.Text = DateTime.Now.ToShortDateString();
        divContainer.Controls.Add(tbStartDate7);
        divContainer.Controls.Add(CreateDiv("separator", "-"));
        TextBox tbEndDate7 = CreateTextBox("EndDateN7_CallLaterDate", TextBoxMode.SingleLine, 45);
        tbEndDate7.Text = DateTime.Now.AddDays(6).ToShortDateString();
        divContainer.Controls.Add(tbEndDate7);

        oPhOdateBetween.Controls.Add(divContainer);
    }
    protected void DataBindEntryDateLast7DaysFilterTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPhOdateBetween = sender as PlaceHolder;
        HtmlGenericControl divContainer = CreateDiv("", "");
        TextBox tbStartDate7 = CreateTextBox("StartDateP7_EntryDate", TextBoxMode.SingleLine, 45);
        tbStartDate7.Text = DateTime.Now.AddDays(-6).ToShortDateString();
        divContainer.Controls.Add(tbStartDate7);
        divContainer.Controls.Add(CreateDiv("separator", "-"));
        TextBox tbEndDate7 = CreateTextBox("EndDateP7_EntryDate", TextBoxMode.SingleLine, 45);
        tbEndDate7.Text = DateTime.Now.ToShortDateString();
        divContainer.Controls.Add(tbEndDate7);

        oPhOdateBetween.Controls.Add(divContainer);
    }
    protected void DataBindCallLaterDateBetweenFilterTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPhOdateBetween = sender as PlaceHolder;
        HtmlGenericControl divContainer = CreateDiv("", "");
        divContainer.Controls.Add(CreateTextBox("StartDate_CallLaterDate", TextBoxMode.SingleLine, 45));
        divContainer.Controls.Add(CreateDiv("separator", "-"));
        divContainer.Controls.Add(CreateTextBox("EndDate_CallLaterDate", TextBoxMode.SingleLine, 45));

        oPhOdateBetween.Controls.Add(divContainer);
    }
    protected void DataBindCallLaterDateByYearAndMonthFilterTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPhOdateByYear = sender as PlaceHolder;
        HtmlGenericControl divContainer = CreateDiv("tdText", "");
        DropDownList ddlYear = CreateYearDDL("YearMonth_Year_CallLaterDate", 45);
        DropDownList ddlMonth = CreateMonthDDL("YearMonth_Month_CallLaterDate", 53);
        ddlMonth.SelectedIndex = DateTime.Now.Month - 1;
        divContainer.Controls.Add(ddlYear);
        divContainer.Controls.Add(ddlMonth);
        oPhOdateByYear.Controls.Add(divContainer);
    }

    protected OboutTextBox CreateTextBox(string Id, TextBoxMode txtMode, int width)
    {
        OboutTextBox obTextBox = new OboutTextBox();
        obTextBox.ID = Id;
        obTextBox.Width = Unit.Percentage(width);
        obTextBox.TextMode = txtMode;
        return obTextBox;
    }
    protected HtmlGenericControl CreateDiv(string className, string innerHTML)
    {
        HtmlGenericControl obDiv = new HtmlGenericControl("DIV");
        obDiv.Attributes.Add("class", className);
        obDiv.InnerHtml = innerHTML;

        return obDiv;
    }
    protected OboutDropDownList CreateYearDDL(string id, int width)
    {
        OboutDropDownList ddlYear = new OboutDropDownList();
        ddlYear.ID = id;
        ddlYear.Width = Unit.Percentage(width);
        //        ddlYear.Height = 0; // Unit.Pixel(150);
        for (int i = DateTime.Now.Year; i >= 2010; i--)
        {
            ddlYear.Items.Add(new ListItem(i.ToString()));
        }
        return ddlYear;
    }
    protected OboutDropDownList CreateMonthDDL(string id, int width)
    {
        OboutDropDownList ddlMonth = new OboutDropDownList();
        ddlMonth.ID = id;
        //        ddlMonth.Height = 0; // Unit.Pixel(150);
        ddlMonth.Width = Unit.Percentage(width);
        System.Globalization.DateTimeFormatInfo dateFormatinfo = new System.Globalization.DateTimeFormatInfo();
        for (int i = 1; i <= 12; i++)
        {
            ddlMonth.Items.Add(new ListItem(dateFormatinfo.GetMonthName(i), i.ToString()));
        }

        return ddlMonth;
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
        _listComboBox[0].Items.Add(new ComboBoxItem("Lead Sales 1.0.12"));
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
            button1.Width = 75;

            Obout.Interface.OboutButton button2 = new Obout.Interface.OboutButton();
            button2.ID = "OboutButton2";
            button2.Text = "Cancel";
            button2.OnClientClick = "cancelChanges(); return false;";
            button2.Width = 75;

            Obout.Interface.OboutButton button3 = new Obout.Interface.OboutButton();
            button3.ID = "OboutButton3";
            button3.Text = "Survey";
            button3.OnClientClick = "survey(); return false;"; 
            button3.Width = 75;

            Obout.Interface.OboutButton button4 = new Obout.Interface.OboutButton();
            button4.ID = "OboutButton4";
            button4.Text = "EMail";
            button4.OnClientClick = "email(); return false;";
            button4.Width = 75;

            templatePlaceHolder.Controls.Add(button1);
            templatePlaceHolder.Controls.Add(button2);
            templatePlaceHolder.Controls.Add(button3);
            templatePlaceHolder.Controls.Add(button4);
        
        }
    }
}
