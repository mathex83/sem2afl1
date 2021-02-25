using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClassLibrary
{
    public static class Tools
    {
        /// <summary> 
        /// Udfører en deep copy af et objekt. 
        /// </summary> 
        /// <typeparam name="T">Typen af det objekt, der skal kopieres.</typeparam> 
        /// <param name="obj">Det objekt der skal kopieres.</param> 
        /// <returns>En deep copy af objektet obj.</returns> 
        public static T Clone<T>(T obj)
        {
            if (!typeof(T).IsSerializable) throw new Exception("The type must be serializable...");
            if (Object.ReferenceEquals(obj, null)) return default(T);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
