using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;

namespace FHICORC.Tests.Factories
{
    public class SmartHealthCardFactory
    {
        public static string CreateJwsToken()
        {
            return "eyJ6aXAiOiJERUYiLCJhbGciOiJFUzI1NiIsImtpZCI6IkVCS09yNzJRUURjVEJVdVZ6QXprZkJUR2V3MFpBMTZHdVd0eTY0blMtc3cifQ" +
            ".3ZJLb9swEIT_SrC9ypIoJG6kW-MCfRyKAk17CXygqbXFgg-BpIS4gf57d2kHfSDJqafqRnH248yQD6BjhA6GlMbYVVUcUZXRypAGlCYNpZKhjxXeSzsajBWpJwxQgNvtoRPrS3G1XrdtWzbiqoBZQfcA6TgidHe_mH_jXp0WK14Q6nmdtnZy-odM2rsXhcrPuhctbAtQAXt0SUvzZdp9R5XY0n7Q4RuGyJwOLsu6FMTjvzeT6w2yJmD0U1B4m-3DeaM4xwHljSHayQkdEI6UkciTMV-DIcHjfFeT4HHxBPgzxaF57lBaPEGk1YZ48MaRJsR8xkHP6LjHj9Kxj00J24UC7jSFfysTs0S7FqtarJoalqV40o142c2HPyuOSaYp5rh84Qn5gmaplHa48X0mKN9rd8jG4zEmtOf3QzczmNelD4eKm62i7is13xNA5Ulo6mtYtksB47mCbGePAR17-71BEnmlppC3OOyttidEkwPXHIuq2vtg6T2yF6mSD4zsdRyNzHXebC7eocMgzcV7H0edpKGiqETj06fJ7ngU6vw1zzbY_JcNNu2_bvCaNxb6fgI" +
            ".4g9eONAq2TvnJ41uNnm4K6_wNKo3KLoS4VUJ2rcyrX1GaeEWBkSMNQmwLLHHrtx7CWbJ5P-rqriuuglJYOQitA";
        }

        public static string CreateJwksJson(
            string kid = "EBKOr72QQDcTBUuVzAzkfBTGew0ZA16GuWty64nS-sw",
            string x5c = "\"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\"")
        {
            // Matches above Jws token signature ^
            return "{\"keys\":[" +
                "{\"kty\":\"EC\"," +
                "\"kid\":\"3Kfdg-XwP-7gXyywtUfUADwBumDOPKMQx-iELL11W9s\"," +
                "\"use\":\"sig\"," +
                "\"alg\":\"ES256\"," +
                "\"crv\":\"P-256\"," +
                "\"x\":\"11XvRWy1I2S0EyJlyf_bWfw_TQ5CJJNLw78bHXNxcgw\"," +
                "\"y\":\"eZXwxvO1hvCY0KucrPfKo7yAyMT6Ajc3N7OkAB6VYy8\"," +
                "\"crlVersion\":1}," +
                "{\"kty\":\"EC\"," +
                "\"kid\":\"" + kid + "\"," +
                "\"use\":\"sig\"," +
                "\"alg\":\"ES256\"," +
                "\"x5c\":[" + x5c + "]," +
                "\"crv\":\"P-256\"," +
                "\"x\":\"PQHApUWm94mflvswQgAnfHlETMwJFqjUVSs7WU6LQy4\"," +
                "\"y\":\"7mj8IO-8V5VZjDbRVsJINC_Rq5ai5CDhFX18ceRsLWQ\"}]}";
        }

        public static SmartHealthCardImmunization CreateImmunization()
        {
            SmartHealthCardImmunization immunization = new SmartHealthCardImmunization();
            immunization.VaccineCode = CreateCodeableConcept();
            return immunization;
        }

        public static CodeableConcept CreateCodeableConcept()
        {
            CodeableConcept codeableConcept = new CodeableConcept();
            SmartHealthCardCoding firstCoding = new SmartHealthCardCoding();
            SmartHealthCardCoding seccondCoding = new SmartHealthCardCoding();

            firstCoding.System = "system1";
            firstCoding.Code = "code1";
            seccondCoding.System = "system2";
            seccondCoding.Code = "code2";
            codeableConcept.Coding = new SmartHealthCardCoding[] { firstCoding, seccondCoding };
            return codeableConcept;
        }
    }
}
