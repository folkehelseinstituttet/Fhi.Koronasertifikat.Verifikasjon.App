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
        public List<ValueSetModel> ValueSetModels { get; set; } = new List<ValueSetModel>();

        private readonly IValueSetService _valueSetService;

        private List<string> _valueSetFileNames = new List<string>()
        {
            "disease-agent-targeted.json",
            "covid-19-lab-result.json",
            "sct-vaccines-covid-19.json",
            "vaccines-covid-19-names.json",
            "vaccines-covid-19-auth-holders.json",
            "covid-19-lab-test-type.json",
            "covid-19-lab-test-manufacturer-and-name.json"
        };

        public DigitalGreenValueSetDgcValueSetTranslator(IValueSetService valueSetService)
        {
            _valueSetService = valueSetService;
        }

        public async void InitValueSet()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DigitalGreenValueSetDgcValueSetTranslator)).Assembly;
            var embeddedResources = assembly.GetManifestResourceNames();
            foreach (string fileName in _valueSetFileNames)
            {
                try
                {
                    using (Stream resourceStream = _valueSetService.GetValueSet(fileName))
                    {
                        using (var streamReader = new StreamReader(resourceStream))
                        {
                            var json = await streamReader.ReadToEndAsync();

                            ValueSetModels.Add(JsonConvert
                                .DeserializeObject<ValueSetModel>(json));
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    var embeddedResource = embeddedResources.Where(x => x.Contains(fileName)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(embeddedResource))
                    {
                        using (Stream resourceStream = assembly.GetManifestResourceStream(embeddedResource))
                        {
                            using (var streamReader = new StreamReader(resourceStream))
                            {
                                var json = await streamReader.ReadToEndAsync();

                                ValueSetModels.Add(JsonConvert
                                    .DeserializeObject<ValueSetModel>(json));
                            }
                        }
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
            {DGCValueSetEnum.TypeOfTest, "covid-19-lab-test-type" },
            {DGCValueSetEnum.TestManufacturer, "covid-19-lab-test-manufacturer-and-name" }
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