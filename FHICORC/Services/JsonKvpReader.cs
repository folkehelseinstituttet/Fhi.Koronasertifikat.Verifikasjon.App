using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace FHICORC.Services
{
    public class JsonKvpReader
    {
        public Dictionary<string, string> Read(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                var json = streamReader.ReadToEnd();

                return JsonConvert
                    .DeserializeObject<Dictionary<string, string>>(json);
            }
        }
    }
}
