using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Xml;

using HSoft.SQL;

namespace HSoft.ClientManager.ImportXML
{
    class Program
    {
        static string[] splitQuoted(string line, char delimeter)
        {
            string[] array;
            List<string> list = new List<string>();
            do
            {
                if (line.StartsWith("\""))
                {
                    line = line.Substring(1);
                    int idx = line.IndexOf("\"");
                    // missing " at the end of a string
                    if (idx == -1) 
                    {
                        line = String.Format("{0}\",",line);
                        idx = line.IndexOf("\"");
                    }
                    while (line.IndexOf("\"", idx) == line.IndexOf("\"\"", idx))
                    {
                        idx = line.IndexOf("\"\"", idx) + 2;
                    }
                    idx = line.IndexOf("\"", idx);
                    list.Add(line.Substring(0, idx));
                    line = line.Substring(idx + 2);
                }
                else
                {
                    list.Add(line.Substring(0, Math.Max(line.IndexOf(delimeter), 0)));
                    line = line.Substring(line.IndexOf(delimeter) + 1);
                }
            }
            while (line.IndexOf(delimeter) != -1);
            list.Add(line);
            array = new string[list.Count];
            list.CopyTo(array);
            return array;
        }

        static void Main(string[] args)
        {
            foreach (String sfile in Directory.GetFiles(@"..\xmlfiles"))
            {

                switch (Path.GetExtension(sfile))
                {
                    case ".csv":
                        {
                            System.IO.StreamReader file = new System.IO.StreamReader(sfile);
                            String sname = file.ReadLine();
                            Console.WriteLine("Importing Survey : {0}", sname);

                            // search existing id
//                            HSoft.SQL.SqlServer _sql = new SqlServer(ConfigurationManager.ConnectionStrings["clientmanager"].ConnectionString);
                            HSoft.SQL.SqlServer _sql = new SqlServer(@"data source=grass.arvixe.com;User Id=rene8706;Password=Taurec86@;Initial Catalog=ClientManager");

                            Guid _guid = Guid.Empty;
                            Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM Survey WHERE Name = '{0}'", sname)).ToString(), out _guid);
                            if (_guid != Guid.Empty)
                            {
                                _sql.Execute(String.Format("DELETE FROM SurveyDetail WHERE SurveyId='{0}'", _guid));
                                _sql.Execute(String.Format("DELETE FROM SurveyQuestion WHERE SurveyId='{0}'", _guid));
                                _sql.Execute(String.Format("DELETE FROM SurveyCustomer WHERE SurveyId='{0}'", _guid));
                                //                                _sql.Execute(String.Format("DELETE FROM Survey WHERE Id='{0}'",_guid));
                            }
                            else
                            {
                                // write Survey
                                _sql.Execute(String.Format("INSERT INTO Survey(Name) VALUES('{0}')", sname));
                                Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM Survey WHERE Name = '{0}'", sname)).ToString(), out _guid);
                            }

                            String sline = file.ReadLine().Trim();
                            String skeys = String.Empty;
                            while (true)
                            {
                                if (file.EndOfStream)
                                {
                                    // really bad
                                    Exception ex = new Exception("End of file. BAD");
                                    throw ex;
                                }
                                if ((sline.Length == 0) || (sline.Replace(",","").Length==0))
                                {
                                }
                                else
                                {
                                    if ((sline.Substring(0, 4) == ",,,,") && (sline[4] != ',')) { break; }
                                    skeys = String.Format("{0}{1}", skeys, sline.Trim());
                                }
                                sline = file.ReadLine().Trim();
                            }
                            skeys = skeys.Replace("�", "");
                            String[] _keyfields = splitQuoted(skeys, ','); //  skeys.Split(',');
                            int icount = 0;
                            int acount = 1;
                            Guid _aguid = Guid.Empty;
                            for (int i = 4; i < _keyfields.Length; i++)
                            {
                                if (_keyfields[i].Length > 0)
                                {
                                    _sql.Execute(String.Format("INSERT INTO SurveyQuestion(SurveyId,Number,Text,Answers,[AnswerCount]) VALUES('{0}',{1},'{2}','',0)", _guid, icount, _keyfields[i].Replace("'", "''")));
                                    Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyQuestion WHERE Number = {0} AND SurveyId='{1}' ", icount, _guid)).ToString(), out _aguid); 
                                    icount++;
                                    acount = 1;
                                }
                                else
                                {
                                    acount++;
                                    _sql.Execute(String.Format("UPDATE t SET t.AnswerCount={0} FROM SurveyQuestion t WHERE t.Id = '{1}'", acount, _aguid));
                                }

                            }
                            String line = String.Empty;

                            String sanswers = sline.Trim();
                            String[] _answerfields = splitQuoted(sanswers, ',' ); // sanswers.Split(',');
                            icount = 0;
                            acount = -1;
                            for (int i = 4; i < _answerfields.Length; i++)
                            {
                                icount--;
                                if (icount <= 0)
                                {
                                    acount++;
                                    int.TryParse(_sql.ExecuteScalar(String.Format("SELECT AnswerCount FROM SurveyQuestion WHERE Number = {0} AND SurveyId='{1}' ", acount, _guid)).ToString(), out icount);
                                }
                                _sql.Execute(String.Format("UPDATE t SET t.Answers=Answers+'{2}'+'|' FROM SurveyQuestion t WHERE t.SurveyId = '{1}' AND Number = {0}", acount, _guid, _answerfields[i].Replace("'", "''")));
                            }                            
                            while ((line = file.ReadLine()) != null)
                            {
                                Guid _cguid = Guid.Empty;
                                while (splitQuoted(line, ',').GetLength(0) <= _answerfields.GetLength(0))
                                {
                                    String snext = file.ReadLine();
                                    line = String.Format("{0}{1}", line, snext.Trim());
                                }
                                String[] _fields = splitQuoted(line, ',');  // line.Split(',');
                                _sql.Execute(String.Format("INSERT INTO SurveyCustomer(SurveyId, ResponseID, SubmissionTime, Respondent, Status, FirstName, LastName, HomePhone, EmailAddress) " +
                                                           " VALUES('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                           _guid,
                                                           _fields[0],
                                                           _fields[1],
                                                           _fields[2].Replace("'", "''"),
                                                           _fields[3],
                                                           _fields[_fields.Length - 5].Replace("'", "''"),
                                                           _fields[_fields.Length - 4].Replace("'", "''"),
                                                           _fields[_fields.Length - 3].Replace("'", "''"),
                                                           _fields[_fields.Length - 2].Replace("'", "''")
                                                           ));
                                Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyCustomer WHERE ResponseID = {0} AND SurveyId='{1}' ", _fields[0], _guid)).ToString(), out _cguid);
                                icount = 0;
                                int qcount = 0;
                                acount = -1;
                                Guid _qguid = Guid.Empty;
                                Guid _dguid = Guid.Empty;
                                String qanswer = String.Empty; 
                                for (int i = 4; i < _fields.Length - 5; i++)
                                {
                                    icount--;
                                    if (icount <= 0)
                                    {
                                        acount++;
                                        int.TryParse(_sql.ExecuteScalar(String.Format("SELECT AnswerCount FROM SurveyQuestion WHERE Number = {0} AND SurveyId='{1}' ", acount, _guid)).ToString(), out icount);
                                        qcount = icount;
                                        Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyQuestion WHERE Number = {0} AND SurveyId='{1}' ", acount, _guid)).ToString(), out _qguid);
                                        qanswer = _sql.ExecuteScalar(String.Format("SELECT Answers FROM SurveyQuestion WHERE Number = {0} AND SurveyId='{1}' ", acount, _guid)).ToString();
                                    }
                                    if (_fields[i] == "1")
                                    {
                                        int xcount = 0;
                                        int.TryParse(_sql.ExecuteScalar(String.Format("SELECT COUNT(*) FROM SurveyDetail WHERE SurveyId='{0}' AND SurveyQuestionId='{1}' AND SurveyCustomerId='{2}'", _guid, _qguid, _cguid)).ToString(), out xcount);
                                        if (xcount == 0)
                                        {
                                            _sql.Execute(String.Format("INSERT INTO SurveyDetail(SurveyId,SurveyQuestionId,SurveyCustomerId,[Index],Answer) " +
                                                                       " VALUES ('{0}','{1}','{2}',{3},'{4}')",
                                                                       _guid, _qguid, _cguid, icount, qanswer.Split('|')[qcount - icount].Replace("'", "''")));
                                        }
                                        else
                                        {
                                            _sql.Execute(String.Format("UPDATE t " +
                                                                       "   SET t.Answer = t.Answer + ' ' + '{3}' " + 
                                                                       "  FROM SurveyDetail t, SurveyQuestion q " +
                                                                       " WHERE t.Id = '{0}' " +
                                                                       "   AND q.SurveyId = '{1}' " +
                                                                       "   AND q.Number = {2} "
                                                                       , _dguid, _guid, acount, qanswer.Split('|')[qcount - icount].Replace("'", "''")));
                                        }
                                        Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyDetail WHERE SurveyId='{0}' AND SurveyQuestionId='{1}' AND SurveyCustomerId='{2}'", _guid, _qguid, _cguid)).ToString(), out _dguid);
                                    }
                                    else if (_fields[i].Length>1)
                                    {
                                        if (_dguid == Guid.Empty)
                                        {
                                            _sql.Execute(String.Format("INSERT INTO SurveyDetail(SurveyId,SurveyQuestionId,SurveyCustomerId) " +
                                                                       " VALUES ('{0}','{1}','{2}')", _guid, _qguid,_cguid));
                                            Guid.TryParse(_sql.ExecuteScalar(String.Format("SELECT Id FROM SurveyDetail WHERE SurveyId='{0}' AND SurveyQuestionId='{1}' AND SurveyCustomerId='{2}'", _guid, _qguid, _cguid)).ToString(), out _dguid);
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
                                                                   , _dguid, _guid, acount, _cguid, _fields[i].Replace("'", "''")));
                                    }
                                }                            


                            }



                            _sql.Close();
                            file.Close();
                        }
                        break;
                    case ".xml":
                        {
                            Console.WriteLine("Importing table : {0}", Path.GetFileNameWithoutExtension(sfile));
                            HSoft.SQL.SqlServer _sql = new SqlServer(@"data source=.\MSSQL2012;User Id=rar;Password=rar;Initial Catalog=DB_66365");
                            HSoft.SQL.SqlServer _sql2 = new SqlServer(@"data source=grass.arvixe.com;User Id=rene8706;Password=Taurec86@;Initial Catalog=ClientManager");

                            _sql.Execute(String.Format("DELETE FROM {0}", Path.GetFileNameWithoutExtension(sfile)));
                            _sql2.Execute(String.Format("DELETE FROM {0}", Path.GetFileNameWithoutExtension(sfile)));

                            DataSet originalDataSet = new DataSet("dataSet");
                            DataTable table = _sql.GetTable(String.Format("SELECT * FROM {0} AS [table] WHERE 1=2", Path.GetFileNameWithoutExtension(sfile)));

                            System.IO.FileStream streamRead = new System.IO.FileStream(sfile, System.IO.FileMode.Open);
                            originalDataSet.ReadXml(streamRead);

                            try
                            {
                                foreach (DataRow dr in originalDataSet.Tables[0].Rows)
                                {
                                    DataRow ndr = table.NewRow();
                                    foreach (DataColumn dc_field in table.Columns)
                                    {
                                        if (((DataRow)dr).Table.Columns.Contains(dc_field.ColumnName))
                                        {
                                            ndr[dc_field.ColumnName] = ((DataRow)dr)[dc_field.ColumnName];
                                        }
                                    }
                                    table.Rows.Add(ndr);
                                    ndr = null;
                                }
                                _sql.InsertDataset(Path.GetFileNameWithoutExtension(sfile), table);
                                _sql2.InsertDataset(Path.GetFileNameWithoutExtension(sfile), table);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("{0} {1}", Path.GetFileNameWithoutExtension(sfile), ex.Message);

                            }
                        }
                        break;
                }
            }
        }
    }
}
