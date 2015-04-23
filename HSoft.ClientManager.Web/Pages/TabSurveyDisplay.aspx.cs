using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Data.OleDb;
using System.IO;
using HSoft.SQL;
using HSoft.ClientManager.Web;

public partial class TabSurveyDisplay : System.Web.UI.Page
{

    static Guid gId = Guid.Empty;
    Int64 rId = Int64.MinValue;
    static DataTable SurveyCustomer = new DataTable();
    static DataTable _dtSurveyAnswers = new DataTable();

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        if (!String.IsNullOrEmpty(Request.QueryString["Id"])) { Guid.TryParse(Request.QueryString["Id"], out gId); }
        if (!String.IsNullOrEmpty(Request.QueryString["sparm1"])) { Int64.TryParse(Request.QueryString["sparm1"], out rId); }

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        String ssql = String.Empty;

        DataRow cdr = _sql.GetTable(String.Format("SELECT * FROM SurveyCustomer sc WHERE sc.isdeleted = 0 AND sc.ResponseID = {0} AND SurveyId = '{1}' ", rId, gId)).Rows[0];

        HtmlTableRow _crow = new HtmlTableRow();
        HtmlTableCell _ccell1 = new HtmlTableCell();
        HtmlTableCell _ccell2 = new HtmlTableCell();
        HtmlTableCell _ccell3 = new HtmlTableCell();
        HtmlTableCell _ccell4 = new HtmlTableCell();

        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
        _ccell1.Controls.Add(new LiteralControl(String.Format("{0} {1}",  myTI.ToTitleCase(cdr["FirstName"].ToString()), myTI.ToTitleCase(cdr["LastName"].ToString()))));
        _ccell2.Controls.Add(new LiteralControl(cdr["EmailAddress"].ToString()));
        _ccell3.Controls.Add(new LiteralControl(cdr["HomePhone"].ToString()));
        _ccell4.Controls.Add(new LiteralControl(cdr["SubmissionTime"].ToString()));

        _crow.Cells.Add(_ccell1);
        _crow.Cells.Add(_ccell2);
        _crow.Cells.Add(_ccell3);
        _crow.Cells.Add(_ccell4);

        clientinfo.Rows.Add(_crow);

        ssql = String.Format("SELECT sq.[Text] Text, sd.Answer, sd.Comment " +
                            "  FROM SurveyCustomer sc, SurveyQuestion sq, SurveyDetail sd " +
                            "WHERE sc.ResponseID = {0} " +
                            "  AND sc.[SurveyId] = sq.[SurveyId] " +
                            "  AND sc.SurveyId = '{1}' " +
                            "  AND sd.[SurveyCustomerId] = sc.Id " +
                            "  AND sd.SurveyQuestionId = sq.Id " +
                            "ORDER BY sq.Number", rId, gId);
        _dtSurveyAnswers = _sql.GetTable(ssql);

        foreach (DataRow dr in _dtSurveyAnswers.Rows)
        {
            HtmlTableRow _row = new HtmlTableRow();
            HtmlTableCell _cell1 = new HtmlTableCell();
            HtmlTableCell _cell2 = new HtmlTableCell();

            _cell1.Controls.Add(new LiteralControl(dr["Text"].ToString()));
            if (!String.IsNullOrEmpty(dr["Answer"].ToString())) { _cell2.Controls.Add(new LiteralControl(dr["Answer"].ToString())); }
            if (!String.IsNullOrEmpty(dr["Comment"].ToString())) 
            {
                if (!String.IsNullOrEmpty(dr["Answer"].ToString())) { _cell2.Controls.Add(new LiteralControl(@"<br/>")); }
                _cell2.Controls.Add(new LiteralControl(@"<b>" + dr["Comment"].ToString()+@"</b>")); 
            }
            _row.Cells.Add(_cell1);
            _row.Cells.Add(_cell2);

            campaign.Rows.Add(_row);
        }
    
    }
}