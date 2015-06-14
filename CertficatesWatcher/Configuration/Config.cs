using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace CertificatesWatcher.Configuration
{
    [XmlRoot(SectionName)]
    public sealed class Config : IConfigurationSectionHandler
    {
        private const string SectionName = "cw";

        [XmlAttribute("mails")]
        public string Mails { get; set; }

        public static Config Current
        {
            get { return (Config) ConfigurationManager.GetSection(SectionName); }
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            var serializer = new XmlSerializer(GetType());
            var reader = new XmlNodeReader(section);
            return serializer.Deserialize(reader);
        }
    }
}