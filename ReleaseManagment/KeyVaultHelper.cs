using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseManagment
{
    public class KeyVaultHelper
    {
        public static string GetTestResultPat(string secretUri)
        {
            var tokenProvider = new AzureServiceTokenProvider("AuthenticateAs=User");
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
            var secret = kv.GetSecretAsync(secretUri).Result;
            return secret.Value;
        }
    }
}
