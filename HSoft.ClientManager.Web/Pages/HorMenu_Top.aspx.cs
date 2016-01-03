using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OboutInc.TextMenu;

using HSoft.SQL;

using HSoft.ClientManager.Web;

public partial class HorMenu_Top : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        TextMenu tm1 = new TextMenu();
        tm1.ID = "tm1";

        tm1.Add(null, "MyPages", "MyPages", "javascript:alert('you clicked me');void(0);", null);
        tm1.Add("MyPages", "StartInfo", "StartInfo");
        tm1.Add(null, "Lead", "Sales");
        tm1.Add("Lead", "LeadSales", "LeadSales");
        tm1.Add("Lead", "LeadSales2", "LeadSales2");
        tm1.Add("Lead", "LeadPromoEmails", "LeadPromoEmails");
        tm1.Add(null, "Survey", "Surveys");
        tm1.Add(null, "Configuration", "Configuration");
        tm1.Add("Configuration", "SalesConfig", "Impersonate");
        tm1.Add(null, "Resources", "Resources");
        tm1.Add("Resources", "RakerURL", "Raker Website");

        this.Controls.Add(tm1);    
    }

}