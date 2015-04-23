using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Obout.Interface;

public partial class Pages_screentest2 : System.Web.UI.Page
{

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
        Page.Header.Title = String.Format("Screen Test 1.0 ({0}x{1})", Session["wx"], Session["wy"]);
        txt1.Text = "window.innerWidth";
        txt2.Text = "window.innerHeight";
        txt3.Text = "window.outerHeight";
        txt4.Text = "window.outerWidth";
        txt5.Text = "window.pageXOffset";
        txt6.Text = "window.pageYOffset";
        txt7.Text = "window.screen.availHeight";
        txt8.Text = "window.screen.availWidth";
        txt9.Text = "window.screen.height";
        txt10.Text = "window.screen.width";
        txt11.Text = "navigator.userAgent";
        txt12.Text = "navigator.appVersion";

        value1.Text = Session["wx"].ToString();
        value2.Text = Session["wy"].ToString();
        value3.Text = Request.QueryString["oh"].ToString();
        value4.Text = Request.QueryString["ow"].ToString();
        value5.Text = Request.QueryString["ox"].ToString();
        value6.Text = Request.QueryString["oy"].ToString();
        value7.Text = Request.QueryString["sx"].ToString();
        value8.Text = Request.QueryString["sy"].ToString();
        value9.Text = Request.QueryString["hx"].ToString();
        value10.Text = Request.QueryString["hy"].ToString();
        value11.Text = Request.QueryString["ua"].ToString();
        value12.Text = Request.QueryString["av"].ToString();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }
}