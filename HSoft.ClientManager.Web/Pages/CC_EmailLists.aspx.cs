using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Data.OleDb;
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

using Obout.Interface;

public partial class CC_EmailLists : System.Web.UI.Page
{

    private static String ssql = String.Empty;

    public ConstantContact _constantContact = null;
    private string _apiKey = string.Empty;
    private string _accessToken = string.Empty;

    SortedList<int, ContactList> CCNoLead = new SortedList<int, ContactList>();
    SortedList<int, ContactList> CCUnSubscribed = new SortedList<int, ContactList>();

    string sCCNoLead = "";
    string sCCUnSubscribed = "";

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["wx"] == null) { Session["wx"] = Request.QueryString["wx"]; }
        if (Session["wy"] == null) { Session["wy"] = Request.QueryString["wy"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

            Tools.devlogincheat();
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

            _apiKey = ConfigurationManager.AppSettings["APIKey"];
            _accessToken = ConfigurationManager.AppSettings["AccessToken"];
            if (_accessToken.Length != new Guid().ToString().Length)
            {
                byte[] decryptedB = Convert.FromBase64String(ConfigurationManager.AppSettings["AccessToken"]);
                _accessToken = System.Text.Encoding.UTF8.GetString(decryptedB).Replace("\0", "");
            }
            _constantContact = new ConstantContact(_apiKey, _accessToken);

            IList<ContactList> _rscl = _constantContact.GetLists(null);

            foreach (ContactList _cl in _rscl)
            {
                if (_cl.Name.StartsWith("No Lead Group") == true)
                {
                    CCNoLead.Add(int.Parse(_cl.Name.Split(' ')[3]), _cl);
                    sCCNoLead = String.Format("{0}'{1}',", sCCNoLead, _cl.Id);
                }
                if (_cl.Name.StartsWith("Unsubscribed Group") == true)
                {
                    CCUnSubscribed.Add(int.Parse(_cl.Name.Split(' ')[2]), _cl);
                    sCCUnSubscribed = String.Format("{0}'{1}',", CCUnSubscribed, _cl.Id);
                }
            }
            sCCNoLead = String.Format("{0}'{1}'", sCCNoLead, "XXX");
            sCCUnSubscribed = String.Format("{0}'{1}'", CCUnSubscribed, "XXX");

            foreach (KeyValuePair<int, ContactList> kvList in CCNoLead)
            {
                HtmlTableRow _row = new HtmlTableRow();
                HtmlTableCell _cell1 = new HtmlTableCell();
                HtmlTableCell _cell2 = new HtmlTableCell();

                _cell1.Controls.Add(new LiteralControl(kvList.Value.Name));
                _cell2.Controls.Add(new LiteralControl(kvList.Value.ContactCount.ToString()));

                _row.Cells.Add(_cell1);
                _row.Cells.Add(_cell2);

                list.Rows.Add(_row);
            
            }

            foreach (KeyValuePair<int, ContactList> kvList in CCUnSubscribed)
            {
                HtmlTableRow _row = new HtmlTableRow();
                HtmlTableCell _cell1 = new HtmlTableCell();
                HtmlTableCell _cell2 = new HtmlTableCell();

                _cell1.Controls.Add(new LiteralControl(kvList.Value.Name));
                _cell2.Controls.Add(new LiteralControl(kvList.Value.ContactCount.ToString()));

                _row.Cells.Add(_cell1);
                _row.Cells.Add(_cell2);

                list.Rows.Add(_row);

            }

            _sql.Close();

    }

    protected void CreateList_Lead_500()
    {
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
        SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db();

        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
        foreach (KeyValuePair<int, ContactList> kvList in CCNoLead)
        {
            if (kvList.Value.ContactCount<500)
            {

                ssql = String.Format("SELECT a.StructureId , a.Value Date,b.Value eMail, c.Value Id, e.Value Status " +
                                     "  FROM [dbo].[ContactStrings] a " +
                                     "   LEFT JOIN [dbo].[ContactStrings] b ON a.StructureId =b.StructureId AND b.MemberPath = 'EmailAddresses.EmailAddr' " +
                                     "   LEFT JOIN [dbo].[ContactStrings] c ON a.StructureId =c.StructureId AND c.MemberPath = 'Id' " +
                                     "   LEFT JOIN [dbo].[ContactStrings] e ON a.StructureId =e.StructureId AND e.MemberPath = 'Status' " +
                                     "   LEFT OUTER JOIN Lead_Flat lf ON lf.ConstantContactID =  c.Value " +
                                     " WHERE a.MemberPath = 'DateCreated' " +
                                     "   AND COALESCE(lf.Priority,'CCOnly') NOT IN ('Not Right','Sold','Spam','Black List','Discarded') " +
                                     "   AND 0 = (SELECT COUNT(*) FROM  [dbo].[ContactStrings] x WHERE a.StructureId = x.StructureId AND x.MemberPath = 'Lists.Id' AND x.Value IN ({0})) " +
                                     "   AND e.Value = 'ACTIVE' " +
                                     "   AND 0 = (SELECT COUNT(*) FROM Lead_Flat WHERE c.Value = ConstantContactID) " +
                                     " ORDER BY a.Value DESC ", sCCNoLead);
                DataTable _Leads = _sql.GetTable(ssql);
                int icount = kvList.Value.ContactCount;
                foreach (DataRow dr in _Leads.Rows)
                {
                    Thread.Sleep(250);
                    CTCT.Components.Contacts.Contact _ct = null;
                    try
                    {
                        _ct = _constantContact.GetContact(dr["Id"].ToString());
                    }
                    catch (Exception ex)
                    {
                        ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                             "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "No Lead Group", "READ", dr["Id"].ToString(), ex.Message.Replace("'", "''"), Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
                        _sql.Execute(ssql);
                        continue;
                    }
                    if (_ct.EmailAddresses.Count == 0) { continue; }
                    _ct.Lists.Add(kvList.Value);
                    try
                    {
                        _ct = _constantContact.UpdateContact(_ct, false);
                        icount++;
                    }
                    catch (Exception ex)
                    {
                        ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                             "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "No Lead Group", "ADD", _ct.ToJSON().Replace("'", "''"), ex.Message.Replace("'", "''"), Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
                        _sql.Execute(ssql);
                        continue;
                    }
                    using (var session = _siso.BeginSession())
                    {
                        session.DeleteById<CTCT.Components.Contacts.Contact>(_ct.Id);
                        session.Insert(_ct);
                    }
                    if (icount >= 500) { break; }
                }
                break;
            }
        }
        _sql.Close();

    }

    protected void Btn_Command(object sender, CommandEventArgs e)
    {
        CC_Command(e.CommandName);
    }

    protected void CC_Command(string scmd)
    {
        switch (scmd)
        {
            case "CC_Lead_500":
                {
                    CreateList_Lead_500();
                }
                break;
            case "CC_Unsubscribed_500":
                {
                }
                break;
        }
        Response.Redirect("/Pages/CC_EmailLists.aspx");
    }

}