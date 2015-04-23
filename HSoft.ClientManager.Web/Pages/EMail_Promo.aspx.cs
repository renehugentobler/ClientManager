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
using Obout.Ajax.UI.HTMLEditor;
using Obout.Ajax.UI.HTMLEditor.ToolbarButton;
using Obout.Ajax.UI.HTMLEditor.ContextMenu;

using HSoft.ClientManager.Web;

public partial class Pages_EMail_Promo : System.Web.UI.Page
{
    Grid grid1 = new Grid();
    Editor editor = new Editor();

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
        grid1.AllowAddingRecords = true;
        grid1.AllowFiltering = false;
        grid1.AllowSorting = false;

        grid1.FolderStyle = "styles/grid/premiere_blue";

        // setting the event handlers
        //        grid1.InsertCommand += new Obout.Grid.Grid.EventHandler(InsertRecord);
        //        grid1.DeleteCommand += new Obout.Grid.Grid.EventHandler(DeleteRecord);
        grid1.UpdateCommand += new Obout.Grid.Grid.EventHandler(UpdateRecord);
        grid1.DeleteCommand += new Obout.Grid.Grid.EventHandler(DeleteRecord);
        grid1.InsertCommand += new Obout.Grid.Grid.EventHandler(InsertRecord);
        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        grid1.ClientSideEvents.OnClientEdit = "onAddEdit";

        grid1.ClientSideEvents.OnClientSelect = "onClientSelect";
        grid1.ClientSideEvents.OnBeforeClientEdit = "onAddEdit";

        Column oCol0 = new Column();
        oCol0.DataField = "";
        oCol0.HeaderText = "";
        oCol0.Width = "90";
        oCol0.AllowEdit = true;
        oCol0.AllowDelete = true;

        Column oColi = new Column();
        oColi.ID = "ID";
        oColi.DataField = "Id";
        oColi.Visible = false;

        Column oColi2 = new Column();
        oColi2.ID = "EmaildId";
        oColi2.DataField = "EmaildId";
        oColi2.Visible = false;

        Column oCol1 = new Column();
        oCol1.ID = "EmailKey";
        oCol1.DataField = "EmailKey";
        oCol1.HeaderText = "Key";
        oCol1.Visible = true;
        oCol1.ReadOnly = false;
        oCol1.Width = "30";

        Column oCol2 = new Column();
        oCol2.ID = "isactive";
        oCol2.DataField = "isactive";
        oCol2.HeaderText = "Active";
        oCol2.Visible = true;
        oCol2.ReadOnly = false;
        oCol2.Width = "50";

        Column oCol3 = new Column();
        oCol3.ID = "EmailDescription";
        oCol3.DataField = "EmailDescription";
        oCol3.HeaderText = "Description";
        oCol3.Visible = true;
        oCol3.ReadOnly = false;
        oCol3.Width = "150";
        oCol3.Wrap = true;

        Column oCol4 = new Column();
        oCol4.ID = "EmailSubject";
        oCol4.DataField = "EmailSubject";
        oCol4.HeaderText = "Subject";
        oCol4.Visible = true;
        oCol4.ReadOnly = false;
        oCol4.Width = "150";
        oCol4.Wrap = true;

        Column oCol5 = new Column();
        oCol5.ID = "EmailBody";
        oCol5.DataField = "EmailBody";
        oCol5.HeaderText = "Body";
        oCol5.Visible = true;
        oCol5.ReadOnly = false;
        oCol5.Width = "700";
        oCol5.Wrap = true;
        oCol5.ParseHTML = true;
        oCol5.TemplateSettings.EditTemplateId = "TemplateEditEmailBody";

        grid1.Columns.Add(oCol0);
        grid1.Columns.Add(oColi);
        grid1.Columns.Add(oColi2);
        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol4);
        grid1.Columns.Add(oCol5);

        GridRuntimeTemplate TemplateEditEmailBody = new GridRuntimeTemplate();
        TemplateEditEmailBody.ID = "TemplateEditEmailBody";
        TemplateEditEmailBody.Template = new Obout.Grid.RuntimeTemplate();
        TemplateEditEmailBody.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateEditEmailBody);
        TemplateEditEmailBody.ControlID = "EditorContent";
        TemplateEditEmailBody.ControlPropertyName = "value";
        TemplateEditEmailBody.UseQuotes = true;

        grid1.Templates.Add(TemplateEditEmailBody);

        phGrid1.Controls.Add(grid1);

        if (!Page.IsPostBack)
        {
            BindGrid();
        }

        EditorID.Value = ((Editor)(grid1.Templates[0].Container.FindControl("Editor"))).ClientID;
    }

	public void CreateEditEmailBody(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
	{
		PlaceHolder oPlaceHolder = new PlaceHolder();
        e.Container.Controls.Add(oPlaceHolder);
        editor.ID = "Editor";
        editor.Height = 500;
        editor.Width = Unit.Percentage(100);

        editor.TopToolbar.Appearance = EditorTopToolbar.AppearanceType.Custom;
        editor.TopToolbar.AddButtons.Add(new Undo());
        editor.TopToolbar.AddButtons.Add(new Redo());
        editor.TopToolbar.AddButtons.Add(new HorizontalSeparator());
        editor.TopToolbar.AddButtons.Add(new Bold());
        editor.TopToolbar.AddButtons.Add(new Italic());
        editor.TopToolbar.AddButtons.Add(new Underline());
        editor.TopToolbar.AddButtons.Add(new HorizontalSeparator());
        editor.TopToolbar.AddButtons.Add(new ForeColorGroup());
        editor.TopToolbar.AddButtons.Add(new HorizontalSeparator());
        editor.TopToolbar.AddButtons.Add(new SpellCheck());

        editor.BottomToolbar.ShowDesignButton = true;
        editor.BottomToolbar.ShowHtmlTextButton = false;
        editor.BottomToolbar.ShowPreviewButton = true;

        oPlaceHolder.Controls.Add(editor);
	}

    void RebindGrid(object sender, EventArgs e) { BindGrid(); }
    void BindGrid()
    {
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = "SELECT e.Id EmaildId,e.EmailDescription,e.EmailSubject,e.EmailBody,e.isdeleted isdeleted_EmaildId,ep.Id,ep.EmailKey,ep.isactive,ep.isdeleted isdeleted_Id " +
               "  FROM Email e, Email_Promo ep " +
               " WHERE e.isdeleted = 0 " +
               "   AND ep.isdeleted = 0 " +
               "   AND e.Id = ep.EmaildId " +
               " ORDER BY EmailKey";

        DataTable _dtEmails = _sql.GetTable(ssql);

        grid1.DataSource = _dtEmails;
        grid1.DataBind();

        _sql.Close();
    }

    void InsertRecord(object sender, GridRecordEventArgs e)
    {
    }

    void DeleteRecord(object sender, GridRecordEventArgs e)
    {
    }

    void UpdateRecord(object sender, GridRecordEventArgs e)
    {
    }

}