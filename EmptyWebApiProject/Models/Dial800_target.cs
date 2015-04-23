using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;

namespace EmptyWebApiProject.Models
{
    /// <summary>
    /// Represents a single Dial800 Call.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Dial800_target
    {
        /// <summary>
        /// Call target
        /// </summary>
        [DataMember(Name = "target", EmitDefaultValue = false)]
        public string target;

        /// <summary>
        /// Call status
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string status;

        /// <summary>
        /// Call duration
        /// </summary>
        [DataMember(Name = "state", EmitDefaultValue = false)]
        public int duration;
    }
}