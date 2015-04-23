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
using Obout.SuperForm;

public partial class Pages_Leads_Sales : System.Web.UI.Page
{

    Grid grid1 = new Grid();

    DataTable _dtEmails = new DataTable();
    static DataTable _dtEmployee = new DataTable();
    DataTable _dtPriority = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
            // for testing only
            if (Session["guser"] == null) { Session["guser"] = "44F7B957-4AA9-466F-B5C8-8840586157B6"; }

            String ssql = String.Empty;

            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            ssql = "SELECT * FROM Employee WHERE isdeleted = 0";
            _dtEmployee = _sql.GetTable(ssql);

            ssql = "SELECT * FROM _LeadEmail WHERE isdeleted = 0";
            _dtEmails = _sql.GetTable(ssql);

            ssql = "SELECT * FROM _LeadPriority WHERE isdeleted = 0 ORDER BY Sequence";
            _dtPriority = _sql.GetTable(ssql);

            _sql.Close();

            sdsEmployee.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;
            sdsPriority.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;
            sdsGrid.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;

            ssql = "SELECT l.Id LeadId, " +
                   "       l.CustomerId CustomerId, " +
                   "       pn.Number LeadPhone , " +
                   "       c.Name CustomerName, " +
                   "       e.Email LeadEmail, " +
                   "       c.OrigEntryDate EntryDate, " +
                   "       l.CallLaterDate CallLater, " +
                   "       s.Name LeadSource, " +
                   "       pr.Name LeadPriority, " +
                   "       dbo.eHistoryFormat(";
            foreach (DataRow dr in _dtEmails.Rows)
            {
                ssql = String.Format("{0} COALESCE((SELECT '{1},' FROM LeadEmail e WHERE c.Id = e.CustomerId AND e.LeadEmailId = '{2}' AND e.isdeleted = 0),'') + ", ssql, dr["shortCode"].ToString().Trim(), dr["Id"]);
            }
            ssql = String.Format("{0}'') eHistory, " +
                                 "      nc.Note ClientNote,ne.Note SalesNote " +
                                 " FROM _LeadPriority pr, Lead l " +
                                 "   LEFT JOIN PhoneNumber pn ON pn.CustomerId = l.CustomerId AND pn.isdeleted = 0 AND pn.IsLead = 1 " +
                                 "   LEFT JOIN _LeadSource s ON s.Id = l.SourceId AND s.isdeleted = 0 " +
                                 "   LEFT JOIN Customer c ON c.Id = l.CustomerId AND c.isdeleted = 0 " +
                                 "   LEFT JOIN Email e ON c.Id = e.CustomerId AND e.isdeleted = 0 " +
                                 "   LEFT JOIN PhoneNumber p ON c.Id = p.CustomerId AND p.isdeleted = 0 AND p.IsLead = 1 " +
                                 "   LEFT JOIN Note nc ON c.Id = nc.CustomerId AND nc.NoteTypeId = (SELECT Id FROM _NoteType WHERE isCustomer=1 AND isEmployee = 0) " +
                                 "   LEFT JOIN Note ne ON c.Id = ne.CustomerId AND ne.NoteTypeId = (SELECT Id FROM _NoteType WHERE isCustomer=1 AND isEmployee = 1 AND isPrivate=0) " +
                                 "   INNER JOIN Employee emp ON emp.Id = l.AssignedToId  AND emp.isdeleted = 0 " +
                                 " WHERE l.isdeleted = 0 " +
                                 "   AND (pr.Id = l.PriorityId AND pr.isdeleted = 0 AND pr.IsLead = 1) ", ssql);
            // Employee restrictions
            ssql = String.Format("{0} AND emp.Id = '{1}' ", ssql, Session["guser"].ToString());
            ssql = String.Format("{0} ORDER BY c.OrigEntryDate DESC, s.Sequence, l.CallLaterDate, pr.Sequence ", ssql);
            sdsGrid.SelectCommand = ssql;

