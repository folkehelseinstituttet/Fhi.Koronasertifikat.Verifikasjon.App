using System;
using Newtonsoft.Json;
using FHICORC.Configuration;
using FHICORC.Core.Services.Interface;
using FHICORC.Models;
using FHICORC.Services.Interfaces;

namespace FHICORC.ViewModels.Certificates
{
    public class PassportViewModel
    {
        private const int PassportPrefetchIntervalInMinute = 30;
        [JsonIgnore]
        public string FullName => string.IsNullOrEmpty(PassportData.FullName)
            ? $"{PassportData.FirstName ?? ""} {PassportData.LastName ?? ""}" 
            : PassportData.FullName;

        [JsonIgnore]
        public string BirthDate => PassportData.DateOfBirth;

        [JsonIgnore]
        public string QRToken => PassportData.SecureToken;
        public PassportData PassportData { get; set; }

        [JsonIgnore]
        public bool IsValid => PassportData.CertificateValidTo > IoCContainer.Resolve<IDateTimeService>().Now;

        [JsonIgnore]
        public bool ShouldPrefetchNewPassport => IsValid && PassportData.CertificateValidTo <
            IoCContainer.Resolve<IDateTimeService>().Now.Add(TimeSpan.FromMinutes(PassportPrefetchIntervalInMinute));

        public PassportViewModel()
        {
            PassportData = new PassportData();
        }
    }
}
