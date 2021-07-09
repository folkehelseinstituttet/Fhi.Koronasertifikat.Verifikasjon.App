﻿using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FHICORC.Services.Mocks
{
    public class MockBusinessRulesRepository : IBusinessRulesRepository
    {
        public async Task<ApiResponse<ICollection<BusinessRule>>> GetBusinessRules()
        {
            var jsonResponse = @"[ { ""Identifier"": ""VR-NX-0001"", ""Type"": ""Acceptance"", ""Country"": ""NX"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Vaccination"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""Vaccine provider must be approved"" }, { ""lang"": ""nb"", ""desc"": ""Vaksineleverandør må være godkjent"" }, { ""lang"": ""nn"", ""desc"": ""Vaksineleverandør må vera godkjend"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [ ""v.0.mp"" ], ""Logic"": { ""in"": [ { ""var"": ""payload.v.0.mp"" }, [ ""EU/1/20/1528"", ""EU/1/20/1507"", ""EU/1/20/1525"", ""EU/1/21/1529"" ] ] } }, { ""Identifier"": ""VR-NX-0002"", ""Type"": ""Acceptance"", ""Country"": ""NX"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Vaccination"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""At least 21 days (7 if COVID-19 has been contracted) must have passed after vaccine dose 1 of 1, or at least 21 and at most 98 days must have passed after vaccine dose 1 of 2, or at least 7 days must have passed after vaccine dose 2 of 2"" }, { ""lang"": ""nb"", ""desc"": ""Minst 21 dager (7 ved gjennomgått covid-19) må ha gått etter vaksinedose 1 av 1, eller minst 21 og maksimalt 98 dager må ha gått etter vaksinedose 1 av 2, eller minst 7 dager må ha passert etter vaksinedose 2 av 2"" }, { ""lang"": ""nn"", ""desc"": ""Minst 21 dagar (7 ved gjennomgått covid-19) må ha gått etter vaksinedose 1 av 1, eller minst 21 og maksimalt 98 dagar må ha gått etter vaksinedose 1 av 2, eller minst 7 dagar må ha passert etter vaksinedose 2 av 2"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [ ""v.0.dn"", ""v.0.sd"", ""v.0.mp"", ""v.0.dt"" ], ""Logic"": { ""if"": [ { ""and"": [ { ""==="": [ { ""var"": ""payload.v.0.dn"" }, 1 ] }, { ""==="": [ { ""var"": ""payload.v.0.sd"" }, 1 ] } ] }, { ""if"": [ { ""in"": [ { ""var"": ""payload.v.0.mp"" }, [ ""EU/1/20/1528"", ""EU/1/20/1507"", ""EU/1/21/1529"" ] ] }, { ""after"": [ { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.v.0.dt"" }, 7, ""day"" ] } ] }, { ""after"": [ { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.v.0.dt"" }, 21, ""day"" ] } ] } ] }, { ""if"": [ { ""and"": [ { ""==="": [ { ""var"": ""payload.v.0.dn"" }, 2 ] }, { ""==="": [ { ""var"": ""payload.v.0.sd"" }, 2 ] } ] }, { ""after"": [ { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.v.0.dt"" }, 7, ""day"" ] } ] }, { ""if"": [ { ""and"": [ { ""==="": [ { ""var"": ""payload.v.0.dn"" }, 1 ] }, { ""==="": [ { ""var"": ""payload.v.0.sd"" }, 2 ] } ] }, { ""after"": [ { ""plusTime"": [ { ""var"": ""payload.v.0.dt"" }, 98, ""day"" ] }, { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.v.0.dt"" }, 21, ""day"" ] } ] }, false ] } ] } ] } }, { ""Identifier"": ""TR-NX-0001"", ""Type"": ""Acceptance"", ""Country"": ""NX"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Test"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""Test provider must be approved"" }, { ""lang"": ""nb"", ""desc"": ""Testleverandør må være godkjent"" }, { ""lang"": ""nn"", ""desc"": ""Testleverandør må vera godkjend"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [ ""t.0.tt"", ""t.0.ma"" ], ""Logic"": { ""if"": [ { ""==="": [ { ""var"": ""payload.t.0.tt"" }, ""LP6464-4"" ] }, true, { ""in"": [ { ""var"": ""payload.t.0.ma"" }, [ ""1833"", ""1232"", ""1468"", ""2108"", ""1304"", ""1870"", ""1331"", ""1223"", ""1173"", ""1919"", ""1375"", ""1144"", ""1437"", ""1257"", ""1363"", ""1767"", ""1263"", ""1333"", ""1267"", ""1268"", ""1180"", ""1481"", ""1162"", ""308"", ""1097"", ""1606"", ""1604"", ""1489"", ""344"", ""345"", ""2017"", ""1769"", ""1218"", ""1466"", ""1278"", ""1343"" ] ] } ] } }, { ""Identifier"": ""TR-NX-0002"", ""Type"": ""Acceptance"", ""Country"": ""NX"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Test"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""At most 24 hours must have passed after negative test result"" }, { ""lang"": ""nb"", ""desc"": ""Maksimum 24 timer må ha gått etter negativt testresultat"" }, { ""lang"": ""nn"", ""desc"": ""Maksimum 24 timar må ha gått etter negativt testresultat"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [ ""t.0.sc"" ], ""Logic"": { ""before"": [ { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.t.0.sc"" }, 24, ""hour"" ] } ] } }, { ""Identifier"": ""RR-NX-0001"", ""Type"": ""Acceptance"", ""Country"": ""NX"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Recovery"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""At least 10 days and at most 180 days must have passed after positive test result"" }, { ""lang"": ""nb"", ""desc"": ""Minst 10 dager og maksimalt 180 dager må ha gått etter positivt testresultat"" }, { ""lang"": ""nn"", ""desc"": ""Minst 10 dagar og maksimalt 180 dagar må ha gått etter positivt testresultat"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [ ""r.0.fr"" ], ""Logic"": { ""after"": [ { ""plusTime"": [ { ""var"": ""payload.r.0.fr"" }, 180, ""day"" ] }, { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.r.0.fr"" }, 10, ""day"" ] } ] } }, { ""Identifier"": ""VR-NO-0001"", ""Type"": ""Acceptance"", ""Country"": ""NO"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Vaccination"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""Vaccine provider must be approved"" }, { ""lang"": ""nb"", ""desc"": ""Vaksineleverandør må være godkjent"" }, { ""lang"": ""nn"", ""desc"": ""Vaksineleverandør må vera godkjend"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [ ""v.0.mp"" ], ""Logic"": { ""in"": [ { ""var"": ""payload.v.0.mp"" }, [ ""EU/1/20/1528"", ""EU/1/20/1507"", ""EU/1/20/1525"", ""EU/1/21/1529"" ] ] } }, { ""Identifier"": ""VR-NO-0002"", ""Type"": ""Acceptance"", ""Country"": ""NO"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Vaccination"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""At least 21 days (7 if COVID-19 has been contracted) must have passed after vaccine dose 1 of 1, or at least 7 days must have passed after vaccine dose 2 of 2"" }, { ""lang"": ""nb"", ""desc"": ""Minst 21 dager (7 ved gjennomgått covid-19) må ha gått etter vaksinedose 1 av 1, eller minst 7 dager må ha gått etter vaksinedose 2 av 2"" }, { ""lang"": ""nn"", ""desc"": ""Minst 21 dagar (7 ved gjennomgått covid-19) må ha gått etter vaksinedose 1 av 1, eller minst 7 dagar må ha gått etter vaksinedose 2 av 2"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [ ""v.0.dn"", ""v.0.sd"", ""v.0.mp"", ""v.0.dt"" ], ""Logic"": { ""if"": [ { ""and"": [ { ""==="": [ { ""var"": ""payload.v.0.dn"" }, 1 ] }, { ""==="": [ { ""var"": ""payload.v.0.sd"" }, 1 ] } ] }, { ""if"": [ { ""in"": [ { ""var"": ""payload.v.0.mp"" }, [ ""EU/1/20/1528"", ""EU/1/20/1507"", ""EU/1/21/1529"" ] ] }, { ""after"": [ { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.v.0.dt"" }, 7, ""day"" ] } ] }, { ""after"": [ { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.v.0.dt"" }, 21, ""day"" ] } ] } ] }, { ""if"": [ { ""and"": [ { ""==="": [ { ""var"": ""payload.v.0.dn"" }, 2 ] }, { ""==="": [ { ""var"": ""payload.v.0.sd"" }, 2 ] } ] }, { ""after"": [ { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.v.0.dt"" }, 7, ""day"" ] } ] }, false ] } ] } }, { ""Identifier"": ""TR-NO-0001"", ""Type"": ""Acceptance"", ""Country"": ""NO"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Test"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""Test based COVID-19 certificates are not admitted"" }, { ""lang"": ""nb"", ""desc"": ""Testbaserte koronasertifikat er ikke tillatt"" }, { ""lang"": ""nn"", ""desc"": ""Testbaserte koronasertifikat er ikkje tillatne"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [], ""Logic"": { ""if"": [ false, false, false ] } }, { ""Identifier"": ""RR-NO-0001"", ""Type"": ""Acceptance"", ""Country"": ""NO"", ""Version"": ""1.0.0"", ""SchemaVersion"": ""1.3.0"", ""Engine"": ""CERTLOGIC"", ""EngineVersion"": ""0.7.5"", ""CertificateType"": ""Recovery"", ""Description"": [ { ""lang"": ""en"", ""desc"": ""At least 10 days and at most 180 days must have passed after positive test result"" }, { ""lang"": ""nb"", ""desc"": ""Minst 10 dager og maksimalt 180 dager må ha gått etter positivt testresultat"" }, { ""lang"": ""nn"", ""desc"": ""Minst 10 dagar og maksimalt 180 dagar må ha gått etter positivt testresultat"" } ], ""ValidFrom"": ""2021-07-01T00:00:00Z"", ""ValidTo"": ""2030-01-01T00:00:00Z"", ""AffectedFields"": [ ""r.0.fr"" ], ""Logic"": { ""after"": [ { ""plusTime"": [ { ""var"": ""payload.r.0.fr"" }, 180, ""day"" ] }, { ""plusTime"": [ { ""var"": ""external.validationClock"" }, 0, ""day"" ] }, { ""plusTime"": [ { ""var"": ""payload.r.0.fr"" }, 10, ""day"" ] } ] } } ]";
            var response = new ApiResponse<ICollection<BusinessRule>>(JsonConvert.DeserializeObject<ICollection<BusinessRule>>(jsonResponse), 200);
            return response;
        }
    }
}