            ComboBox _ComboBox = new ComboBox();
            _ComboBox.ID = "SalesName";
            _ComboBox.Width = Unit.Pixel(250);
            _ComboBox.Height = Unit.Pixel(15);
            _ComboBox.EmptyText = "Unknow Salesperson";
            _ComboBox.Mode = ComboBoxMode.TextBox;
            _ComboBox.Enabled = false;

            _ComboBox.Items.Add(new ComboBoxItem(_dtEmployee.Select(String.Format("Id='{0}'", Session["guser"]))[0]["DisplayName"].ToString()));
            _ComboBox.SelectedIndex = 0;

            cbName.Controls.Add(_ComboBox);

            grid1 = new Grid();
            grid1.ID = "Grid1";
            grid1.DataSourceID = "sdsGrid";
            grid1.Serialize = false;
            grid1.AutoGenerateColumns = false;
            grid1.PageSize = -1;
            grid1.AllowFiltering = true;
            grid1.AllowAddingRecords = false;
            grid1.AllowColumnReordering = true;
            grid1.AllowPageSizeSelection = false;
            grid1.AllowPaging = false;
            grid1.AllowSorting = true;
            grid1.Enabled = true;
            grid1.ShowLoadingMessage = true;

            grid1.ScrollingSettings.FixedColumnsPosition = GridFixedColumnsPositionType.Left;
            grid1.ScrollingSettings.NumberOfFixedColumns = 2;
            grid1.Width = Unit.Percentage(100);
            grid1.FolderStyle = "styles/grid/premiere_blue";

            grid1.TemplateSettings.RowEditTemplateId = "tplRowEdit";

