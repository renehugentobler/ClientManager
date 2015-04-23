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

using Obout.Grid;
using Obout.ComboBox;
using OboutInc.Window;
using Obout.SuperForm;
using OboutInc.Calendar2;
using Obout.Interface;

using HSoft.ClientManager.Web;

public partial class Pages_TabInfo_Status : System.Web.UI.Page
{

    Grid grid1 = new Grid();
    private Window window1;
    private SuperForm SuperForm1;

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }
       
        grid1.ID = "grid1";
        grid1.CallbackMode = false;
        grid1.Serialize = false;
        grid1.AutoGenerateColumns = false;
        grid1.PageSize = -1;
        grid1.AllowFiltering = false;
        grid1.AllowSorting = false;
        grid1.Height = int.Parse(Session["wy"].ToString()) - 75;

        grid1.FolderStyle = "styles/grid/premiere_blue";
        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        grid1.ClientSideEvents.OnBeforeClientEdit = "Grid1_ClientEdit";
        grid1.ClientSideEvents.ExposeSender = true;
        grid1.ClientSideEvents.OnClientSelect = "onClientSelect";

        Column oColi = new Column();
        oColi.ID = "ID";
        oColi.DataField = "Id";
        oColi.Visible = false;
        oColi.ReadOnly = true;
        oColi.HeaderText = "ID";
        oColi.Width = "150";

        Column oCol1 = new Column();
        oCol1.ID = "Name";
        oCol1.DataField = "Name";
        oCol1.HeaderText = "Name";
        oCol1.Visible = true;
        oCol1.ReadOnly = true;
        oCol1.Width = "180";
        oCol1.Wrap = true;

        Column oCol2 = new Column();
        oCol2.ID = "Column3";
        oCol2.DataField = "EMail";
        oCol2.HeaderText = "EMail";
        oCol2.Width = "180";
        oCol2.Wrap = true;

        Column oCol3 = new Column();
        oCol3.ID = "Phone";
        oCol3.DataField = "Phone";
        oCol3.HeaderText = "Phone";
        oCol3.Visible = true;
        oCol3.ReadOnly = true;
        oCol3.Width = "90";

        Column oCol4 = new Column();
        oCol4.ID = "LastUpdate";
        oCol4.DataField = "updatedate";
        oCol4.HeaderText = "LastUpdate";
        oCol4.Visible = true;
        oCol4.ReadOnly = true;
        oCol4.Width = "80";
        oCol4.DataFormatString = "{0:MM/dd/yyyy}";

        Column oCol5 = new Column();
        oCol5.ID = "EntryDate";
        oCol5.DataField = "EntryDate";
        oCol5.HeaderText = "Entry Date";
        oCol5.Visible = true;
        oCol5.ReadOnly = true;
        oCol5.Width = "80";
        oCol5.DataFormatString = "{0:MM/dd/yyyy}";

        Column oCol7 = new Column();
        oCol7.ID = "Priority";
        oCol7.DataField = "Priority";
        oCol7.HeaderText = "Priority";
        oCol7.Visible = true;
        oCol7.ReadOnly = true;
        oCol7.Width = "80";

        Column oCol10 = new Column();
        oCol10.ID = "AssignedTo";
        oCol10.DataField = "AssignedTo";
        oCol10.HeaderText = "AssignedTo";
        oCol10.Visible = true;
        oCol10.ReadOnly = false;
        oCol10.Width = "140";

        Column oCol11 = new Column();
        oCol11.ID = "LeadNote";
        oCol11.DataField = "LeadNote";
        oCol11.HeaderText = "LeadNote";
        oCol11.Visible = true;
        oCol11.ReadOnly = true;
        oCol11.Width = "350";
        oCol11.Wrap = true;
        oCol11.AllowSorting = false;
        oCol11.ParseHTML = true;
        oCol11.AllowFilter = true;

        Column oCol12 = new Column();
        oCol12.ID = "SalesNote";
        oCol12.DataField = "SalesNote";
        oCol12.HeaderText = "Sales Note";
        oCol12.Visible = true;
        oCol12.ReadOnly = true;
        oCol12.Width = "450";
        oCol12.Wrap = true;
        oCol12.AllowSorting = false;
        oCol12.ParseHTML = true;
        oCol12.AllowFilter = true;

        Column oCol15 = new Column();
        oCol15.ID = "Column15";
        oCol15.DataField = "Survey";
        oCol15.HeaderText = " ";
        oCol15.Visible = true;
        oCol15.ReadOnly = true;
        oCol15.Width = "35";
        oCol15.AllowSorting = false;
        oCol15.AllowFilter = false;
        oCol15.ParseHTML = true;
        oCol15.HtmlEncode = true;
        oCol15.ShowFilterCriterias = false;

        Column oCol25 = new Column();
        oCol25.ID = "TimeZone";
        oCol25.DataField = "TimeZone";
        oCol25.HeaderText = "Zone";
        oCol25.Width = "40";
        oCol25.AllowSorting = true;
        oCol25.Wrap = false;
        oCol25.AllowFilter = true;
        oCol25.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol25.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));

        Column oCol27 = new Column();
        oCol27.ID = "CallLaterDate";
        oCol27.DataField = "CallLaterDate";
        oCol27.HeaderText = "Call Later";
        oCol27.Width = "80";
        oCol27.AllowSorting = true;
        oCol27.Wrap = false;
        oCol27.AllowFilter = true;
        oCol27.DataFormatString = "{0:MM/dd/yyyy}";
        oCol27.NullDisplayText = "missing!";
        oCol27.ApplyFormatInEditMode = true;

        Column oCol28 = new Column();
        oCol28.ID = "Source";
        oCol28.DataField = "Source";
        oCol28.HeaderText = "Source";
        oCol28.Width = "80";
        oCol28.AllowSorting = true;
        oCol28.Wrap = false;
        oCol28.AllowFilter = true;

        grid1.Columns.Add(oColi);
        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol15);
        grid1.Columns.Add(oCol4);
        grid1.Columns.Add(oCol5);
        grid1.Columns.Add(oCol7);
        grid1.Columns.Add(oCol10);
        grid1.Columns.Add(oCol11);
        grid1.Columns.Add(oCol12);

        grid1.Columns.Add(oCol25);
        grid1.Columns.Add(oCol27);
        grid1.Columns.Add(oCol28);

        phGrid1.Controls.Add(grid1);

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
        SuperForm1.AutoGenerateEditButton = true;
        SuperForm1.AutoGenerateDeleteButton = false;
        SuperForm1.DataKeyNames = new string[] { "Id" };
        SuperForm1.DefaultMode = DetailsViewMode.Insert;

        Obout.SuperForm.BoundField field1 = new Obout.SuperForm.BoundField();
        field1.DataField = "Name";
        field1.HeaderText = "Name";
        field1.FieldSetID = "FieldSet1";
        field1.AllowEdit = true;
        field1.Enabled = true;

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
        field3.AllowEdit = true;
        field3.Enabled = true;

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
        field5.DataFormatString = "{0:MM/dd/yyyy HH:mm:ss}";
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
//        field0.EditItemTemplate = new AddButtonsItemTemplate();

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

    void RebindGrid(object sender, EventArgs e) { BindGrid(); }
    void BindGrid()
    {

        String sStatusId = "F3DC2498-6F4F-449E-813C-EFDA32A9D24A";
        String sparm1 = "7";
        if (!String.IsNullOrEmpty(Request.QueryString["sStatusId"])) { sStatusId = Request.QueryString["sStatusId"]; }
        if (!String.IsNullOrEmpty(Request.QueryString["sparm1"])) { sparm1 = Request.QueryString["sparm1"]; }

        if (sStatusId == "F3DC2498-6F4F-449E-813C-EFDA32A9D24A")
        {
            grid1.Columns[4].HeaderText = "Sold";
            grid1.Columns[8].Visible= false;
        }

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = String.Format("SELECT DISTINCT lf.Id, lf.ConstantContactID, lf.Customer, lf.Name, lf.EMail, lf.Phone, lf.EntryDate, lf.CallLaterDate, lf.SourceId, lf.Source, lf.PriorityId, lf.Priority, lf.StatusId, lf.Status, lf.MsgHistory, lf.LeadNote, dbo.fromHDR(lf.SalesNote) SalesNote, lf.AssignedToId, lf.AssignedTo, lf.TimeZone, lf.createdby, lf.createdate, lf.updatedby, lf.updatedate, lf.isdeleted " +
                             "       ,CASE WHEN sc.Respondent IS NULL AND sc2.EmailAddress IS NULL THEN '' " +
                             "        ELSE " +
                             "          '<button onclick=''return false;''>S</button>' " +
                             "        END Survey " +
//                             "  FROM EmployeeHirarchy eh, _LeadPriority lp, Lead_Flat lf" +
                             "  FROM _LeadPriority lp, Lead_Flat lf" +
                             "    LEFT JOIN SurveyCustomer sc ON lf.EMail = sc.Respondent " +
                             "    LEFT JOIN SurveyCustomer sc2 ON lf.EMail = sc2.EmailAddress " +
                             " WHERE lf.StatusId = '{2}' " +
//                             "   AND lf.AssignedToId = eh.SubEmployeeId " +
                             "   AND lp.Id = lf.PriorityId " +
////                             "   AND lp.IsLead = 1 " +
//                            "   AND eh.EmployeeId = '{0}' " +
////                             "   {1} " +
                             " ORDER BY lf.updatedate DESC, EntryDate DESC ", Session["guser"].ToString(), 
                             sparm1=="-1"?"":String.Format("AND CAST(lf.updatedate AS DATE) >= '{0:d}' ",DateTime.Now.Date.AddDays(int.Parse(sparm1)*-1)), 
                             sStatusId);
//        DataTable _dtLeads = _sql.GetTable(ssql);

        sdsLeadFlat.SelectCommand = ssql;

//        grid1.DataSource = _dtLeads;
//        grid1.DataBind();
        grid1.DataSourceID = "sdsLeadFlat";

        _sql.Close();
    }

    void UpdateRecord(object sender, GridRecordEventArgs e)
    {
    }

    public class AddButtonsItemTemplate2 : ITemplate
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

            templatePlaceHolder.Controls.Add(button1);
            templatePlaceHolder.Controls.Add(button2);
            templatePlaceHolder.Controls.Add(button3);
        }
    }

}