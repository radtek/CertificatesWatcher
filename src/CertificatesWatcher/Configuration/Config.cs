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
            new Lazy<Config>(() => (Config)ConfigurationManager.GetSection(SectionName) ?? new Config());


        [ConfigurationProperty("mails", IsRequired = true)]
        public string Mails { get { return (string)this["mails"]; } }

        [ConfigurationProperty("daysToExpiration", IsRequired = false, DefaultValue = 30)]
        public int DaysToExpiration { get { return (int)this["daysToExpiration"]; } }

        public static Config Current
        {
            get { return _current.Value; }
        }
    }
}