using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CoreEditor.Utilities
{
    public static class Serializer
    {
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Create);
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(fs, instance);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        internal static T? FromFile<T>(string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Open);
                var serializer = new DataContractSerializer(typeof(T));
                return (T?)serializer.ReadObject(fs);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return default;
            }
        }
    }
}
