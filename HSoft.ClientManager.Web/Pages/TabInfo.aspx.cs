using System;
using System.Collections.Generic;
using System.Linq;

using OboutInc.EasyMenu_Pro;

using HSoft.ClientManager.Web;

public partial class Pages_TabInfo : System.Web.UI.Page
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        InfoTab.SelectedItemId = "Sold";
        InfoTab.AddItem(new MenuItem("Sold", "<span style='cursor:default'>Sold 28 Days</span>", "", "", "", "SelectTab('Sold','Priority','F3DC2498-6F4F-449E-813C-EFDA32A9D24A',28)"));
        InfoTab.AddItem(new MenuItem("ReadyClose", "<span style='cursor:default'>Ready Close</span>", "", "", "", "SelectTab('ReadyClose','Priority','FB783F05-C483-4F2C-BF65-97DFDAE738CD',-1)"));
        InfoTab.AddItem(new MenuItem("HotLeads", "<span style='cursor:default'>Hot Leads</span>", "", "", "", "SelectTab('HotLeads','Priority','6139FF9E-A66F-4974-8DD7-EFA293EB35B1',-1)"));
        InfoTab.AddItem(new MenuItem("ReturningLeads", "<span style='cursor:default'>Returning Leads</span>", "", "", "", "SelectTab('ReturningLeads','Status','D27F9479-EF66-495C-B279-8A852047319E',60)"));

        if ((Session["guser"].ToString().ToUpper() == "7D5AA961-5478-4FA1-B5DB-D6A2071ED834") ||
            (Session["guser"].ToString().ToUpper() == "0BA4012E-5541-4A76-92BB-C7122344DC3A") ||
            (Session["guser"].ToString().ToUpper() == "DCDB22C2-65F4-46E4-91D1-CC123F83DCE2"))
        {
            InfoTab.AddItem(new MenuItem("Logins", "<span style='cursor:default'>Logins</span>", "", "", "", "SelectTab('Logins','Logins','','')"));
        }

        tabIframe.Src = String.Format("TabInfo_{0}.aspx?s{0}Id={1}&sparm1=28", "Priority","F3DC2498-6F4F-449E-813C-EFDA32A9D24A");
        tabIframe.Attributes.Add("height",(int.Parse(Session["wy"].ToString())-55).ToString());

    }
}