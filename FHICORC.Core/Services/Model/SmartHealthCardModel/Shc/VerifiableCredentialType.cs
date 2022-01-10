using System;
using System.Collections.Generic;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public enum VerifiableCredentialType
    {
        HealthCard,
        Covid19,
        Immunization,
        Laboratory
    }

    public static class VerifiableCredentialTypeSupport
    {
        public static readonly Dictionary<string, VerifiableCredentialType> VerifiableCredentialTypeDictionary = new Dictionary<string, VerifiableCredentialType>()
        {
          { "https://smarthealth.cards#covid19", VerifiableCredentialType.Covid19 },
          { "https://smarthealth.cards#health-card", VerifiableCredentialType.HealthCard },
          { "https://smarthealth.cards#immunization", VerifiableCredentialType.Immunization },
          { "https://smarthealth.cards#laboratory", VerifiableCredentialType.Laboratory }
        };
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class EnumInfoAttribute : Attribute
    {
        readonly string literal;
        readonly string description;

        /// <summary>
        // This is a positional argument
        /// </summary>
        /// <param name="literal"></param>
        /// <param name="description"></param>
        public EnumInfoAttribute(string literal, string description)
        {
            this.literal = literal;
            this.description = description;
        }

        public EnumInfoAttribute(string literal)
        {
            this.literal = literal;
            this.description = "Enum description not defined";
        }

        public string Literal
        {
            get { return literal; }
        }
        public string Description
        {
            get { return description; }
        }
    }
}
