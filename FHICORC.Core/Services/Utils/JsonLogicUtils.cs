using JsonLogic.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace FHICORC.Core.Services.Utils
{
    public static class JsonLogicUtils
    {
        public static EvaluateOperators GetStringSupportedOperators()
        {
            EvaluateOperators operators = EvaluateOperators.Default;

            operators.DeleteOperator("<");
            operators.DeleteOperator("<=");
            operators.DeleteOperator(">");
            operators.DeleteOperator(">=");

            operators.AddOperator("<", GenericArgsSatisfy((prev, next) => prev < next, (prev, next) => string.CompareOrdinal(prev, next) < 0));
            operators.AddOperator("<=", GenericArgsSatisfy((prev, next) => prev <= next, (prev, next) => string.CompareOrdinal(prev, next) <= 0));
            operators.AddOperator(">", GenericArgsSatisfy((prev, next) => prev > next, (prev, next) => string.CompareOrdinal(prev, next) > 0));
            operators.AddOperator(">=", GenericArgsSatisfy((prev, next) => prev >= next, (prev, next) => string.CompareOrdinal(prev, next) >= 0));
            operators.AddOperator("plusTime", (logic, tokens, data) =>
            {
                Console.WriteLine(data);
                var date = Convert.ToString(logic.Apply(tokens[0], data));
                var value = Convert.ToDouble(logic.Apply(tokens[1], data));
                var unit = Convert.ToString(logic.Apply(tokens[2], data));

                var d = DateTime.Parse(date);
                switch (unit)
                {
                    case "day":
                        d = d.AddDays(value);
                        break;
                    case "hour":
                        d = d.AddHours(value);
                        break;
                }
                var result = d.ToString("O");
                return d.ToString("O");
            });
            operators.AddOperator("after", operators.GetOperator(">"));
            operators.AddOperator("before", operators.GetOperator("<"));
            operators.AddOperator("not-after", operators.GetOperator("<="));
            operators.AddOperator("not-before", operators.GetOperator(">="));

            return operators;
        }

        private static Func<IProcessJsonLogic, JToken[], object, object> GenericArgsSatisfy(Func<Double, Double, bool> criteriaDouble, Func<string, string, bool> criteriaText)
        {
            return (p, args, data) =>
            {
                var values = args
                    .Where(a => a != null)
                    .Select(a => p.Apply(a, data))
                    .Select(a => JToken.FromObject(a))
                    .ToArray();

                // all values text?
                var allText = values.All(a => a.Type == JTokenType.String);
                if (allText)
                {
                    var valuesText = args.Select(a => a == null ? "" : p.Apply(a, data).ToString()).ToArray();
                    for (int i = 1; i < valuesText.Length; i++)
                    {

                        if (!criteriaText(valuesText[i - 1], valuesText[i])) return false;
                    }

                    return true;
                }

                // not all values are of type text, assume these are Doubles or any other type and therefore handle this as before
                var valuesDouble = values.Select(a => a == null ? 0d : Double.Parse(p.Apply(a, data).ToString())).ToArray();
                for (int i = 1; i < valuesDouble.Length; i++)
                {

                    if (!criteriaDouble(valuesDouble[i - 1], valuesDouble[i])) return false;



                }

                return true;
            };
        }
    }
}
