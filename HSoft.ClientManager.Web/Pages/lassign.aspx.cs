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

using Obout.ComboBox;
using Obout.Grid;
using OboutInc.Calendar2;
using Obout.Interface;

using HSoft.ClientManager.Web;

public partial class Pages_lassign : System.Web.UI.Page
{
    Grid grid1 = new Grid();
    Obout.ComboBox.ComboBox cbo1 = new Obout.ComboBox.ComboBox();
    Obout.ComboBox.ComboBox cbo2 = new Obout.ComboBox.ComboBox();
    OboutTextBox oTextBox = new OboutTextBox();
    OboutInc.Calendar2.Calendar cal1 = new OboutInc.Calendar2.Calendar();

    static DataTable _dtEmployee = new DataTable();
    static DataTable _dtSalesEmployee = new DataTable();
    static DataTable _dtPriority = new DataTable();
    static DataTable _dtFilterPriority = new DataTable();
    static DataTable _dtStatus = new DataTable();
    static DataTable _dtAssignedTo = new DataTable();

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        grid1.ID = "grid1";
        grid1.CallbackMode = false;
        grid1.Serialize = true;
        grid1.AutoGenerateColumns = false;
        grid1.PageSize = -1;
        grid1.Height = int.Parse(Session["wy"].ToString()) - 40;
        grid1.EnableRecordHover = true;

        grid1.FolderStyle = "styles/grid/premiere_blue";

        // setting the event handlers
//        grid1.InsertCommand += new Obout.Grid.Grid.EventHandler(InsertRecord);
//        grid1.DeleteCommand += new Obout.Grid.Grid.EventHandler(DeleteRecord);
        grid1.UpdateCommand += new Obout.Grid.Grid.EventHandler(UpdateRecord);
        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        grid1.ClientSideEvents.OnClientSelect = "onClientSelect";

