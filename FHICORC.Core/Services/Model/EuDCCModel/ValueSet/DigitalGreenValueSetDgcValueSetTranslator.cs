using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using FHICORC.Core.Services.Interface;

namespace FHICORC.Core.Services.Model.EuDCCModel.ValueSet
{
    public class DigitalGreenValueSetDgcValueSetTranslator: IDgcValueSetTranslator
    {
        private List<ValueSetModel> ValueSetModels = new List<ValueSetModel>();

        private List<string> _valueSetFileNames = new List<string>()
        {
            "disease-agent-targeted.json",
            "test_result.json",
            "vaccine-prophylaxis.json",
            "vaccine-medicinal-product.json",
            "vaccine-mah-manf.json"
        };

        public async void InitValueSet()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DigitalGreenValueSetDgcValueSetTranslator)).Assembly;
            var embededResources = assembly.GetManifestResourceNames();
            foreach (string embededResource in embededResources)
            {
                if (!_valueSetFileNames.Any(x => embededResource.Contains(x))) continue;
                using (Stream resourceStream = assembly.GetManifestResourceStream(embededResource))
                {
                    using (var streamReader = new StreamReader(resourceStream))
                    {
                        var json = await streamReader.ReadToEndAsync();

                        ValueSetModels.Add( JsonConvert
                            .DeserializeObject<ValueSetModel>(json));
                    }  
                }
                
            }
        }
        
        private static Dictionary<DGCValueSetEnum, string> fieldKeyMapping = new Dictionary<DGCValueSetEnum, string>()
        {
            {DGCValueSetEnum.Disease, "disease-agent-targeted"},
            {DGCValueSetEnum.TestResult, "covid-19-lab-result"},
            {DGCValueSetEnum.VaccineProphylaxis, "sct-vaccines-covid-19"},
            {DGCValueSetEnum.VaccineMedicinalProduct, "vaccines-covid-19-names"},
            {DGCValueSetEnum.VaccineAuthorityHolder, "vaccines-covid-19-auth-holders"},
        };

        public object Translate(DGCValueSetEnum key, string value)
        {
            bool canGetSupportedKey = fieldKeyMapping.TryGetValue(key, out var valueSetId);
            if (!canGetSupportedKey) return value;

            ValueSetModel valueSetModel = ValueSetModels.First(x => x.ValueSetId == valueSetId);

            valueSetModel.ValueSetValues.TryGetValue(value, out var translatedValue);

            return translatedValue?.Active ?? false ? translatedValue.Display : value;

        }

        public string GetDGCCode(DGCValueSetEnum key, object value)
        {
            fieldKeyMapping.TryGetValue(key, out var valueSetId);
            ValueSetModel valueSetModel = ValueSetModels.First(x => x.ValueSetId == valueSetId);
            return (string) (valueSetModel.ValueSetValues.FirstOrDefault(x => x.Value.Display == value).Key ?? value);
        }
    }
}