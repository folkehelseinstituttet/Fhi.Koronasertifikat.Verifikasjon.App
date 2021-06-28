using System;
using System.Linq;
using FHICORC.Core.Services.Model.NO;
using FHICORC.Utils;

namespace FHICORC.Models
{
    public class PassportData
    {

        public PassportData()
        {
        }

        public PassportData( string token, NO1Payload payload)
        {
            SecureToken = token;
            CertificateValidFrom = payload.iat;
            CertificateValidTo = payload.exp;
            FullName = payload.DGCPayloadData.DGC.PersonName.FullName;
            DateOfBirth = payload.DGCPayloadData.DGC.DateOfBirth;
        }
        
        public PassportData( string token, Core.Services.Model.EuDCCModel._1._3._0.DCCPayload payload)
        {
            SecureToken = token;
            DateOfBirth = payload.DCCPayloadData.DCC.DateOfBirth;
            FirstName = payload.DCCPayloadData.DCC.PersonName.Forename;
            LastName =  payload.DCCPayloadData.DCC.PersonName.Surname;
            
            CertificateValidTo = payload.ExpirationTime;
            CertificateValidFrom = payload.IssueAt;
            if (payload.DCCPayloadData.DCC != null)
            {
                if (payload.DCCPayloadData.DCC.Vaccinations?.Any() ?? false)
                {
                    var latestVaccination = payload.DCCPayloadData.DCC.Vaccinations.OrderByDescending(x => x.DoseNumber).First();
                    MedicinialProduct = latestVaccination.VaccineMedicinalProduct ?? "-";
                    Disease = latestVaccination.Disease ?? "-";
                    MarketingAuthorizationHolder = latestVaccination.Manufacturer ?? "-";
                    VaccinationType = latestVaccination.VaccineProphylaxis ?? "-";
                    DoseNumber = latestVaccination.DoseNumber;
                    TotalNumberOfDose = latestVaccination.TotalSeriesOfDose.ToString() ?? "-";
                    VaccinationDate = latestVaccination.DateOfVaccination;
                    VaccinationCountry = latestVaccination.CountryOfVaccination ?? "-";
                    CertificateIdentifier = latestVaccination.CertificateId ?? "-";
                    CertificateIssuer = latestVaccination.CertificateIssuer ?? "-";
                }
                if (payload.DCCPayloadData.DCC.Tests != null && payload.DCCPayloadData.DCC.Tests.Any())
                {
                    var result = payload.DCCPayloadData.DCC.Tests.First();
                    Disease = result.Disease ?? "-";
                    TestName = result.TypeOfTest ?? "-"; ;
                    NAATestName = result.NAATestName ?? "-"; ;
                    TestManufacturer = result.TestManufacturer ?? "-"; ;
                    SampleCollectedTime = result.SampleCollectedTime;
                    Result = result.ResultOfTest ?? "-"; ;
                    TestCenter = result.TestingCentre ?? "-"; ;
                    TestCountry = result.CountryOfTest ?? "-";
                    CertificateIdentifier = result.CertificateId ?? "-";
                    CertificateIssuer = result.CertificateIssuer ?? "-";
                }
                if(payload.DCCPayloadData.DCC.Recovery != null && payload.DCCPayloadData.DCC.Recovery.Any())
                {
                    var recovery = payload.DCCPayloadData.DCC.Recovery.OrderBy(x=>x.ValidFrom).First();
                    RecoveryDisease = recovery.Disease ?? "-";
                    DateFirstPositiveTest = recovery.DateOfFirstPositiveResult;
                    CountryOfTest = recovery.CountryOfTest ?? "-";
                    CertificateIdentifier = recovery.CertificateId ?? "-";
                    CertificateIssuer = recovery.CertificateIssuer ?? "-";
                    CertificateValidFrom = DateTime.Parse(recovery.ValidFrom);
                    CertificateValidTo = DateTime.Parse(recovery.ValidTo);
                }
            }
        }

        public string SecureToken { get; set; }

        //private info
        public string DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        //Vaccine info
        public string MedicinialProduct { get; set; }
        public string MarketingAuthorizationHolder { get; set; }
        public string VaccinationType { get; set; }
        public string TotalNumberOfDose { get; set; }
        public int DoseNumber { get; set; }
        public string VaccinationDate { get; set; }
        public string VaccinationCountry { get; set; }

        //Test
        public string TestName { get; set; }
        public string TypeOfTest { get; set; }
        public string Result { get; set; }
        public DateTime? SampleCollectedTime { get; set; }
        public string Disease { get; set; }
        public string TestManufacturer { get; set; }
        public string SampleOrigin { get; set; }
        public string TestCountry { get; set; }
        public string TestCenter { get; set; }
        public string NAATestName { get; set; }

        //Recovery
        public string RecoveryDisease { get; set; }
        public string DateFirstPositiveTest { get; set; }
        public string CountryOfTest { get; set; }
        public string RecoveryValidTo { get; set; }
        public string RecoveryValidFrom { get; set; }

        //Certificate
        public string CertificateIssuer { get; set; }
        public string CertificateIdentifier { get; set; }
        public string CertificateSchemaVersion { get; set; }
        public DateTime? CertificateValidFrom { get; set; }
        public DateTime? CertificateValidTo { get; set; }

    }
}