        GridRuntimeTemplate TemplateEditAssignTo = new GridRuntimeTemplate();
        TemplateEditAssignTo.ID = "TemplateEditAssignTo";
        TemplateEditAssignTo.Template = new Obout.Grid.RuntimeTemplate();
        TemplateEditAssignTo.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateEditAssignToTemplate);
        TemplateEditAssignTo.ControlID = "cbo1";
        TemplateEditAssignTo.ControlPropertyName = "value";
        TemplateEditAssignTo.UseQuotes = true;

        GridRuntimeTemplate TemplateEditPriority = new GridRuntimeTemplate();
        TemplateEditPriority.ID = "TemplateEditPriority";
        TemplateEditPriority.Template = new Obout.Grid.RuntimeTemplate();
        TemplateEditPriority.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateEditPriorityTemplate);
        TemplateEditPriority.ControlID = "cbo2";
        TemplateEditPriority.ControlPropertyName = "value";
        TemplateEditPriority.UseQuotes = true;

        GridRuntimeTemplate tplDatePicker = new GridRuntimeTemplate();
        tplDatePicker.ID = "tplDatePicker";
        tplDatePicker.Template = new Obout.Grid.RuntimeTemplate();
        tplDatePicker.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateDatePickerTemplate);
        tplDatePicker.ControlID = "txtOrderDate";
        tplDatePicker.ControlPropertyName = "value";
        tplDatePicker.UseQuotes = true;

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

        grid1.Templates.Add(TemplateEditAssignTo);
        grid1.Templates.Add(TemplateEditPriority);
        grid1.Templates.Add(tplDatePicker);
        grid1.Templates.Add(HeadingTemplate);
        grid1.Templates.Add(TemplatePriority);
        grid1.Templates.Add(TemplateStatus);
        grid1.Templates.Add(TemplateAssignedTo);

        // grid filter
        GridFilteringSettings gfs = new GridFilteringSettings();
        gfs.FilterPosition = GridFilterPosition.Top;
        gfs.FilterLinksPosition = GridElementPosition.Top;
        //        gfs.InitialState = GridFilterState.Visible;
        gfs.InitialState = GridFilterState.Hidden;
        grid1.FilteringSettings = gfs;
        grid1.AllowFiltering = true;

        Column oCol0 = new Column();
        oCol0.DataField = "";
        oCol0.HeaderText = "";
        oCol0.Width = "45";
        oCol0.AllowEdit = true;
        oCol0.AllowDelete = false;

        Column oColi = new Column();
        oColi.ID="ID";
        oColi.DataField = "Id";
        oColi.Visible=false;

        Column oCol1 = new Column();
        oCol1.ID="Name";
        oCol1.DataField="Name";
        oCol1.HeaderText="Name"; 
        oCol1.Visible=true; 
        oCol1.ReadOnly=true;
        oCol1.Width = "120";
        oCol1.AllowSorting=true;
        oCol1.Wrap=true;
        oCol1.AllowFilter=true;
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol2 = new Column();
        oCol2.ID="Email";
        oCol2.DataField="Email";
        oCol2.HeaderText="Email" ;
        oCol2.Visible=true; 
        oCol2.ReadOnly=true; 
        oCol2.Width = "180";
        oCol2.AllowSorting=true;
        oCol2.AllowFilter=true;
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol3 = new Column();
        oCol3.ID="Phone";
        oCol3.DataField="Phone";
        oCol3.HeaderText="Phone" ;
        oCol3.Visible=true; 
        oCol3.ReadOnly=true; 
        oCol3.Width = "90";
        oCol3.AllowSorting = true;
        oCol3.AllowFilter=true;
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol4 = new Column();
        oCol4.ID="TimeZone";
        oCol4.DataField="TimeZone";
        oCol4.HeaderText="Zone" ;
        oCol4.Visible=true; 
        oCol4.ReadOnly=true; 
        oCol4.Width = "50";
        oCol4.AllowSorting=true;
        oCol4.AllowFilter=true;
        oCol4.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol4.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));

        Column oCol5 = new Column();
        oCol5.ID="EntryDate";
        oCol5.DataField="EntryDate";
        oCol5.HeaderText="Entry Date" ;
        oCol5.Visible=true; 
        oCol5.ReadOnly=true;
        oCol5.Width = "170";
        oCol5.AllowSorting = true;
        oCol5.AllowFilter = true;
        oCol5.NullDisplayText = "missing!";
        oCol5.DataFormatString = "{0:MM/dd/yyyy hh:mm tt}";

        Column oCol6 = new Column();
        oCol6.ID="CallLaterDate";
        oCol6.DataField="CallLaterDate";
        oCol6.HeaderText="Call Later" ;
        oCol6.Visible=true; 
        oCol6.ReadOnly=false;
        oCol6.Width = "170";
        oCol6.AllowSorting = true;
        oCol6.AllowFilter = true;
        oCol6.NullDisplayText = "missing!";
        oCol6.DataFormatString = "{0:MM/dd/yyyy}";
        oCol6.ApplyFormatInEditMode =true;
