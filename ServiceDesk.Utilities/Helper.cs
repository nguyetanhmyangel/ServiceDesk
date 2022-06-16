using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using Telerik.Web.UI;
using static System.String;

namespace ServiceDesk.Utilities
{
    public static class Helper 
    {
        #region Extention Method
        public const string DateSeparator = "/";
        private static readonly string[] CardinalNumber = new string[10]
          {" không", " một", " hai", " ba", " bốn", " năm", " sáu", " bẩy", " tám", " chín"};

        private static readonly string[] Currency = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };

        public static bool SameString(object v1, object v2)
        {
            return String.Equals(Convert.ToString(v1).Trim(), Convert.ToString(v2).Trim());
        }

        // Compare object as string (case insensitive)
        public static bool SameText(object v1, object v2)
        {
            return String.Equals(Convert.ToString(v1).Trim().ToLower(), Convert.ToString(v2).Trim().ToLower());
        }

        // Check if empty string
        public static bool Empty(object value)
        {
            return String.Equals(Convert.ToString(value).Trim(), String.Empty);
        }

        // Check if not empty string
        public static bool NotEmpty(object value)
        {
            return !Empty(value);
        }

        // Convert object to integer
        public static int ConvertToInt(object value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        // Convert object to double
        public static double ConvertToDouble(object value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }

        public static decimal ConvertToDecimal(object value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
        }

        // Convert object to bool
        public static bool ConvertToBool(object value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }

        public static bool ConvertToBool(string value)
        {
            var result = false;
            try
            {
                if (value != null && value == "1")
                    result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool CheckDateEx(string value, string format, string sep)
        {
            if (value == "") return true;
            while (value.Contains("  ")) value = value.Replace("  ", " ");
            value = value.Trim();
            var pattern = "";
            var sYear = "";
            var sMonth = "";
            var sDay = "";
            var arDT = value.Split(' ');
            if (arDT.Length > 0)
            {
                sep = "\\" + sep;
                switch (format)
                {
                    case "std":
                        pattern = "^([0-9]{4})" + sep + "([0]?[1-9]|[1][0-2])" + sep + "([0]?[1-9]|[1|2][0-9]|[3][0|1])";
                        break;

                    case "us":
                        pattern = "^([0]?[1-9]|[1][0-2])" + sep + "([0]?[1-9]|[1|2][0-9]|[3][0|1])" + sep + "([0-9]{4})";
                        break;

                    case "euro":
                        pattern = "^([0]?[1-9]|[1|2][0-9]|[3][0|1])" + sep + "([0]?[1-9]|[1][0-2])" + sep + "([0-9]{4})";
                        break;
                }

                var re = new Regex(pattern);
                if (!re.IsMatch(arDT[0])) return false;
                var arD = arDT[0].Split(Convert.ToChar(DateSeparator));
                switch (format)
                {
                    case "std":
                        sYear = arD[0];
                        sMonth = arD[1];
                        sDay = arD[2];
                        break;

                    case "us":
                        sYear = arD[2];
                        sMonth = arD[0];
                        sDay = arD[1];
                        break;

                    case "euro":
                        sYear = arD[2];
                        sMonth = arD[1];
                        sDay = arD[0];
                        break;
                }

                if (!CheckDay(ConvertToInt(sYear), ConvertToInt(sMonth), ConvertToInt(sDay))) return false;
            }

            return arDT.Length <= 1 || CheckTime(arDT[1]);
        }

        // Check Date format (yyyy/mm/dd)
        public static bool CheckDate(string value)
        {
            return CheckDateEx(value, "std", DateSeparator);
        }

        // Check time
        public static bool CheckTime(string value)
        {
            if (value == "") return true;
            var re = new Regex("^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]");
            return re.IsMatch(value);
        }

        // Check day
        public static bool CheckDay(int checkYear, int checkMonth, int checkDay)
        {
            var maxDay = 31;
            switch (checkMonth)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    maxDay = 30;
                    break;

                case 2:
                    if (checkYear % 4 > 0)
                        maxDay = 28;
                    else if (checkYear % 100 == 0 && checkYear % 400 > 0)
                        maxDay = 28;
                    else
                        maxDay = 29;
                    break;
            }
            return CheckRange(Convert.ToString(checkDay), 1, maxDay);
        }

        // Check range
        public static bool CheckRange(string value, object min, object max)
        {
            if (value == "") return true;
            return CheckNumber(value) && NumberRange(value, min, max);
        }

        // Định nghĩa hàm IsNumberic
        public static bool IsNumeric(object expression)
        {
            if (expression == null) return false;
            double number;
            return double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture),
                NumberStyles.Any, NumberFormatInfo.InvariantInfo, out number);
        }

        // Định nghĩa hàm IsNumberic theo cách khác
        public static bool IsNumeric(string numberString)
        {
            return numberString.All(char.IsNumber);
        }

        // Check number
        public static bool CheckNumber(string value)
        {
            return value == "" || IsNumeric(value.Trim());
        }

        // Check number range
        public static bool NumberRange(string value, object min, object max)
        {
            return (min == null || !(Convert.ToDouble(value) < Convert.ToDouble(min))) && (max == null || !(Convert.ToDouble(value) > Convert.ToDouble(max)));
        }

        // Check integer
        public static bool CheckInteger(string value)
        {
            if (value == "") return true;
            var re = new Regex("^\\-?\\+?[0-9]+");
            return re.IsMatch(value);
        }

        public static bool CheckPhone(string value)
        {
            if (value == "") return true;
            var re = new Regex("^\\(\\d{3}\\) ?\\d{3}( |-)?\\d{4}|^\\d{3}( |-)?\\d{3}( |-)?\\d{4}");
            return re.IsMatch(value);
        }

        // Check US zip code
        public static bool CheckZip(string value)
        {
            if (value == "") return true;
            var re = new Regex("^\\d{5}|^\\d{5}-\\d{4}");
            return re.IsMatch(value);
        }

        // Check email
        public static bool CheckEmail(string value)
        {
            if (value == "") return true;
            var re = new Regex("^[A-Za-z0-9\\._\\-+]+@[A-Za-z0-9_\\-+]+(\\.[A-Za-z0-9_\\-+]+)+");
            return re.IsMatch(value);
        }

        // Compare values with special handling for null values
        public static bool CompareValue(object v1, object v2)
        {
            if (Convert.IsDBNull(v1) && Convert.IsDBNull(v2))
                return true;
            if (Convert.IsDBNull(v1) || Convert.IsDBNull(v2))
                return false;
            return SameString(v1, v2);
        }

        public static string AppPath()
        {
            var path = HttpContext.Current.Request.ServerVariables["APPL_MD_PATH"];
            var pos = path.IndexOf("Root", StringComparison.InvariantCultureIgnoreCase);
            if (pos > 0)
                path = path.Substring(pos + 4);
            return path;
        }

        public static string CurrentPage()
        {
            return GetPageName(HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
        }

        public static string ReferPage()
        {
            return GetPageName(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]);
        }

