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

        //New Code
        private List<string> errors = new List<string>();

        public void AddError(string error)
        {
            this.errors.Add(error);
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            this.errors.AddRange(errors);
        }

        public bool HasError
        {
            get
            {
                return this.errors.Count > 0;
            }
        }

        public string[] GetErrors()
        {
            return this.errors.ToArray();
        }

        public Validator Empty(string value, string error = null, string name = null)
        {
            if ( Util.IsNullOrEmpty(value) )
            {
                AddError(error ?? string.Format("{0} deve ser especificado", name) );
            }
            return this;
        }

        public Validator Min(string value, int min, string error = null, string name = null)
        {
            if ( !Util.IsNullOrEmpty(value) )
            {
                if (value.Length < min)
                {
                    AddError(error ?? string.Format("{0} deve ter no minimo {1} caractéres", name, min) );
                }
            }
            return this;
        }

        public Validator Max(string value, int max, string error = null, string name = null)
        {
            if ( !Util.IsNullOrEmpty(value) )
            {
                if (value.Length > max)
                {
                    AddError(error ?? string.Format("{0} deve ter no máximo {1} caractéres", name, max) );
                }
            }
            return this;
        }

        public Validator Range(string value, int min, int max, string error = null, string name = null)
        {
            if ( !Util.IsNullOrEmpty(value) )
            {
                if (value.Length < min || value.Length > max)
                {
                    AddError(error ?? string.Format("{0} deve ter entre {1} e {2} caractéres", name, min, max) );
                }
            }
            return this;
        }

        public Validator Custom(string value, Func<string, bool> callback, string error = null, string name = null)
        {
            if ( !Util.IsNullOrEmpty(value) )
            {
                if ( !callback(value) )
                {
                    AddError(error ?? string.Format("{0} não é válido", name) );
                }
            }
            return this;
        }

        public Validator Min(int value, int min, string error = null, string name = null)
        {
            if (value < min)
            {
                AddError(error ?? string.Format("{0} deve ter valor mínimo de {1}", name, min) );
            }
            return this;
        }

        public Validator Max(int value, int max, string error = null, string name = null)
        {
            if (value > max)
            {
                AddError(error ?? string.Format("{0} deve ter valor máximod de {1}", name, max) );
            }
            return this;
        }

        #region Validations

        private static Regex DanfeRegex = new Regex(@"^\d{44}$");

        public static bool IsValidDanfe(string danfe)
        {
            if (Util.IsNullOrEmpty(danfe))
            {
                return false;
            }

            return DanfeRegex.IsMatch(danfe);
        }

        private static Regex CnpjRegex = new Regex(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$");

        public static bool IsValidCnpj(string cnpj)
        {
            if (Util.IsNullOrEmpty(cnpj))
            {
                return false;
            }

            if (CnpjRegex.IsMatch(cnpj))
            {
                if (cnpj == "04.597.732/0002-42" || cnpj == "04.597.732/0001-61" || cnpj == "04.597.732/0003-23")
                {
                    return false;
                }
                List<int> digits = new List<int>();

                foreach (char number in cnpj.Replace(".", "").Replace("/", "").Replace("-", "") )
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

        private static Regex CpfRegex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");

        public static bool IsValidCpf(string cpf)
        {
            if (Util.IsNullOrEmpty(cpf))
            {
                return false;
            }

            if (CpfRegex.IsMatch(cpf))
            {
                List<int> digits = new List<int>();

                foreach (char number in cpf.Replace(".", "").Replace("-", "") )
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

        private static Regex CepRegex = new Regex(@"^\d{5}-\d{3}$");

        public static bool IsValidCep(string cep)
        {
            if (Util.IsNullOrEmpty(cep))
            {
                return false;
            }

            return CepRegex.IsMatch(cep);
        }

        private static Regex PhoneRegex = new Regex(@"^\(\d{2}\)\s\d{4,5}-\d{4}$");

        public static bool IsValidPhone(string phone)
        {
            if ( Util.IsNullOrEmpty(phone) )
            {
                return false;
            }

            if ( !PhoneRegex.IsMatch(phone) )
            {
                return false;
            }

            bool repeat = true;

            for (int i = 0; i < (phone.Length - 6); i++)
            {
                if (phone[i + 6] != '-')
                {
                    if (phone[i + 6] != phone[5] )
                    {
                        repeat = false;

                        break;
                    }
                }
            }

            if (repeat)
            {
                return false;
            }

            if ( !PhoneValidator.ValidatePhone(phone) )
            {
                return false;
            }

            return true;
        }

        public static bool IsValidPhones(string[] phones)
        {
            return !phones.Where(p => !Util.IsNullOrEmpty(p)).GroupBy(p => p).Select(g => g.Count()).Any(v => v > 1);
        }

        private static Regex EmailRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*$");

        public static bool IsValidEmail(string email)
        {
            if (Util.IsNullOrEmpty(email))
            {
                return false;
            }

            return EmailRegex.IsMatch(email);
        }

        private static Regex PercentageRegex = new Regex(@"");

        public static bool IsValidPercentage(string percentage)
        {
            return true; //TODO: implementar
        }

        private static Regex MoneyRegex = new Regex(@"");

        public static bool IsValidMoney(string money)
        {
            return true; //TODO: implementar
        }

        private static Regex NameRegex = new Regex(@"^.+\s.+$");

        public static bool IsValidName(string name)
        {
            if (Util.IsNullOrEmpty(name))
            {
                return false;
            }

            return NameRegex.IsMatch(name);
        }

        private static Regex ImageRegex = new Regex(@"^\.(bmp|jpg|jpeg|png)$");

        public static bool IsValidImage(string extension)
        {
            if (Util.IsNullOrEmpty(extension))
            {
                return false;
            }

            return ImageRegex.IsMatch(extension);
        }

        private static Regex ImageOrPdfRegex = new Regex(@"^\.(bmp|jpg|jpeg|png|pdf)$");

        public static bool IsValidImageOrPdf(string extension)
        {
            if (Util.IsNullOrEmpty(extension))
            {
                return false;
            }

            return ImageOrPdfRegex.IsMatch(extension);
        }

        public static bool IsValidSn(string sn)
        {
            if (Util.IsNullOrEmpty(sn))
            {
                return false;
            }

            if (sn.Length != 15)
            {
                return false;
            }

            foreach (var character in sn)
            {
                bool isHex = (character >= '0' && character <= '9') || (character >= 'a' && character <= 'f') || (character >= 'A' && character <= 'F');

                if (!isHex)
                {
                    return false;
                }
            }

            return true;
        }
        
        private static Regex LoginRegex = new Regex(@"^\w+$");

        public static bool IsValidLogin(string login)
        {
            if (Util.IsNullOrEmpty(login))
            {
                return false;
            }

            return LoginRegex.IsMatch(login);
        }

        public static bool IsValidPassword(string password)
        {
            if ( Util.IsNullOrEmpty(password) )
            {
                return false;
            }

            bool hasLetter = false;

            bool hasDigit = false;

            foreach (var character in password)
            {
                if (char.IsLetter(character) )
                {
                    hasLetter = true;
                }

                if (char.IsDigit(character) )
                {
                    hasDigit = true;
                }
            }
            return hasLetter && hasDigit;
        }

        private static Regex NumberRegex = new Regex(@"^\d{1,5}$");

        public static bool IsValidNumber(string number)
        {
            if ( Util.IsNullOrEmpty(number) )
            {
                return false;
            }

            return NumberRegex.IsMatch(number);
        }

        #endregion

        #region Old Validations

        public static string[] UFList = new string[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };

        #endregion        
    
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