//        oCol6.TemplateSettings.EditTemplateId = "tplDatePicker";

        Column oCol7 = new Column();
        oCol7.ID="Source";
        oCol7.DataField="Source";
        oCol7.HeaderText="Source" ;
        oCol7.Visible=true; 
        oCol7.ReadOnly=true; 
        oCol7.Width = "80";
        oCol7.AllowSorting = true;
        oCol7.AllowFilter = true;

        Column oCol8 = new Column();
        oCol8.ID="Status";
        oCol8.DataField="Status";
        oCol8.HeaderText="Status" ;
        oCol8.Visible=true; 
        oCol8.ReadOnly=true; 
        oCol8.Width = "110";
        oCol8.AllowSorting = true;
        oCol8.AllowFilter = true;
        oCol8.ShowFilterCriterias = false;
        oCol8.TemplateSettings.FilterTemplateId = "TemplateStatus";

        Column oCol9 = new Column();
        oCol9.ID="Priority";
        oCol9.DataField="Priority";
        oCol9.HeaderText="Priority"; 
        oCol9.Visible=true; 
        oCol9.ReadOnly=false; 
        oCol9.Width = "110";
        oCol9.AllowSorting = true;
        oCol9.AllowFilter = true;
        oCol9.ShowFilterCriterias = false;
        oCol9.TemplateSettings.FilterTemplateId = "TemplatePriority";
        oCol9.TemplateSettings.EditTemplateId = "TemplateEditPriority";

        Column oCol10 = new Column();
        oCol10.ID="AssignedTo";
        oCol10.DataField="AssignedTo";
        oCol10.HeaderText="AssignedTo"; 
        oCol10.Visible=true;
        oCol10.ReadOnly=false; 
        oCol10.Width = "120";
        oCol10.AllowSorting = true;
        oCol10.AllowFilter = true;
        oCol10.ShowFilterCriterias = false;
        oCol10.TemplateSettings.FilterTemplateId = "TemplateAssignedTo";
        oCol10.TemplateSettings.EditTemplateId = "TemplateEditAssignTo";

        Column oCol11 = new Column();
        oCol11.ID="LeadNote";
        oCol11.DataField="LeadNote";
        oCol11.HeaderText="Lead Note"; 
        oCol11.Visible=true; 
        oCol11.ReadOnly=true; 
        oCol11.Width = "300";
        oCol11.Wrap = true;
        oCol11.AllowSorting = false;
        oCol11.ParseHTML = true;
        oCol11.AllowFilter = true;
        oCol11.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol11.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));

        Column oCol12 = new Column();
        oCol12.ID="SalesNote";
        oCol12.DataField="SalesNote";
        oCol12.HeaderText="Sales Note"; 
        oCol12.Visible=true; 
        oCol12.ReadOnly=true; 
        oCol12.Width = "300";
        oCol12.Wrap = true;
        oCol12.AllowSorting = false;
        oCol12.ParseHTML=true;
        oCol12.AllowFilter = true;
        oCol12.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol12.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
                
        grid1.Columns.Add(oCol0);
        grid1.Columns.Add(oColi);
        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol9);
        grid1.Columns.Add(oCol10);
        grid1.Columns.Add(oCol6);
        grid1.Columns.Add(oCol12);
        grid1.Columns.Add(oCol8);
        grid1.Columns.Add(oCol11);
        grid1.Columns.Add(oCol4);
        grid1.Columns.Add(oCol5);
        grid1.Columns.Add(oCol7);

        grid1.Filtering += new EventHandler(grid1_Filtering);
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

        oCol5.FilterCriteria.Option.Type = FilterOptionType.Custom;
        oCol5.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol5.FilterOptions.Add(Last7Days_EntryDate);
        oCol5.FilterOptions.Add(Between_EntryDate);
        oCol5.FilterOptions.Add(ByYearAndMonth_EntryDate);

        oCol6.FilterCriteria.Option.Type = FilterOptionType.Custom;
        oCol6.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol6.FilterOptions.Add(new FilterOption(FilterOptionType.IsNull));
        oCol6.FilterOptions.Add(Next7Days_CallLaterDate);
        oCol6.FilterOptions.Add(Between_CallLaterDate);
        oCol6.FilterOptions.Add(ByYearAndMonth_CallLaterDate);

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

        // add the grid to the controls collection of the PlaceHolder
        phGrid1.Controls.Add(grid1);
        phGrid1.Controls.Add(cbo1);
        phGrid1.Controls.Add(cbo2);
