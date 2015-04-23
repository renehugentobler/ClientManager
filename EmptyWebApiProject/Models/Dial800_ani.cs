using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;

namespace EmptyWebApiProject.Models
{
    /// <summary>
    /// Represents a single Dial800 Call ani.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Dial800_ani
    {
        /// <summary>
        /// Call number
        /// </summary>
        [DataMember(Name = "number", EmitDefaultValue = false)]
        public string number;

        /// <summary>
        /// Call city
        /// </summary>
        [DataMember(Name = "city", EmitDefaultValue = false)]
        public string city;

        /// <summary>
        /// Call state
        /// </summary>
        [DataMember(Name = "state", EmitDefaultValue = false)]
        public string state;
    }
}