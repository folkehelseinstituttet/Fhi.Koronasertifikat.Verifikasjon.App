using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;
using FHICORC.Enums;
using FHICORC.Services;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Translator
{
    public class DGCValueSetTranslator : IDgcValueSetTranslator
    {
        private readonly ITextService _textService;
        private LanguageSelection _selectedLanguage { get; set; }
        private List<ValueSetModel> valueSetModels;
        public List<ValueSetModel> ValueSetModels { get => valueSetModels; set => valueSetModels = value; }

        public List<CsvValueSet> _valueSets = new List<CsvValueSet>();
        public DGCValueSetTranslator(ITextService textService)
        {
            _textService = textService;
            _selectedLanguage = LocaleService.Current.GetLanguage();
        }

        public void SelectLanguage(LanguageSelection languageSelection)
        {
            _selectedLanguage = languageSelection;
        }

        public void InitValueSet()
        {
            Stream fileStream = _textService.GetDgcValueSet();
            using (var streamReader = new StreamReader(fileStream))
            using (var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                DetectDelimiter = true,
            }))
            {
                csv.Context.RegisterClassMap<CsvValueSetMap>();
                var records = csv.GetRecords<CsvValueSet>();
                _valueSets.AddRange(records.ToList());
            }
        }

        public object Translate(DGCValueSetEnum key, string code)
        {
            CsvValueSet valueModel = _valueSets.FirstOrDefault(x => x.Group == key && x.Code == code);
            if (valueModel == null) return code;
            return _selectedLanguage == LanguageSelection.English
                ? valueModel.EnDisplay
                : valueModel.DkDisplay;
        }

        public string GetDGCCode(DGCValueSetEnum key, object value)
        {
            CsvValueSet valueModel =
                _valueSets.FirstOrDefault(x => x.Group == key && (x.EnDisplay == value || x.DkDisplay == value));
            return (string)(valueModel == null ? value : valueModel.Code);
        }
    }
    public class CsvValueSet
    {
        public DGCValueSetEnum Group { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string EnDisplay { get; set; }
        public string DkDisplay { get; set; }

    }

    public class CsvValueSetMap : ClassMap<CsvValueSet>
    {
        public CsvValueSetMap()
        {
            Map(set => set.Group).Index(0).TypeConverter<DGCValueSetEnumConverter>();
            Map(set => set.Description).Index(1);
            Map(set => set.Code).Index(2);
            Map(set => set.EnDisplay).Index(3);
            Map(set => set.DkDisplay).Index(8);
        }

        public static Dictionary<string, DGCValueSetEnum> CsvToEnumValueMap = new Dictionary<string, DGCValueSetEnum>()
        {
            {"dis", DGCValueSetEnum.Disease},
            {"vap", DGCValueSetEnum.VaccineProphylaxis},
            {"mep", DGCValueSetEnum.VaccineMedicinalProduct},
            {"aut", DGCValueSetEnum.VaccineAuthorityHolder},
            {"res", DGCValueSetEnum.TestResult},
            {"typ", DGCValueSetEnum.TypeOfTest}
        };

        public class DGCValueSetEnumConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                bool canGetValue = CsvValueSetMap.CsvToEnumValueMap.TryGetValue(text, out var value);
                return canGetValue ? value : DGCValueSetEnum.Unknown;
            }
        }
    }
}