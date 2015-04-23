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

using HSoft.SQL;
using HSoft.ClientManager.Web;

using SisoDb.Sql2012;
using SisoDb.Configurations;

using CTCT;
using CTCT.Components;
using CTCT.Components.Contacts;
using CTCT.Components.AccountService;
using CTCT.Components.EmailCampaigns;
using CTCT.Components.Tracking;
using CTCT.Services;
using CTCT.Exceptions;

using CSVFile;
using Obout.Interface;

public partial class Pages_CC_Campain : System.Web.UI.Page
{

    private static String ssql = String.Empty;
    private static DataTable _dtsurveys = null;

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            Tools.devlogincheat();
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            _dtsurveys = _sql.GetTable("SELECT * FROM Survey WHERE isdeleted = 0");
            _sql.Close();
        }

        foreach (DataRow dr in _dtsurveys.Rows)
        {

            HtmlTableRow _row = new HtmlTableRow();
            HtmlTableCell _cell1 = new HtmlTableCell();
            HtmlTableCell _cell2 = new HtmlTableCell();
            HtmlTableCell _cell3 = new HtmlTableCell();
            HtmlTableCell _cell4 = new HtmlTableCell();

            _cell1.Controls.Add(new LiteralControl(dr["Name"].ToString()));
            _cell2.Controls.Add(new LiteralControl(String.Format("{0:MM/dd/yyyy}",dr["Opened"])));
            _cell3.Controls.Add(new LiteralControl(String.Format("{0:MM/dd/yyyy HH:mm}",dr["updatedate"])));

            String _file = String.Format(@"e:\HostingSpaces\rene8706\uncleharry.biz\wwwroot\clientmanager\Pages\upload\campaign\{0}.csv", dr["Name"]);
            if (File.Exists(_file))
            {
                DateTime fileCreatedDate = File.GetCreationTime(String.Format(_file));
                OboutButton _button = new OboutButton();
                _button.ID=String.Format("Survey_{0}",dr["Id"]);
                _button.Command += new CommandEventHandler(Btn_Command);
                _button.CommandName = "LoadSurvey";
                _button.CommandArgument = String.Format("{0},{1},{2}",_file, dr["Name"].ToString(), dr["Id"].ToString() );
                _button.Text = String.Format("{0:MM/dd/yyyy HH:mm}", fileCreatedDate);
                _cell4.Controls.Add(_button);
            }
            else
            {
            }
            _row.Cells.Add(_cell1);
            _row.Cells.Add(_cell2);
            _row.Cells.Add(_cell3);
            _row.Cells.Add(_cell4);

            campaigns.Rows.Add(_row);
        }

    }

    protected void Btn_Command(object sender, CommandEventArgs e)
    {
        CC_Command(e.CommandName,e.CommandArgument.ToString());
    }

    protected void CC_Command(string scmd, string sarg)
    {
        switch (scmd)
        {
            case "Refresh":
                {
                }
                break;
            case "LoadSurvey":
                {
                    String _sfile = sarg.Split(',')[0];
                    String _sname = sarg.Split(',')[1];
                    String _sguid = sarg.Split(',')[2];

                    HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
                    DataRow _drsurveys = _sql.GetTable(String.Format("SELECT * FROM Survey WHERE isdeleted = 0 AND Id = '{0}'", _sguid)).Rows[0];

                    DateTime _dlastupdate = DateTime.Parse(_drsurveys["updatedate"].ToString());
                    DateTime _dlastrecord = DateTime.Parse(_drsurveys["updatedate"].ToString());

                    using (CSVReader cr = new CSVReader(_sfile, ',', '"'))
                    {
                        foreach (string[] line in cr)
                        {
                            Int64 _i;
                            if (!Int64.TryParse(line[0],out _i)) 
                            { 
                                continue; 
                            }
                            // check if new survey entry

                            DataTable _customer = _sql.GetTable(String.Format("SELECT * FROM SurveyCustomer WHERE isdeleted = 0 AND ResponseID = '{0}' AND SurveyId='{1}'", line[0], _sguid));
                            if (_customer.Rows.Count == 0)
                            {
                                if (_dlastrecord < DateTime.Parse(line[1])) { _dlastrecord = DateTime.Parse(line[1]); }
                                _sql.Execute(String.Format("INSERT INTO SurveyCustomer(SurveyId, ResponseID, SubmissionTime, Respondent, Status, FirstName, LastName, HomePhone, EmailAddress) " +
                                                            " VALUES('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                            _sguid,
                                                            line[0],
                                                            line[1],
                                                            line[2].Replace("'", "''"),
                                                            line[3],
                                                            line[line.Length - 5].Replace("'", "''"),
                                                            line[line.Length - 4].Replace("'", "''"),
                                                            line[line.Length - 3].Replace("'", "''"),
                                                            line[line.Length - 2].Replace("'", "''")
                                                            ));
                                Guid _cguid = Guid.Empty;
                                Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyCustomer WHERE ResponseID = {0} AND SurveyId='{1}' ", line[0], _sguid)).ToString(), out _cguid);
                                int icount = 0;
                                int qcount = 0;
                                int acount = -1;
                                Guid _qguid = Guid.Empty;
                                Guid _dguid = Guid.Empty;
                                String qanswer = String.Empty;
                                for (int i = 4; i < line.Length - 5; i++)
                                {
                                    icount--;
                                    if (icount <= 0)
                                    {
                                        acount++;
                                        int.TryParse(_sql.ExecuteScalar(String.Format("SELECT AnswerCount FROM SurveyQuestion WHERE Number = {0} AND SurveyId='{1}' ", acount, _sguid)).ToString(), out icount);
                                        qcount = icount;
                                        Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyQuestion WHERE Number = {0} AND SurveyId='{1}' ", acount, _sguid)).ToString(), out _qguid);
                                        qanswer = _sql.ExecuteScalar(String.Format("SELECT Answers FROM SurveyQuestion WHERE Number = {0} AND SurveyId='{1}' ", acount, _sguid)).ToString();
                                    }
                                    if (line[i] == "1")
                                    {
                                        int xcount = 0;
                                        int.TryParse(_sql.ExecuteScalar(String.Format("SELECT COUNT(*) FROM SurveyDetail WHERE SurveyId='{0}' AND SurveyQuestionId='{1}' AND SurveyCustomerId='{2}'", _sguid, _qguid, _cguid)).ToString(), out xcount);
                                        if (xcount == 0)
                                        {
                                            _sql.Execute(String.Format("INSERT INTO SurveyDetail(SurveyId,SurveyQuestionId,SurveyCustomerId,[Index],Answer) " +
                                                                        " VALUES ('{0}','{1}','{2}',{3},'{4}')",
                                                                        _sguid, _qguid, _cguid, icount, qanswer.Split('|')[qcount - icount].Replace("'", "''")));
                                        }
                                        else
                                        {
                                            _sql.Execute(String.Format("UPDATE t " +
                                                                        "   SET t.Answer = t.Answer + ' ' + '{3}' " + 
                                                                        "  FROM SurveyDetail t, SurveyQuestion q " +
                                                                        " WHERE t.Id = '{0}' " +
                                                                        "   AND q.SurveyId = '{1}' " +
                                                                        "   AND q.Number = {2} "
                                                                        , _dguid, _sguid, acount, qanswer.Split('|')[qcount - icount].Replace("'", "''")));
                                        }
                                        Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyDetail WHERE SurveyId='{0}' AND SurveyQuestionId='{1}' AND SurveyCustomerId='{2}'", _sguid, _qguid, _cguid)).ToString(), out _dguid);
                                    }
                                    else if (line[i].Length > 1)
                                    {
                                        if (_dguid == Guid.Empty)
                                        {
                                            _sql.Execute(String.Format("INSERT INTO SurveyDetail(SurveyId,SurveyQuestionId,SurveyCustomerId) " +
                                                                        " VALUES ('{0}','{1}','{2}')", _sguid, _qguid, _cguid));
                                            Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyDetail WHERE SurveyId='{0}' AND SurveyQuestionId='{1}' AND SurveyCustomerId='{2}'", _sguid, _qguid, _cguid)).ToString(), out _dguid);
                                        }
                                        _sql.Execute(String.Format("UPDATE t " +
                                                                    "   SET t.SurveyId = '{1}' " +
                                                                    "      ,t.SurveyQuestionId = q.Id " +
                                                                    "      ,t.SurveyCustomerId = '{3}' " +
                                                                    "      ,t.Comment = '{4}' " +
                                                                    "  FROM SurveyDetail t, SurveyQuestion q " +
                                                                    " WHERE t.Id = '{0}' " +
                                                                    "   AND q.SurveyId = '{1}' " +
                                                                    "   AND q.Number = {2} "
                                                                    , _dguid, _sguid, acount, _cguid, line[i].Replace("'", "''")));
                                    }
                                }
                            }
                        }
                        _sql.Execute(String.Format("UPDATE t SET updatedate = '{0}' FROM Survey t WHERE t.Id='{1}' ", _dlastrecord, _sguid));
                    }
                    _dtsurveys = _sql.GetTable("SELECT * FROM Survey WHERE isdeleted = 0");
                    _sql.Close(); _sql.Close();

//                    File.Move(_sfile,String.Format("{0}.{1}",_sfile,File.GetCreationTime(String.Format(_sfile)).ToString("yyyyMMddHHmm"))); 

                }
                break;
        }
        Response.Redirect("/Pages/CC_Survey.aspx");
    }

}