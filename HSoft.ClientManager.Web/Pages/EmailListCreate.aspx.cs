using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;

using HSoft.SQL;
using HSoft.ClientManager.Web;

using SisoDb.Sql2012;
using SisoDb.Configurations;

using CTCT;
using CTCT.Components;
using CTCT.Components.Contacts;
using CTCT.Components.AccountService;
using CTCT.Components.EmailCampaigns;
using CTCT.Exceptions; 

using Obout.Grid;


public partial class Pages_EmailListCreate : System.Web.UI.Page
{

    Grid grid1 = new Grid();
    DataTable dt = new DataTable();

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        dt.Clear();
        dt.Columns.Add("Id");
        dt.Columns.Add("Date");
        dt.Columns.Add("eMail");
        dt.Columns.Add("Added");

//        Boolean bres = false;

        ConstantContact _constantContact = null;
        string _apiKey = string.Empty;
        string _accessToken = string.Empty;

        _apiKey = ConfigurationManager.AppSettings["APIKey"];
        _accessToken = ConfigurationManager.AppSettings["AccessToken"];
        if (_accessToken.Length != new Guid().ToString().Length)
        {
            byte[] decryptedB = Convert.FromBase64String(ConfigurationManager.AppSettings["AccessToken"]);
            _accessToken = System.Text.Encoding.UTF8.GetString(decryptedB).Replace("\0", "");
        }
        _constantContact = new ConstantContact(_apiKey, _accessToken);

        IList<ContactList> _rscl = _constantContact.GetLists(null);


        SortedList<int, ContactList> CCLists = new SortedList<int, ContactList>();
        SortedList<string, SortedList<int, ContactList>> CCListsA = new SortedList<string, SortedList<int, ContactList>>();

        foreach (ContactList _cl in _rscl)
        {
            if (_cl.Name.StartsWith("Leads Group")==true)
            {
                string[] sa = _cl.Name.Split(' ');
                string su = String.Empty;
                string si = String.Empty;
                if (sa.Length > 3)
                {
                    su = sa[2];
                    si = sa[3];
                }
                else
                {
                    su = "A";
                    si = sa[2];
                }
                if (!CCListsA.ContainsKey(su))
                {
                    CCListsA.Add(su, new SortedList<int, ContactList>()); 
                }
                SortedList<int, ContactList> _ccl = CCListsA[su];
                _ccl.Add(int.Parse(si), _cl);
            }
        }

