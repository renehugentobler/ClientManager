using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Net.Mail;

using HSoft.SQL;

public partial class Pages_Lead_Email : System.Web.UI.Page
{

    static DataTable _dtEmail = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {

        // for testing only
        if (Session["guser"] == null) { Session["guser"] = "44F7B957-4AA9-466F-B5C8-8840586157B6"; }
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        String ssql = String.Empty;

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        _dtEmail = _sql.GetTable("SELECT * FROM _LeadEmail WHERE isdeleted = 0");


        System.Net.Mail.MailMessage message = new MailMessage();
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
        smtp.Timeout = 1000;

        String strName = "Rene";
        String stremail = "Harry";
        String strSignature = "Harry Raker";

        foreach (DataRow dr in _dtEmail.Rows)
        {
            String sEmail = String.Format(dr["EmailBody"].ToString(), strName, strSignature, stremail);
            String ssubject = dr["EmailSubject"].ToString();

        try
        {
            message = new System.Net.Mail.MailMessage();

                message.To.Add("rene.hugentobler@gmail.com");
                message.IsBodyHtml = true;
            message.Subject = ssubject;
            message.Body = sEmail;

            smtp.Send(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("{0} {1}", ex.Message, ex.InnerException.Message);
        }

        }
        smtp.Dispose();
    }
}