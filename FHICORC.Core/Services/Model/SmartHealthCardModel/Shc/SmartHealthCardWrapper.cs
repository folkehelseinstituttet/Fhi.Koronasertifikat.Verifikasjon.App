using System;
using System.Collections.Generic;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardWrapper : ITokenPayload
    {
        public SmartHealthCardWrapper(SmartHealthCardModel smartHealthCardModel,
            List<SmartHealthCardVaccineInfo> vaccineInfo)
        {
            SmartHealthCard = smartHealthCardModel;
            VaccineInfo = vaccineInfo;
        }

        public SmartHealthCardModel SmartHealthCard { get; private set; }

        public List<SmartHealthCardVaccineInfo> VaccineInfo { get; private set; }

        public DateTime? ExpiredDateTime() => SmartHealthCard.ExpirationDate;

        public DateTime? IssueDateTime() => SmartHealthCard.IssuanceDate;
    }
}
