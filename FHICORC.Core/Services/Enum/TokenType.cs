using System.Collections.Generic;

namespace FHICORC.Core.Services.Enum
{
    public enum TokenType
    {
        NO1,
        HC1,
        Unknown
    }

    public static class TokenTypeExtension
    {
        private static Dictionary<string, TokenType> tokenTypeDictionary = new Dictionary<string, TokenType>()
        {
            {"NO1", TokenType.NO1},
            {"HC1", TokenType.HC1},
        };

        public static TokenType GetTokenType(string prefix)
        {
            return tokenTypeDictionary.TryGetValue(prefix, out var result) ? result : TokenType.Unknown;
        }
    }
}