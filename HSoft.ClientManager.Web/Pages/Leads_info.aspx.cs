using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using HSoft.SQL;

using OboutInc.Calendar2;
using Obout.Grid;
using Obout.Grid.Design;

public partial class Pages_Leads_info : System.Web.UI.Page
{
    DataTable _dtEmails = new DataTable();
    DataTable _gridLeads = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            String ssql = "SELECT Id,shortCode,Text FROM _LeadEmail WHERE isdeleted = 0";
            _dtEmails = _sql.GetTable(ssql);
            _sql.Close();
        }
       
        gridLeads.ID = "gridLeds";
            gridLeads.AllowAddingRecords = false;
            gridLeads.AllowFiltering = true;

            // grid scrolloIg
            gridLeads.ScrollingSettings.FixedColumnsPosition = GridFixedColumnsPositionType.Left;
            gridLeads.ScrollingSettings.NumberOfFixedColumns = 2;
            gridLeads.ScrollingSettings.ScrollWidth = new Unit(100, UnitType.Percentage);
            gridLeads.ScrollingSettings.ScrollHeight = 500;

            gridLeads.CallbackMode = true;
            // gridLeads.Serialize = true;
            gridLeads.AutoGenerateColumns = false;
            gridLeads.Width = Unit.Percentage(100);

            gridLeads.PageSize = 50;
            gridLeads.AllowPaging = true;
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
            Edit.Width = "100";
            Edit.Wrap = true;
            gridLeads.Columns.Add(Edit);
        
            Column Id = new Column();
            Id.DataField = "Id";
            Id.HeaderText = "Id";
            Id.Visible = false;
            gridLeads.Columns.Add(Id);

            Column Number = new Column();
            Number.DataField = "Number";
            Number.HeaderText = "Number";
            Number.Width = "70";
            Number.AllowFilter = false;
            Number.ReadOnly = true;
            gridLeads.Columns.Add(Number);

            Column eMail = new Column();
            eMail.AllowFilter = true;
            eMail.DataField = "Email";
            eMail.HeaderText = "eMail";
            eMail.Width = "160";
            eMail.Wrap = true;
            eMail.ReadOnly = true;
            gridLeads.Columns.Add(eMail);

            Column Name = new Column();
            Name.AllowFilter = true;
            Name.DataField = "Name";
            Name.HeaderText = "Name";
            Name.Width = "150";
            Name.ReadOnly = true;
            gridLeads.Columns.Add(Name);

            SortedList<String, CheckBoxColumn> LeadMail = new SortedList<String, CheckBoxColumn>();
            foreach (DataRow dr in _dtEmails.Rows)
            {
                LeadMail.Add(dr["Id"].ToString(), new CheckBoxColumn());
                LeadMail[dr["Id"].ToString()].AllowFilter = false;
                LeadMail[dr["Id"].ToString()].AllowSorting = false;
                LeadMail[dr["Id"].ToString()].DataField = String.Format("Email_{0}", dr["shortCode"].ToString().Trim());
                LeadMail[dr["Id"].ToString()].HeaderText = dr["shortCode"].ToString().Trim();
                LeadMail[dr["Id"].ToString()].Width = "40";
                gridLeads.Columns.Add(LeadMail[dr["Id"].ToString()]);
            }
            
            Column entryDate = new Column();
            entryDate.AllowFilter = false;
            entryDate.DataField = "OrigEntryDate";
            entryDate.HeaderText = "Entry Date";
            entryDate.Width = "90";
            entryDate.DataFormatString = "{0:MM/dd/yyyy}";
            entryDate.ReadOnly = true;
            gridLeads.Columns.Add(entryDate);

            Column callLaterDate = new Column();
            callLaterDate.AllowFilter = true;
            callLaterDate.DataField = "CallLaterDate";
            callLaterDate.HeaderText = "Call Later";
            callLaterDate.Width = "140";
            callLaterDate.NullDisplayText = "missing";
            callLaterDate.DataFormatString = "{0:MM/dd/yyyy}";
            callLaterDate.ReadOnly = false;

            FilterOptionTemplateSettings fots = new FilterOptionTemplateSettings();
            fots.FilterTemplateId = "callLaterDateBetweenFilter";
            fots.FilterControlsIds = "StartDate_callLaterDate,EndDate_callLaterDate";
            fots.FilterControlsPropertyNames = "value,value";

            CustomFilterOption cfo = new CustomFilterOption();
            cfo.IsDefault = true;
            cfo.ID = "Between_callLaterDate";
            cfo.Text = "Between";
            cfo.TemplateSettings = fots;

            FilterCriteria fc = new FilterCriteria();
            fc.Values = new Hashtable();
            fc.Values["StartDate_callLaterDate"] = "1/1/1994";
            fc.Values["EndDate_callLaterDate"] = "10/1/1994";
            fc.Option = cfo;

            callLaterDate.FilterOptions.Add(cfo);
            callLaterDate.FilterCriteria = fc;
            
            gridLeads.Columns.Add(callLaterDate);

            Column empName = new Column();
            empName.AllowFilter = true;
            empName.DataField = "empName";
            empName.HeaderText = "Assigned To";
            empName.Width = "150";
            empName.ReadOnly = true;
            gridLeads.Columns.Add(empName);

            Column cNote = new Column();
            cNote.AllowFilter = true;
            cNote.AllowSorting = false;
            cNote.DataField = "cNote";
            cNote.HeaderText = "Comment";
            cNote.Width = "300";
            cNote.ReadOnly = true;
            cNote.Wrap = true;
            gridLeads.Columns.Add(cNote);

            Column eNote = new Column();
            eNote.AllowFilter = true;
            eNote.AllowSorting = false;
            eNote.DataField = "eNote";
            eNote.HeaderText = "Notes";
            eNote.Width = "300";
            eNote.ReadOnly = true;
            eNote.Wrap = true;
            gridLeads.Columns.Add(eNote);

//            if (!Page.IsPostBack)
//            {
                CreateLeadsGrid();
//            }

    }
    public void CreateLeadsGrid()
    {
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
        String ssql = String.Format("SELECT TOP {0} c.Id, " +
                                "       c.Number, " +
                                "       LTRIM(RTRIM(c.FirstName)) + ' ' + LTRIM(RTRIM(c.LastName)) Name, " +
                                "       c.OrigEntryDate OrigEntryDate, " +
                                "       CONVERT(nvarchar(10),l.CallLaterDate,103) CallLaterDate, " +
                                "       e.Email Email, " +
                                "       LTRIM(RTRIM(emp.FirstName)) + ' ' + LTRIM(RTRIM(emp.LastName)) empName, ", 50);
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
        _gridLeads = _sql.GetTable(ssql);

        gridLeads.DataSource = _gridLeads;
        gridLeads.DataBind();

        _sql.Close();

    }


}