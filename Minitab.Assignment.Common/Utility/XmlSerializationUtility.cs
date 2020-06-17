using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Minitab.Assignment.Common.Utility
{
    public static class XmlSerializationUtility
    {
        public static string GetXmlString<T>(T obj)
        {
            var xml = "";
            using (var stringwriter = new StringWriter())
            {
                Serialize(obj, stringwriter);
                xml = stringwriter.ToString();
            }
            return xml;
        }

        public static T Deserialize<T>(Stream stream) where T : new()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        private static void Serialize<T>(T obj, TextWriter writer)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(xmlWriter, obj, names);
        }
    }
}
