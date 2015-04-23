using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;

using OboutInc.EasyMenu_Pro;

using HSoft.SQL;
using HSoft.ClientManager.Web;

public partial class Pages_TabSurvey : System.Web.UI.Page
{

    static DataTable _dtSurvey = new DataTable();
    static String sEmail = String.Empty;

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        if (!String.IsNullOrEmpty(Request.QueryString["email"])) { sEmail = Request.QueryString["email"].ToString(); }

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = String.Format("SELECT DISTINCT s.Id, s.Name, sc.ResponseID, sc.SubmissionTime FROM SurveyCustomer sc,Survey s WHERE s.Id = sc.SurveyId AND (sc.Respondent='{0}' OR sc.EmailAddress='{0}') AND sc.isdeleted = 0 AND s.isdeleted = 0", sEmail);
        _dtSurvey = _sql.GetTable(ssql);
        if (_dtSurvey.Rows.Count==0) 
        {
            /* close page */
            return;
        }

        Guid fId = Guid.Empty;
        Int64 rId = Int64.MinValue;
        foreach (DataRow dr in _dtSurvey.Rows)
        {
            if (fId == Guid.Empty) 
            { 
                Guid.TryParse(dr["Id"].ToString(),out fId);
                Int64.TryParse(dr["ResponseID"].ToString(), out rId); 
            }
            SurveyTab.AddItem(new MenuItem(dr["ResponseID"].ToString(),
                            String.Format("<span style='cursor:default'>{0}</span>", dr["Name"])
                            , ""
                            , ""
                            , ""
                            , String.Format("SelectTab('{2}','{1}','{0}',{3})", dr["Id"], dr["Name"], dr["ResponseID"], dr["ResponseID"])));
        }

        tabIframe.Src = String.Format("TabSurveyDisplay.aspx?Id={0}&sparm1={1}", fId, rId);
        tabIframe.Attributes.Add("height", (600 - 55).ToString());


        _sql.Close();

    }
}