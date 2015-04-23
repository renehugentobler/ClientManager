using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using System.Data.SqlClient;

using System.Web.UI.HtmlControls;

using HSoft.SQL;

using Obout.ComboBox;
using Obout.Interface;


public partial class Testing_Impersonate : System.Web.UI.Page
{
    private ComboBox ImpersonateSelect;
    DataTable _dtEmployee = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {

            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            String ssql = "SELECT Id,DisplayName FROM Employee WHERE isdeleted = 0";
            _dtEmployee = _sql.GetTable(ssql);
            _sql.Close();

            ImpersonateSelect = new ComboBox();
            ImpersonateSelect.ID = "ImpersonateSelect";
            ImpersonateSelect.SelectedIndexChanged += new ComboBoxSelectedIndexChangedEventHandler(ImpersonateSelect_SelectedIndexChanged);
            ImpersonateSelect.AutoPostBack = true;

            foreach (DataRow dr in _dtEmployee.Rows)
            {
                ImpersonateSelect.Items.Add(new ComboBoxItem(dr["DisplayName"].ToString(), dr["Id"].ToString()));
            }
            ImpersonateSelect.SelectedValue = Session["guser"].ToString();

            phImpersonate.Controls.Add(ImpersonateSelect);

    }


    private void ImpersonateSelect_SelectedIndexChanged(object sender,System.EventArgs e)
    {

        Session["guser"] = ImpersonateSelect.SelectedValue;

    }

}