using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using HSoft.SQL;

using Obout.ComboBox;
using Obout.Grid;
using Obout.SuperForm;

using HSoft.ClientManager.Web;

public partial class Pages_Dial800 : System.Web.UI.Page
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

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        sdsDial800.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;

        grid1.Height = int.Parse(Session["wy"].ToString()) - 20;
    }
}