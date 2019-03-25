using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AillieoUtils
{
    public static class SerializeHelper
    {

        public static bool DeserializeBytesToData<T>(string filename, out T obj)
        {
            if(!FileUtils.FileExists(filename))
            {
                obj = default(T);
                return false;
            }
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            obj = (T)formatter.Deserialize(stream);
            stream.Close();
            return true;
        }


        public static bool SerializeDataToBytes<T>(T obj, string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
            return true;
        }


        public static byte[] SerializeDataToBytes<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            byte[] byteArray = stream.ToArray();
            stream.Close();
            return byteArray;
        }


        public static T DeserializeBytesToData<T>(byte[] byteArray)
        {
            MemoryStream stream = new MemoryStream(byteArray);
            IFormatter formatter = new BinaryFormatter();
            T t = (T)formatter.Deserialize(stream);
            stream.Close();
            return t;
        }
    }

}