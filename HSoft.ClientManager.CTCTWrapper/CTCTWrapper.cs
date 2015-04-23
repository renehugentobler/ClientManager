
namespace HSoft.ClientManager.CTCTWrapper
{

    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Data;
    using System.Data.OleDb;
    using System.Text.RegularExpressions;

    using SisoDb.Sql2012;
    using SisoDb.Configurations;

    using CTCT;
    using CTCT.Components;
    using CTCT.Components.Contacts;
    using CTCT.Components.AccountService;
    using CTCT.Components.EmailCampaigns;
    using CTCT.Exceptions; 

    using HSoft.SQL;

    public class CTCTDB
    {
        private static SisoDb.ISisoDatabase _siso = null;
        private static HSoft.SQL.SqlServer _sql = null;

        public ConstantContact _constantContact = null;
        private string _apiKey = string.Empty;
        private string _accessToken = string.Empty;

        bool invalid = false;
        public bool IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }


        public Boolean InsertContact(int ClientId)
        {
            return true;
        }

        public int UpdateContacts(DateTime lastupdate)
        {
            int iCount = 0;
            Pagination _page = null;
            while (1 == 1)
            {
                ResultSet<Contact> _contacts = null;
                try
                {
                    _contacts = _constantContact.GetContacts(lastupdate, _page);
                }
                catch (Exception ex) { throw ex; }
                if (_contacts==null)
                {
                    Exception ex = new Exception("No results returned, possible connection failure.");
                    throw ex;
                }
                using (var session = _siso.BeginSession())
                {
                    foreach (Contact _contact in _contacts.Results)
                    {
                        if (session.Query<Contact>().Count(o => o.Id == _contact.Id) != 0)
                        {
                            session.Update(_contact);
                        }
                        else
                        {
                            session.Insert(_contact);
                        }
                    }
                }
                iCount = iCount + _contacts.Results.Count;
                if (_contacts.Meta.Pagination.Next == null) { break; }
                if (_page == null) { _page = new Pagination(); }
                _page.Next = _contacts.Meta.Pagination.Next;
            }
            return iCount;
        }

        public CTCTDB(string scommand)
        {

            Console.WriteLine(scommand);

            // attach SisoDB
            _siso = ConfigurationManager.ConnectionStrings["clientmanager"].ConnectionString.CreateSql2012Db();
            _siso.CreateIfNotExists();

            // attach MSSQLDB
            _sql = new SqlServer(ConfigurationManager.ConnectionStrings["clientmanager"].ConnectionString);

            // Connect to ConstantContact API
            _apiKey = ConfigurationManager.AppSettings["APIKey"];
            _accessToken = ConfigurationManager.AppSettings["AccessToken"];
            if (_accessToken.Length != new Guid().ToString().Length)
            {
                byte[] decryptedB = Convert.FromBase64String(ConfigurationManager.AppSettings["AccessToken"]);
                _accessToken = System.Text.Encoding.UTF8.GetString(decryptedB).Replace("\0", "");
            }
            _constantContact = new ConstantContact(_apiKey, _accessToken);

            switch (scommand)
            {
                case "GET ALL":
                    {
                        // Update Contacts
                        DateTime _lastmod = DateTime.Parse(_sql.ExecuteScalar("SELECT lastmodified FROM _CCupdates WHERE tablename = 'Contact'").ToString());
                        Console.WriteLine("Records Inserted/Updated : {0}", UpdateContacts(_lastmod));
                    }
                    break;
                case "PUT ALL":
                    {
                        using (var session = _siso.BeginSession())
                        {
                            
                            IList<ContactList> _rscl = _constantContact.GetLists(null);

                            ContactList _cl = _rscl.FirstOrDefault(o => o.Name == "Added by Client Manager");

                            DataTable dt = _sql.GetTable("SELECT lf.* FROM Lead_Flat lf,_LeadPriority lp WHERE lf.PriorityId = lp.Id AND lf.Customer != '0' AND lf.isdeleted = 0 AND lp.isdeleted = 0 AND lp.IsLead = 1 ORDER BY Customer");
                            int fcount = 0, mcount = 0, pcount = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                String ssql = String.Format("SELECT Value FROM [ContactStrings] WHERE MemberPath = 'Id' AND StructureId = (SELECT StructureId FROM ContactStrings WHERE MemberPath = 'EmailAddresses.EmailAddr' AND LOWER(Value) = '{0}')", dr["EMail"].ToString().ToLower());
                                String Id = _sql.ExecuteScalar(ssql).ToString();
                                var result = session.Query<Contact>()
                                    .Where(o => o.Id == Id);
                                if (result.FirstOrDefault() != null)
                                {
                                    // Console.WriteLine("{2} Found email {0} {1}", dr["Customer"], Id, ++fcount);
                                    continue;
                                }
                                ssql = String.Format("SELECT Value FROM [ContactStrings] WHERE MemberPath = 'Id' AND StructureId IN (SELECT StructureId FROM ContactStrings WHERE MemberPath IN ('HomePhone','WorkPhone','CellPhone') AND LOWER(Value) = '{0}')", dr["EMail"].ToString().ToLower());
                                Id = _sql.ExecuteScalar(ssql).ToString();
                                result = session.Query<Contact>()
                                    .Where(o => o.Id == Id);
                                if (result.FirstOrDefault() != null)
                                {
                                    if (result.FirstOrDefault() != null)
                                    {
                                        // Console.WriteLine("{2} Found phone {0} {1}", dr["Customer"], Id, ++pcount);
                                        continue;
                                    }
                                }
                                Console.WriteLine("{2} Missing {0} ", dr["Customer"],mcount,++mcount);
                                Contact _ct = new Contact();

                                if (IsValidEmail(dr["EMail"].ToString()))
                                {
                                    EmailAddress _em = new EmailAddress();
                                    _em.EmailAddr = dr["EMail"].ToString();
                                    _em.ConfirmStatus = "NO_CONFIRMATION_REQUIRED";
//                                    _em.OptInDate = DateTime.Parse(dr["EntryDate"].ToString()).ToString("u");
                                    _em.OptInSource = "ACTION_BY_OWNER";
                                    _em.Status = "ACTIVE";
                                    _ct.EmailAddresses.Add(_em);

                                    _ct.Lists.Add(_cl);

                                    _ct.HomePhone = dr["Phone"].ToString();
                                    _ct.FirstName = dr["Name"].ToString();
                                    _ct.Source = "Client Manager";
                                    _ct.Status = "ACTIVE";

//                                    _ct.DateCreated = DateTime.Parse(dr["EntryDate"].ToString()).ToString("u");

                                    Contact _ctres = new Contact();
                                    try
                                    {
                                        _ctres = _constantContact.AddContact(_ct, false);
                                        Console.WriteLine(_ctres.Id);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                            Console.WriteLine("{0} {1} {2}", fcount, mcount, pcount);
                        }
                    }
                    break;
            }
        
        }

    }
}
