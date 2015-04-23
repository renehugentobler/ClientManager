using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using System.Data.SqlClient;

using System.Web.UI.HtmlControls;

using HSoft.SQL;

using OboutInc.Calendar2;
using Obout.Grid;
using Obout.Grid.Design;
using Obout.Interface;


public partial class Pages_Leads_test : System.Web.UI.Page
{

    Grid gridLeads = new Grid();
    DataTable _dtEmails = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            String ssql = "SELECT Id,shortCode,Text FROM _LeadEmail WHERE isdeleted = 0";
            _dtEmails = _sql.GetTable(ssql);
            _sql.Close();
        }

        gridLeads.ID = "gridLeads";
        gridLeads.CallbackMode = true;
        gridLeads.Serialize = true;
        gridLeads.AutoGenerateColumns = false;

        gridLeads.FolderStyle = "styles/grid/premiere_blue";
        gridLeads.AllowFiltering = true;

        // grid scrolloIg
        gridLeads.ScrollingSettings.FixedColumnsPosition = GridFixedColumnsPositionType.Left;
        gridLeads.ScrollingSettings.NumberOfFixedColumns = 2;
        gridLeads.ScrollingSettings.ScrollWidth = new Unit(100, UnitType.Percentage);
        gridLeads.ScrollingSettings.ScrollHeight = 500;

        gridLeads.PageSize = 20;
        gridLeads.AllowPageSizeSelection = false;

        // grid filter
        GridFilteringSettings gfs = new GridFilteringSettings();
        gfs.FilterPosition = GridFilterPosition.Top;
        gfs.FilterLinksPosition = GridElementPosition.Top;
        gfs.InitialState = GridFilterState.Visible;
        gridLeads.FilteringSettings = gfs;

        Column Edit = new Column();
        Edit.AllowEdit = true;
        Edit.AllowDelete = false;
        Edit.Width = "120";

        Column Id = new Column();
        Id.DataField = "Id";
        Id.HeaderText = "Id";
        Id.Visible = false;

        Column Number = new Column();
        Number.DataField = "Number";
        Number.HeaderText = "Number";
        Number.Width = "80";
        Number.AllowFilter = false;
        Number.ReadOnly = true;

        Column eMail = new Column();
        eMail.AllowFilter = true;
        eMail.DataField = "Email";
        eMail.HeaderText = "eMail";
        eMail.Width = "160";
        eMail.Wrap = true;
        eMail.ReadOnly = true;

        Column Name = new Column();
        Name.AllowFilter = true;
        Name.DataField = "Name";
        Name.HeaderText = "Name";
        Name.Width = "150";
        Name.ReadOnly = true;

        Column entryDate = new Column();
        entryDate.AllowFilter = false;
        entryDate.DataField = "OrigEntryDate";
        entryDate.HeaderText = "Entry Date";
        entryDate.Width = "100";
        entryDate.DataFormatString = "{0:MM/dd/yyyy}";
        entryDate.ReadOnly = true;

        Column callLaterDate = new Column();
        callLaterDate.AllowFilter = true;
        callLaterDate.DataField = "CallLaterDate";
        callLaterDate.HeaderText = "Call Later";
        callLaterDate.Width = "170";
        callLaterDate.NullDisplayText = "missing";
        callLaterDate.DataFormatString = "{0:MM/dd/yyyy}";
        callLaterDate.ReadOnly = false;

        CustomFilterOption NoFilter_CallLaterDate = new CustomFilterOption();
        NoFilter_CallLaterDate.ID = "NoFilter_CallLaterDate";
        NoFilter_CallLaterDate.Text = "No Filter";
        NoFilter_CallLaterDate.IsDefault = true;
        NoFilter_CallLaterDate.TemplateSettings.FilterControlsIds = "StartDate_CallLaterDate,EndDate_CallLaterDate";
        NoFilter_CallLaterDate.TemplateSettings.FilterControlsPropertyNames = "value,value";
        NoFilter_CallLaterDate.TemplateSettings.FilterTemplateId = "CallLaterDateNoFilterFilter";

        CustomFilterOption Between_CallLaterDate = new CustomFilterOption();
        Between_CallLaterDate.ID = "Between_CallLaterDate";
        Between_CallLaterDate.Text = "Between";
        Between_CallLaterDate.TemplateSettings.FilterControlsIds = "StartDate_CallLaterDate,EndDate_CallLaterDate";
        Between_CallLaterDate.TemplateSettings.FilterControlsPropertyNames = "value,value";
        Between_CallLaterDate.TemplateSettings.FilterTemplateId = "CallLaterDateBetweenFilter";

        CustomFilterOption IsMissing_CallLaterDate = new CustomFilterOption();
        IsMissing_CallLaterDate.ID = "IsMissing";
        IsMissing_CallLaterDate.Text = "Is Missing";
        IsMissing_CallLaterDate.TemplateSettings.FilterControlsIds = "StartDate_CallLaterDate,EndDate_CallLaterDate";
        IsMissing_CallLaterDate.TemplateSettings.FilterControlsPropertyNames = "value,value";
        IsMissing_CallLaterDate.TemplateSettings.FilterTemplateId = "CallLaterDateMissingFilter";

        callLaterDate.FilterCriteria.Option.Type = FilterOptionType.Custom;
        callLaterDate.FilterOptions.Add(Between_CallLaterDate);
        callLaterDate.FilterOptions.Add(IsMissing_CallLaterDate);
        callLaterDate.FilterOptions.Add(NoFilter_CallLaterDate);

        GridRuntimeTemplate CallLaterDateBetweenFilter = new GridRuntimeTemplate();
        CallLaterDateBetweenFilter.ID = "CallLaterDateBetweenFilter";
        CallLaterDateBetweenFilter.Template = new Obout.Grid.RuntimeTemplate();
        CallLaterDateBetweenFilter.Template.CreateTemplate += new GridRuntimeTemplateEventHandler(CreateCallLaterDateBetweenFilterTemplate);

        FilterCriteria criteria1 = new FilterCriteria();
        // use "Between" filter option (with index 0 in FilterOptions collection)
        CustomFilterOption option1 = callLaterDate.FilterOptions[0] as CustomFilterOption;

        criteria1.Option = option1;
        criteria1.Values = new Hashtable();
        criteria1.Values["StartDate_CallLaterDate"] = DateTime.Now.Date.AddDays(-30).ToString("MM/dd/yyyy");
        criteria1.Values["EndDate_CallLaterDate"] = DateTime.Now.Date.ToString("MM/dd/yyyy");

        callLaterDate.FilterCriteria = criteria1;

        Column empName = new Column();
        empName.AllowFilter = true;
        empName.DataField = "empName";
        empName.HeaderText = "Assigned To";
        empName.Width = "150";
        empName.ReadOnly = true;

        Column cNote = new Column();
        cNote.AllowFilter = true;
        cNote.AllowSorting = false;
        cNote.DataField = "cNote";
        cNote.HeaderText = "Comment";
        cNote.Width = "300";
        cNote.ReadOnly = true;
        cNote.Wrap = true;

        Column eNote = new Column();
        eNote.AllowFilter = true;
        eNote.AllowSorting = false;
        eNote.DataField = "eNote";
        eNote.HeaderText = "Notes";
        eNote.Width = "300";
        eNote.ReadOnly = true;
        eNote.Wrap = true;

        gridLeads.Templates.Add(CallLaterDateBetweenFilter);

        gridLeads.Columns.Add(Edit);
        gridLeads.Columns.Add(Id);
        gridLeads.Columns.Add(Number);
        gridLeads.Columns.Add(eMail);
        gridLeads.Columns.Add(Name);
        gridLeads.Columns.Add(entryDate);
        gridLeads.Columns.Add(callLaterDate);
        gridLeads.Columns.Add(empName);
        gridLeads.Columns.Add(cNote);
        gridLeads.Columns.Add(eNote);

        // add the grid to the controls collection of the PlaceHolder
        phgridLeads.Controls.Add(gridLeads);

        gridLeads.Filtering += new EventHandler(gridLeads_Filtering);

        if (!Page.IsPostBack)
        {

            BindGrid();
        }		

    }

    public void CreateCallLaterDateBetweenFilterTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPhOdate = new PlaceHolder();
        e.Container.Controls.Add(oPhOdate);
        oPhOdate.DataBinding += new EventHandler(DataBindCallLaterDateBetweenFilterTemplate);
    }

    protected void DataBindCallLaterDateBetweenFilterTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPhCLdateBetween = sender as PlaceHolder;
        HtmlGenericControl divContainer = CreateDiv("tdText", "");
        divContainer.Controls.Add(CreateTextBox("StartDate_CallLaterDate", TextBoxMode.SingleLine, 49));
