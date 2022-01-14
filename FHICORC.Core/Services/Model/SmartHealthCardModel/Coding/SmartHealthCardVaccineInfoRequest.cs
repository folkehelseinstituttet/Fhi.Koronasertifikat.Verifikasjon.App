using System.Linq;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Coding
{
    public class SmartHealthCardVaccineInfoRequest
    {
        [JsonProperty("Codes")]
        public SmartHealthCardCodingRequest[] Codes { get; set; }

        public SmartHealthCardVaccineInfoRequest(SmartHealthCardCoding[] Coding)
        {
            Codes = Coding.Select(x => new SmartHealthCardCodingRequest(x)).ToArray();
        }
    }

    public class SmartHealthCardCodingRequest
    {
        public SmartHealthCardCodingRequest(SmartHealthCardCoding coding)
        {
            System = coding.System;
            Code = coding.Code;
        }

        [JsonIgnore]
        public string Id
        {
            get
            {
                return System + Code;
            }
        }

        [JsonProperty("System")]
        public string System { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                SmartHealthCardCoding p = (SmartHealthCardCoding)obj;
                return Id.Equals(p.Id);
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
