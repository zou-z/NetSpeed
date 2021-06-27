using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace NetSpeed.Util
{
    internal abstract class AppSettingBase<T> where T : class, new()
    {
        /// <summary>
        /// 设置文件的路径
        /// </summary>
        protected static string SettingFilePath;

        /// <summary>
        /// 用于对设置文件进行读写的实例
        /// </summary>
        protected static T Instance { get; set; }

        protected static T LoadInstance()
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return (jsonSerializer.Deserialize<T>(LoadFile(SettingFilePath) ?? "")) ?? new T();
        }

        protected static void SaveInstance(T t)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            SaveFile(jsonSerializer.Serialize(t), SettingFilePath);
        }

        private static string LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] data = new byte[fs.Length];
                return fs.Read(data, 0, data.Length) == 0 ? null : Encoding.UTF8.GetString(data);
            }
        }

        private static void SaveFile(string data, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                byte[] array = Encoding.UTF8.GetBytes(data);
                fs.Write(array, 0, array.Length);
                fs.Close();
            }
        }
    }
}