            // creating the Template for editing Rows
            //------------------------------------------------------------------------
            GridRuntimeTemplate RowEditTemplate = new GridRuntimeTemplate();
            RowEditTemplate.ID = "tplRowEdit";
            RowEditTemplate.Template = new Obout.Grid.RuntimeTemplate();
            RowEditTemplate.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateRowEditTemplate);

            grid1.Templates.Add(RowEditTemplate);

            // grid filter
            GridFilteringSettings gfs = new GridFilteringSettings();
            gfs.FilterPosition = GridFilterPosition.Top;
            gfs.FilterLinksPosition = GridElementPosition.Top;
            gfs.InitialState = GridFilterState.Visible;
            grid1.FilteringSettings = gfs;

            // creating the columns

            Column oCol14 = new Column();
            oCol14.DataField = "";
            oCol14.Visible = true;
            oCol14.HeaderText = "";
            oCol14.Width = "120";
            oCol14.AllowEdit = true;
            oCol14.AllowDelete = false;

            Column oCol1 = new Column();
            oCol1.DataField = "LeadId";
            oCol1.ReadOnly = true;
            oCol1.HeaderText = "Lead Id";
            oCol1.Width = "150";
            oCol1.Visible = false;
            oCol1.TemplateSettings.RowEditTemplateControlId = "LeadId";
            oCol1.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol2 = new Column();
            oCol2.AllowFilter = true;
            oCol2.DataField = "CustomerName";
            oCol2.HeaderText = "Name";
            oCol2.Width = "160";
            oCol2.Wrap = true;
            oCol2.AllowSorting = true;
            oCol2.AllowFilter = true;
            oCol2.TemplateSettings.RowEditTemplateControlId = "SuperForm1_CustomerName";
            oCol2.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol3 = new Column();
            oCol3.AllowFilter = true;
            oCol3.DataField = "LeadEmail";
            oCol3.HeaderText = "EMail";
            oCol3.Width = "160";
            oCol3.Wrap = true;
            oCol3.AllowSorting = true;
            oCol3.AllowFilter = true;
            oCol3.TemplateSettings.RowEditTemplateControlId = "SuperForm1_LeadEmail";
            oCol3.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol3b = new Column();
            oCol3b.AllowFilter = true;
            oCol3b.DataField = "LeadPhone";
            oCol3b.HeaderText = "Phone";
            oCol3b.Width = "80";
            oCol3b.Wrap = true;
            oCol3b.AllowSorting = true;
            oCol3b.AllowFilter = true;
            oCol3b.TemplateSettings.RowEditTemplateControlId = "SuperForm1_LeadPhone";
            oCol3b.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol4 = new Column();
            oCol4.AllowFilter = false;
            oCol4.DataField = "EntryDate";
            oCol4.HeaderText = "Entry Date";
            oCol4.Width = "90";
            oCol4.AllowSorting = true;
            oCol4.AllowFilter = true;
            oCol4.NullDisplayText = "missing!";
            oCol4.DataFormatString = "{0:yyyy/MM/dd}";
            oCol4.TemplateSettings.RowEditTemplateControlId = "SuperForm1_EntryDate";
            oCol4.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol5 = new Column();
            oCol5.AllowFilter = false;
            oCol5.DataField = "CallLater";
            oCol5.HeaderText = "Call Later";
            oCol5.Width = "90";
            oCol5.DataFormatString = "{0:yyyy/MM/dd}";
            oCol5.AllowSorting = true;
            oCol5.AllowFilter = true;
            oCol5.NullDisplayText = "missing!";
            oCol5.TemplateSettings.RowEditTemplateControlId = "SuperForm1_CallLater";
            oCol5.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol6 = new Column();
            oCol6.DataField = "LeadSource";
            oCol6.HeaderText = "Source";
            oCol6.Width = "100";
            oCol6.AllowSorting = true;
            oCol6.AllowFilter = true;
            oCol6.TemplateSettings.RowEditTemplateControlId = "SuperForm1_LeadSource";
            oCol6.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol7 = new Column();
            oCol7.DataField = "LeadPriority";
            oCol7.HeaderText = "Priority";
            oCol7.Width = "80";
            oCol7.AllowSorting = true;
            oCol7.AllowFilter = true;
            oCol7.TemplateSettings.RowEditTemplateControlId = "SuperForm1_LeadPriority";
            oCol7.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol8 = new Column();
            oCol8.AllowFilter = true;
            oCol8.DataField = "eHistory";
            oCol8.HeaderText = "History";
            oCol8.Width = "60";
            oCol8.ReadOnly = true;
            oCol8.AllowSorting = false;
            oCol8.AllowFilter = false;
            oCol8.TemplateSettings.RowEditTemplateControlId = "SuperForm1_eHistory";
            oCol8.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol9 = new Column();
            oCol9.AllowFilter = true;
            oCol9.AllowSorting = false;
            oCol9.DataField = "ClientNote";
            oCol9.HeaderText = "Comment";
            oCol9.Width = "300";
            oCol9.Wrap = true;
            oCol9.TemplateSettings.RowEditTemplateControlId = "SuperForm1_ClientNote";
            oCol9.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            Column oCol10 = new Column();
            oCol10.AllowFilter = true;
            oCol10.AllowSorting = false;
            oCol10.DataField = "SalesNote";
            oCol10.HeaderText = "Comment";
            oCol10.Width = "300";
            oCol10.Wrap = true;
            oCol10.TemplateSettings.RowEditTemplateControlId = "SuperForm1_SalesNote";
            oCol10.TemplateSettings.RowEditTemplateControlPropertyName = "value";

            // add the columns to the Columns collection of the grid
            grid1.Columns.Add(oCol14);

            grid1.Columns.Add(oCol1);
            grid1.Columns.Add(oCol2);
            grid1.Columns.Add(oCol3);
            grid1.Columns.Add(oCol3b);
            grid1.Columns.Add(oCol4);
            grid1.Columns.Add(oCol5);
            grid1.Columns.Add(oCol6);
            grid1.Columns.Add(oCol7);
            grid1.Columns.Add(oCol8);

            grid1.Columns.Add(oCol9);
            grid1.Columns.Add(oCol10);

            // add the grid to the controls collection of the PlaceHolder        
            phGrid1.Controls.Add(grid1);

    }

    public void CreateRowEditTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPH1 = new PlaceHolder();
        e.Container.Controls.Add(oPH1);
        oPH1.DataBinding += new EventHandler(DataBindRowEditTemplate);
    }
    protected void DataBindRowEditTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPH1 = sender as PlaceHolder;
        Obout.Grid.TemplateContainer oContainer = oPH1.NamingContainer as Obout.Grid.TemplateContainer;

        Literal hiddenInput = new Literal();
        hiddenInput.Text = "<input type=\"hidden\" id=\"LeadId\" />";

        SuperForm SuperForm1 = new SuperForm();
        SuperForm1.ID = "SuperForm1";
