using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Xunit.Abstractions;

namespace Tests
{
    public class CertUtilTests
    {
        private const string _password = "P@55w0rd";

        [Fact(Skip = "not a real test")]
        //[Fact]
        public void GenerateRsaPfx()
        {
            using var rsa = RSA.Create(2048);

            var csr = new CertificateRequest("cn=localhost", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            using var cert = csr.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes("localhost.pfx", cert.Export(X509ContentType.Pfx, _password));
        }

        [Fact(Skip = "not a real test")]
        //[Fact]
        public void GenerateEcdsaPfx()
        {
            using var ecdsa = ECDsa.Create();

            var csr = new CertificateRequest("cn=localhost", ecdsa, HashAlgorithmName.SHA256);

            using var cert = csr.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes("localhost.pfx", cert.Export(X509ContentType.Pfx, _password));
        }

        private readonly ITestOutputHelper _output;

        public CertUtilTests(ITestOutputHelper output)
        {
            _output = output;
        }

    }
}
