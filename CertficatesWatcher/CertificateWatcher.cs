using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CertficatesWatcher
{
    public class CertificateWatcher
    {
        private readonly ICollection<X509Store> _stores = new List<X509Store>
            {
                new X509Store(StoreName.My, StoreLocation.LocalMachine),
                new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine)
            };

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


        public ICollection<X509Certificate2> GetExpiringCertificates()
        {
            using (var wrapper = new StoresWraper(_stores))
            {
                return
                    wrapper.Certificates.Where(cert => (cert.NotAfter - DateTime.Now) < TimeSpan.FromDays(30)).OrderBy(cert=>cert.NotAfter).ToList();
            }
        }
    }
}
