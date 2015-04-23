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
using Obout.ListBox;

using HSoft.ClientManager.Web;

public partial class Pages_Info : System.Web.UI.Page
{
    static DataTable _dtEmployee = new DataTable();
    static DataTable _dtEmployeeHirarchy = new DataTable();

    private Obout.ListBox.ListBox _ListBox = new Obout.ListBox.ListBox();

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        String ssql = String.Empty;

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = "SELECT * FROM Employee WHERE isdeleted = 0";
        _dtEmployee = _sql.GetTable(ssql);

        ssql = String.Format("SELECT eh.Id,EmployeeId,SubEmployeeId,DisplayName,LastName " +
                             "  FROM EmployeeHirarchy eh,Employee e  " +
                             "  WHERE eh.isdeleted = 0  " +
                             "  AND e.isdeleted=0  " +
                             "  AND e.Id = eh.SubEmployeeId  " +
                             "  AND eh.EmployeeId = '{0}'  " +
                             "  AND e.isSales=1 " +
                             "  AND e.isactive = 1 " +
                             "  ORDER BY LastName", Session["guser"].ToString());
        _dtEmployeeHirarchy = _sql.GetTable(ssql);

        _sql.Close();

        cbName.Controls.Clear();
        List<ComboBox> _listComboBox = new List<ComboBox>();

        foreach (String sguid in Session["ghirarchy"].ToString().Split(','))
        {
            if ((sguid.Length != 0) && (sguid!="00000000-0000-0000-0000-000000000000"))
            {
                _listComboBox.Add(new ComboBox());
                _listComboBox[_listComboBox.Count - 1].ID = String.Format("History{0}", sguid);
                _listComboBox[_listComboBox.Count - 1].Width = Unit.Pixel(120);
                _listComboBox[_listComboBox.Count - 1].Height = Unit.Pixel(15);
                _listComboBox[_listComboBox.Count - 1].Mode = ComboBoxMode.TextBox;
                _listComboBox[_listComboBox.Count - 1].Enabled = false;
                _listComboBox[_listComboBox.Count - 1].Items.Add(new ComboBoxItem(_dtEmployee.Select(String.Format("Id='{0}'", sguid))[0]["DisplayName"].ToString()));
                _listComboBox[_listComboBox.Count - 1].SelectedIndex = 0;

                cbName.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
            }
        }

        _ListBox = new Obout.ListBox.ListBox();
        _ListBox.ID = "Hirarchy";
        _ListBox.Width = Unit.Pixel(250);
        // _ListBox.Height = Unit.Pixel(15 + 15 * (_dtEmployee.Rows.Count-1));

        foreach (DataRow dr in _dtEmployeeHirarchy.Rows)
        {
            _ListBox.Items.Add(new ListBoxItem(dr["DisplayName"].ToString(), dr["SubEmployeeId"].ToString().ToUpper()));
        }
        ListBoxItem dummy = new ListBoxItem("Select one or multiple","");

        dummy.Enabled=false;
        dummy.Selected = false;
        _ListBox.Items.Add(dummy);

        if (!Page.IsPostBack)
        {
            Boolean _bisall = false;
            for (int i = 0; i < _ListBox.Items.Count; i++)
            {
                if (_ListBox.Items[i].Selected)
                {
                    if (_ListBox.Items[i].Value.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        _bisall = true;
                    }
                }
            }

            for (int i = 0; i < _ListBox.Items.Count; i++)
            {
                if (_ListBox.Items[i].Enabled)
                {
                    if (Session["ghirarchy"].ToString().Contains(_ListBox.Items[i].Value.ToString()))
                    {
                        _ListBox.Items[i].Selected = true;
                    }
                    else
                    {
                        _ListBox.Items[i].Selected = _bisall;
                    }
                }
            }
        }

        _ListBox.HeaderTemplate = new HeaderTemplate();
        _ListBox.AutoPostBack = false;
        _ListBox.Enabled = true;
        _ListBox.SelectionMode = ListSelectionMode.Multiple;
        lbEmployee.Controls.Add(_ListBox);

    }

    protected void Postback(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            //            Boolean _bisall = false;
            //        }
            //    }
            //
            Boolean _bisall = false;
            for (int i = 0; i < _ListBox.Items.Count; i++)
            {
                if (_ListBox.Items[i].Selected)
                {
                    if (_ListBox.Items[i].Value.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        _bisall = true;
                    }
                }
            }
            if (_bisall)
            {
                for (int i = 0; i < _ListBox.Items.Count; i++)
                {
                    if (_ListBox.Items[i].Value.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        _ListBox.Items[i].Selected = true;
                    }
                    else
                    {
                        _ListBox.Items[i].Selected = false;
                    }
                }
            }

            Session["ghirarchy"] = "";
            for (int i = 0; i < _ListBox.Items.Count; i++)
            {
                if (_ListBox.Items[i].Enabled)
                {
                    if (_ListBox.Items[i].Selected)
                    {
                        Session["ghirarchy"] = String.Format("{1}{0},", _ListBox.Items[i].Value, Session["ghirarchy"]);
                    }
                }
            }

            cbName.Controls.Clear();
            List<ComboBox> _listComboBox = new List<ComboBox>();
            foreach (String sguid in Session["ghirarchy"].ToString().Split(','))
            {
                if ((sguid.Length != 0) && (sguid != "00000000-0000-0000-0000-000000000000"))
                {
                    _listComboBox.Add(new ComboBox());
                    _listComboBox[_listComboBox.Count - 1].ID = String.Format("History{0}", sguid);
                    _listComboBox[_listComboBox.Count - 1].Width = Unit.Pixel(120);
                    _listComboBox[_listComboBox.Count - 1].Height = Unit.Pixel(15);
                    _listComboBox[_listComboBox.Count - 1].Mode = ComboBoxMode.TextBox;
                    _listComboBox[_listComboBox.Count - 1].Enabled = false;
                    _listComboBox[_listComboBox.Count - 1].Items.Add(new ComboBoxItem(_dtEmployee.Select(String.Format("Id='{0}'", sguid))[0]["DisplayName"].ToString()));
                    _listComboBox[_listComboBox.Count - 1].SelectedIndex = 0;

                    cbName.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
                }
            }

        }
    }
    public class HeaderTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            Literal header = new Literal();
            header.Text = "<div class=\"header\">Priviledges for</div>";
            container.Controls.Add(header);
        }
    }
}
