using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookStore.Utility
{
    public class SerializeObject
    {
        public static string SerializeObjectToString<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static T DeSerializeObjectFromString<T>(string deSer, T toSerialize)
        {
            XmlSerializer deserializer = new XmlSerializer(toSerialize.GetType());
            using (TextReader tr = new StringReader(deSer))
            {
                T deserializedObj = (T)deserializer.Deserialize(tr);
                return deserializedObj;
            }
        }
    }
}
