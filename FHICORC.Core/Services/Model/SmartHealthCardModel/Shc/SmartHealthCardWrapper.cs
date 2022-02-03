using System;
using System.Collections.Generic;
using System.Diagnostics;
using FHICORC.Core.Services.Enum;
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

        public TokenValidateResult Validate()
        {
            if (ExpiredDateTime() != null && ExpiredDateTime()?.ToUniversalTime() < DateTime.UtcNow)
            {
                Debug.Print("Smart health card is expired");
                return TokenValidateResult.Expired;
            }
            else if (SmartHealthCard.VerifiableCredential.CredentialSubject.Patients.Count > 1)
            {
                Debug.Print("Smart health card invalid, because it has more than 1 patient. " +
                    $"Patient count was: {SmartHealthCard.VerifiableCredential.CredentialSubject.Patients.Count}");
                return TokenValidateResult.Invalid;
            }

            return TokenValidateResult.Valid;
        }
    }
}
