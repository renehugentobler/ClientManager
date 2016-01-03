using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.OleDb;
using System.Collections;

using System.Net.Mail;

using HSoft.SQL;

using Obout.ComboBox;
using Obout.Grid;

using HSoft.ClientManager.Web;

public partial class Pages_Email_Promo_Send : System.Web.UI.Page
{
    Grid grid1 = new Grid();
    DataTable _dtPromos = new DataTable();
    DateTime _dtlimit = DateTime.Now.Date.AddDays(1);

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.devlogincheat();
        if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        if (_dtPromos.Rows.Count==0)
        {
            ssql = "SELECT ep.*,e.EmailDescription " +
                   "  FROM Email_Promo ep,Email e " +
                   " WHERE ep.isactive = 1 " +
                   "   AND ep.isdeleted = 0 "  +
                   "   AND e.isdeleted = 0 " +
                   "   AND ep.EmailId = e.Id " +
                   " ORDER BY EmailKey";
            _dtPromos = _sql.GetTable(ssql);
        }

        grid1.ID = "grid1";
        grid1.CallbackMode = false;
        grid1.Serialize = true;
        grid1.AutoGenerateColumns = false;
        grid1.PageSize = -1;
//         grid1.Height = int.Parse(Session["wy"].ToString()) - 40;
        grid1.AllowAddingRecords = false;
        grid1.AllowPaging = false;

        grid1.FolderStyle = "styles/grid/premiere_blue";

        grid1.UpdateCommand += new Obout.Grid.Grid.EventHandler(UpdateRecord);
        grid1.Rebind += new Obout.Grid.Grid.DefaultEventHandler(RebindGrid);

        grid1.RowCreated += new Obout.Grid.GridRowEventHandler(grid1_RowCreated);

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
        oCol0.AllowEdit = true;
        oCol0.AllowDelete = false;

        Column oColi = new Column();
        oColi.ID = "ID";
        oColi.DataField = "Id";
        oColi.Visible = false;

