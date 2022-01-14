using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class CodeableConcept
    {
        [JsonIgnore]
        public string Id
        {
            get
            {
                return Coding.Count().ToString()
                 + string.Join("-", Coding.Select(x => x.Id).OrderBy(x => x));
            }
        }

        [JsonProperty("coding")]
        public SmartHealthCardCoding[] Coding { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CodeableConcept p = (CodeableConcept)obj;
                return p.Id.Equals(Id);
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
