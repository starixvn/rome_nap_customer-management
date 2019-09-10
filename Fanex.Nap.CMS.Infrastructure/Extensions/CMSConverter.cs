namespace Fanex.Nap.CMS.Infrastructure.Extensions
{
    using System;

    public interface CMSConverter
    {
        byte ToByte(object value);

        short ToInt16(object value);

        decimal ToDecimal(object value);

        bool ToBoolean(object value);

        string ToString(object value);

        double ToDouble(object value);

        DateTime ToDateTime(object value);

        int ToInt32(object value);

        int? ToInt32Nullable(object value);

        long ToInt64(object value);

        decimal Format(object value, int prec = 2);

        string ToStringFormat(object value, int prec = 2);
    }

    public class CMSConverterImplement: CMSConverter
    {
        public byte ToByte(object value)
        {
            byte val = 0;
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            byte.TryParse(value.ToString(), out val);
            return val;
        }

        public short ToInt16(object value)
        {
            short val = 0;
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            short.TryParse(value.ToString(), out val);
            return val;
        }

        public decimal ToDecimal(object value)
        {
            var val = 0.0m;
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return 0.0m;
            }

            decimal.TryParse(value.ToString(), out val);
            return val;
        }

        public bool ToBoolean(object value)
        {
            var val = false;
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return false;
            }

            if (value.ToString() == "1")
            {
                return true;
            }

            bool.TryParse(value.ToString(), out val);
            return val;
        }

        public string ToString(object value)
        {
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return string.Empty;
            }

            return Convert.ToString(value);
        }

        public double ToDouble(object value)
        {
            var val = 0.0d;
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return 0.0d;
            }

            double.TryParse(value.ToString(), out val);
            return val;
        }

        public DateTime ToDateTime(object value)
        {
            var val = DateTime.MinValue;
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return DateTime.MinValue;
            }

            DateTime.TryParse(value.ToString(), out val);
            return val;
        }

        public int ToInt32(object value)
        {
            var val = 0;
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return 0;
            }

            if (value is decimal || value is double)
            {
                val = Convert.ToInt32(value);
            }
            else if(value is bool)
            {
                return ((bool)value) ? 1 : 0;
            }
            else
            {
                int.TryParse(value.ToString(), out val);
            }

            return val;
        }

        public int? ToInt32Nullable(object value)
        {
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return null;
            }

            return ToInt32(value);
        }

        public long ToInt64(object value)
        {
            var val = 0L;
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return 0L;
            }

            if (value is decimal || value is double)
            {
                val = Convert.ToInt64(value);
            }
            else if (value is bool)
            {
                return ((bool)value) ? 1 : 0;
            }
            else
            {
                long.TryParse(value.ToString(), out val);
            }

            return val;
        }

        public long? ToInt64Nullable(object value)
        {
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return null;
            }

            return ToInt64(value);
        }

        public decimal Format(object value, int prec = 2)
        {
            var val = ToDecimal(value);
            return decimal.Round(val, prec);
        }

        public string ToStringFormat(object value, int prec = 2)
        {
            var val = ToDecimal(value);
            var ret = decimal.Round(val, prec);
            return ret.ToString("#,##0.########");
        }
    }
}
