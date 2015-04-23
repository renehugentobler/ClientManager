﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CTCT.Components.Contacts;

namespace CTCT.Components.EmailCampaigns
{
    /// <summary>
    /// Represents a campaign Test Send in Constant Contact class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class TestSend : Component
    {
        /// <summary>
        /// Format of the email to send (HTML, TEXT, HTML_AND_TEXT).
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Personal message to send along with the test send.
        /// </summary>
        [DataMember(Name = "personal_message")]
        public string PersonalMessage { get; set; }

        /// <summary>
        /// Array of email addresses to send the test send to.
        /// </summary>
        [DataMember(Name = "email_addresses")]
        public IList<string> EmailAddresses { get; set; }
    }

    /// <summary>
    /// Email format.
    /// </summary>
    [Serializable]
    public enum EmailFormat
    {
        /// <summary>
        /// Html format.
        /// </summary>
        HTML,
        /// <summary>
        /// Text format.
        /// </summary>
        TEXT,
        /// <summary>
        /// Html and text format.
        /// </summary>
        HTML_AND_TEXT
    }
}
