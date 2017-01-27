using System;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace Common
{
public static class Validator
    {
        private static Regex CNPJRegex = new Regex(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$");

        public static bool IsValidCNPJ(string CNPJ)
        {
            if (CNPJRegex.IsMatch(CNPJ))
            {
                string numbers = CNPJ.Replace(".", "").Replace("/", "").Replace("-", "");
                
                List<int> digits = new List<int>();

                foreach (char number in numbers)
                {
                    int digit = (int)char.GetNumericValue(number);

                    digits.Add(digit);
                }
                int first = digits[0]; //Repeat pattern

                for (int i = 1; i < 14; i++)
                {
                    if (digits[i] != first)
                    {
                        break;
                    }
                    if (i == 13)
                    {
                        return false;
                    }
                }
                int sum = 0; //First digit

                for (int i = 0; i < 12; i++)
                {
                    if (i < 4)
                    {
                        sum += digits[i] * (5 - i);
                    }
                    else
                    {
                        sum += digits[i] * (13 - i);
                    }
                }
                int modulo = sum % 11;

                if (modulo < 2)
                {
                    if (digits[12] != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (digits[12] != 11 - modulo)
                    {
                        return false;
                    }
                }
                sum = 0; //Second digit

                for (int i = 0; i < 13; i++)
                {
                    if (i < 5)
                    {
                        sum += digits[i] * (6 - i);
                    }
                    else
                    {
                        sum += digits[i] * (14 - i);
                    }
                }
                modulo = sum % 11;

                if (modulo < 2)
                {
                    if (digits[13] != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (digits[13] != 11 - modulo)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private static Regex CPFRegex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");

        public static bool IsValidCPF(string CPF)
        {
            if (CPFRegex.IsMatch(CPF))
            {
                string numbers = CPF.Replace(".", "").Replace("-", "");


                List<int> digits = new List<int>();

                foreach (char number in numbers)
                {
                    int digit = (int)char.GetNumericValue(number);

                    digits.Add(digit);
                }
                int first = digits[0]; //Repeat pattern

                for (int i = 1; i < 11; i++)
                {
                    if (digits[i] != first)
                    {
                        break;
                    }
                    if (i == 10)
                    {
                        return false;
                    }
                }
                int sum = 0; //First Digit

                for (int i = 0; i < 9; i++)
                {
                    sum += digits[i] * (10 - i);
                }

                int modulo = sum % 11;

                if (modulo < 2)
                {
                    if (digits[9] != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (digits[9] != 11 - modulo)
                    {
                        return false;
                    }
                }
                sum = 0; //Second Digit

                for (int i = 0; i < 10; i++)
                {
                    sum += digits[i] * (11 - i);
                }

                modulo = sum % 11;

                if (modulo < 2)
                {
                    if (digits[10] != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (digits[10] != 11 - modulo)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }

    public static class Common
    {
        public static string FormatToCpfCnpj(string value)
        {
            value = ReplaceToVsales(value);

            if (value == null)
            {
                return null;
            }
            
            if (value.Length == 11)
            {
                return string.Format("{0}.{1}.{2}-{3}", value.Substring(0, 3), value.Substring(3, 3), value.Substring(6, 3), value.Substring(9, 2));
            }
            else if(value.Length == 14)
            {
                return string.Format("{0}.{1}.{2}/{3}-{4}", value.Substring(0, 2), value.Substring(2, 3), value.Substring(5, 3), value.Substring(8, 4), value.Substring(12, 2));
            }
            else
            {
                return value;
            }
        }

        public static string GetUserIpAddress()
        {
            HttpContext context = HttpContext.Current;
           
            string forwarded = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if ( !String.IsNullOrEmpty(forwarded) )
            {
                string[] addresses = forwarded.Split(',');

                if (addresses.Length > 0)
                {
                    return addresses[addresses.Length - 1];
                }
            }
                       
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static bool IsHexa(string texto)
        {
            Regex hexa = new Regex(("[A-F]|[a-f]|[0-9]"));

            foreach (var item in texto)
            {
                if (!hexa.IsMatch(item.ToString()))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Método para verificar se extenção do arquivo de upload, é compatível com as extenções pré-determinadas.
        /// </summary>
        /// <param name="file">Arquivo para upload</param>
        /// <returns>Verdadeiro: Caso extensão seja compatível. Falso: Caso extensão não seja compatível.</returns>
        public static bool IsFileSupported(HttpPostedFileBase file)
        {
            switch (file.ContentType)
            {
                case ("image/jpeg"):
                    return true;

                case ("image/png"):
                    return true;

                case ("image/bmp"):
                    return true;

                case ("image/x-windows-bmp"):
                    return true;

                case ("image/pjpeg"):
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Verifica se possui os caracteres permitidos na string informada.
        /// </summary>
        /// <param name="text">String para ser comparada.</param>
        /// <returns>Verdadeiro: Caso exista caractere especial. Falso: Caso não exista.</returns>
        public static bool ContainInvalidCharacters(string text)
        {
            return !new Regex("^[0-9a-zA-ZáàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ .-]*$").IsMatch(text);
        }

        public static bool IsInvalidText(string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return ContainInvalidCharacters(text);
            }

            return true;
        }

        public static bool IsInvalidText(string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return ContainInvalidCharacters(text);
            }

            return true;
        }

        /// <summary>
        /// Método responsável por mostrar o user friendly names dos enums, apenas adicionando [Description("Nome Amigável")] e utilizar enum.GetDescription()
        /// </summary>
        /// <param name="value">enum</param>
        /// <returns>String com o nome do enum</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<DateTime> DateRange(DateTime fromDate, DateTime toDate)
        {
            return Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1)
                             .Select(d => fromDate.AddDays(d));
        }
    }
}