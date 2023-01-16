using System.IO;

namespace GoogleAPI_Library.Models
{
    public class GoogleCredentialOptions
    {
        public string ApplicationName { get; set; }
        public string[] Scopes { get; set; }
    }

    public class GoogleCredentialOptions_Stream : GoogleCredentialOptions
    {
        public Stream ClientSecretStream { get; set; }
    }

    public class GoogleCredentialOptions_FilePath : GoogleCredentialOptions
    {
        public string ClientSecretPath { get; set; }
    }
   
}