//        SuperForm1.DataSourceID = "SqlDataSource2";
        SuperForm1.AutoGenerateRows = false;
        SuperForm1.AutoGenerateInsertButton = false;
        SuperForm1.AutoGenerateEditButton = false;
        SuperForm1.AutoGenerateDeleteButton = false;
        SuperForm1.AutoGenerateDateFields = false;
        SuperForm1.DataKeyNames = new string[] { "LeadId" };
        SuperForm1.Width = Unit.Percentage(99);
        SuperForm1.DefaultMode = DetailsViewMode.Insert;
        SuperForm1.EnableButtonsOnChange = true;
        SuperForm1.ItemCommand += SuperForm1_ItemCommand;
        SuperForm1.ModeChanging += SuperForm1_ModeChaning;

        Obout.SuperForm.BoundField field2 = new Obout.SuperForm.BoundField();
        field2.DataField = "CustomerName";
        field2.HeaderText = "Name";
        field2.ReadOnly = true;
        field2.FieldSetID = "FieldSet1";
        field2.Enabled = false;

        Obout.SuperForm.BoundField field3 = new Obout.SuperForm.BoundField();
        field3.DataField = "LeadEmail";
        field3.HeaderText = "EMail";
        field3.ReadOnly = true;
        field3.FieldSetID = "FieldSet1";
        field3.Enabled = false;

        Obout.SuperForm.BoundField field3b = new Obout.SuperForm.BoundField();
        field3b.DataField = "LeadPhone";
        field3b.HeaderText = "Phone";
        field3b.ReadOnly = true;
        field3b.FieldSetID = "FieldSet1";
        field3b.Enabled = false;
        field3b.HtmlEncode = false;

        Obout.SuperForm.DateField field4 = new Obout.SuperForm.DateField();
        field4.DataField = "EntryDate";
        field4.HeaderText = "Entry Date";
        field4.FieldSetID = "FieldSet1";
        field4.DataFormatString = "{0:MM/dd/yyyy HH:mm:ss}";
        field4.ReadOnly = true;
        field4.Enabled = false;
        field4.ApplyFormatInEditMode = true;

        Obout.SuperForm.DateField field5 = new Obout.SuperForm.DateField();
        field5.DataField = "CallLater";
        field5.HeaderText = "Call Later";
        field5.FieldSetID = "FieldSet1";
        field5.DataFormatString = "{0:MM/dd/yyyy}";
        field5.ReadOnly = false;
        field5.Enabled = true;
        field5.ApplyFormatInEditMode = true;
        field5.Required = true;

        Obout.SuperForm.BoundField field6 = new Obout.SuperForm.BoundField();
        field6.DataField = "LeadSource";
        field6.HeaderText = "Source";
        field6.AllowEdit = false;
        field6.Enabled = false;
        field6.FieldSetID = "FieldSet1";

        Obout.SuperForm.DropDownListField field7 = new Obout.SuperForm.DropDownListField();
        field7.DataField = "LeadPriority";
        field7.DataTextField = "Name";
        field7.DataValueField = "Name";
        field7.HeaderText = "Priority";
        field7.FieldSetID = "FieldSet1";
        field7.DataSourceID = "sdsPriority";

        Obout.SuperForm.BoundField field8 = new Obout.SuperForm.BoundField();
        field8.DataField = "eHistory";
        field8.HeaderText = "History";
        field8.AllowEdit = false;
        field8.Enabled = false;
        field8.FieldSetID = "FieldSet1";

        Obout.SuperForm.MultiLineField field21 = new Obout.SuperForm.MultiLineField();
        field21.DataField = "ClientNote";
        field21.HeaderText = String.Empty;
        field21.AllowEdit = false;
        field21.Enabled = false;
        field21.FieldSetID = "FieldSet2";
        field21.HeaderStyle.Width = 1;

        Obout.SuperForm.MultiLineField field31 = new Obout.SuperForm.MultiLineField();
        field31.DataField = "SalesNote";
        field31.HeaderText = String.Empty;
        field31.AllowEdit = true;
        field31.Enabled = true;
        field31.FieldSetID = "FieldSet2";
        field31.HeaderStyle.Width = 1;

        Obout.SuperForm.TemplateField field41 = new Obout.SuperForm.TemplateField();
        field41.FieldSetID = "FieldSet1";
        field41.EditItemTemplate = new ButtonsEditItemTemplate();

        Obout.SuperForm.FieldSetRow fieldSetRow1 = new Obout.SuperForm.FieldSetRow();
        Obout.SuperForm.FieldSet fieldSet1 = new Obout.SuperForm.FieldSet();
        fieldSet1.ID = "FieldSet1";
        fieldSet1.Title = "Lead Information";
        fieldSet1.CssClass = "lead-information";

        Obout.SuperForm.FieldSet fieldSet2 = new Obout.SuperForm.FieldSet();
        fieldSet2.ID = "FieldSet2";
        fieldSet2.ColumnSpan = 3;
        fieldSet2.Title = "Notes";

        Obout.SuperForm.FieldSet fieldSet3 = new Obout.SuperForm.FieldSet();
        fieldSet3.ID = "FieldSet3";
        fieldSet3.Title = "Additional Information";

        fieldSetRow1.Items.Add(fieldSet1);
        fieldSetRow1.Items.Add(fieldSet2);
