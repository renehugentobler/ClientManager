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
using System.IO;

using Obout.Interface;

using HSoft.SQL;
using HSoft.ClientManager.Web;

using SisoDb.Sql2012;
using SisoDb.Configurations;

public partial class Pages_CC_Emails : System.Web.UI.Page
{

    private static String ssql = String.Empty;
    private static DataTable _dtemails = null;
    private static DataTable _dtclicks = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        if (!IsPostBack)
        {
            Tools.devlogincheat();
            if (Session["ghirarchy"] == null) { Session["ghirarchy"] = String.Format("{0},", Session["guser"]); }

            LastUpdate.Text = _sql.ExecuteScalar("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Campaings' AND isdeleted = 0").ToString();
            //            CountContacts.Text = _sql.ExecuteScalar("SELECT COUNT(*) FROM ContactStrings WHERE MemberPath = 'Status'").ToString();
            //            CountActive.Text = _sql.ExecuteScalar("SELECT COUNT(*) FROM ContactStrings WHERE MemberPath = 'Status' AND LOWER(Value) ='active'").ToString();
            //            CountLeads.Text = _sql.ExecuteScalar("SELECT COUNT(*) FROM Lead_Flat WHERE isdeleted = 0 ").ToString();
            //            Linked.Text = _sql.ExecuteScalar("SELECT COUNT(*) FROM Lead_Flat WHERE ConstantContactID >0 AND isdeleted = 0 ").ToString();
        }

        ssql = "SELECT StructureId Id, " +
               "       (SELECT Value FROM EmailCampaignStrings WHERE StructureId = ecs.StructureId AND MemberPath = 'Name') Name, " +
               "       (SELECT Value FROM EmailCampaignDates WHERE StructureId = ecs.StructureId AND MemberPath = 'ModifiedDate') Opened, " +
               "       COALESCE((SELECT isTracked FROM EmailCampaignTracking WHERE EmailCampaignId = ecs.StructureId),0) IsTracked, " +
               "       COALESCE((SELECT LeadText FROM EmailCampaignTracking WHERE EmailCampaignId = ecs.StructureId),'') LeadText" +
               "  FROM EmailCampaignStructure ecs " +
               " ORDER BY (SELECT Value FROM EmailCampaignDates WHERE StructureId = ecs.StructureId AND MemberPath = 'ModifiedDate') DESC ";
        _dtemails = _sql.GetTable(ssql);

        foreach (DataRow dr in _dtemails.Rows)
        {

            HtmlTableRow _row = new HtmlTableRow();
            HtmlTableCell _cell1 = new HtmlTableCell();
            HtmlTableCell _cell2 = new HtmlTableCell();
            HtmlTableCell _cell3 = new HtmlTableCell();
            HtmlTableCell _cell4 = new HtmlTableCell();
            HtmlTableCell _cell5 = new HtmlTableCell();
            HtmlTableCell _cell6 = new HtmlTableCell();
            HtmlTableCell _cell7 = new HtmlTableCell();

            _cell4.Controls.Add(new LiteralControl(dr["Id"].ToString()));
            _cell1.Controls.Add(new LiteralControl(dr["Name"].ToString()));
            _cell7.Controls.Add(new LiteralControl(dr["LeadText"].ToString()));
            _cell2.Controls.Add(new LiteralControl(String.Format("{0:MM/dd/yyyy}", dr["Opened"])));

            String _lastUpdate = String.Empty;
            try { _lastUpdate = _sql.ExecuteScalar2(String.Format("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Campaign_{0}' AND isdeleted = 0", dr["Id"])).ToString(); } catch { }
            _cell3.Controls.Add(new LiteralControl(String.Format("{0:MM/dd/yyyy}", _lastUpdate)));

            if (dr["IsTracked"].ToString() == "1" )
            {
                _cell5.Controls.Add(new LiteralControl("Yes"));
                String scount = _sql.ExecuteScalar(String.Format("SELECT COUNT(*) FROM [CampainActivity] WHERE [CampaignId] = '{0}' AND [isdeleted] = 0 ",dr["Id"])).ToString();
                _cell6.Controls.Add(new LiteralControl(scount));
            
            }
            {
                _cell5.Controls.Add(new LiteralControl(""));
            }


            _row.Cells.Add(_cell4);
            _row.Cells.Add(_cell1);
            _row.Cells.Add(_cell2);
            _row.Cells.Add(_cell3);
            _row.Cells.Add(_cell5);
            _row.Cells.Add(_cell6);
            _row.Cells.Add(_cell7);

            emails.Rows.Add(_row);
        }

        ssql = "SELECT lf.Name Name, ca.EmailAddress Email,lf.Phone, MAX(ca.OpenDate) OpenDate,ecs.Value List, cs.Value Name2" +
               "  FROM CampainActivity ca " +
               "     LEFT OUTER JOIN Lead_Flat lf ON ca.ContactId  = lf.ConstantContactID " +
               "     LEFT OUTER JOIN [ContactStrings] cs ON cs.StructureId = ca.ContactId AND cs.MemberPath = 'LastName' " +
               "     LEFT       JOIN EmailCampaignStrings ecs ON ecs.StructureId = ca.CampaignId AND ecs.MemberPath = 'Name' " +
               " GROUP BY lf.Name,ca.EmailAddress,lf.Phone,ecs.Value, cs.Value " +
               " ORDER BY MAX(ca.OpenDate) desc";
        _dtclicks = _sql.GetTable(ssql);

        foreach (DataRow dr in _dtclicks.Rows)
        {
            HtmlTableRow _row = new HtmlTableRow();
            HtmlTableCell _cell1 = new HtmlTableCell();
            HtmlTableCell _cell2 = new HtmlTableCell();
            HtmlTableCell _cell3 = new HtmlTableCell();
            HtmlTableCell _cell4 = new HtmlTableCell();
            HtmlTableCell _cell5 = new HtmlTableCell();

            if (dr["Name"].ToString().Length > 0)
            {
                _cell1.Controls.Add(new LiteralControl(dr["Name"].ToString()));
            }
            else
            {
                _cell1.Controls.Add(new LiteralControl("* "));
                _cell1.Controls.Add(new LiteralControl(dr["Name2"].ToString()));
            }
            _cell2.Controls.Add(new LiteralControl(dr["Email"].ToString()));
            _cell3.Controls.Add(new LiteralControl(dr["Phone"].ToString()));
            _cell4.Controls.Add(new LiteralControl(dr["OpenDate"].ToString()));
            _cell5.Controls.Add(new LiteralControl(dr["List"].ToString()));

            _row.Cells.Add(_cell1);
            _row.Cells.Add(_cell2);
            _row.Cells.Add(_cell3);
            _row.Cells.Add(_cell4);
            _row.Cells.Add(_cell5);

            clicks.Rows.Add(_row);

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
            case "UpdateCampaigns" :
                {
                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
                    Tools.SetDate(DateTime.Now,"Campaings");
                    Tools.update_Campains();
                    _sql.Close();
                }
                break;
            case "Update":
                {
                    Tools.update_Campain(true, Session["guser"].ToString());
                }
                break;
        }
        Response.Redirect("/Pages/CC_Emails.aspx");
    }
}