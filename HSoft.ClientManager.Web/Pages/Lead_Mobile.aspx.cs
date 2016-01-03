using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Obout.Grid;
using Obout.Interface;
using Obout.Ajax.UI;
using Obout.Ajax.UI.HTMLEditor;

using HSoft.SQL;

using HSoft.ClientManager.Web;

public partial class Lead_Mobile : System.Web.UI.Page
{

    private Grid grid1 = new Grid();

    static HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
    static String ssql = String.Empty;
    static DataTable _dtLeads = new DataTable();
    static String sassigned = String.Empty;

    static int maxdevicewidth = 480;

    protected void Page_Load(object sender, EventArgs e)
    {

        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        sassigned = Session["ghirarchy"].ToString();
        sassigned = String.Format("{0}{1}", sassigned.Replace(",", "','"), Guid.Empty);

        grid1.ID = "grid1";
        grid1.CallbackMode = true;
        grid1.Serialize = true;
        grid1.AutoGenerateColumns = false;
        grid1.AllowAddingRecords = false;
        grid1.AllowPageSizeSelection = true;
        grid1.PageSize = 5;
        grid1.ShowHeader = false;
        grid1.ShowFooter = false;
        grid1.NumberOfPagesShownInFooter = 7;
        grid1.ShowLoadingMessage = true;
        grid1.PagingSettings.ShowRecordsCount = false;
        grid1.PagingSettings.PageSizeSelectorPosition = GridElementPosition.Top;
        grid1.PagingSettings.Position = GridElementPosition.Top;
        grid1.ShowTotalNumberOfPages = false;
        grid1.PageSizeOptions = "1,5,10,25,-1";

        // grid1.TemplateSettings.RowEditTemplateId = "tplRowEdit";

        grid1.FolderStyle = "styles/grid/grand_gray";

        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        GridRuntimeTemplate FormTemplate = new GridRuntimeTemplate();
        FormTemplate.ID = "tplFormView";
        FormTemplate.Template = new Obout.Grid.RuntimeTemplate();
        FormTemplate.Template.CreateTemplate += new GridRuntimeTemplateEventHandler(CreateFormViewTemplate);

        grid1.Templates.Add(FormTemplate);

        // creating the columns
        Column oCol1 = new Column();
        oCol1.ID = "Id";
        oCol1.DataField = "Id";
        oCol1.HeaderText = "ID";
        oCol1.Width = maxdevicewidth.ToString();    // iphone 5
        oCol1.Visible = false;

        Column oCol2 = new Column();
        oCol2.ID = "Name";
        oCol2.DataField = "Name";
        oCol2.HeaderText = "Name";
        oCol2.Width = maxdevicewidth.ToString();    // iphone 5
        oCol2.Visible = false;

        Column oCol3 = new Column();
        oCol3.ID = "Column3";
        oCol3.DataField = "EMail";
        oCol3.HeaderText = "EMail";
        oCol3.Width = maxdevicewidth.ToString();    // iphone 5
        oCol3.Visible = false;

        Column oCol4 = new Column();
        oCol4.ID = "Phone";
        oCol4.DataField = "Phone";
        oCol4.HeaderText = "Phone";
        oCol4.Width = maxdevicewidth.ToString();    // iphone 5
        oCol4.Visible = false;

        Column oCol5 = new Column();
        oCol5.ID = "TimeZone";
        oCol5.DataField = "TimeZone";
        oCol5.HeaderText = "Zone";
        oCol5.Width = maxdevicewidth.ToString();    // iphone 5
        oCol5.Visible = false;

        Column oCol6 = new Column();
        oCol6.ID = "EntryDate";
        oCol6.DataField = "EntryDate";
        oCol6.HeaderText = "Entry Date";
        oCol6.Width = maxdevicewidth.ToString();    // iphone 5
        oCol6.Visible = false;

        Column oCol7 = new Column();
        oCol7.ID = "CallLaterDate";
        oCol7.DataField = "CallLaterDate";
        oCol7.HeaderText = "Call Later";
        oCol7.Width = maxdevicewidth.ToString();    // iphone 5
        oCol7.Visible = false;
        oCol7.TemplateSettings.TemplateId = "tplFormView";

        Column oCol9 = new Column();
        oCol9.ID = "Priority";
        oCol9.DataField = "Priority";
        oCol9.HeaderText = "Priority";
        oCol9.Width = maxdevicewidth.ToString();    // iphone 5
        oCol9.Visible = false;
        oCol9.TemplateSettings.TemplateId = "tplFormView";

        Column oCol14 = new Column();
        oCol14.ID = "Status";
        oCol14.DataField = "Status";
        oCol14.HeaderText = "Status";
        oCol14.Width = maxdevicewidth.ToString();    // iphone 5
        oCol14.Visible = false;

        Column oCol12 = new Column();
        oCol12.ID = "LeadNote";
        oCol12.DataField = "LeadNote";
        oCol12.HeaderText = "Lead Note";
        oCol12.Width = maxdevicewidth.ToString();    // iphone 5
        oCol12.ParseHTML = true;
        oCol12.HtmlEncode = true;
        oCol12.Visible = false;

        Column oCol13 = new Column();
        oCol13.ID = "Column13";
        oCol13.DataField = "SalesNote";
        oCol13.HeaderText = "Sales Note";
        oCol13.Width = maxdevicewidth.ToString();    // iphone 5
        oCol13.ParseHTML = true;
        oCol13.HtmlEncode = true;
        oCol13.Visible = true;
        oCol13.TemplateSettings.TemplateId = "tplFormView";

        // add the columns to the Columns collection of the grid
        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol4);
        grid1.Columns.Add(oCol5);
        grid1.Columns.Add(oCol6);
        grid1.Columns.Add(oCol7);
        grid1.Columns.Add(oCol9);
        grid1.Columns.Add(oCol14);
        grid1.Columns.Add(oCol12);
        grid1.Columns.Add(oCol13);

