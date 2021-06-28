using System;
using System.Collections.Generic;
using FHICORC.Core.Services.Model;

namespace FHICORC.Models
{
    public class PublicKeyStorageModel
    {
        public List<PublicKeyDto> PublicKeys { get; set; } = new List<PublicKeyDto>();
        public DateTime LastFetchTimestamp { get; set; }
    }
}