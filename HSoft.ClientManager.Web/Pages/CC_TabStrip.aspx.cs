using System;
using System.Collections.Generic;
using System.Linq;

using OboutInc.EasyMenu_Pro;

using HSoft.ClientManager.Web;


public partial class Pages_ConstantContact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        CCTab.SelectedItemId = "Init";
        CCTab.AddItem(new MenuItem("Info", "<span style='cursor:default'>Info</span>", "", "", "", "SelectTab('Info','Info','',0)"));
        CCTab.AddItem(new MenuItem("Contacts", "<span style='cursor:default'>Contacts</span>", "", "", "", "SelectTab('Contact','Contact','',0)"));
        CCTab.AddItem(new MenuItem("Survey", "<span style='cursor:default'>Survey</span>", "", "", "", "SelectTab('Survey','Survey','',0)"));
        CCTab.AddItem(new MenuItem("EMails", "<span style='cursor:default'>EMails</span>", "", "", "", "SelectTab('EMails','EMails','',0)"));
        CCTab.AddItem(new MenuItem("EmailLists", "<span style='cursor:default'>Email Lists</span>", "", "", "", "SelectTab('EmailLists','EmailLists','',0)"));

        tabIframe.Src = String.Format("CC_Info.aspx", "", "");
//        tabIframe.Attributes.Add("height", "600px");
        tabIframe.Attributes.Add("height", (int.Parse(Session["wy"].ToString()) - 55).ToString());

    
    }
}