        // Get page name
        public static string GetPageName(string url)
        {
            if (!IsNullOrEmpty(url))
            {
                if (url.Contains("?")) url = url.Substring(0, url.LastIndexOf("?", StringComparison.Ordinal));
                // Remove path
                return url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1);
            }
            return "";
        }

        // Get full URL
        public static string FullUrl()
        {
            //var e = new Extension();
            var sUrl = "http";
            var bSsl = ObjectEquals(HttpContext.Current.Request.ServerVariables["HTTPS"], "off");
            var sPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            var defPort = bSsl ? "443" : "80";
            sPort = sPort == defPort ? "" : ":" + sPort;
            if (bSsl) sUrl += "s";
            return sUrl + "://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + sPort +
                   HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
        }

        // Convert to full URL
        public static string ConvertFullUrl(string url)
        {
            if (IsNullOrEmpty(url)) return "";
            if (url.Contains("://")) return url;
            var sUrl = FullUrl();
            return sUrl.Substring(0, sUrl.LastIndexOf("/", StringComparison.Ordinal) + 1) + url;
        }

        private static bool IsLocalUrl(string url)
        {
            return !IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
        {
            if (!IsNullOrEmpty(returnUrl) && !IsLocalUrl(returnUrl))
            {
                PageRedirecting("~/" + returnUrl);
            }
            else
            {
                response.Redirect("~/Index.aspx");
            }
        }

        public static void RedirectToReturnUrl(string returnUrl, int role, HttpResponse response)
        {
            if (!IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else if (role == 1)
                response.Redirect("~/Index.aspx");
            else if (role == 2)
                response.Redirect("~/Issues/TaskExecute.aspx");
            else if (role == 8 || role == 16)
                response.Redirect("~/Issues/Task.aspx");
            else if (role == -1 || role == 32)
                response.Redirect("~/Issues/Task.aspx");                  
            else
                response.Redirect("~/Account/Login.aspx");
        }

        // Compare object as string
        public static bool ObjectEquals(object v1, object v2)
        {
            return String.Equals(Convert.ToString(v1).Trim(), Convert.ToString(v2).Trim());
        }

        /// <summary>
        ///     Định nghĩa các hàm Right,Left,Mid và Space tương tự như VB
        /// </summary>
        /// Returns the first few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the
        /// given length the complete string is returned. If length is zero or
        /// less an empty string is returned
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Left(string s, int length)
        {
            length = Math.Max(length, 0);

            return s.Length > length ? s.Substring(0, length) : s;
        }

        /// Returns the last few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the
        /// given length the complete string is returned. If length is zero or
        /// less an empty string is returned
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Right(string s, int length)
        {
            length = Math.Max(length, 0);
            return s.Length > length ? s.Substring(s.Length - length, length) : s;
        }

        public static string Space(string s, int length)
        {
            const string s1 = "";
            return $"{s1.PadLeft(length)}{s}";
        }

        //start at the specified index in the string ang get N number of
        //characters depending on the lenght and assign it to a variable

        public static string Mid(string param, int startIndex, int length)
        {
            var result = param.Substring(startIndex, length);
            return result;
        }

        //start at the specified index and return all characters after it
        //and assign it to a variable

        public static string Mid(string param, int startIndex)
        {
            var result = param.Substring(startIndex);
            return result;
        }

        /// <summary>
        ///     Hàm tạo một file mới (Kiếm tra nếu chưa có fileName thì tạo mới)
        /// </summary>
        public static void CreateFile(string fileName, string strStr)
        {
            try
            {
                StreamWriter write;
                if (File.Exists(fileName) == false)
                {
                    write = new StreamWriter(fileName);
                    write.WriteLine(strStr);
                    write.Close();
                }
                else
                {
                    var s = File.OpenText(fileName);
                    string line = null;
                    while ((line = s.ReadLine()) != null) strStr += line;
                    s.Close();
                    write = new StreamWriter(fileName);
                    write.WriteLine(strStr);
                    write.Close();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Hàm đọc nội dung 1 file đã tồn tại. Nếu không tìm thấy file sẽ tra về ""
        /// </summary>
        public static string ReadFile(string fileName)
        {
            var content = "";
            if (File.Exists(fileName) == false) return "";
            var s = File.OpenText(fileName);
            string line;
            while ((line = s.ReadLine()) != null) content += line;
            s.Close();
            return content;
        }

        /// <summary>
        ///     Cập nhật nhật nôi dung của file
        /// </summary>
        public static void UpDateFile(string fileName, string newConTent)
        {
            try
            {
                //StreamReader s;
                if (File.Exists(fileName) == false)
                {
                    Console.WriteLine(@"No Have fileName");
                }
                else
                {
                    var write = new StreamWriter(fileName);
                    write.WriteLine(newConTent);
                    write.Close();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Xóa một file được chọn
        /// </summary>
        /// <param name="fileName"></param>
        public static void DeleteFile(string fileName)
        {
            try
            {
                if (!File.Exists(fileName)) return;
                var fi = new FileInfo(fileName);
                fi.Delete();
            }
            catch (Exception)
            {
                //
            }
        }

        // Hàm tạo chuỗi ngẫu nhiên (So Ky tu muon lay, Chuoi nguon (--Lấy từ chuỗi nào))
        public static string CreateArrayRandom(int leng, string arrSource)
        {
            var randNum = new Random();
            var chars = new char[leng];
            //int allowedCharCount = ChuoiNguon.Length;
            for (var i = 0; i < leng; i++) chars[i] = arrSource[(int)(arrSource.Length * randNum.NextDouble())];
            return new string(chars);
        }

        // Hàm tạo chuỗi Pass ngẫu nhiên từ danh sách các kỹ tự
        public static string CreateRandomPassword(int passwordLength)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randNum = new Random();
            var chars = new char[passwordLength];
            //int allowedCharCount = _allowedChars.Length;
            for (var i = 0; i < passwordLength; i++)
                chars[i] = allowedChars[(int)(allowedChars.Length * randNum.NextDouble())];
            return new string(chars);
        }

        public static string CleanStringForUrl(string str)
        {
            var sb = new StringBuilder();
            str = HttpContext.Current.Server.HtmlDecode(str.Trim());
            str = str.Replace("&", "and");
            str = Regex.Replace(str, "Đ|đ|&#273;|&#272;", "d", RegexOptions.IgnoreCase);
            str = str.Normalize(NormalizationForm.FormKD);
            foreach (var t in str)
                if (char.IsWhiteSpace(t))
                    sb.Append('-');
                else if (char.GetUnicodeCategory(t) != UnicodeCategory.NonSpacingMark
                         && !char.IsPunctuation(t)
                         && !char.IsSymbol(t)
                )
                    sb.Append(t);
            return sb.ToString();
        }

        public static string ReadCurrency(long currency, string strTail)
        {
            int lan, i;
            long num;
            var result = "";
            var position = new int[6];

            if (currency < 0) return "Số tiền âm !";
            if (currency == 0) return "Không đồng !";
            if (currency > 0)
                num = currency;
            else
                num = -currency;
            //Kiểm tra số quá lớn
            if (currency > 8999999999999999)
            {
                return "";
            }

            position[5] = (int)(num / 1000000000000000);
            num = num - long.Parse(position[5].ToString()) * 1000000000000000;
            position[4] = (int)(num / 1000000000000);
            num = num - long.Parse(position[4].ToString()) * +1000000000000;
            position[3] = (int)(num / 1000000000);
            num = num - long.Parse(position[3].ToString()) * 1000000000;
            position[2] = (int)(num / 1000000);
            position[1] = (int)(num % 1000000 / 1000);
            position[0] = (int)(num % 1000);
            if (position[5] > 0)
                lan = 5;
            else if (position[4] > 0)
                lan = 4;
            else if (position[3] > 0)
                lan = 3;
            else if (position[2] > 0)
                lan = 2;
            else if (position[1] > 0)
                lan = 1;
            else
                lan = 0;

            for (i = lan; i >= 0; i--)
            {
                var tmp = ReadCardinalNumber(position[i]);
                result += tmp;
                if (position[i] != 0) result += Currency[i];
                if (i > 0 && !IsNullOrEmpty(tmp)) result += ","; //&& (!string.IsNullOrEmpty(tmp))
            }

            if (result.Substring(result.Length - 1, 1) == ",") result = result.Substring(0, result.Length - 1);
            result = result.Trim() + strTail;
            return result.Substring(0, 1).ToUpper() + result.Substring(1);
        }

        // Hàm đọc số có 3 chữ số
        private static string ReadCardinalNumber(int number)
        {
            var result = "";
            // trăm
            var hundred = number / 100;
            // chục
            var dozen = number % 100 / 10;
            // đơn vị
            var unit = number % 10;
            if (hundred == 0 && dozen == 0 && unit == 0) return "";
            if (hundred != 0)
            {
                result += CardinalNumber[hundred] + " trăm";
                if (dozen == 0 && unit != 0) result += " linh";
            }

            if (dozen != 0 && dozen != 1)
            {
                result += CardinalNumber[dozen] + " mươi";
                if (dozen == 0 && unit != 0) result = result + " linh";
            }

            if (dozen == 1) result += " mười";

            switch (unit)
            {
                case 1:
                    if (dozen != 0 && dozen != 1)
                        result += " mốt";
                    else
                        result += CardinalNumber[unit];
                    break;

                case 5:
                    if (dozen == 0)
                        result += CardinalNumber[unit];
                    else
                        result += " lăm";
                    break;

                default:
                    if (unit != 0) result += CardinalNumber[unit];
                    break;
            }

            return result;
        }

        public static string RandomToInt(int codeCount)
        {
            const string allChar = "0,1,2,3,4,5,6,7,8,9";
            var allCharArray = allChar.Split(',');
            var code = "";
            var temp = -1;
            var rand = new Random();
            for (var i = 0; i < codeCount; i++)
            {
                if (temp != -1) rand = new Random(i * temp * (int)DateTime.Now.Ticks);
                var t = rand.Next(10);
                if (temp != -1 && temp == t) return RandomToInt(codeCount);
                temp = t;
                code += allCharArray[t];
            }
            return code;
        }

        public static string RandomCode(int codeCount)
        {
            const string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            var allCharArray = allChar.Split(',');
            var code = "";
            var temp = -1;
            var rand = new Random();
            for (var i = 0; i < codeCount; i++)
            {
                if (temp != -1) rand = new Random(i * temp * (int)DateTime.Now.Ticks);
                var t = rand.Next(36);
                if (temp != -1 && temp == t) return RandomCode(codeCount);
                temp = t;
                code += allCharArray[t];
            }
            return code;
        }

        /// <summary>
        ///     Mã hóa và giải mã
        /// </summary>
        public static string Encrypt(string password)
        {
            var encoding = new UnicodeEncoding();
            var hashBytes = encoding.GetBytes(password);

            // Compute the SHA-1 hash
            var sha1 = new SHA1CryptoServiceProvider();
            var cryptPassword = sha1.ComputeHash(hashBytes);
            return BitConverter.ToString(cryptPassword);
        }

        public static string Encrypt(string data, string key)
        {
            try
            {
                if (data.Length == 0)
                    throw new ArgumentException("Data must be at least 1 character in length.");
                var formattedKey = FormatKey(key);
                if (data.Length % 2 != 0) data += '\0'; // Make sure array is even in length.
                var dataBytes = Encoding.ASCII.GetBytes(data);
                var cipher = String.Empty;
                var tempData = new uint[2];
                for (var i = 0; i < dataBytes.Length; i += 2)
                {
                    tempData[0] = dataBytes[i];
                    tempData[1] = dataBytes[i + 1];
                    Code(tempData, formattedKey);
                    cipher += ConvertUIntToString(tempData[0]) + ConvertUIntToString(tempData[1]);
                }

                return UrlEncode(cipher);
            }
            catch
            {
                return data;
            }
        }

        public static string Decrypt(string data, string key)
        {
            try
            {
                data = UrlDecode(data);
                var formattedKey = FormatKey(key);
                var x = 0;
                var tempData = new uint[2];
                var dataBytes = new byte[data.Length / 8 * 2];
                for (var i = 0; i < data.Length; i += 8)
                {
                    tempData[0] = ConvertStringToUInt(data.Substring(i, 4));
                    tempData[1] = ConvertStringToUInt(data.Substring(i + 4, 4));
                    Decode(tempData, formattedKey);
                    dataBytes[x++] = (byte)tempData[0];
                    dataBytes[x++] = (byte)tempData[1];
                }

                var decipheredString = Encoding.ASCII.GetString(dataBytes, 0, dataBytes.Length);
                if (decipheredString[decipheredString.Length - 1] == '\0')
                    decipheredString = decipheredString.Substring(0, decipheredString.Length - 1);
                return decipheredString;
            }
            catch
            {
                return data;
            }
        }

        private static uint[] FormatKey(string key)
        {
            if (key.Length == 0)
                throw new ArgumentException("Key must be between 1 and 16 characters in length");
            key = key.PadRight(16, ' ').Substring(0, 16); // Ensure that the key is 16 chars in length.
            var formattedKey = new uint[4];

            // Get the key into the correct format for TEA usage.
            var j = 0;
            for (var i = 0; i < key.Length; i += 4)
                formattedKey[j++] = ConvertStringToUInt(key.Substring(i, 4));
            return formattedKey;
        }

        private static void Code(uint[] v, uint[] k)
        {
            var y = v[0];
            var z = v[1];
            uint sum = 0;
            const uint delta = 0x9E3779B9;
            uint n = 32;
            while (n-- > 0)
            {
                y += (((z << 4) ^ (z >> 5)) + z) ^ (sum + k[sum & 3]);
                sum += delta;
                z += (((y << 4) ^ (y >> 5)) + y) ^ (sum + k[(sum >> 11) & 3]);
            }

            v[0] = y;
            v[1] = z;
        }

        private static void Decode(uint[] v, uint[] k)
        {
            var y = v[0];
            var z = v[1];
            var sum = 0xC6EF3720;
            var delta = 0x9E3779B9;
            uint n = 32;
            while (n-- > 0)
            {
                z -= (((y << 4) ^ (y >> 5)) + y) ^ (sum + k[(sum >> 11) & 3]);
                sum -= delta;
                y -= (((z << 4) ^ (z >> 5)) + z) ^ (sum + k[sum & 3]);
            }

            v[0] = y;
            v[1] = z;
        }

        private static uint ConvertStringToUInt(string input)
        {
            uint output = input[0];
            output += (uint)input[1] << 8;
            output += (uint)input[2] << 16;
            output += (uint)input[3] << 24;
            return output;
        }

        private static string ConvertUIntToString(uint input)
        {
            var output = new StringBuilder();
            output.Append((char)(input & 0xFF));
            output.Append((char)((input >> 8) & 0xFF));
            output.Append((char)((input >> 16) & 0xFF));
            output.Append((char)((input >> 24) & 0xFF));
            return output.ToString();
        }

        private static string UrlEncode(string str)
        {
            var encoding = new UnicodeEncoding();
            str = Convert.ToBase64String(encoding.GetBytes(str));
            str = str.Replace('+', '-');
            str = str.Replace('/', '_');
            str = str.Replace('=', '.');
            return str;
        }

        private static string UrlDecode(string str)
        {
            str = str.Replace('-', '+');
            str = str.Replace('_', '/');
            str = str.Replace('.', '=');
            var dataBytes = Convert.FromBase64String(str);
            var encoding = new UnicodeEncoding();
            return encoding.GetString(dataBytes);
        }

        public static string Encode64(string dec)
        {
            var bt = Encoding.Unicode.GetBytes(dec);
            var enc = Convert.ToBase64String(bt);
            return enc;
        }

        public static string Decode64(string enc)
        {
            var bt = Convert.FromBase64String(enc);
            var dec = Encoding.Unicode.GetString(bt);
            return dec;
        }

        // Note: Remove accepted elements in the following array at your own risk.
        public static string[] RemoveXssKeywords = new string[] { "javascript", "vbscript", "expression", "<applet", "<meta", "<xml", "<blink", "<link", "<style", "<script", "<embed", "<object", "<iframe", "<frame", "<frameset", "<ilayer", "<layer", "<bgsound", "<title", "<base", "onabort", "onactivate", "onafterprint", "onafterupdate", "onbeforeactivate", "onbeforecopy", "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus", "onbeforepaste", "onbeforeprint", "onbeforeunload", "onbeforeupdate", "onblur", "onbounce", "oncellchange", "onchange", "onclick", "oncontextmenu", "oncontrolselect", "oncopy", "oncut", "ondataavailable", "ondatasetchanged", "ondatasetcomplete", "ondblclick", "ondeactivate", "ondrag", "ondragend", "ondragenter", "ondragleave", "ondragover", "ondragstart", "ondrop", "onerror", "onerrorupdate", "onfilterchange", "onfinish", "onfocus", "onfocusin", "onfocusout", "onhelp", "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete", "onload", "onlosecapture", "onmousedown", "onmouseenter", "onmouseleave", "onmousemove", "onmouseout", "onmouseover", "onmouseup", "onmousewheel", "onmove", "onmoveend", "onmovestart", "onpaste", "onpropertychange", "onreadystatechange", "onreset", "onresize", "onresizeend", "onresizestart", "onrowenter", "onrowexit", "onrowsdelete", "onrowsinserted", "onscroll", "onselect", "onselectionchange", "onselectstart", "onstart", "onstop", "onsubmit", "onunload" };

        // Remove XSS
        public static object RemoveXSS(object val)
        {
            // Handle null value
            if (Empty(val)) return val;
            // Remove all non-printable characters. CR(0a) and LF(0b) and TAB(9) are allowed
            // This prevents some character re-spacing such as <java\0script>
            // Note that you have to handle splits with \n, \r, and \t later since they *are* allowed in some inputs
            var regEx = new Regex("([\\x00-\\x08][\\x0b-\\x0c][\\x0e-\\x20])", RegexOptions.IgnoreCase);
            // Create regular expression.
            val = regEx.Replace(Convert.ToString(val), "");
            // Straight replacements, the user should never need these since they're normal characters
            // This prevents like <IMG SRC=&#X40&#X61&#X76&#X61&#X73&#X63&#X72&#X69&#X70&#X74&#X3A&#X61&#X6C&#X65&#X72&#X74&#X28&#X27&#X58&#X53&#X53&#X27&#X29>
            var search = "abcdefghijklmnopqrstuvwxyz";
            search = search + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            search = search + "1234567890!@#$%^&*()";
            search = search + "~`\";:?+/={}[]-_|'\\";
            for (var i = 0; i <= search.Length - 1; i++)
            {
                // ;? matches the ;, which is optional
                // 0{0,7} matches any padded zeros, which are optional and go up to 8 chars
                // &#x0040 @ search for the hex values
                regEx = new Regex("(&#[x|X]0{0,8}" + Conversion.Hex(Strings.Asc(search[i])) + ";?)");
                // With a ;
                val = regEx.Replace(Convert.ToString(val), Convert.ToString(search[i]));
                // &#00064 @ 0{0,7} matches '0' zero to seven times
                regEx = new Regex("(&#0{0,8}" + Strings.Asc(search[i]) + ";?)");
                // With a ;
                val = regEx.Replace(Convert.ToString(val), Convert.ToString(search[i]));
            }

            // Now the only remaining whitespace attacks are \t, \n, and \r
            var found = true;
            // Keep replacing as long as the previous round replaced something
            while (found)
            {
                var valBefore = Convert.ToString(val);
                for (var i = 0; i <= RemoveXssKeywords.GetUpperBound(0); i++)
                {
                    var pattern = "";
                    for (var j = 0; j <= RemoveXssKeywords[i].Length - 1; j++)
                    {
                        if (j > 0)
                        {
                            pattern += "(";
                            pattern += "(&#[x|X]0{0,8}([9][a][b]);?)?";
                            pattern += "|(&#0{0,8}([9][10][13]);?)?";
                            pattern += ")?";
                        }
                        pattern = pattern + RemoveXssKeywords[i][j];
                    }
                    var replacement = RemoveXssKeywords[i].Substring(0, 2) + "<x>" + RemoveXssKeywords[i].Substring(2);
                    // Add in <> to nerf the tag
                    regEx = new Regex(pattern);
                    val = regEx.Replace(Convert.ToString(val), replacement);
                    // Filter out the hex tags
                    if (SameString(valBefore, val))
                    {
                        // No replacements were made, so exit the loop
                        found = false;
                    }
                }
            }
            return val;
        }

        // MD5
        public static string Md5(string inputStr)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            var data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(inputStr));
            var sBuilder = new StringBuilder();
            for (var i = 0; i <= data.Length - 1; i++) sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        // CRC32
        public static uint Crc32(string inputStr)
        {
            var bytes = Encoding.Unicode.GetBytes(inputStr);
            var crc = 0xffffffff;
            const uint poly = 0xedb88320;
            var table = new uint[256];
            for (uint i = 0; i < table.Length; ++i)
            {
                var temp = i;
                for (var j = 8; j > 0; --j)
                    if ((temp & 1) == 1)
                        temp = (temp >> 1) ^ poly;
                    else
                        temp >>= 1;
                table[i] = temp;
            }

            foreach (var t in bytes)
            {
                var index = (byte)((crc & 0xff) ^ t);
                crc = (crc >> 8) ^ table[index];
            }

            return ~crc;
        }

        public static string TeaEncrypt(string data, string key)
        {
            try
            {
                if (data.Length == 0)
                    throw new ArgumentException("Data must be at least 1 character in length.");
                var formattedKey = FormatKey(key);
                if (data.Length % 2 != 0) data += '\0'; // Make sure array is even in length.
                var dataBytes = Encoding.Unicode.GetBytes(data);
                var cipher = String.Empty;
                var tempData = new uint[2];
                for (var i = 0; i < dataBytes.Length; i += 2)
                {
                    tempData[0] = dataBytes[i];
                    tempData[1] = dataBytes[i + 1];
                    Code(tempData, formattedKey);
                    cipher += ConvertUIntToString(tempData[0]) + ConvertUIntToString(tempData[1]);
                }
                return UrlEncode(Compress(cipher));
            }
            catch
            {
                return data;
            }
        }

        public static string TeaDecrypt(string data, string key)
        {
            try
            {
                data = Decompress(UrlDecode(data));
                var formattedKey = FormatKey(key);
                var x = 0;
                var tempData = new uint[2];
                var dataBytes = new byte[data.Length / 8 * 2];
                for (var i = 0; i < data.Length; i += 8)
                {
                    tempData[0] = ConvertStringToUInt(data.Substring(i, 4));
                    tempData[1] = ConvertStringToUInt(data.Substring(i + 4, 4));
                    Decode(tempData, formattedKey);
                    dataBytes[x++] = (byte)tempData[0];
                    dataBytes[x++] = (byte)tempData[1];
                }
                var decipheredString = Encoding.Unicode.GetString(dataBytes, 0, dataBytes.Length);
                if (decipheredString[decipheredString.Length - 1] == '\0')
                    decipheredString = decipheredString.Substring(0, decipheredString.Length - 1);
                return decipheredString;
            }
            catch
            {
                return data;
            }
        }

        public static string Compress(string text)
        {
            var buffer = Encoding.Unicode.GetBytes(text);
            var ms = new MemoryStream();
            using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }
            ms.Position = 0;
            var outStream = new MemoryStream();
            var compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);
            var gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string Decompress(string compressedText)
        {
            var gzBuffer = Convert.FromBase64String(compressedText);
            using (var ms = new MemoryStream())
            {
                var msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
                var buffer = new byte[msgLength];
                ms.Position = 0;
                using (var zip = new System.IO.Compression.GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }
                return Encoding.Unicode.GetString(buffer);
            }
        }

        // Last URL
        public static string LastUrl => Claim.Cookie[Config.LastUrl];

        public static string RedirectUrl => IsNullOrEmpty(Claim.Cookie[Config.LastUrl]) ? Config.DefaultUrl : Claim.Cookie[Config.LastUrl];

        public static void SaveLastUrl()
        {
            var s = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
            var q = HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
            if (NotEmpty(q)) s = s + "?" + q;
            if (LastUrl == s) s = "";
            Claim.Cookie[Config.LastUrl] = s;
        }

        public static void ResetUrl()
        {
            Claim.Cookie[Config.LastUrl] = string.Empty;
        }

        public static void PageRedirecting(string url)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Redirect(url);
        }

        public static bool AllowExtenstion(string name,IEnumerable extenstion)
        {
            var result = true;
            foreach (var c in extenstion)
                if (!c.Equals(name.ToLower()))
                    result = false;
                else
                    return true;
            return result;
        }

        public static int DownloadFile(string nameFile,string folder,HttpResponse response)
        {
            var result = 0;
            if (!IsNullOrEmpty(nameFile))
            {
                var target = HttpContext.Current.Server.MapPath("folder");
                var fullPath = Path.Combine(target, nameFile);
                if (File.Exists(fullPath))
                {
                    var binaryData = File.ReadAllBytes(fullPath);
                    response.Clear();
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("content-disposition", "attachment; filename=" + nameFile);
                    response.BinaryWrite(binaryData);
                    response.Flush();
                    response.End();
                    result = 1;
                }
            }
            return result;
        }

        public static void UploadFile(RadAsyncUpload upload,string folder,out string fileName)
        {
            var target = HttpContext.Current.Server.MapPath(folder);
            //Get the full file name
            fileName = upload.UploadedFiles[0].GetNameWithoutExtension() + "_" + DateTime.Now.Day +
                           DateTime.Now.Month + DateTime.Now.Year + "_" + Guid.NewGuid() + upload.UploadedFiles[0].GetExtension();
            try
            {
                //Save the file
                upload.UploadedFiles[0].SaveAs(Path.Combine(target, fileName));
            }
            catch (Exception e)
            {
                //return string.Empty;
            }
        }

        #endregion

        #region Message

       public static void DisplayMessage(bool visible, bool error, HtmlGenericControl div, Label danger, Label success, string text)
        {
            div.Visible = visible;
            if (error)
            {
                div.Attributes.Remove("class");
                div.Attributes.Add("class", "alert alert-danger fade in");
            }
            else
            {
                div.Attributes.Remove("class");
                div.Attributes.Add("class", "alert alert-success fade in");
            }

            var label = error ? danger : success;
            label.Text = text;
        }

        public static void DisplayMessage(bool isError, string text, Label label)
        {
            label.Font.Italic = true;
            label.Visible = true;
            label.ForeColor = isError ? Color.Red : Color.Green;
            label.Text = text;
        }

        public static void Notification(RadNotification notification,string text,string notifiIcon)
        {
            notification.Text = text;
            //info,delete, deny, edit, ok, warning, none
            notification.TitleIcon = notifiIcon;
            notification.ContentIcon = notifiIcon;
            notification.Show();
            //notification.ShowSound = this.showSound.SelectedValue;

            // Enum
            //notification.Position = (NotificationPosition)Enum.Parse(typeof(NotificationPosition), position);
            //notification.Animation = (NotificationAnimation)Enum.Parse(typeof(NotificationAnimation), animation);
            //notification.ContentScrolling = (NotificationScrolling)Enum.Parse(typeof(Telerik.Web.UI.NotificationScrolling), this.ContentScrolling.SelectedValue);

            //Unit
            //notification.Width = this.Width.Text != string.Empty ? Unit.Parse(this.Width.Text) : notification.Width;
            //notification.Height = this.Height.Text != string.Empty ? Unit.Parse(this.Height.Text) : notification.Height;

            //Integer
            //notification.OffsetX = !Object.Equals(this.OffsetX.Value, null) ? int.Parse(this.OffsetX.Value.ToString()) : notification.OffsetX;
            //notification.OffsetY = !Object.Equals(this.OffsetY.Value, null) ? int.Parse(this.OffsetY.Value.ToString()) : notification.OffsetY;
            //notification.AutoCloseDelay = !Object.Equals(this.AutoCloseDelay.Value, null) ? int.Parse(this.AutoCloseDelay.Value.ToString()) : notification.AutoCloseDelay;
            //notification.AnimationDuration = !Object.Equals(this.AnimationDuration.Value, null) ? int.Parse(this.AnimationDuration.Value.ToString()) : notification.AnimationDuration;
            //notification.Opacity = int.Parse(this.opacity.Value.ToString());

            //Boolean
            //notification.Pinned = this.Pinned.Checked;
            //notification.EnableRoundedCorners = this.corners.Checked;
            //notification.EnableShadow = this.shadow.Checked;
            //notification.KeepOnMouseOver = this.keepMouse.Checked;
            //notification.VisibleTitlebar = this.titlebar.Checked;
            //notification.ShowCloseButton = this.closeBtn.Checked;
        }

        #endregion Message

        #region Radgrid Helper

        public static void HierarchyGridFullCommand(RadGrid grd, GridTableView view, bool canAdd, bool canEdit, bool canDelete)
        {
            foreach (GridColumn col in view.Columns)
            {
                //phân quyền AddNew
                try
                {
                    if (grd.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
                    {
                        var commandItem = grd.MasterTableView.GetItems(GridItemType.CommandItem)[0] as GridCommandItem;
                        commandItem.FindControl("AddNewRecordButton").Visible = canAdd;
                    }
                }
                catch
                {
                    // ignored
                }

                // phân quyền Delete của Grid
                try
                {
                    var deleteColumn = (GridButtonColumn)grd.MasterTableView.GetColumn("DeleteCommandColumn");
                    if (deleteColumn != null)
                        deleteColumn.Visible = canDelete;
                }
                catch
                {
                    // ignored
                }

                // phân quyền Edit của Grid
                try
                {
                    if (grd.MasterTableView.GetColumn("EditCommandColumn") != null)
                        grd.MasterTableView.GetColumn("EditCommandColumn").Display = canEdit;
                }
                catch
                {
                    // ignored
                }

                if (view.HasDetailTables)
                    foreach (GridDataItem item in view.Items)
                    {
                        foreach (var innerView in item.ChildItem.NestedTableViews)
                        {
                            HierarchyGridFullCommand(grd, innerView, canAdd, canEdit, canDelete);
                        }
                    }
            }
        }

        public static void HierarchyGridCommandByApproved(RadGrid grd, GridTableView view, bool canAdd, bool canEdit, bool canDelete)
        {
            grd.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = canAdd;
            foreach (GridDataItem item in grd.MasterTableView.Items)
            {
                //var datakeyName = item.OwnerTableView.DataKeyValues[item.ItemIndex][dataKeyName].ToString();
                var chkLock = (CheckBox)item.FindControl("chkLock");
                var approved = chkLock.Checked;
                var edit = item["EditCommandColumn"].Controls[0] as ImageButton;
                var delete = item["DeleteCommandColumn"].Controls[0] as ImageButton;
                if (!approved && canEdit)
                {
                    if (edit != null)
                    {
                        edit.Enabled = true;
                        edit.ImageUrl = "~/Images/Grid/grd_Edit.gif";
                    }
                }
                else
                {
                    if (edit != null)
                    {
                        edit.Enabled = false;
                        edit.ImageUrl = "~/Images/Lock.gif";
                    }
                }
                if (!approved && canDelete)
                {
                    if (delete != null)
                    {
                        delete.Enabled = true;
                        delete.ImageUrl = "~/Images/Grid/vista_Delete.gif";
                    }
                }
                else
                {
                    delete.Enabled = false;
                    delete.ImageUrl = "~/Images/Lock.gif";
                }
                //if (edit != null) edit.Enabled = !approved && canEdit;
                //if (delete != null) delete.Enabled = !approved && canDelete;
                if (item.HasChildItems)
                {
                    foreach (GridDataItem citem in item.ChildItem.NestedTableViews[0].Items)
                    {
                        // phân quyền addNew Detail không hiểu sao ko ăn, tính sau.
                        //item.ChildItem.NestedTableViews[0].CommandItemSettings.ShowAddNewRecordButton = !approved && canAdd;
                        // phân quyền Delete của Grid
                        try
                        {
                            var deleteColumn = (GridButtonColumn)item.ChildItem.NestedTableViews[0].GetColumn("DeleteCommandColumn");
                            if (deleteColumn != null)
                                deleteColumn.Visible = !approved && canDelete;
                        }
                        catch {/*ignored*/}

                        // phân quyền Edit của Grid
                        try
                        {
                            var editColumn = item.ChildItem.NestedTableViews[0].GetColumn("EditCommandColumn");
                            if (editColumn != null)
                            {
                                editColumn.Display = !approved && canEdit;
                            }
                        }
                        catch {/*ignored*/}
                    }
                }
            }
        }

        public static void DetailGridCommandByApproved(RadGrid grd, GridTableView view, bool canAdd, bool canEdit, bool canDelete)
        {
            //if (view.HasDetailTables)
            //{
            //    foreach (GridDataItem item in view.Items)
            //    {
            //        var edit = item["EditCommandColumn"].Controls[0] as ImageButton;
            //        var delete = item["DeleteCommandColumn"].Controls[0] as ImageButton;
            //        if (edit != null) edit.Enabled = canEdit;
            //        if (delete != null) delete.Enabled = canDelete;
            //        grd.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = canAdd;
            //        foreach (var innerView in item.ChildItem.NestedTableViews)
            //        {
            //            DetailGridCommandByApproved(grd, innerView, canAdd, canEdit, canDelete);
            //        }
            //    }
            //}

            foreach (GridColumn col in view.Columns)
            {
                grd.MasterTableView.DetailTables[0].CommandItemSettings.ShowAddNewRecordButton = canAdd;
                // phân quyền Delete của Grid
                try
                {
                    var deleteColumn = (GridButtonColumn)grd.MasterTableView.DetailTables[0].GetColumn("DeleteCommandColumn");
                    if (deleteColumn != null)
                        deleteColumn.Visible = canDelete;
                }
                catch
                {
                    // ignored
                }

                // phân quyền Edit của Grid
                try
                {
                    var editColumn = grd.MasterTableView.DetailTables[0].GetColumn("EditCommandColumn");
                    if (editColumn != null)
                    {
                        editColumn.Display = canEdit;
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void HierarchyGridOnlyAddCommand(RadGrid grd, GridTableView view, bool canAdd)
        {
            foreach (GridColumn col in view.Columns)
            {
                //phân quyền AddNew
                if (grd.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
                {
                    var commandItem = grd.MasterTableView.GetItems(GridItemType.CommandItem)[0] as GridCommandItem;
                    if (commandItem != null) commandItem.FindControl("AddNewRecordButton").Visible = canAdd;
                }
                grd.MasterTableView.DetailTables[0].CommandItemSettings.ShowAddNewRecordButton = canAdd;
            }
        }

        public static void HierarchyGridOnyEditCommand(RadGrid grd, GridTableView view, bool canEdit)
        {
            foreach (GridColumn col in view.Columns)
            {
                // phân quyền Edit của Grid
                try
                {
                    if (grd.MasterTableView.GetColumn("EditCommandColumn") != null)
                        grd.MasterTableView.GetColumn("EditCommandColumn").Display = canEdit;
                }
                catch
                {
                    // ignored
                }

                if (view.HasDetailTables)
                {
                    foreach (GridDataItem item in view.Items)
                    {
                        foreach (var innerView in item.ChildItem.NestedTableViews)
                        {
                            HierarchyGridOnyEditCommand(grd, innerView, canEdit);
                        }
                    }
                }
            }
        }

        public static void HierarchyGridOnlyDeleteCommand(RadGrid grd, GridTableView view, bool canDelete)
        {
            foreach (GridColumn col in view.Columns)
            {
                // phân quyền Delete của Grid
                try
                {
                    var deleteColumn = (GridButtonColumn)grd.MasterTableView.GetColumn("DeleteCommandColumn");
                    if (deleteColumn != null)
                        deleteColumn.Visible = canDelete;
                }
                catch
                {
                    // ignored
                }

                if (view.HasDetailTables)
                    foreach (GridDataItem item in view.Items)
                    {
                        foreach (var innerView in item.ChildItem.NestedTableViews)
                        {
                            HierarchyGridOnlyDeleteCommand(grd, innerView, canDelete);
                        }
                    }
            }
        }

        public static void HierarchyGridAddEditCommand(RadGrid grd, GridTableView view, bool canAdd, bool canEdit)
        {
            foreach (GridColumn col in view.Columns)
            {
                //phân quyền AddNew
                if (grd.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
                {
                    var commandItem = grd.MasterTableView.GetItems(GridItemType.CommandItem)[0] as GridCommandItem;
                    if (commandItem != null) commandItem.FindControl("AddNewRecordButton").Visible = canAdd;
                }

                grd.MasterTableView.DetailTables[0].CommandItemSettings.ShowAddNewRecordButton = canAdd;
                // phân quyền Edit của Grid
                try
                {
                    if (grd.MasterTableView.GetColumn("EditCommandColumn") != null)
                        grd.MasterTableView.GetColumn("EditCommandColumn").Display = canEdit;
                }
                catch
                {
                    // ignored
                }

                if (view.HasDetailTables)
                    foreach (GridDataItem item in view.Items)
                    {
                        foreach (var innerView in item.ChildItem.NestedTableViews)
                        {
                            HierarchyGridAddEditCommand(grd, innerView, canAdd, canEdit);
                        }
                    }
            }
        }

        public static void HierarchyGridAddDelete(RadGrid grd, GridTableView view, bool canAdd, bool canDelete)
        {
            foreach (GridColumn col in view.Columns)
            {
                //phân quyền AddNew
                if (grd.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
                {
                    var commandItem = grd.MasterTableView.GetItems(GridItemType.CommandItem)[0] as GridCommandItem;
                    if (commandItem != null) commandItem.FindControl("AddNewRecordButton").Visible = canAdd;
                }

                grd.MasterTableView.DetailTables[0].CommandItemSettings.ShowAddNewRecordButton = canAdd;
                // phân quyền Delete của Grid
                try
                {
                    var deleteColumn = (GridButtonColumn)grd.MasterTableView.GetColumn("DeleteCommandColumn");
                    if (deleteColumn != null)
                        deleteColumn.Visible = canDelete;
                }
                catch
                {
                    // ignored
                }

                if (view.HasDetailTables)
                    foreach (GridDataItem item in view.Items)
                    {
                        foreach (var innerView in item.ChildItem.NestedTableViews)
                        {
                            HierarchyGridAddDelete(grd, innerView, canAdd, canDelete);
                        }
                    }
            }
        }

        public static void HierarchyGridEditDeleteCommand(RadGrid grd, GridTableView view, bool canEdit, bool canDelete)
        {
            foreach (GridColumn col in view.Columns)
            {
                // phân quyền Delete của Grid
                try
                {
                    var deleteColumn = (GridButtonColumn)grd.MasterTableView.GetColumn("DeleteCommandColumn");
                    if (deleteColumn != null)
                        deleteColumn.Visible = canDelete;
                }
                catch
                {
                    // ignored
                }

                // phân quyền Edit của Grid
                try
                {
                    if (grd.MasterTableView.GetColumn("EditCommandColumn") != null)
                        grd.MasterTableView.GetColumn("EditCommandColumn").Display = canEdit;
                }
                catch
                {
                    // ignored
                }

                if (view.HasDetailTables)
                    foreach (GridDataItem item in view.Items)
                    {
                        foreach (var innerView in item.ChildItem.NestedTableViews)
                        {
                            HierarchyGridEditDeleteCommand(grd, innerView, canEdit, canDelete);
                        }
                    }
            }
        }

        public static void TextLimit(string labelId, int length, GridItemEventArgs e)
        {
            try
            {
                var label = (Label)e.Item.FindControl(labelId);
                if (label != null && label.Text.Length > length)
                    label.Text = label.Text.Substring(0, length) + @"...";
            }
            catch (Exception ex)
            {
                //Console.WriteLine(exception);
                //throw;
            }
        }

        public static void CssToLabel(string labelId ,string cssClass, GridItemEventArgs e)
        {
            try
            {
                var label = (Label)e.Item.FindControl(labelId);
                if (label != null)
                    //label.Attributes.CssStyle.Add("float", "right");
                    label.CssClass = cssClass;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(exception);
                //throw;
            }
        }

        public static void DisplayMessage(RadGrid grd, bool result, string text)
        {
            if (result)
                grd.Controls.Add(new LiteralControl(
                    $"<span style='color: blue;padding: 5px;font-weight: bold;'>{text}</span>"));
            else
                grd.Controls.Add(new LiteralControl(
                    $"<span style='color: red;padding: 5px;font-weight: bold;'>{text}</span>"));
        }

        public static void GridItemDeleted(RadGrid grd, GridDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(grd, false, " Cannot be deleted. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(grd, true, " Delete is successful");
            }
        }

        public static void GridItemUpdated(RadGrid grd, GridUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
                DisplayMessage(grd, false, " Cannot be updated. Reason: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(grd, true, " Update is successful ");
            }
        }

        public static void GridItemInserted(RadGrid grd, GridInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
                DisplayMessage(grd, false, " Cannot be inserted. Reason: " + e.Exception.Message);
            }
            else
            {
                grd.CurrentPageIndex = 0;
                DisplayMessage(grd, true, " Insert is successful ");
            }
        }

        public static void GridTableHierarchyAllCommand(GridTableView view, bool editAllowed)
        {
            //Recursively set editability for this table view and all its children.

            //This should be unnecessary since we hide command buttons, but is further protection.
            view.AllowAutomaticInserts = editAllowed;
            view.AllowAutomaticDeletes = editAllowed;
            view.AllowAutomaticUpdates = editAllowed;

            //If editing is not allowed, remove any edit or command-button columns.
            if (!editAllowed)
            {
                foreach (GridColumn col in view.Columns)
                    if (col is GridEditCommandColumn || col is GridButtonColumn)
                        col.Visible = false;
            }

            //Propagate the editability changes to the child views of each item in this view.
            //Telerik's support notes:
            //This code finds all nested tables from the data items instead of the nested table.
            //This is because the GridTableView.DetailTables collection is only a set of templates
            //for creating the nested tables. The real detail tables are contained
            //in the GridDataItem.ChildItem.NestedTableViews collection.
            if (view.HasDetailTables)
                foreach (GridDataItem item in view.Items)
                {
                    if (item.HasChildItems)
                        foreach (var innerView in item.ChildItem.NestedTableViews)
                        {
                            GridTableHierarchyAllCommand(innerView, editAllowed);
                        }
                }
        }

        #endregion Radgrid Helper

        #region RadComboBox Helper

        public static void AddComboBoxItems(RadComboBox comboBox, DataTable data, int perRequest, string comboId, string comboName, RadComboBoxItemsRequestedEventArgs e)
        {
            var itemOffset = e.NumberOfItems;
            var endOffset = Math.Min(itemOffset + perRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;
            for (var i = itemOffset; i < endOffset; i++)
            {
                var item = new RadComboBoxItem
                {
                    Text = data.Rows[i][comboName].ToString(),
                    Value = data.Rows[i][comboId].ToString()
                };
                comboBox.Items.Add(item);
                comboBox.DataBind();
            }

            e.Message = data.Rows.Count <= 0 ? "No matches" : $"Items <b>1</b>-<b>{endOffset}</b> out of <b>{data.Rows.Count}</b>";
        }

        public static void AddComboBoxItems(RadComboBox comboBox, DataTable data, int perRequest, string comboId, string comboName, List<RadControlAttributes> attributes, RadComboBoxItemsRequestedEventArgs e)
        {
            var itemOffset = e.NumberOfItems;
            var endOffset = Math.Min(itemOffset + perRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;
            for (var i = itemOffset; i < endOffset; i++)
            {
                var item = new RadComboBoxItem
                {
                    Text = data.Rows[i][comboName].ToString(),
                    Value = data.Rows[i][comboId].ToString()
                };
                foreach (var a in attributes.OrderBy(o=>o.Id))
                {
                    item.Attributes.Add(a.Name, data.Rows[i][a.Name].ToString());
                }

                comboBox.Items.Add(item);
                comboBox.DataBind();
            }

            e.Message = data.Rows.Count <= 0 ? "No matches" : $"Items <b>1</b>-<b>{endOffset}</b> out of <b>{data.Rows.Count}</b>";
        }

        public static void SetComboBoxValue(RadComboBox combo, string text, int value)
        {
            var selectedItem = new RadComboBoxItem();
            combo.Text = text;
            combo.SelectedValue = value.ToString();
            combo.Items.Add(selectedItem);
            selectedItem.DataBind();
            //Claim.Session[value] = value;
        }

        public static void SetComboBoxValue(RadComboBox cbo, SqlDataReader dataReader, string value, string text, string defaultValue, string defaultText)
        {
            cbo.Items.Clear();
            cbo.DataSource = dataReader;
            cbo.DataTextField = text;
            cbo.DataValueField = value;
            cbo.DataBind();
            cbo.Items.Insert(0, new RadComboBoxItem(defaultText, defaultValue));
        }

        public static void SetComboboxReadOnly(RadComboBox cbo)
        {
            cbo.AllowCustomText = false;
            cbo.MarkFirstMatch = false;
            cbo.EnableLoadOnDemand = false;
            cbo.ShowDropDownOnTextboxClick = false;
            cbo.ShowToggleImage = false;
            foreach (RadComboBoxItem radComboBoxItem in cbo.Items) radComboBoxItem.Enabled = false;
        }

        public static void BindYearComboBox(RadComboBox cbo, int subtrahend, int step)
        {
            var currentYear = DateTime.Now.Year;
            cbo.Items.Clear();
            for (var i = currentYear - subtrahend; i <= currentYear + step; i++)
            {
                var year = "Year " + i;
                cbo.Items.Insert(0, new RadComboBoxItem(year, i.ToString()));
            }
        }

        public static void BindMonthComboBox(RadComboBox cbo)
        {
            cbo.Items.Clear();
            for (var i = 1; i < 12; i++)
            {
                var year = "Month " + i;
                cbo.Items.Insert(0, new RadComboBoxItem(year, i.ToString()));
            }
        }

        #endregion RadComboBox Helper

        #region CheckBox
        public static void AllCheck(RadGrid grd, CheckBox checkBox, string checkAllId, string checkId)
        {
            var header = (GridHeaderItem)checkBox.NamingContainer;
            var checkAll = (CheckBox)header.FindControl(checkAllId);
            foreach (GridDataItem grdRow in grd.MasterTableView.Items)
            {
                var chkAdd =
                    (CheckBox)grdRow.FindControl(checkId);
                if (checkAll != null && checkAll.Checked && chkAdd != null)
                    chkAdd.Checked = true;
                else
                if (chkAdd != null) chkAdd.Checked = false;
            }
        }
        #endregion CheckBox

        public static void AddMultiSelectItems(RadMultiSelect comboBox, DataTable data, int perRequest, string comboId, string comboName, List<RadControlAttributes> attributes)
        {

            for (var i = 0; i < 10; i++)
            {
                var item = new MultiSelectItem
                {
                    Text = data.Rows[i][comboName].ToString(),
                    Value = data.Rows[i][comboId].ToString()
                };
                foreach (var a in attributes.OrderBy(o => o.Id))
                {
                    item.Attributes.Add(a.Name, data.Rows[i][a.Name].ToString());
                }

                comboBox.Items.Add(item);
                comboBox.DataBind();
            }

        }

        public static void SetPickerReadOnly(RadDatePicker picker)
        {
            picker.DateInput.ReadOnly = true;
            picker.Calendar.Enabled = false;
            //picker.EnableTyping = false;
            //picker.DatePopupButton.Enabled = false;
        }

        public static void SetControlReadOnly(Control control)
        {
            try
            {
                switch (control)
                {
                    case RadDatePicker picker:
                        picker.DateInput.ReadOnly = true;
                        picker.Calendar.Enabled = false;
                        break;
                    case RadComboBox radComboBox:
                        radComboBox.Enabled = false;
                        break;
                    case RadMultiSelect radMultiSelect:
                        radMultiSelect.Enabled = false;
                        break;
                    case RadSlider radSlider:
                        radSlider.Enabled = false;
                        break;
                    case RadButton button:
                        button.Enabled = false;
                        break;
                    case CheckBox checkBox:
                    {
                        checkBox.Enabled = false;
                        break;
                    }
                    case RadTextBox radTextBox:
                    {
                        radTextBox.Enabled = true;
                        radTextBox.ReadOnly = true;
                        //radTextBox.ForeColor = Color.GhostWhite;
                        break;
                    }
                    case TextBox textBox:
                    {
                        textBox.Enabled = true;
                        textBox.ReadOnly = true;
                        //textBox.ForeColor = Color.GhostWhite;
                        break;
                    }
                }

                if (control.HasControls())
                {
                    foreach (Control nested in control.Controls)
                    {
                        SetControlReadOnly(nested);
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
        }

        // check upload file extenstion
        public static bool CheckExtenstion(string ext)
        {
            var result = true;
            IEnumerable extenstion = new[] { ".jpeg", ".jpg", ".png", ".gif", ".pdf", ".word", ".zip", ".tiff", ".tif" };
            foreach (var c in extenstion)
                if (!c.Equals(ext.ToLower()))
                    result = false;
                else
                    return true;
            return result;
        }
    }
}