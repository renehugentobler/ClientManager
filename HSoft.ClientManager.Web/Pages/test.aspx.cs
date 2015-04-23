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
using HSoft.ClientManager.Web;

using Obout.Grid;
using Obout.ComboBox;
using Obout.Interface;
using Obout.SuperForm;

public partial class Pages_test : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        MailAddress from = new MailAddress("leadsreceiver@uncleharry.biz", "Leads Receiver");
        MailAddress to = new MailAddress("4438255040@tmomail.net", "Leads Receiver");

        Tools.sendMail(from, to, "Test", "Another test");

    }

}