﻿using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardCoding
    {
        [JsonIgnore]
        public string Id
        {
            get
            {
                return System + Code;
            }
        }

        [JsonProperty("system")]
        public string System { get; set; }

        [JsonProperty("code")]
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
