using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

using WebInfo.Components;

using SisoDb.Sql2012;
using SisoDb.Configurations;

namespace WebInfo
{

    public class HTMLPageInfo : Component
    {
        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "TimeStamp", EmitDefaultValue = false)]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "Cookies", EmitDefaultValue = false)]
        public List<Cookie> Cookies { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "Form", EmitDefaultValue = false)]
        public List<NameValue> Form { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "QueryString", EmitDefaultValue = false)]
        public List<NameValue> QueryString { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "UserHostAddress", EmitDefaultValue = false)]
        public string UserHostAddress { get; set; }

        public HTMLPageInfo(NameValueCollection _QueryString, string _UserHostAddress, NameValueCollection _Form)
        {
            this.Cookies = new List<Cookie>();
            this.Form = new List<NameValue>();
            this.QueryString = new List<NameValue>();

            this.UserHostAddress = _UserHostAddress;
            foreach (string key in _Form) { this.Form.Add(new NameValue(key, _Form[key])); }
            foreach (string key in _QueryString) { this.QueryString.Add(new NameValue(key, _QueryString[key])); }
        }

        public HTMLPageInfo(NameValueCollection _QueryString, string _UserHostAddress, NameValueCollection _Form, HttpCookieCollection _Cookies) 
        {

            this.Cookies = new List<Cookie>();
            this.Form = new List<NameValue>();
            this.QueryString = new List<NameValue>();

            this.UserHostAddress = _UserHostAddress;
            foreach (string key in _Form) { this.Form.Add(new NameValue(key, _Form[key])); }
            foreach (string key in _QueryString) { this.QueryString.Add(new NameValue(key, _QueryString[key])); }
            foreach (string key in _Cookies.AllKeys) 
            {
                HttpCookie cookie = _Cookies[key];
                this.Cookies.Add(new Cookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path)); 
            }

        }

        public void Save()
        {
            {
                SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString.CreateSql2012Db();

                this.TimeStamp = DateTime.Now;
                using (var session = _siso.BeginSession())
                {
                    session.Insert(this);
                }
            }

            {
                SisoDb.ISisoDatabase _siso = ConfigurationManager.ConnectionStrings["ClientManager2"].ConnectionString.CreateSql2012Db();

                this.TimeStamp = DateTime.Now;
                using (var session = _siso.BeginSession())
                {
                    session.Insert(this);
                }
            }
        }

    }
}