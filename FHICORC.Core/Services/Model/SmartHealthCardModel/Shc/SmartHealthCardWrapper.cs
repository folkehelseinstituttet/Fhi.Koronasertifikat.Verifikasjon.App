using System;
using System.Collections.Generic;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardWrapper : ITokenPayload
    {
        public SmartHealthCardWrapper(SmartHealthCardModel smartHealthCardModel,
            List<SmartHealthCardVaccineInfo> vaccineInfo, SmartHealthCardIssuer smartHealthCardIssuer)
        {
            SmartHealthCard = smartHealthCardModel;
            VaccineInfo = vaccineInfo;
            SmartHealthCardIssuer = smartHealthCardIssuer;
        }

        public SmartHealthCardModel SmartHealthCard { get; private set; }

        public SmartHealthCardIssuer SmartHealthCardIssuer { get; private set; }

        public List<SmartHealthCardVaccineInfo> VaccineInfo { get; private set; }

        public DateTime? ExpiredDateTime() => SmartHealthCard.ExpirationDate;

        public DateTime? IssueDateTime() => SmartHealthCard.IssuanceDate;
    }
}