        foreach (KeyValuePair<string, SortedList<int, ContactList>> kvListA in CCListsA)
        {
            string skey = kvListA.Key;
            SortedList<int, ContactList> _cls = kvListA.Value;

            string slist = "'XXX'";
            Int16 _errcnt = 0;
            foreach (KeyValuePair<int, ContactList> kvList in _cls)
            {
                if ((kvList.Value.ContactCount >= 500) || ("1127501113,1816366451,1389001238,2004642327".Contains(kvList.Value.Id)))
                {
                    // got the list
                    slist = String.Format("{0},'{1}'", slist, kvList.Value.Id);
                }
                else
                {
                    slist = String.Format("{0},'{1}'", slist, kvList.Value.Id);
                    // now there is a new list
                    String ssql = String.Empty;
                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

                    ssql = String.Format("SELECT a.StructureId , a.Value Date,b.Value eMail, c.Value Id, e.Value Status " +
                                         "  FROM [dbo].[ContactStrings] a " +
                                         "   LEFT JOIN [dbo].[ContactStrings] b ON a.StructureId =b.StructureId AND b.MemberPath = 'EmailAddresses.EmailAddr' " +
                                         "   LEFT JOIN [dbo].[ContactStrings] c ON a.StructureId =c.StructureId AND c.MemberPath = 'Id' " +
                                         "   LEFT JOIN [dbo].[ContactStrings] e ON a.StructureId =e.StructureId AND e.MemberPath = 'Status' " +
                                         "   LEFT OUTER JOIN Lead_Flat lf ON lf.ConstantContactID =  c.Value " +
                        //                                     "   LEFT OUTER JOIN [dbo].[ContactStrings] d ON a.StructureId = d.StructureId AND d.MemberPath = 'Lists.Id' AND d.Value = 1127501113 " +
                                         " WHERE a.MemberPath = 'DateCreated' " +
                                         "   AND a.Value <= DateAdd(day,-30,GetDate()) " +
                                         "   AND COALESCE(lf.Priority,'CCOnly') NOT IN ('Not Right','Sold','Spam','Black List','Discarded') " +
                                         "   AND 0 = (SELECT COUNT(*) FROM  [dbo].[ContactStrings] x WHERE a.StructureId = x.StructureId AND x.MemberPath = 'Lists.Id' AND x.Value IN ({0})) " +
                        // "   AND COALESCE(d.Value,0) NOT IN (1127501113) " +
                                         " ORDER BY a.Value DESC ", slist);
                    DataTable _Leads = _sql.GetTable(ssql);
                    int icount = kvList.Value.ContactCount;
                    SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db();
                    foreach (DataRow dr in _Leads.Rows)
                    {
                        CTCT.Components.Contacts.Contact _ct = null;
                        if (dr["Status"].ToString() == "OPTOUT") { continue; }
                        try
                        {
                            _ct = _constantContact.GetContact(dr["Id"].ToString());
                        }
                        catch (Exception ex)
                        {
                            ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                                 "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "CClist", "ADD", dr["Id"].ToString(), "", Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
                            _sql.Execute(ssql);
                            if (_errcnt++ > 10) { throw ex; }
                        }
                        Thread.Sleep(250);
                        _ct.Lists.Add(kvList.Value);
                        if (_ct.EmailAddresses.Count == 0) { continue; }
                        try
                        {
                            _ct = _constantContact.UpdateContact(_ct, false);
                            ++icount;
                        }
                        catch (Exception ex)
                        {
                            ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                                 "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "CClist", "ADD", _ct.ToJSON().Replace("'", "''"), dr["Id"].ToString().Replace("'", "''"), Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
                            _sql.Execute(ssql);
                            if (_errcnt++ > 50) { throw ex; }
                        }
                        //                    Boolean bfound = false;
                        //                    foreach (ContactList _cl in _ct.Lists)
                        //                    {
                        //                        if (_cl.Id == kvList.Value.Id)
                        //                        {
                        //                            bfound = true;
                        //                        }
                        //                    }
                        DataRow _dr = dt.NewRow();
                        _dr["Id"] = dr["Id"];
                        _dr["Date"] = dr["Date"];
                        _dr["eMail"] = dr["eMail"];
                        _dr["eMail"] = dr["eMail"];
                        //                    _dr["Added"] = bfound;
                        dt.Rows.Add(_dr);

                        //                    if (!bfound) { continue; }

                        using (var session = _siso.BeginSession())
                        {
                            session.DeleteById<CTCT.Components.Contacts.Contact>(dr["Id"].ToString());
                            session.Insert(_ct);
                        }
                        if (icount >= 500) { break; }
                    }
                    break;
                }
            }
        }




        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        grid1.ID = "grid1";
        grid1.CallbackMode = false;
        grid1.Serialize = true;
        grid1.AutoGenerateColumns = false;
        grid1.PageSize = -1;
        grid1.Height = int.Parse(Session["wy"].ToString()) - 40;
        grid1.EnableRecordHover = true;
        grid1.AllowAddingRecords = true;
        grid1.AllowPaging = false;
        grid1.AllowPageSizeSelection = false;

        grid1.FolderStyle = "styles/grid/premiere_blue";

        // setting the event handlers
        grid1.InsertCommand += new Obout.Grid.Grid.EventHandler(InsertRecord);
//        grid1.DeleteCommand += new Obout.Grid.Grid.EventHandler(DeleteRecord);
//        grid1.UpdateCommand += new Obout.Grid.Grid.EventHandler(UpdateRecord);
        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        grid1.ClientSideEvents.OnClientSelect = "onClientSelect";

