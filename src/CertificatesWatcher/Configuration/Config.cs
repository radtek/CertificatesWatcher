using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        [ConfigurationProperty("ignoringCertificates")]
        public CertificateElementCollection IgnoringCertificates
        {
            get { return (CertificateElementCollection)this["ignoringCertificates"]; }
        }

        [ConfigurationProperty("watchingStores")]
        public StoreElementCollection WatchingStores
        {
            get { return (StoreElementCollection)this["watchingStores"]; }
        }

        public static Config Current
        {
            get { return _current.Value; }
        }
    }

    public class CertificateElement : ConfigurationElement
    {
        [ConfigurationProperty("thumbprint", IsKey = true, IsRequired = true)]
        public string Thumbprint
        {
            get { return (string)this["thumbprint"]; }
        }

        [ConfigurationProperty("comment")]
        public string Comment
        {
            get { return (string)this["comment"]; }
        }
    }

    public class CertificateElementCollection : ConfigurationElementCollection, IEnumerable<CertificateElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CertificateElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CertificateElement)element).Thumbprint;
        }

        public new IEnumerator<CertificateElement> GetEnumerator()
        {
            return new Enumerator<CertificateElement>(base.GetEnumerator());
        }
    }

    public class StoreElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public StoreName Name
        {
            get { return (StoreName)this["name"]; }
        }
    }

    public class StoreElementCollection : ConfigurationElementCollection, IEnumerable<StoreElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StoreElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StoreElement)element).Name;
        }

        public new IEnumerator<StoreElement> GetEnumerator()
        {
            return new Enumerator<StoreElement>(base.GetEnumerator());
        }
    }

    public class Enumerator<T>: IEnumerator<T>
    {
        private readonly IEnumerator _enumerator;

        public Enumerator(IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        public void Dispose()
        { }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public T Current { get { return (T)_enumerator.Current; } }

        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }
    }
}