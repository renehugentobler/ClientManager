using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

using HSoft.SQL;

using Obout.ComboBox;

using HSoft.ClientManager.Web;

public partial class Pages_Tracker : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        ComboBox _ComboBox = new ComboBox();
        _ComboBox.ID = "CodeVersion";
        _ComboBox.Width = Unit.Pixel(250);
        _ComboBox.Height = Unit.Pixel(15);
        _ComboBox.EmptyText = "UnknowVersion";
        _ComboBox.Mode = ComboBoxMode.TextBox;
        _ComboBox.Enabled = false;

        _ComboBox.Items.Add(new ComboBoxItem(String.Format("Client Manager Version : {0}.{1}.{2}",0,9,4)));
        _ComboBox.SelectedIndex = 0;

        cbName.Controls.Add(_ComboBox);

    
    }
}