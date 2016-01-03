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

    public class NameValue: Component
    {
        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "key", EmitDefaultValue = false)]
        public string key { get; set; }

        /// <summary>
        /// Gets or sets the url of the creator.
        /// </summary>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        public string valueText { get; set; }

        public NameValue(string key, string value) 
        {
            this.key = key;
            this.valueText = value;
        }
    }

}