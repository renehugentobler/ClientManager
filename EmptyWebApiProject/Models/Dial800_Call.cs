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
    public class Dial800_Call
    {
        /// <summary>
        /// Call id
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int id { get; set; }

        /// <summary>
        /// Call dnis
        /// </summary>
        [DataMember(Name = "dnis", EmitDefaultValue = false)]
        public string dnis;

        /// <summary>
        /// Call ani
        /// </summary>
        [DataMember(Name = "ani", EmitDefaultValue = false)]
        public Dial800_ani ani;

        /// <summary>
        /// Call targets
        /// </summary>
        [DataMember(Name = "targets", EmitDefaultValue = false)]
        public List<Dial800_target> targets;

        /// <summary>
        /// Call start 
        /// </summary>
        [DataMember(Name = "start", EmitDefaultValue = false)]
        public String start;

        /// <summary>
        /// Call end
        /// </summary>
        [DataMember(Name = "end", EmitDefaultValue = false)]
        public String end;

        /// <summary>
        /// Call recording url
        /// </summary>
        [DataMember(Name = "recording", EmitDefaultValue = false)]
        public String recording;

    }
}