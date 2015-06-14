using System;
using System.Configuration;
using System.Xml.Serialization;

namespace CertificatesWatcher.Configuration
{
    [XmlRoot(SectionName)]
    public sealed class Config : ConfigurationSection
    {
        private const string SectionName = "cw";

        private static readonly Lazy<Config> _current =
            new Lazy<Config>(() => (Config)ConfigurationManager.GetSection(SectionName));


        [ConfigurationProperty("mails", IsRequired = true)]
        public string Mails { get; set; }

        [ConfigurationProperty("daysToExpiration", IsRequired = false, DefaultValue = 30)]
        public int DaysToExpiration { get; set; }

        public static Config Current
        {
            get { return _current.Value; }
        }
    }
}