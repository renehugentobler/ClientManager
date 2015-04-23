using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HSoft.ClientManager.Web;

public partial class Pages_NewMenu : System.Web.UI.Page
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Request.QueryString["wx"] != null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Request.QueryString["wx"] != null) { Session["wy"] = Request.QueryString["wy"]; }
        Page.Header.Title = String.Format("Client Manager 1.0 ({0}x{1})", Session["wx"], Session["wy"]);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }
    
    }
}