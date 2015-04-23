using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using HSoft.SQL;

using Obout.Grid;
using Obout.ComboBox;

using HSoft.ClientManager.Web;

public partial class Pages_Survey : System.Web.UI.Page
{
    Grid grid1 = new Grid();
    static DataTable _dtSurveyQuestion = new DataTable();
    static Guid _guid = new Guid();

    protected void Page_Load(object sender, EventArgs e)
    {

        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        String ssql = String.Empty;
        String ssurvey = Server.UrlDecode(Request.QueryString["parm"]);
        if (String.IsNullOrWhiteSpace(ssurvey)) { ssurvey = "Copy of Income Challenge 85000"; }

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = String.Format("SELECT sq.* FROM SurveyQuestion sq,Survey s WHERE s.isdeleted = 0 AND sq.isdeleted = 0 AND s.Name='{0}' AND s.Id=sq.SurveyId", ssurvey);
        _dtSurveyQuestion = _sql.GetTable(ssql);

        Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM Survey WHERE Name = '{0}'", ssurvey)).ToString(), out _guid);

        sdsGrid.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;

        ssql = String.Format("SELECT DISTINCT sc.ResponseID CustomerId, " +
                             "       CASE WHEN c.Name IS NULL THEN sc.FirstName + ' ' + sc.LastName ELSE c.Name END CustomerName, " +
                             "       CASE WHEN c.Email IS NULL THEN CASE WHEN sc.Respondent = 'Anonymous' THEN sc.EmailAddress ELSE sc.Respondent END ELSE c.Email END CustomerEmail, " +
                             "       SubmissionTime, " +
                             "       Status, " +
                             "       HomePhone " +
                             "", ssurvey, ssql);
        for (int i = 0; i < _dtSurveyQuestion.Rows.Count - 1; i++)
        {
            ssql = String.Format("{1}, " +
                                 "sd{2}.Answer + CASE WHEN sd{2}.Comment IS NULL THEN '' ELSE '<br><b>'+sd{2}.Comment+'</b>' END Field{2}  " +
                                 "", ssurvey, ssql, i);
        }
        ssql = String.Format("{1} " +
                             "  FROM SurveyCustomer sc " +
                             " LEFT JOIN Customer c ON (sc.Respondent=c.Email OR sc.EmailAddress=c.Email) " +
                             "", ssurvey,ssql);
        for (int i = 0; i < _dtSurveyQuestion.Rows.Count - 1; i++)
        {
            ssql = String.Format("{1} " +
                             " LEFT JOIN SurveyQuestion sq{2} ON sq{2}.SurveyId='{3}' AND sq{2}.Number={2} " +
                             " LEFT JOIN SurveyDetail sd{2} ON sd{2}.SurveyQuestionId=sq{2}.Id AND sd{2}.SurveyCustomerId=sc.Id " +
                                 "", ssurvey, ssql, i, _guid);
        }
        ssql = String.Format("{1} " +
                             " WHERE sc.SurveyId='{2}' " +
                             " ORDER BY SubmissionTime DESC ", ssurvey, ssql, _guid);
        sdsGrid.SelectCommand = ssql;

        ComboBox _ComboBox = new ComboBox();
        _ComboBox.ID = "SalesName";
        _ComboBox.Width = Unit.Pixel(400);
        _ComboBox.Height = Unit.Pixel(15);
        _ComboBox.EmptyText = "Unknow Survey";
        _ComboBox.Mode = ComboBoxMode.TextBox;
        _ComboBox.Enabled = false;

        _ComboBox.Items.Add(new ComboBoxItem(ssurvey));
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
        grid1.AllowPaging = true;
        grid1.AllowSorting = true;
        grid1.Enabled = true;
        grid1.ShowLoadingMessage = true;
        grid1.ScrollingSettings.FixedColumnsPosition = GridFixedColumnsPositionType.Left;
        grid1.ScrollingSettings.NumberOfFixedColumns = 1;
        grid1.ScrollingSettings.ScrollWidth = new Unit(100, UnitType.Percentage);
//        grid1.ScrollingSettings.ScrollHeight = 600;
        grid1.ScrollingSettings.ScrollHeight = int.Parse(Session["wy"].ToString()) - 160;
        grid1.EnableRecordHover = true;

//        grid1.Width = int.Parse(Session["wh"].ToString()) - 120;
//        grid1.Height = int.Parse(Session["wy"].ToString()) - 120;
        grid1.FolderStyle = "styles/grid/premiere_blue";

        grid1.AllowPageSizeSelection = true;

        // grid filter
        GridFilteringSettings gfs = new GridFilteringSettings();
        gfs.FilterPosition = GridFilterPosition.Top;
        gfs.FilterLinksPosition = GridElementPosition.Top;
        gfs.InitialState = GridFilterState.Hidden;
        grid1.FilteringSettings = gfs;

        Column oCol1 = new Column();
        oCol1.AllowFilter = true;
        oCol1.DataField = "CustomerName";
        oCol1.HeaderText = "Name";
        oCol1.Width = "120";
        oCol1.Wrap = true;
        oCol1.AllowSorting = true;
        oCol1.AllowFilter = true;

        Column oCol2 = new Column();
        oCol2.AllowFilter = true;
        oCol2.DataField = "CustomerEmail";
        oCol2.HeaderText = "Email";
        oCol2.Width = "180";
        oCol2.Wrap = true;
        oCol2.AllowSorting = true;
        oCol2.AllowFilter = true;

        Column oCol5 = new Column();
        oCol5.AllowFilter = true;
        oCol5.DataField = "HomePhone";
        oCol5.HeaderText = "Phone";
        oCol5.Width = "100";
        oCol5.Wrap = true;
        oCol5.AllowSorting = true;
        oCol5.AllowFilter = true;

        Column oCol3 = new Column();
        oCol3.AllowFilter = true;
        oCol3.DataField = "SubmissionTime";
        oCol3.HeaderText = "Date";
        oCol3.Width = "80";
        oCol3.DataFormatString = "{0:MM/dd/yyyy}";
        oCol3.Wrap = true;
        oCol3.AllowSorting = true;
        oCol3.AllowFilter = true;

        Column oCol4 = new Column();
        oCol4.AllowFilter = true;
        oCol4.DataField = "Status";
        oCol4.HeaderText = "Status";
        oCol4.Width = "60";
        oCol4.Wrap = true;
        oCol4.AllowSorting = true;
        oCol4.AllowFilter = true;

        List<Column> _listcolumns = new List<Column>();
        for (int i = 0; i < _dtSurveyQuestion.Rows.Count-1; i++)
        {
            _listcolumns.Add(new Column());
            _listcolumns[_listcolumns.Count - 1].AllowFilter = true;
            _listcolumns[_listcolumns.Count - 1].DataField = String.Format("Field{0}",i);
            _listcolumns[_listcolumns.Count - 1].HeaderText = _dtSurveyQuestion.Select(String.Format("Number={0}",i))[0]["Text"].ToString();
            _listcolumns[_listcolumns.Count - 1].Width = "200";
            _listcolumns[_listcolumns.Count - 1].Wrap = true;
            _listcolumns[_listcolumns.Count - 1].AllowSorting = false;
            _listcolumns[_listcolumns.Count - 1].AllowFilter = false;
            _listcolumns[_listcolumns.Count - 1].ParseHTML = true;
        }

        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol5);
        grid1.Columns.Add(oCol3);
//        grid1.Columns.Add(oCol4);
        for (int i = 0; i < _dtSurveyQuestion.Rows.Count - 1; i++)
        {
            grid1.Columns.Add(_listcolumns[i]);
        }
        // add the grid to the controls collection of the PlaceHolder        
        phGrid1.Controls.Add(grid1);
    }
}