        Column oCol1 = new Column();
        oCol1.ID = "Name";
        oCol1.DataField = "Name";
        oCol1.HeaderText = "Name";
        oCol1.Visible = true;
        oCol1.ReadOnly = true;
        oCol1.Width = "120";
        oCol1.AllowSorting = true;
        oCol1.Wrap = true;
        oCol1.AllowFilter = true;
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol1.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        Column oCol2 = new Column();
        oCol2.ID = "Email";
        oCol2.DataField = "Email";
        oCol2.HeaderText = "Email";
        oCol2.Visible = true;
        oCol2.ReadOnly = true;
        oCol2.Width = "180";
        oCol2.AllowSorting = true;
        oCol2.AllowFilter = true;
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.EqualTo));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.StartsWith));
        oCol2.FilterOptions.Add(new FilterOption(FilterOptionType.EndsWith));

        grid1.Columns.Add(oCol0);
        grid1.Columns.Add(oColi);
        grid1.Columns.Add(oCol1);

        List<CheckBoxColumn> oCole = new List<CheckBoxColumn>();
        foreach (DataRow dr in _dtPromos.Rows)
        {
            CheckBoxColumn _col = new CheckBoxColumn(); ;
            _col.ID = String.Format("em{0}", dr["EmailKey"]);
            _col.DataField = String.Format("em{0}", dr["EmailKey"]);
            _col.HeaderText = dr["EmailKey"].ToString();
            _col.Visible = true;
            _col.ReadOnly = false;
            _col.Width = "40";
            _col.AllowSorting = true;
            _col.AllowFilter = false;
            _col.ControlType = GridControlType.Obout;
            oCole.Add(_col);
            grid1.Columns.Add(_col);
        }

        Column oCol5 = new Column();
        oCol5.ID = "EntryDate";
        oCol5.DataField = "EntryDate";
        oCol5.HeaderText = "Entry Date";
        oCol5.Visible = true;
        oCol5.ReadOnly = true;
        oCol5.Width = "80";
        oCol5.AllowSorting = true;
        oCol5.AllowFilter = false;
        oCol5.NullDisplayText = "missing!";
        oCol5.DataFormatString = "{0:MM/dd/yyyy hh:mm tt}";
        oCol5.DataFormatString = "{0:MM/dd/yyyy}";

        Column oCol8 = new Column();
        oCol8.ID = "Status";
        oCol8.DataField = "Status";
        oCol8.HeaderText = "Status";
        oCol8.Visible = true;
        oCol8.ReadOnly = true;
        oCol8.Width = "90";
        oCol8.AllowSorting = true;
        oCol8.AllowFilter = false;

        Column oCol9 = new Column();
        oCol9.ID = "Priority";
        oCol9.DataField = "Priority";
        oCol9.HeaderText = "Priority";
        oCol9.Visible = true;
        oCol9.ReadOnly = true;
        oCol9.Width = "100";
        oCol9.AllowSorting = true;
        oCol9.AllowFilter = false;

        Column oCol10 = new Column();
        oCol10.ID = "AssignedTo";
        oCol10.DataField = "AssignedTo";
        oCol10.HeaderText = "AssignedTo";
        oCol10.Visible = true;
        oCol10.ReadOnly = false;
        oCol10.Width = "120";
        oCol10.AllowSorting = false;
        oCol10.AllowFilter = false;

        Column oCol11 = new Column();
        oCol11.ID = "LeadNote";
        oCol11.DataField = "LeadNote";
        oCol11.HeaderText = "Lead Note";
        oCol11.Visible = true;
        oCol11.ReadOnly = true;
        oCol11.Width = "350";
        oCol11.Wrap = true;
        oCol11.AllowSorting = false;
        oCol11.ParseHTML = true;
        oCol11.AllowFilter = true;
        oCol11.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol11.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));

        Column oCol12 = new Column();
        oCol12.ID = "SalesNote";
        oCol12.DataField = "SalesNote";
        oCol12.HeaderText = "Sales Note";
        oCol12.Visible = true;
        oCol12.ReadOnly = true;
        oCol12.Width = "350";
        oCol12.Wrap = true;
        oCol12.AllowSorting = false;
        oCol12.ParseHTML = true;
        oCol12.AllowFilter = true;
        oCol12.FilterOptions.Add(new FilterOption(FilterOptionType.NoFilter));
        oCol12.FilterOptions.Add(new FilterOption(FilterOptionType.Contains));

        grid1.Columns.Add(oCol5);
        grid1.Columns.Add(oCol9);
        grid1.Columns.Add(oCol11);
        grid1.Columns.Add(oCol12);
        grid1.Columns.Add(oCol8);
        grid1.Columns.Add(oCol2);
        grid1.Columns.Add(oCol10);

        GridRuntimeTemplate HeadingTemplate = new GridRuntimeTemplate();
        HeadingTemplate.ID = "HeadingTemplate1";
        HeadingTemplate.Template = new Obout.Grid.RuntimeTemplate();
        HeadingTemplate.Template.CreateTemplate += new Obout.Grid.GridRuntimeTemplateEventHandler(CreateHeadingTemplate); 
        
        grid1.TemplateSettings.HeadingTemplateId = "HeadingTemplate1";
        grid1.Templates.Add(HeadingTemplate);

        // add the grid to the controls collection of the PlaceHolder
        phGrid1.Controls.Add(grid1);

        if (!Page.IsPostBack)
        {
            BindGrid();
        }
    }

    protected void grid1_RowCreated(object sender, GridRowEventArgs e)
    {
        int icount = 3;
        if (e.Row.RowType == GridRowType.Header)
        {
            foreach (DataRow dr in _dtPromos.Rows)
            {
                e.Row.Cells[icount++].ToolTip = dr["EmailDescription"].ToString();
            }
        }
        if (e.Row.RowType == GridRowType.DataRow)
        {
//            if (e.Row.Cells[16].Text == "0")
//            {
//                e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
//                e.Row.Cells[1]..Text = "test"; // e.Row.Cells[5].Text;
//            }
        }
    }

    public void CreateHeadingTemplate(Object sender, Obout.Grid.GridRuntimeTemplateEventArgs e)
    {
        List<ComboBox> _listComboBox = new List<ComboBox>();
        _listComboBox.Add(new ComboBox());

        _listComboBox[0].ID = String.Format("Header");
        _listComboBox[0].Width = Unit.Pixel(150);
        _listComboBox[0].Height = Unit.Pixel(15);
        _listComboBox[0].Mode = ComboBoxMode.TextBox;
        _listComboBox[0].Enabled = false;
        _listComboBox[0].Items.Add(new ComboBoxItem("Lead Emails"));
        _listComboBox[0].SelectedIndex = 0;
        e.Container.Controls.Add(_listComboBox[0]);
    }

    void BindGrid()
    {
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        String sassigned = Session["ghirarchy"].ToString();
        sassigned = String.Format("{0}{1}", sassigned.Replace(",", "','"), Guid.Empty);

        ssql = "SELECT TOP 50 ";
        foreach (DataRow dr in _dtPromos.Rows)
        {
            ssql = String.Format("{0} " +
                                 "(SELECT CONVERT(BIT,COUNT(*)) FROM Email_Lead el WHERE el.isdeleted = 0 AND el.LeadId = lf.Id AND UPPER(el.EmailId) = '{1}') em{2}, ",ssql,dr["EmailId"].ToString().ToUpper(),dr["EmailKey"]);
        }
        ssql = String.Format("{0} * " +
                             "  FROM Lead_Flat lf, _LeadPriority lp " + // , _email_verify ev" +
                             " WHERE lf.isdeleted=0 " +
                             "   AND PriorityId = lp.Id " +
                             "   AND lp.[IsLead] = 1 " +
                             "   AND lp.isdeleted = 0 " +
                             "   AND lf.AssignedToId IN ('{1}') " +
                             // "   AND lf.Customer = 5985 " +
//                             "   AND ev.Email = lf.Email " +
//                             "   AND ((ev.isValidated = 0) OR (ev.isValidated = 1 AND ev.iserror=0)) " +  
                             "   AND EntryDate <= '{2}' " +
                             " ORDER BY EntryDate DESC ", ssql, sassigned,_dtlimit);
        DataTable _dtLeads = _sql.GetTable(ssql);

        grid1.DataSource = _dtLeads;
        grid1.DataBind();

        _sql.Close();
    }
    void RebindGrid(object sender, EventArgs e) { BindGrid(); }

    void UpdateRecord(object sender, GridRecordEventArgs e)
    {

        _dtlimit = DateTime.Parse(e.Record["EntryDate"].ToString()).Date.AddDays(1);

        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        ssql = "SELECT ";
        foreach (DataRow dr in _dtPromos.Rows)
        {
            ssql = String.Format("{0} " +
                                 "(SELECT CONVERT(BIT,COUNT(*)) FROM Email_Lead el WHERE el.isdeleted = 0 AND el.LeadId = lf.Id AND UPPER(el.EmailId) = '{1}') em{2}, ", ssql, dr["EmailId"].ToString().ToUpper(), dr["EmailKey"]);
        }
        ssql = String.Format("{0} * " +
                             "  FROM Lead_Flat lf, _LeadPriority lp " +
                             " WHERE lf.isdeleted=0 " +
                             "   AND PriorityId = lp.Id " +
                             "   AND lp.[IsLead] = 1 " +
                             "   AND lp.isdeleted = 0 " +
                             "   AND lf.Id = '{1}' " +
                             " ORDER BY EntryDate DESC ", ssql,e.Record["Id"].ToString());
        DataRow _dtLead = _sql.GetTable(ssql).Rows[0];
        DataRow _emp = _sql.GetTable(String.Format("SELECT * FROM Employee WHERE Id = '{0}'", _dtLead["AssignedToId"])).Rows[0];

        String strName = _dtLead["Name"].ToString();
        String stremail = _emp["EmailName"].ToString();
        String strSignature = _emp["DisplayName"].ToString();

        MailAddress _smtpfrom = new MailAddress(String.Format("emailer@uncleharry.biz", stremail), "Sales Uncleharry");
        MailAddress _from = new MailAddress(String.Format("{0}@uncleharry.biz", stremail), "Sales Uncleharry");
        MailAddress _to = new MailAddress(String.Format("{0}", _dtLead["EMail"]), strName);
//        _to = new MailAddress("tatyana@uncleharry.biz", strName);                                       // send to Tatyana for testing
//        _to = new MailAddress("rene.hugentobler@gmail.com", strName);
        MailAddress _sender = new MailAddress(String.Format("{0}@uncleharry.biz", stremail), "Sales Uncleharry");

        SortedList<int, int> _emails = new SortedList<int, int>();
        foreach (String _num in _dtLead["MsgHistory"].ToString().Split(','))
        {
            if (_num.Trim().Length > 0) { _emails.Add(int.Parse(_num.Trim()), int.Parse(_num.Trim())); }
        }
        Boolean _new = false;
        foreach (DataRow dr in _dtPromos.Rows)
        {
            string ekey = String.Format("em{0}", dr["EmailKey"]);
            if (_dtLead[ekey].ToString().ToLower() == "false")
            {
                if (e.Record[ekey].ToString().ToLower() == "true")
                {
                    _new = true;

                    _emails.Add(int.Parse(ekey.ToLower().Split('m')[1]), int.Parse(ekey.ToLower().Split('m')[1]));

                    DataRow _drEmail = _sql.GetTable(String.Format("SELECT e.* FROM Email e, Email_Promo ep WHERE e.isdeleted = 0 AND ep.isdeleted = 0 AND ep.isactive = 1 AND ep.EmailKey = {0} AND ep.EmailId = e.Id", dr["EmailKey"])).Rows[0];

                    String sEmail = String.Format(_drEmail["EmailBody"].ToString(), strName, strSignature, stremail);
                    String ssubject = _drEmail["EmailSubject"].ToString();

                    try
                    {
                        Tools.sendMail(_smtpfrom, @"Taurec86@",_from, _to, ssubject, sEmail, true);
                        // SMTPServer.Send(message);
                    }
                    catch (Exception ex)
                    {
                        Exception ex2 = new Exception(String.Format("{0}", ex.Message));
//                            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", "OpenPopupWithHtml('" + ex.Message + "', 'ERROR');");
//                            return;
                        throw ex2;
                    }

                    ssql = String.Format("INSERT INTO Email_Lead(EmailId, LeadId, EmployeeId) VALUES ('{0}','{1}','{2}')", _drEmail["Id"], e.Record["Id"], Session["guser"]);
                    _sql.Execute(ssql);
                }
            }
        }

        if (_new)
        {
            String _history = String.Empty;
            foreach (KeyValuePair<int, int> _key in _emails)
            {
                _history = String.Format("{0}{1},", _history, _key.Value);
            }
            ssql = String.Format("UPDATE t SET MsgHistory = '{0}' FROM Lead_Flat t WHERE Id = '{1}' ", _history.Substring(0, _history.Length - 1), e.Record["Id"]);    
            _sql.Execute(ssql);
        }
//        SMTPServer.Dispose();       
        _sql.Close();     
    }


}