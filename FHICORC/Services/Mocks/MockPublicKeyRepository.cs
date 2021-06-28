using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Mocks
{
    public class MockPublicKeyRepository : IPublicKeyRepository
    {
        public MockPublicKeyRepository()
        {
        }

        public async Task<ApiResponse<List<PublicKeyDto>>> GetPublicKey()
        {
            ApiResponse<List<PublicKeyDto>> result = new ApiResponse<List<PublicKeyDto>>("");
            result.Data = new List<PublicKeyDto>()
            {
                new PublicKeyDto()
                {
                    Kid = "9Bw2LNt8sQI=",
                    PublicKey = "MIIBojANBgkqhkiG9w0BAQEFAAOCAY8AMIIBigKCAYEA2UFwSjL071d6yizNhnB86PtmwL/VGG9nqygMKin7eURZ6isbM6NLYCNAkpzkRU0s5VCnAf6ZLe3ZPB/YilQxPybKb1Ev1Db/QFHJxcHnZBrfU268yGkCX03S6lWjcX1vAWti91AqPXz4kuClJjl3WwcadwSh+2+pqzyGj1zI9ZVSLZ3vtJsnmOUAB+4xqnXHzYyLUfzqC37WfSGtU9rRigkA5fi0BtP0fSw/pb2RBVGG707uySHeaWtfPTY8uKtoBzuriZYYGbNhkctf0QJfcajdWR8a41Q3WKfvaFFJwGvx9a782hGWkCdTqVXa6k8V554AcRpLusr4K7eXt6jPSxhpCUOV6BhsTG5qyQ6HKOwskfSQFVqhIPFv5E2NrZbwQiGZ+l0cLQONUdCt6X+LVAP32T9z+xniOBV1clR8xTTy1BM4E4Pr70+MK9YQZH1194CTpntUKcokxZ60Cwn1rAEFdZ9hxsJWpYuRgvtNF5gxqV38fXAB+P62WLOaqNVZAgMBAAE="
                }
            };
            result.StatusCode = 200;
            return result;
        }
    }
}
