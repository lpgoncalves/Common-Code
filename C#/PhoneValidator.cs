using System.Linq;

namespace SGV.Core
{
    public static class PhoneValidator
    {
        public static bool ValidatePhone(string phone)
        {
            phone = phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            string ddd = phone.Substring(0, 2);

            string number = phone.Substring(2, phone.Length - ddd.Length);

            return FixedNumber(ddd, number) || SpecializedMobile(ddd, number) || Mobile(ddd, number);

            return true;
        }

        public static bool ValidateMobile(string phone)
        {
            phone = phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            string ddd = phone.Substring(0, 2);

            string number = phone.Substring(2, phone.Length - ddd.Length);

            return SpecializedMobile(ddd, number) || Mobile(ddd, number);
        }

        private static bool FixedNumber(string ddd, string number)
        {
            return fixedNumberRules.Any(phoneRule => phoneRule.Validate(ddd, number));
        }

        private static bool SpecializedMobile(string ddd, string number)
        {
            return regrasNumeroMovelEspecializado.Any(phoneRule => phoneRule.Validate(ddd, number));
        }

        private static bool Mobile(string ddd, string number)
        {
            return regrasNumeroMovel.Any(phoneRule => phoneRule.Validate(ddd, number));
        }

        // FONTE http://www.anatel.gov.br/setorregulado/index.php/plano-de-numeracao-brasileiro?id=331

        private static IPhoneRule[] fixedNumberRules = new IPhoneRule[] {

            new DefaultPhoneRule(new string[] { "11", "12", "13", "14", "15", "16", "17", "18", "19",
                                                "21", "22", "24",
                                                "27", "28",
                                                "31", "32", "33", "34", "35", "37", "38",
                                                "41", "42", "43", "44", "45", "46",
                                                "47", "48", "49",
                                                "51", "53", "54", "55",
                                                "61",
                                                "62", "64",
                                                "63",
                                                "65", "66", "69",
                                                "67",
                                                "68",
                                                "69",
                                                "71", "73", "74", "75", "77",
                                                "79",
                                                "81", "87",
                                                "82",
                                                "83",
                                                "84",
                                                "85", "88",
                                                "86", "89",
                                                "91", "93", "94",
                                                "92", "97",
                                                "95",
                                                "96",
                                                "98", "99" }, new string[] { "2", "3", "4", "5" }, 8)
        };

        // FONTE http://www.anatel.gov.br/setorregulado/index.php/plano-de-numeracao-brasileiro?id=334

        private static IPhoneRule[] regrasNumeroMovelEspecializado = new IPhoneRule[] { 

	        new DefaultPhoneRule(new string[] { "11", "12", "13", "14", "15", "16", "17", "18", "19" }, new string[] { "70", "77", "78", "79" }, 8),	

	        new DefaultPhoneRule(new string[] { "21", "22", "24" }, new string[] { "70", "77", "78" }, 8),

	        new DefaultPhoneRule(new string[] { "31", "34", "37" }, new string[] { "77", "78" }, 8),

	        new DefaultPhoneRule(new string[] { "27",
                                                "41", "42", "43", "44",
                                                "47", "48",
                                                "51", "54",
                                                "61",
                                                "62",
                                                "65",
                                                "71", "73", "75",
                                                "81",
                                                "85" }, new string[] { "78" }, 8)
        };

        // FONTE http://www.anatel.gov.br/setorregulado/index.php/plano-de-numeracao-brasileiro?id=330

        private static IPhoneRule[] regrasNumeroMovel = new IPhoneRule[] { 

            new DefaultPhoneRule(new string[] { "41", "42", "43", "44", "45", "46",
                                                "47", "48", "49",
                                                "51", "53", "54", "55" }, new string[] { "9", "8", "7", "5", "4" }, 9),

            new DefaultPhoneRule(new string[] { "61",
                                                "62", "64",
                                                "63",
                                                "65", "66", "69",
                                                "67",
                                                "68",
                                                "69" }, new string[] { "9", "8", "7", "5", "4" }, 9), 

            new DefaultPhoneRule(new string[] { "11", "12", "13", "14", "15", "16", "17", "18", "19",
                                                "21", "22", "24", 
                                                "27", "28",
                                                "31","32", "33", "34", "35", "37", "38",
                                                "71", "73", "74", "75", "77", 
                                                "79", 
                                                "81", "87",
                                                "82",
                                                "83",
                                                "84",
                                                "85", "88",
                                                "86", "89",
                                                "91", "93", "94",
                                                "92", "97",
                                                "95",
                                                "96",
                                                "98", "99" }, new string[] { "9", "5", "4" }, 9)
        };

        interface IPhoneRule
        {
            bool Validate(string ddd, string number);
        }

        class DefaultPhoneRule : IPhoneRule
        {
            /// <summary> 
            /// </summary>
            /// <param name="ddds">DDD que essa regra se aplica</param>
            /// <param name="values">Número de telefone deve começar com...</param>
            /// <param name="length">Número de telefone deve ter tamanho...</param>
            public DefaultPhoneRule(string[] ddds, string[] values, int length)
            {
                Length = length;

                DDDs = ddds;

                Values = values;
            }

            public int Length { get; set; }

            public string[] DDDs { get; set; }

            public string[] Values { get; set; }

            public bool Validate(string ddd, string number)
            {
                return number.Length == Length && DDDs.Contains(ddd) && Values.Any(value => number.StartsWith(value));
            }
        }
    }
}