using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Runtime.Serialization;

using WebInfo.Components;
/// 
namespace WebInfo
{

    public class Cookie : Component
    {
        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "Name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "Value", EmitDefaultValue = false)]
        public string ValueText { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "Domain", EmitDefaultValue = false)]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "Path", EmitDefaultValue = false)]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "TimeStamp", EmitDefaultValue = false)]
        public string TimeStamp { get; set; }

        public Cookie(string Name,string Value, string Domain,string Path) 
        { 
            this.Name=Name;
            this.ValueText=Value;
            this.Domain=Domain;
            this.Path=Path;
            this.TimeStamp = TimeStamp;
        }
    }
}