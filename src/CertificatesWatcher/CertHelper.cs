using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace CertificatesWatcher
{
    public static class CertHelper
    {
        private static readonly Regex OrganizationRegex = new Regex(@"O *= *(.+?)($|,)", RegexOptions.Multiline);

        public static string GetOrganization(this X509Certificate cert)
        {
            var organizationMatch = OrganizationRegex.Match(cert.Subject);

            if (organizationMatch.Success)
            {
                return organizationMatch.Groups[1].Value;
            }

            return "unknown";
        }
    }
}
