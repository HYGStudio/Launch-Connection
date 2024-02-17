using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Launch_Connection.Services;

public class Secure
{
    public class Des
    {
        [Obsolete("Obsolete")]
        public static JObject Encrypt(string str, string key)
        {
            try
            {
                var des = new DESCryptoServiceProvider();
                var inputByteArray = Encoding.Default.GetBytes(str);
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(key);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return JObject.Parse(JsonConvert.SerializeObject(new
                {
                    status = true,
                    data = Convert.ToBase64String(ms.ToArray())
                }));
            }
            catch (Exception error)
            {
                return JObject.Parse(JsonConvert.SerializeObject(new
                {
                    status = false,
                    data = error.Message
                }));
            }
        }

        [Obsolete("Obsolete")]
        public static JObject Decrypt(string str, string key)
        {
            try
            {
                var des = new DESCryptoServiceProvider();
                var inputByteArray = Convert.FromBase64String(str);
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(key);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return JObject.Parse(JsonConvert.SerializeObject(new
                {
                    status = true,
                    data = Encoding.Default.GetString(ms.ToArray())
                }));
            }
            catch (Exception error)
            {
                return JObject.Parse(JsonConvert.SerializeObject(new
                {
                    status = false,
                    data = error.Message
                }));
            }
        }
    }
}