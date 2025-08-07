using System;
using System.IO;
using System.Xml.Serialization;

namespace Fanex.Nap.CMS.Infrastructure.Models
{
    public class AuthInfo
    {
        private int _userId;
        private int _onuserId;

        public int UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "userid");
                this._userId = value;
            }
        }

        public int OnUserId
        {
            get
            {
                return this._onuserId;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "onUserid");
                this._onuserId = value;
            }
        }

        public static AuthInfo CreateUserData(string authInfoXml)
        {
            return (AuthInfo)new XmlSerializer(typeof(AuthInfo)).Deserialize((TextReader)new StringReader(authInfoXml));
        }

        public string ToXml()
        {
            string str = string.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                new XmlSerializer(typeof(AuthInfo)).Serialize((TextWriter)stringWriter, (object)this);
                stringWriter.Close();
                str = stringWriter.ToString();
            }
            return str;
        }
    }
}
