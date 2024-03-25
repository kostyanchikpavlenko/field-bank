using Newtonsoft.Json;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Extensions.Web.Core.Extensions
{
    public static class ObjectExtensions
    {
        private const string One = "1";
        private const string True = "true";
        private const string Yes = "yes";


       public static string ToStringSafe(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            return value.ToString();
        }

        public static short ToInt16(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            return short.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static short? ToNullInt16(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return short.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static int ToInt32(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            return int.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static int? ToNullInt32(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return int.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static long ToInt64(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            } 

            return long.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static long? ToNullInt64(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return long.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static bool ToBoolean(this object value)
        {
            return value.ToNullableBoolean() ?? false;
        }

        public static bool? ToNullableBoolean(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            var convertedValue = value.ToString().ToLower();

            if (convertedValue.SameAs(One) 
                || convertedValue.SameAs(True)
                || convertedValue.SameAs(Yes))
            {
                return true;
            }

            return false;
        }

        public static char ToChar(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            return char.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static char? ToNullableChar(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return char.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static double ToDouble(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            return double.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static DateTime ToDateTime(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            return DateTime.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static DateTime? ToNullableDateTime(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return DateTime.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static byte ToByte(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            return byte.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static decimal ToDecimal(this object value)
        {
            if(value == null || value == DBNull.Value)
            {
                return default;
            }

            return decimal.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static decimal? ToNullableDecimal(this object value)
        {
            if(value == null || value == DBNull.Value)
            {
                return null;
            }

            return decimal.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static Guid ToGuid(this object value)
        {
            if(value == null || value == DBNull.Value)
            {
                return default;
            }

            return Guid.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static Guid? ToNullableGuid(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return Guid.TryParse(value.ToString(), out var convertedValue) ? convertedValue : default;
        }

        public static byte[] ToByteArray(this object value)
        {
            if(value == null || value == DBNull.Value)
            {
                return default;
            }

            byte[] convertedValue;

            try
            {
                convertedValue = Nullable.GetUnderlyingType(typeof(byte[])) == null
                  ? (byte[])Convert.ChangeType(value, typeof(byte[]))
                  : (byte[])Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(byte[])));
            }
            catch (Exception)
            {
                convertedValue = default;
            }

            return convertedValue;
        }

        public static T ConvertTo<T>(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            T convertedValue;

            try
            {
                convertedValue = Nullable.GetUnderlyingType(typeof(T)) == null
                  ? (T)Convert.ChangeType(value, typeof(T))
                  : (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(T)));
            }
            catch (Exception)
            {
                convertedValue = default;
            }

            return convertedValue;
        }

        public static T ConvertToEnum<T>(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            T convertedValue;

            try
            {
                convertedValue = (T)Enum.Parse(typeof(T), value.ToString(), true);
            }
            catch (Exception)
            {
                convertedValue = default;
            }

            return convertedValue;
        }

        public static object GetDbNullOrValue<T>(this T value, bool useDbNullIfDefault = true)
        {
            if (value == null || (useDbNullIfDefault && value.Equals(default(T))))
            {
                return DBNull.Value;
            }

            return value;
        }

        public static byte[] ToSha512(this object @object)
        {
            var @string = JsonConvert.SerializeObject(@object, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore

            });

            var data = Encoding.Unicode.GetBytes(@string.ToUpperInvariant());

            var shaManager = new SHA512Managed();
            var result = shaManager.ComputeHash(data);

            return result;
        }

        public static string ToJson(this object @object, NullValueHandling valueHandling = NullValueHandling.Ignore)
        {
            var @string = JsonConvert.SerializeObject(@object, new JsonSerializerSettings
            {
                NullValueHandling = valueHandling
            });

            return @string;
        }

        public static long GetCount(this object instance)
        {
            switch (instance)
            {
                case IDictionary dictionary:
                    return dictionary.Count;
                case IEnumerable<object> enumerable:
                    return enumerable.ToList().Count();
                case object _:
                    return 1;
                default:
                    return default;
            }
        }
    }
}