        // grid filter
        GridFilteringSettings gfs = new GridFilteringSettings();
        gfs.FilterPosition = GridFilterPosition.Top;
        gfs.FilterLinksPosition = GridElementPosition.Top;
        //        gfs.InitialState = GridFilterState.Visible;
        gfs.InitialState = GridFilterState.Hidden;
        grid1.FilteringSettings = gfs;
        grid1.AllowFiltering = true;
        
        Column oCol0 = new Column();
        oCol0.DataField = "";
        oCol0.HeaderText = "";
        oCol0.Width = "45";
        oCol0.AllowEdit = false;
        oCol0.AllowDelete = false;

        Column oColi = new Column();
        oColi.ID = "ID";
        oColi.DataField = "Id";
        oColi.Visible = false;

        Column oCol1 = new Column();
        oCol1.ID = "Id";
        oCol1.DataField = "Id";
        oCol1.HeaderText = "Id";
        oCol1.Visible = true;
        oCol1.ReadOnly = true;
        oCol1.Width = "80";
        oCol1.AllowSorting = true;
        oCol1.AllowFilter = true;
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol2 = new Column();
        oCol2.ID = "eMail";
        oCol2.DataField = "eMail";
        oCol2.HeaderText = "eMail";
        oCol2.Visible = true;
        oCol2.ReadOnly = true;
        oCol2.Width = "240";
        oCol2.AllowSorting = true;
        oCol2.AllowFilter = true;
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol3 = new Column();
        oCol3.ID = "Name";
        oCol3.DataField = "Name";
        oCol3.HeaderText = "Name";
        oCol3.Visible = true;
        oCol3.ReadOnly = true;
        oCol3.Width = "400";
        oCol3.AllowSorting = true;
        oCol3.AllowFilter = true;
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol3.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol4 = new Column();
        oCol4.ID = "Date";
        oCol4.DataField = "Date";
        oCol4.HeaderText = "Date";
        oCol4.Visible = true;
        oCol4.ReadOnly = true;
        oCol4.Width = "180";
        oCol4.AllowSorting = true;
        oCol4.AllowFilter = false;

        Column oCol5 = new Column();
        oCol5.ID = "Date";
        oCol5.DataField = "Date";
        oCol5.HeaderText = "Date";
        oCol5.Visible = true;
        oCol5.ReadOnly = true;
        oCol5.Width = "40";
        oCol5.AllowSorting = true;
        oCol5.AllowFilter = false;

//        grid1.Columns.Add(oCol0);
//        grid1.Columns.Add(oColi);
        grid1.Columns.Add(oCol1);
        grid1.Columns.Add(oCol2);
//        grid1.Columns.Add(oCol3);
        grid1.Columns.Add(oCol4);
        grid1.Columns.Add(oCol5);

        // add the grid to the controls collection of the PlaceHolder
        phGrid1.Controls.Add(grid1);

        if (!Page.IsPostBack)
        {
            BindGrid();
        }
        

    }
    void BindGrid()
    {
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        String sassigned = Session["ghirarchy"].ToString();
        sassigned = String.Format("{0}{1}", sassigned.Replace(",", "','"), Guid.Empty);

        ssql = String.Format("SELECT a.Value Id, b.Value Status, c.Value Name, d.Value Modified" +
                             "  FROM [EmailCampaignStrings] a " +
                             "     LEFT OUTER JOIN [EmailCampaignStrings] b ON a.StructureId = b.StructureId AND b.MemberPath = 'Status' " +
                             "     LEFT OUTER JOIN [EmailCampaignStrings] c ON a.StructureId = c.StructureId AND c.MemberPath = 'Name' " +
                             "     LEFT OUTER JOIN [EmailCampaignDates] d ON a.StructureId = d.StructureId AND d.MemberPath = 'ModifiedDate' " +
                             "  WHERE a.MemberPath = 'Id' " +
                             "   ORDER BY a.Value Desc ", sassigned);
        DataTable _dtLists = _sql.GetTable(ssql);

//        grid1.DataSource = _dtLists;
        grid1.DataSource = dt;
        grid1.DataBind();

        _sql.Close();
    }
    void RebindGrid(object sender, EventArgs e) { BindGrid(); }
    void InsertRecord(object sender, GridRecordEventArgs e)
    {
    }

}