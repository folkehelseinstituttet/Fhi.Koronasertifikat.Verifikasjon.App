using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class VerifiableCredential
    {
        public VerifiableCredential(List<VerifiableCredentialType> VerifiableCredentialTypeList, CredentialSubject CredentialSubject)
        {
            this.VerifiableCredentialTypeList = VerifiableCredentialTypeList;
            this.CredentialSubject = CredentialSubject;
        }

        [JsonProperty(propertyName: "type", ItemConverterType = typeof(VerifiableCredentialTypeConverter), Required = Required.Always)]
        public List<VerifiableCredentialType> VerifiableCredentialTypeList { get; set; }

        [JsonProperty("credentialSubject", Required = Required.Always)]
        public CredentialSubject CredentialSubject { get; set; }
    }
}
