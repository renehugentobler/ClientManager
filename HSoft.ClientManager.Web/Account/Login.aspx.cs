using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HSoft.SQL;

using HSoft.ClientManager.Web;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Login_Click(object sender, EventArgs e)
    {
        Guid? guser = null;

        String ssql = String.Empty;

        if ((UserName.Text.Trim() != string.Empty) && (Password.Text.Trim() != string.Empty))
        {
            if (UserName.Text.Trim().Length>32) { UserName.Text = UserName.Text.Trim().Substring(0,32); }
            guser = SqlServer.Validate_Login(UserName.Text.Trim(), Password.Text.Trim(), System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
        }

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
        if ((guser == null) || (guser == new Guid()))
        {
            error.Visible = true;
            error.Text = "Please enter a valid username and password";

            ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                 "VALUES ('{0}','{1}','{4}','{2}','{5}','{4}') ", "LOGIN", "Id", UserName.Text, "FAILED", Guid.Empty, HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.UserHostAddress);
            _sql.Execute(ssql);
            _sql.Close();
        
        }
        else
        {
            error.Visible = false;
            Session["guser"] = guser.ToString().ToUpper();
            Session["ghirarchy"] = String.Format("{0},", Session["guser"]);


            ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                 "VALUES ('{0}','{1}','{4}','{3}','{5}','{4}') ", "LOGIN", "Id", guser.ToString().ToUpper(), HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]?? HttpContext.Current.Request.UserHostAddress, Session["guser"],null);
            _sql.Execute(ssql);
            _sql.Close();

//            Tools.InitData();
            ClientScript.RegisterStartupScript(GetType(), "Redirect", "Redirect()", true);
//            Response.Redirect(Page.ResolveClientUrl("/Pages/NewMenu.aspx"));
        }
    }

}