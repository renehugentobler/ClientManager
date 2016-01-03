using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Text;
using System.Runtime.Serialization;

using HSoft.SQL;

namespace HSOFT.SQL.API
{
    /// <summary>
    /// Summary description for SQL
    /// </summary>
    [WebService(Namespace = "http://uncleharry.biz/API/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    /// <summary>
    /// Represents the LM Notes.
    /// </summary>
    [DataContract]
    [Serializable]
    public class LMNotes 
    {

        /// <summary>
        /// Holds the Id
        /// </summary>        
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Holds the Status
        /// </summary>        
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string status{ get; set; }

        /// <summary>
        /// Holds the lead notes
        /// </summary>        
        [DataMember(Name = "leadnote", EmitDefaultValue = false)]
        public string leadnote { get; set; }

        /// <summary>
        /// Holds the salesnotes
        /// </summary>        
        [DataMember(Name = "salenote", EmitDefaultValue = false)]
        public string salesnote { get; set; }

        /// <summary>
        /// Holds the SQL Error
        /// </summary>        
        [DataMember(Name = "error", EmitDefaultValue = false)]
        public string error { get; set; }

        public LMNotes()
        {
            this.status = "failed";
            this.Id = String.Empty;
            this.error = String.Empty;
            this.leadnote = String.Empty;
            this.salesnote = String.Empty;
        }

        public LMNotes(string Id)
        {
            this.status = "failed";
            this.Id = Id;
            this.error = String.Empty;
            this.leadnote = String.Empty;
            this.salesnote = String.Empty;
        }

    }
        
    public class SQLService : System.Web.Services.WebService
    {

        [WebMethod]
        public string Version()
        {
            return AssemblyName.GetAssemblyName(Assembly.GetAssembly(typeof(SQLService)).Location).Version.ToString();
        }

        [WebMethod]
        public LMNotes getLMNotes(string Id)
        {
            String ssql = String.Empty;
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

            ssql = String.Format("SELECT [LeadNote],[SalesNote] FROM Lead_Flat WHERE Id = '{0}'", Id);
            LMNotes _lmn = new LMNotes(Id);

            try
            {
                DataRow dr = _sql.GetRow(ssql);
                _lmn.leadnote = dr["leadnote"].ToString();
                _lmn.salesnote = dr["salesnote"].ToString();
                _lmn.status = "success";
                
            }
            catch (Exception ex)
            {
                _lmn.error = ex.Message;
            }

            return _lmn;
        }

    
    }
}