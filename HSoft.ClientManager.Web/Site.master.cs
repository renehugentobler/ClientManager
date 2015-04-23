using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using HSoft.SQL;

public partial class SiteMaster : MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    private String contentMarginLeft = "";

    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    void master_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    public void CommandBtn_Click(Object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "masterCmd":
                {
                    switch (e.CommandArgument.ToString())
                    {
                        case "cmdHideMenu":
                            {
                                if (leftcolumn.Visible == true)
                                {
                                    contentMarginLeft = content.Attributes.CssStyle["margin-left"];
                                    content.Attributes.CssStyle.Remove("margin-left");
                                    content.Attributes.CssStyle.Add("margin-left", "0px");
                                }
                                else
                                {
                                    content.Attributes.CssStyle.Add("margin-left", contentMarginLeft);
                                }
                                leftcolumn.Visible = !leftcolumn.Visible;
                            }
                            break;
                    }
                }
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        String ssql = String.Format("SELECT 'E' usertype,* FROM Employee WHERE isdeleted = 0 AND Id = '{0}'", Session["guser"]);
        DataTable table = _sql.GetTable(ssql);
        if (table.Rows.Count == 0)
        {
            ssql = String.Format("SELECT 'C' usertype,* FROM Customer WHERE isdeleted = 0 AND Id = '{0}'", Session["guser"]);
            table = _sql.GetTable(ssql);
        }
        DataRow dr = table.Rows[0];
        MasterUserName.Text = String.Format("{0} {1}", dr["FirstName"], dr["LastName"]);

        _sql.Close();

    }
}