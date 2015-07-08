using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CertificatesWatcher.Configuration;

namespace CertificatesWatcher
{
    public class CertificateWatcher
    {
        private readonly ICollection<X509Store> _stores;
        private readonly HashSet<string> _ignoringThumbprints;

        public CertificateWatcher()
        {
            _stores = Config.Current.WatchingStores.Select(store => new X509Store(store.Name, StoreLocation.LocalMachine)).ToArray();
            _ignoringThumbprints = new HashSet<string>(Config.Current.IgnoringCertificates.Select(cert => cert.Thumbprint), StringComparer.InvariantCultureIgnoreCase); 
        }

        private sealed class StoresWraper : IDisposable
        {
            private readonly ICollection<X509Store> _stores;

            public StoresWraper(ICollection<X509Store> stores)
            {
                _stores = stores;
                OpenStores();
            }

            public IEnumerable<X509Certificate2> Certificates
            {
                get
                {
                    IEnumerable<X509Certificate2> certificates = Enumerable.Empty<X509Certificate2>();

                    return _stores.Aggregate(certificates, (current, store) => current.Union(store.Certificates.Cast<X509Certificate2>()));
                }
            }

            private void OpenStores()
            {
                foreach (var store in _stores)
                {
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                }
            }

            private void CloseStores()
            {
                foreach (var store in _stores)
                {
                    store.Close();
                }
            }

            public void Dispose()
            {
                CloseStores();
            }
        }


        public ICollection<X509Certificate2> GetExpiringCertificates(TimeSpan beforeExpiration)
        {
            using (var wrapper = new StoresWraper(_stores))
            {
                return (from cert in wrapper.Certificates
                        where (cert.NotAfter - DateTime.Now) < beforeExpiration &&
                              !_ignoringThumbprints.Contains(cert.Thumbprint)
                        orderby cert.NotAfter
                        select cert).ToArray();
            }
        }
    }
}
