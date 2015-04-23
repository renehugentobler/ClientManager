using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HSoft.ClientManager.Web;

public partial class NewMenu_Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

    }
}