//        fieldSetRow1.Items.Add(fieldSet3);

//        Obout.SuperForm.FieldSetRow fieldSetRow2 = new Obout.SuperForm.FieldSetRow();
//        Obout.SuperForm.FieldSet fieldSet4 = new Obout.SuperForm.FieldSet();
//        fieldSet4.ID = "FieldSet4";
//        fieldSet4.ColumnSpan = 3;
//        fieldSet4.CssClass = "command-row";
//        fieldSetRow2.Items.Add(fieldSet4);

        SuperForm1.FieldSets.Add(fieldSetRow1);
//        SuperForm1.FieldSets.Add(fieldSetRow2);

        SuperForm1.Fields.Add(field7);

        SuperForm1.Fields.Add(field2);
        SuperForm1.Fields.Add(field3);
        SuperForm1.Fields.Add(field3b);
        SuperForm1.Fields.Add(field4);
        SuperForm1.Fields.Add(field5);
        SuperForm1.Fields.Add(field6);
        SuperForm1.Fields.Add(field8);

        SuperForm1.Fields.Add(field21);
        SuperForm1.Fields.Add(field31);

        SuperForm1.Fields.Add(field41);

        oPH1.Controls.Add(hiddenInput);
        oPH1.Controls.Add(SuperForm1);
    }

    protected void SuperForm1_ModeChaning(object sender, DetailsViewModeEventArgs e)
    {
        if (1 == 1)
        {
        }
    }

    protected void SuperForm1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Cancel":
                {
                }
                break;
            case "Save":
                {
                }
                break;
            default: // oops
                {
                }
                break;
        }
    }

    public class ButtonsEditItemTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            PlaceHolder templatePlaceHolder = new PlaceHolder();
            container.Controls.Add(templatePlaceHolder);

            Obout.Interface.OboutButton save = new Obout.Interface.OboutButton();
            Obout.Interface.OboutButton cancel = new Obout.Interface.OboutButton();

            templatePlaceHolder.Controls.Add(save);
            templatePlaceHolder.Controls.Add(cancel);

            save.Text = "Save";
            save.CommandName = "Save";
//            save.OnClientClick = "Grid1.save(); return false;";
            save.Width = Unit.Pixel(75);

            cancel.Text = "Cancel";
//            cancel.OnClientClick = "Grid1.cancel(); return false;";
            save.CommandName = "Cancel";
            cancel.Width = Unit.Pixel(75);
        }
    }
}