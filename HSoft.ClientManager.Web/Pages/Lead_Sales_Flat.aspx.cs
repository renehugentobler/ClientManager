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

public partial class Pages_Lead_Sales_Flat : System.Web.UI.Page
{

    static DataTable _dtEmployee = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {

        // for testing only
        if (Session["guser"] == null) { Session["guser"] = "7d5aa961-5478-4fa1-b5db-d6a2071ed834"; }
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        String ssql = String.Empty;

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = "SELECT * FROM Employee WHERE isdeleted = 0";
        _dtEmployee = _sql.GetTable(ssql);

        sdsLead_Flat.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;
//        sdsLead_Flat.SelectCommand = String.Format("SELECT * FROM Lead_Flat WHERE AssignedToId = '{0}'", Session["guser"].ToString());
        String sassigned = Session["ghirarchy"].ToString();
        sassigned = String.Format("{0}{1}",sassigned.Replace(",","','"),Guid.Empty);
        sdsLead_Flat.SelectCommand = String.Format("SELECT * " +
                                                   "  FROM Lead_Flat " +
                                                   " WHERE AssignedToId IN ('{0}') ORDER BY EntryDate DESC", sassigned);

        sdsSuperForm1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;
        sdsPriority.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;
        sdsEmployee.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString;

        List<ComboBox> _listComboBox = new List<ComboBox>();
        foreach (String sguid in Session["ghirarchy"].ToString().Split(','))
        {
            if (sguid.Length != 0)
            {
                _listComboBox.Add(new ComboBox());
                _listComboBox[_listComboBox.Count - 1].ID = String.Format("History{0}", sguid);
                _listComboBox[_listComboBox.Count - 1].Width = Unit.Pixel(200);
                _listComboBox[_listComboBox.Count - 1].Height = Unit.Pixel(15);
                _listComboBox[_listComboBox.Count - 1].Mode = ComboBoxMode.TextBox;
                _listComboBox[_listComboBox.Count - 1].Enabled = false;
                _listComboBox[_listComboBox.Count - 1].Items.Add(new ComboBoxItem(_dtEmployee.Select(String.Format("Id='{0}'", sguid))[0]["DisplayName"].ToString()));
                _listComboBox[_listComboBox.Count - 1].SelectedIndex = 0;

                cbName.Controls.Add(_listComboBox[_listComboBox.Count - 1]);
            }
        }

        // grid filter
        GridFilteringSettings gfs = new GridFilteringSettings();
        gfs.FilterPosition = GridFilterPosition.Top;
        gfs.FilterLinksPosition = GridElementPosition.Top;
        //        gfs.InitialState = GridFilterState.Visible;
        gfs.InitialState = GridFilterState.Hidden;
        grid1.FilteringSettings = gfs;
        grid1.AllowFiltering = true;


    }

    public void OnItemUpdated(object sender, object e)
    {
        // try to update the record
        // ...
        // throw an exception if something goes wrong
        SuperForm form = sender as SuperForm;
        string AssignTo = String.Empty;
        try
        {
            AssignTo = DataBinder.Eval(form.DataItem, "AssignTo").ToString();
        }
        catch { }
        Console.WriteLine(AssignTo);
        DetailsViewUpdateEventArgs  dal = e as DetailsViewUpdateEventArgs;
        object c = dal;
        //        throw new Exception("An error occured when trying to update the record.");
    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        SuperForm form = sender as SuperForm;
        string AssignTo = String.Empty;
        try
        {
            AssignTo = DataBinder.Eval(form.DataItem, "AssignTo").ToString();
        }
        catch { }
        Console.WriteLine(AssignTo);
    }


    public void OnItemUpdating(object sender, object e)
    {
        // try to update the record
        // ...
        // throw an exception if something goes wrong
        throw new Exception("An error occured when trying to update the record.");
    }

}