        // add the grid to the controls collection of the PlaceHolder        
        phGrid1.Controls.Add(grid1);

        if (!Page.IsPostBack)
        {
            CreateGrid();
        }
    }

    void CreateFormViewTemplate(object sender, GridRuntimeTemplateEventArgs e)
    {
        PlaceHolder oPH1 = new PlaceHolder();
        e.Container.Controls.Add(oPH1);
        oPH1.DataBinding += new EventHandler(DataBindFormViewTemplate);
    }

    protected void DataBindFormViewTemplate(Object sender, EventArgs e)
    {
        PlaceHolder oPH1 = sender as PlaceHolder;
        Obout.Grid.TemplateContainer oContainer = oPH1.NamingContainer as Obout.Grid.TemplateContainer;

        Table oTable = new Table();
        oTable.CssClass = "rowEditTable ob_gCW ob_gC_Fc";

        TableRow oTr = new TableRow();

        TableCell oCell1 = new TableCell();

        // For Ship Information
        Table oTableLeadInformation = new Table();

        oTableLeadInformation.Rows.Add(CreateTableRowWithLabel("Name:", "lblName", "<b>" + oContainer.DataItem["Name"].ToString() + "</b>", oContainer.PageRecordIndex.ToString()));
        oTableLeadInformation.Rows.Add(CreateTableRowWithLabel("Reach:", "lblEMail", "<b>"+oContainer.DataItem["Phone"].ToString() + "</b> " + oContainer.DataItem["TimeZone"].ToString(), oContainer.DataItem["EMail"].ToString()));
        oTableLeadInformation.Rows.Add(CreateTableRowWithLabel("Dates:", "lblDates",
                                                                String.Format("{0:MM/dd/yyyy}", DateTime.Parse(oContainer.DataItem["CallLaterDate"].ToString()).AddHours(3)),
                                                                String.Format("{0:MM/dd/yyyy hh:mmtt}", DateTime.Parse(oContainer.DataItem["EntryDate"].ToString()).AddHours(3))));
        oTableLeadInformation.Rows.Add(CreateTableRowWithLabel("Stats:", "lblStats",
                                                                oContainer.DataItem["Priority"].ToString(),
                                                                oContainer.DataItem["Status"].ToString()));
        if (oContainer.DataItem["LeadNote"].ToString().Length != 0)
        {
            oTableLeadInformation.Rows.Add(CreateTableRowWithLabel(null, "lblNote",
                                                                    oContainer.DataItem["LeadNote"].ToString(),
                                                                    null,1,BorderStyle.Dotted));
        }
        if (oContainer.DataItem["SalesNote"].ToString().Length != 0)
        {
            oTableLeadInformation.Rows.Add(CreateTableRowWithLabel(null, "lblSales",
                                                                    oContainer.DataItem["SalesNote"].ToString(),
                                                                    null, 1, BorderStyle.Solid));
        }

        oCell1.Controls.Add(oTableLeadInformation);
        oTr.Cells.Add(oCell1);
        oTable.Rows.Add(oTr);

/*
        TableRow oTr2 = new TableRow();
        TableCell oCell2_1 = new TableCell();
        oCell2_1.ColumnSpan = 3;
        oCell2_1.HorizontalAlign = HorizontalAlign.Left;

        Button oSaveEdit = new Button();
        oSaveEdit.CssClass = "tdTextSmall";
        oSaveEdit.OnClientClick = "grid1.editRecord(" + oContainer.PageRecordIndex + ");return false";
        oSaveEdit.Text = "Edit";

        oCell2_1.Controls.Add(oSaveEdit);

        oTr2.Controls.Add(oCell2_1);
        oTable.Rows.Add(oTr2);
*/
  
        oPH1.Controls.Add(oTable);
    }

    public TableRow CreateTableRowWithLabel(string sLabelText, string sControlId, string sValue, string sValue2, int border = 0, BorderStyle bstyle = BorderStyle.None)
    {
        TableRow oTr = new TableRow();
        TableCell oCell1 = new TableCell();
        TableCell oCell2 = new TableCell();
        TableCell oCell3 = new TableCell();

        Label oLabel = new Label();

        if (sLabelText == null)
        {
            sValue = sValue.Trim();

            sValue = sValue.Replace("<br><br>", "<br>");
            sValue = sValue.Replace("<br> <br>", "<br>");
            
            while (sValue.StartsWith("<br>"))
            {
                sValue = sValue.Remove(0, 4);
                sValue = sValue.Trim();
            }
            while (sValue.EndsWith("<br>"))
            {
                sValue = sValue.Remove(sValue.Length-4, 4);
                sValue = sValue.Trim();
            }

            sValue = sValue.Replace("HDR", "<font color=red>" + "HDR" + "</font>");
            sValue = sValue.Replace("hdr", "<font color=red>" + "hdr" + "</font>");
            sValue = sValue.Replace("FHM", "<font color=blue>" + "FHM" + "</font>");
            sValue = sValue.Replace("fhm", "<font color=blue>" + "fhm" + "</font>");

            oLabel.Text = sValue;
            oLabel.BorderWidth = border;
            oLabel.BorderStyle = bstyle;
            oLabel.ID = sControlId;
            oLabel.Width = Unit.Pixel(maxdevicewidth-12);
            oLabel.ToolTip = sValue;
            oCell1.Wrap = true;
            oCell1.Width = Unit.Pixel(maxdevicewidth - 8);
            oCell1.ColumnSpan = 3;
            oCell1.Controls.Add(oLabel);
            oTr.Cells.Add(oCell1);
        }
        else
        {
            oLabel.Text = sLabelText;
            oCell1.Controls.Add(oLabel);
            oCell1.Width = Unit.Percentage(0);

            Label oLabel2 = new Label();
            Label oLabel3 = new Label();

            oLabel2.ID = sControlId;
            if (sValue != null) { oLabel2.Text = sValue; }
            oCell2.Controls.Add(oLabel2);

            if (sValue2 != null)
            {
                oCell2.Width = Unit.Percentage(50);
                oCell3.Width = Unit.Percentage(50);
                oLabel3.ID = sControlId;
                if (sControlId == "lblName")
                {
                    oCell3.HorizontalAlign = HorizontalAlign.Right;

                    Button oSaveEdit = new Button();
                    oSaveEdit.CssClass = "tdTextSmall";
                    oSaveEdit.OnClientClick = "grid1.editRecord(" + sValue2 + ");return false";
                    oSaveEdit.Text = " Edit ";
                    oCell3.Controls.Add(oSaveEdit);

                    Label oLabelf = new Label();
                    oLabelf.Text = "&nbsp;&nbsp;";
                    oCell3.Controls.Add(oLabelf);

                    Button oEMail = new Button();
                    oEMail.CssClass = "tdTextSmall";
                    oEMail.OnClientClick = "openemail(); return false;";
                    oEMail.Text = " Mail ";
                    oCell3.Controls.Add(oEMail);

                }
                else
                {
                    oLabel3.Text = sValue2;
                    oCell3.Controls.Add(oLabel3);
                    oCell3.Wrap = true;
                }
            }
            else
            {
                oCell2.ColumnSpan = 2;
                oCell2.Width = Unit.Percentage(100);
            }

            oTr.Cells.Add(oCell1);
            oTr.Cells.Add(oCell2);
            if (sValue2 != null) { oTr.Cells.Add(oCell3); }
        }

        return oTr;
    }

    void CreateGrid()
    {
        if (_dtLeads.Rows.Count==0)
        {
            ssql = String.Format("SELECT TOP 50 " +
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
                // " ,l.MsgHistory" +
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
//                                 "         OR (p.Id='F3DC2498-6F4F-449E-813C-EFDA32A9D24A' AND '{1}' IN ('DCDB22C2-65F4-46E4-91D1-CC123F83DCE2','0BA4012E-5541-4A76-92BB-C7122344DC3A','7D5AA961-5478-4FA1-B5DB-D6A2071ED834')) " +
                                 "       ) " +
                                 "   AND l.isdeleted = 0 " +
                                 "   AND l.CallLaterDate < '{2:d}' " +
                                 " ORDER BY EntryDate DESC", sassigned, Session["guser"], DateTime.Now.AddDays(14));
            _dtLeads = _sql.GetTable(ssql);
        }
        grid1.DataSource = _dtLeads;
        grid1.DataBind();
    }

    void RebindGrid(object sender, EventArgs e)
    {
        CreateGrid();
    }

}