//        divContainer.Controls.Add(CreateDiv("separator", "-"));
        divContainer.Controls.Add(CreateTextBox("EndDate_CallLaterDate", TextBoxMode.SingleLine, 49));

        oPhCLdateBetween.Controls.Add(divContainer);
    }
    protected HtmlGenericControl CreateDiv(string className, string innerHTML)
    {
        HtmlGenericControl obDiv = new HtmlGenericControl("DIV");
        obDiv.Attributes.Add("class", className);
        obDiv.InnerHtml = innerHTML;
        return obDiv;
    }
    protected OboutTextBox CreateTextBox(string Id, TextBoxMode txtMode, int width)
    {
        OboutTextBox obTextBox = new OboutTextBox();
        obTextBox.ID = Id;
        obTextBox.Width = Unit.Percentage(width);
        obTextBox.TextMode = txtMode;
        return obTextBox;
    }

    protected void gridLeads_Filtering(object sender, EventArgs e)
    {
        // filter for OrderDate
        Column callLaterDateColumn = gridLeads.Columns.GetColumnByDataField("callLaterDate");
        
        if (callLaterDateColumn.FilterCriteria.Option is CustomFilterOption)
        {
            CustomFilterOption filterOption = callLaterDateColumn.FilterCriteria.Option as CustomFilterOption;
            switch (filterOption.ID)
            {
                case "IsMissing":
                    callLaterDateColumn.FilterCriteria.FilterExpression = "(" + callLaterDateColumn.DataField + " IS NULL)";
                    break;
                case "Between_CallLaterDate":
                    string startDate = callLaterDateColumn.FilterCriteria.Values["StartDate_CallLaterDate"].ToString();
                    string endDate = callLaterDateColumn.FilterCriteria.Values["EndDate_CallLaterDate"].ToString();

                    if (!string.IsNullOrEmpty(startDate) || !string.IsNullOrEmpty(endDate))
                    {
                        if (string.IsNullOrEmpty(startDate)) { startDate = endDate; }
                        if (string.IsNullOrEmpty(endDate)) { endDate = startDate; }
                        // we filter between start date at 12:00AM and end date at 11:59PM
                        callLaterDateColumn.FilterCriteria.FilterExpression = "(" + callLaterDateColumn.DataField + " IS NOT NULL AND (" + callLaterDateColumn.DataField + " >= #" + startDate + "# AND " + callLaterDateColumn.DataField + " <= #" + endDate + "#))";
                    }
                    else
                    {
                        callLaterDateColumn.FilterCriteria.FilterExpression = String.Empty;
                    }
                    break;
            }
        }
    }

    void BindGrid()
    {

        SqlConnection sqlconnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
        sqlconnection.Open();

        String ssql = String.Format("SELECT TOP {0} c.Id, " +
                                "       c.Number, " +
                                "       LTRIM(RTRIM(c.FirstName)) + ' ' + LTRIM(RTRIM(c.LastName)) Name, " +
                                "       c.OrigEntryDate OrigEntryDate, " +
                                "       CONVERT(nvarchar(10),l.CallLaterDate,103) CallLaterDate, " +
                                "       e.Email Email, " +
                                "       LTRIM(RTRIM(emp.FirstName)) + ' ' + LTRIM(RTRIM(emp.LastName)) empName, ", 100);
        foreach (DataRow dr in _dtEmails.Rows)
        {
            ssql = String.Format("{0} (SELECT COUNT(*) FROM LeadEmail e WHERE c.Id = e.CustomerId AND e.LeadEmailId = '{2}' AND e.isdeleted = 0) Email_{1} ,", ssql, dr["shortCode"], dr["Id"]);
        }
        ssql = String.Format("{0} nc.Note cNote,ne.Note eNote " +
                                " FROM Customer c " +
                                "   INNER JOIN Email e ON c.Id = e.CustomerId AND e.[Primary] = 1 AND e.isdeleted = 0 " +
                                "   INNER JOIN Lead l ON c.Id = l.CustomerId AND e.isdeleted = 0 " +
                                "   INNER JOIN Employee emp ON emp.Id = l.AssignedToId  AND emp.isdeleted = 0 " +
                                "   INNER JOIN PhoneNumber p ON c.Id = p.CustomerId AND p.isdeleted = 0 AND IsLead = 1 " +
                                "   INNER JOIN Note nc ON c.Id = nc.CustomerId AND nc.NoteTypeId = (SELECT Id FROM _NoteType WHERE isCustomer=1 AND isEmployee = 0) " +
                                "   INNER JOIN Note ne ON c.Id = ne.CustomerId AND ne.NoteTypeId = (SELECT Id FROM _NoteType WHERE isCustomer=1 AND isEmployee = 1 AND isPrivate=0) " +
                                " WHERE e.isdeleted = 0 " +
                                " ORDER BY OrigEntryDate DESC, Id " +
                                "", ssql);


        SqlCommand myComm = new SqlCommand(ssql, sqlconnection);

        if (sqlconnection.State != ConnectionState.Open)
        {
            try { sqlconnection.Close(); }
            catch { }
            if (sqlconnection.State == ConnectionState.Closed)
            {
                try { sqlconnection.Open(); }
                catch (SqlException _sex) { throw _sex; }
            }
            else
            {
                throw new Exception(String.Format("SQL ConnectionState : {1}", sqlconnection.State));
            }
        }

        SqlDataReader myReader = myComm.ExecuteReader();

        gridLeads.DataSource = myReader;
        gridLeads.DataBind();

        sqlconnection.Close();
    }


}