//        phGrid1.Controls.Add(cal1);

        if (!Page.IsPostBack)
        {
            BindGrid();
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
        for (int i=DateTime.Now.Year;i>=2010; i--)
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

    protected override void OnPreRender(EventArgs e)
    {
        cal1.TextBoxId = oTextBox.ClientID;

        base.OnPreRender(e);
    }

    void BindGrid()
    {
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);


        if (_dtAssignedTo.Rows.Count == 0)
        {
            ssql = "SELECT * FROM Employee WHERE isdeleted = 0 AND isSales = 1 AND isactive = 1 AND Id!='00000000-0000-0000-0000-000000000000' ";
            _dtAssignedTo = _sql.GetTable(ssql);
        }

        if (_dtEmployee.Rows.Count == 0)
        {
            ssql = "SELECT * FROM Employee WHERE isdeleted = 0 AND isactive = 1 AND Id!='00000000-0000-0000-0000-000000000000'";
            _dtEmployee = _sql.GetTable(ssql);
        }

        if (_dtPriority.Rows.Count == 0)
        {
            // ssql = "SELECT * FROM _LeadPriority WHERE isdeleted = 0 AND Short!='Undefined'";
            ssql = "SELECT * FROM _LeadPriority WHERE isdeleted = 0";
            _dtPriority = _sql.GetTable(ssql);
        }

        if (_dtFilterPriority.Rows.Count == 0)
        {
            ssql = "SELECT * FROM _LeadPriority WHERE isdeleted = 0 ";
            _dtFilterPriority = _sql.GetTable(ssql);
        }

        if (_dtStatus.Rows.Count == 0)
        {
            ssql = "SELECT * FROM _LeadStatus WHERE isdeleted = 0";
            _dtStatus = _sql.GetTable(ssql);
        }

        if (_dtSalesEmployee.Rows.Count == 0)
        {
//            ssql = String.Format("SELECT eh.Id,EmployeeId,SubEmployeeId,DisplayName,LastName " +
//                             "  FROM EmployeeHirarchy eh,Employee e  " +
//                             "  WHERE eh.isdeleted = 0  " +
//                             "  AND e.isdeleted=0  " +
//                             "  AND e.Id = eh.SubEmployeeId  " +
//                             "  AND eh.EmployeeId = '{0}'  " +
//                             "  AND e.isSales=1 " +
//                             "  AND e.isactve=1" +
//                             "  AND e.isdeleted=1" +
//                             "  AND eh.SubEmployeeId != '{1}' " +
//                             "  ORDER BY LastName", Session["guser"].ToString(),Guid.Empty);
            ssql = "SELECT * FROM Employee WHERE isdeleted = 0 AND isSales = 1 AND isactive = 1 AND Id!='00000000-0000-0000-0000-000000000000'";
            _dtSalesEmployee = _sql.GetTable(ssql);
        }

        String sassigned = Session["ghirarchy"].ToString();
        sassigned = String.Format("{0}{1}", sassigned.Replace(",", "','"), Guid.Empty);
        ssql = String.Format("SELECT * " +
                             "  FROM Lead_Flat " +
                             " WHERE AssignedToId IN ('{0}') AND isdeleted=0 ORDER BY EntryDate DESC ", sassigned);
        DataTable _dtLeads = _sql.GetTable(ssql);

        grid1.DataSource = _dtLeads;
        grid1.DataBind();

        _sql.Close();
    }
    void RebindGrid(object sender, EventArgs e) { BindGrid(); }

    void UpdateRecord(object sender, GridRecordEventArgs e)
    {

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        DataRow dr = _sql.GetTable(String.Format("SELECT * FROM Lead_Flat WHERE Id = '{0}'",e.Record["Id"])).Rows[0];

        Boolean bisupdate = false;
        foreach (String skey in e.Record.Keys)
        {
            if (dr.Table.Columns.Contains(skey))
            {
                Boolean bisdiff = false;
                switch (dr[skey].GetType().FullName)
                {
                    case "System.DateTime":
                        {
                            if (DateTime.Parse(dr[skey].ToString()) != DateTime.Parse(e.Record[skey].ToString())) { bisdiff = true; }
                        }
                        break;
                    case "System.String":
                        {
                            if (dr[skey].ToString() != e.Record[skey].ToString().Replace("\r","")) 
                            { 
                                bisdiff = true; 
                            }
                        }
                        break;
                    default:
                        { /* oops */ }
                        break;
                }

                if (bisdiff)
                {
                    ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                         "VALUES ('{0}','{1}','{5}','{2}','{3}','{4}') ", "Lead_Flat", skey, dr[skey].ToString().Replace("'", "''"), e.Record[skey].ToString().Replace("'", "''"), Session["guser"], e.Record["Id"]);
                    _sql.Execute(ssql);
                    bisupdate = true;
                }
            }
        }

        if (bisupdate)
        {
            String sPriorityId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadPriority WHERE (Name='{0}' AND isdeleted=0)", e.Record["Priority"])).ToString();
            // String sStatusId = _sql.ExecuteScalar(String.Format("SELECT Id FROM _LeadStatus WHERE (Name='{0}' AND isdeleted=0)", e.Record["Status"])).ToString();
            String sAssignedToId = _sql.ExecuteScalar(String.Format("SELECT Id FROM Employee WHERE (DisplayName='{0}' AND isdeleted=0)", e.Record["AssignedTo"])).ToString();

            ssql = String.Format("UPDATE [Lead_Flat] " +
                     "   SET CallLaterDate={0} , " +
                     "       AssignedTo='{1}' , " +
                     "       Priority='{2}' , " +
                     "       AssignedToId='{3}' , " +
                     "       PriorityId='{4}' " +
                     " WHERE Id = '{5}' ", e.Record["CallLaterDate"].ToString().Length==0?"null":String.Format("'{0}'",e.Record["CallLaterDate"]), e.Record["AssignedTo"], e.Record["Priority"], sAssignedToId, sPriorityId, e.Record["Id"]);
            _sql.Execute(ssql);
        }

        _sql.Close();
    }

	public void CreateEditAssignToTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
	{
		PlaceHolder oPlaceHolder = new PlaceHolder();
        e.Container.Controls.Add(oPlaceHolder);
       
        cbo1.ID = "cbo1";
        cbo1.Height = _dtSalesEmployee.Rows.Count*20;
        cbo1.Width = Unit.Percentage(100);
        cbo1.FolderStyle = "styles/premiere_blue/Combobox";

        foreach (DataRow dr in _dtSalesEmployee.Rows)
        {
            cbo1.Items.Add(new ComboBoxItem(dr["DisplayName"].ToString(), dr["DisplayName"].ToString()));
        }

        oPlaceHolder.Controls.Add(cbo1);
	}

    public void CreateEditPriorityTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPlaceHolder = new PlaceHolder();
        e.Container.Controls.Add(oPlaceHolder);

        cbo2.ID = "cbo2";
        cbo2.Height = _dtFilterPriority.Rows.Count * 20;
        cbo2.Width = Unit.Percentage(100);
        cbo2.FolderStyle = "styles/premiere_blue/Combobox";

        foreach (DataRow dr in _dtPriority.Rows)
        {
            cbo2.Items.Add(new ComboBoxItem(dr["Name"].ToString(), dr["Name"].ToString()));
        }

        oPlaceHolder.Controls.Add(cbo2);
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
        _listComboBox[0].Items.Add(new ComboBoxItem("Lead Assignment"));
        _listComboBox[0].SelectedIndex = 0;
        e.Container.Controls.Add(_listComboBox[0]);
        List<String> _emp = new List<string>();
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

                e.Container.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
            }
        }

//        foreach (String sguid in Session["ghirarchy"].ToString().Split(','))
//        {
//            if (sguid.Length != 0)
//            {
//                _listComboBox.Add(new ComboBox());
//                _listComboBox[_listComboBox.Count - 1].ID = String.Format("History{0}", sguid);
//                _listComboBox[_listComboBox.Count - 1].Width = Unit.Pixel(120);
//                _listComboBox[_listComboBox.Count - 1].Height = Unit.Pixel(15);
//                _listComboBox[_listComboBox.Count - 1].Mode = ComboBoxMode.TextBox;
//                _listComboBox[_listComboBox.Count - 1].Enabled = false;
//                _listComboBox[_listComboBox.Count - 1].Items.Add(new ComboBoxItem(_dtEmployee.Select(String.Format("Id='{0}'", sguid))[0]["DisplayName"].ToString()));
//                _listComboBox[_listComboBox.Count - 1].SelectedIndex = 0;
//
//                e.Container.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
//            }
//        }
    }

    // Create the methods that will load the data into the templates		

    //------------------------------------------------------------------------
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
        oTextBox.ID = "txtOrderDate";
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
    //-
}