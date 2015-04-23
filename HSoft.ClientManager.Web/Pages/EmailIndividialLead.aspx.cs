using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Mail;

using Obout.Interface;
using Obout.Ajax.UI.HTMLEditor;
using Obout.Ajax.UI.HTMLEditor.ToolbarButton;

using HSoft.SQL;

using HSoft.ClientManager.Web;

public partial class EmailIndividialLead : System.Web.UI.Page
{

    public static DataRow _dr = null;
    public static DataRow _de = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.UICulture = "en";
        //Page.UICulture = "auto";
        if (!Page.IsPostBack)
        {
            Tools.devlogincheat();

            String sId = "0208773E-F9DF-456B-B0FA-00181B862E0F";
            if (!String.IsNullOrEmpty(Request.QueryString["Id"])) { sId = Request.QueryString["Id"]; }

            String ssql = String.Empty;
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            ssql = String.Format("SELECT * FROM Lead_Flat WHERE Id = '{0}' AND isdeleted = 0", sId);
            _dr = _sql.GetRow(ssql);

            ssql = String.Format("SELECT * FROM Employee WHERE Id = '{0}' AND isdeleted = 0", Session["guser"]);
            _de = _sql.GetRow(ssql);

            _sql.Close();

            String scontent = String.Format("Dear {0}" +
                                            "<br/><br/><br/><br/>" +
                                            "regards <a href='mailto:{2}@uncleharry.com?Uncleharry Sales' target='_top'>{1}</a> ", _dr["Name"], _de["DisplayName"], _de["EmailName"]);

            editor.EditPanel.Content = scontent;
            subject.Text = "Uncleharry Sales";

        }
    
    }

    protected void Submit_click(object sender, EventArgs e)
    {
//        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditorResponse", "alert('Submitted:\\n\\n" + editor.EditPanel.Content.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'") + "');", true);

        MailAddress _smtpfrom = new MailAddress("emailer@uncleharry.biz", "Sales Uncleharry");
        MailAddress _from = new MailAddress(String.Format("{0}@uncleharry.biz", _de["EmailName"]), _de["DisplayName"].ToString());
        MailAddress _to = new MailAddress(_dr["Email"].ToString(), _dr["Name"].ToString());
        // MailAddress _to = new MailAddress("rene.hugentobler@gmail.com", _dr["Name"].ToString());
        // MailAddress _to = new MailAddress("harry@rakerappliancerepair.com", _dr["Name"].ToString());
        // Tools.sendMail(from, to, subject.Text, editor.EditPanel.Content, true);

        Tools.sendMail(_smtpfrom, @"Taurec86@", _from, _to, subject.Text, editor.EditPanel.Content, true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditorResponse2", "window.close();", true);
    
    }

    protected void Close_click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditorResponse", "window.close();", true);
    }

}
/*
    DataRow _dr = null;
    DataRow _de = null;
    OboutTextBox tbSubject = new OboutTextBox();

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (!Page.IsPostBack)
        {
            String sId = "0208773E-F9DF-456B-B0FA-00181B862E0F";
            if (!String.IsNullOrEmpty(Request.QueryString["Id"])) { sId = Request.QueryString["Id"]; }

            String ssql = String.Empty;
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            ssql = String.Format("SELECT * FROM Lead_Flat WHERE Id = '{0}' AND isdeleted = 0", sId);
            _dr = _sql.GetRow(ssql);

            ssql = String.Format("SELECT * FROM Employee WHERE Id = '{0}' AND isdeleted = 0", Session["guser"]);
            _de = _sql.GetRow(ssql);

            String scontent = String.Format("Dear {0}" +
                                            "<br/><br/><br/><br/>" +
                                            "regards <a href='mailto:{2}@uncleharry.com?Uncleharry Sales' target='_top'>{1}</a> ", _dr["Name"],_de["DisplayName"], _de["EmailName"]);

            editor.EditPanel.Content = scontent;

            _sql.Close();
        }


        tbSubject.ID = "subject";
        tbSubject.Width = Unit.Pixel(400);
        tbSubject.AutoCompleteType = AutoCompleteType.None;
//        tbSubject.FolderStyle = "styles/premiere_blue/interface/OboutTextBox";
        Subject.Controls.Add(tbSubject);

    }

    protected void Editable_click(object sender, EventArgs e)
    {
        editor.Enabled = !editor.Enabled;
        Submit.Enabled = editor.Enabled;
    }

    protected void Submit_click(object sender, EventArgs e)
    {
//        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditorResponse", "alert('Submitted:\\n\\n" + editor.EditPanel.Content.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'") + "');", true);

        MailAddress from = new MailAddress(String.Format("{0}@uncleharry.biz", _de["EmailName"]), _de["DisplayName"].ToString());
//        MailAddress to = new MailAddress(_dr["Email"].ToString(), _de["Name"].ToString());
        MailAddress to = new MailAddress("rene.hugentobler@gmail.com", _de["Name"].ToString());
        Tools.sendMail(from, to, tbSubject.Text, editor.EditPanel.Content, true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditorResponse2", "window.close();", true);
    }

    protected void Close_click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditorResponse", "window.close();", true);
    }

}

    */