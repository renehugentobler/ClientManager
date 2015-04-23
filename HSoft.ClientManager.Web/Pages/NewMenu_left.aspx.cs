using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HSoft.SQL;

using HSoft.ClientManager.Web;

public partial class NewMenu_left : OboutInc.oboutAJAXPage
{

    private static DataTable _dtSurvey = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {

        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }
        
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = "SELECT * FROM Survey WHERE isdeleted = 0";
        _dtSurvey = _sql.GetTable(ssql);

        foreach (DataRow dr in _dtSurvey.Rows)
        {
            pro7.AddChildAt(String.Format("Survey_{0}",dr["Id"]), "Survey", dr["Name"].ToString().Replace("Copy of ",""), null, String.Format("LoadMainPage('Survey','{0}')",dr["Name"]), null);
        }

        if ((Session["guser"].ToString().ToUpper() == "7D5AA961-5478-4FA1-B5DB-D6A2071ED834") ||
            (Session["guser"].ToString().ToUpper() == "0BA4012E-5541-4A76-92BB-C7122344DC3A") ||
            (Session["guser"].ToString().ToUpper() == "DCDB22C2-65F4-46E4-91D1-CC123F83DCE2"))
        {
            pro7.AddChildAt("FutureLeads", "Configuration", String.Format("Future Leads"), null, "LoadMainPage('Lead_Future',null)", null);
            pro7.AddChildAt("ConstantContact", "Configuration", String.Format("Constant Contact"), null, "LoadMainPage('CC_TabStrip',null)", null);
            pro7.AddChildAt("LeadAssign", "Lead", String.Format("Assign Leads"), null, "LoadMainPage('lassign')", null);
////            pro7.AddChildAt("CreateEmailList", "Configuration", String.Format("Create Email List"), null, "LoadMainPage('EmailListCreate')", null);
            pro7.AddChildAt("ConstantContactURL", "Resources", String.Format("Constant Contact"), "https://login.constantcontact.com/login/login.sdo?nosell", null, null,"_top");
        }

//        if ((Session["guser"].ToString().ToUpper() == "7D5AA961-5478-4FA1-B5DB-D6A2071ED834") ||
//            (Session["guser"].ToString().ToUpper() == "0BA4012E-5541-4A76-92BB-C7122344DC3A") ||
//            (Session["guser"].ToString().ToUpper() == "DCDB22C2-65F4-46E4-91D1-CC123F83DCE2"))
//        {
//            pro7.AddChildAt("EmailPromo", "Lead", String.Format("Promo Emails"), null, "LoadMainPage('Email_Promo_Send',null)", null);
//        }

    
    }

}