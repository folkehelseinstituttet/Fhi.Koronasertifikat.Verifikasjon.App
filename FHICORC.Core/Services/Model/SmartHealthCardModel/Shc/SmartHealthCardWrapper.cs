using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
            else if (!ValidDateOfBirth())
            {
                return TokenValidateResult.Invalid;
            }

            return TokenValidateResult.Valid;
        }

        public bool ValidDateOfBirth()
        {
            string dateOfBirth = SmartHealthCard.VerifiableCredential.CredentialSubject.Patients.First().DateOfBirth;
            Regex dateTimeRegex = new Regex(@"^([0-9]([0-9]([0-9][1-9]|[1-9]0)|[1-9]00)|[1-9]000)(-(0[1-9]|1[0-2])(-(0[1-9]|[1-2][0-9]|3[0-1])(T([01][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60)(\.[0-9]+)?(Z|(\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00)))?)?)?");
            if (dateTimeRegex.IsMatch(dateOfBirth))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
