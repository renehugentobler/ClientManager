using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CTCT.Util;

namespace CTCT.Components.AccountServices
{
    /// <summary>
    /// Summary class.
    /// </summary>
    [DataContract]
    [Serializable]
    public class AccountSummary : Component
    {
        /// <summary>
        /// Activity website.
        /// </summary>
        [DataMember(Name = "website", EmitDefaultValue = false)]
        public string Website { get; set; }
        /// <summary>
        /// Gets the organization_name.
        /// </summary>
        [DataMember(Name = "organization_name", EmitDefaultValue = false)]
        public string OrganizationName { get; set; }
        /// <summary>
        /// Gets the time_zone.
        /// </summary>
        [DataMember(Name = "time_zone", EmitDefaultValue = false)]
        public string TimeZone { get; set; }
        /// <summary>
        /// Gets the first_name.
        /// </summary>
        [DataMember(Name = "first_name", EmitDefaultValue = false)]
        private string FirstName { get; set; }
        /// <summary>
        /// Gets the last_name.
        /// </summary>
        [DataMember(Name = "last_name", EmitDefaultValue = false)]
        public string LastName { get; set; }
        /// <summary>
        /// Gets the email.
        /// </summary>
        [DataMember(Name = "email", EmitDefaultValue = false)]
        private string Email { get; set; }
        /// <summary>
        /// Gets the email.
        /// </summary>
        [DataMember(Name = "phone", EmitDefaultValue = false)]
        private string Phone { get; set; }
        /// <summary>
        /// Gets the email.
        /// </summary>
        [DataMember(Name = "country_code", EmitDefaultValue = false)]
        private string CountryCode { get; set; }
        /// <summary>
        /// Gets the email.
        /// </summary>
        [DataMember(Name = "state_code", EmitDefaultValue = false)]
        private string StateCode { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public AccountSummary()
        { 
        }
    }
}
