using FirebaseAdmin.Auth;
using System.Threading.Tasks;

namespace LocalThreads.Api.Services.Implementations.Shared
{
    public class FirebaseAuthService
    {
        public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
        {
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                return decodedToken;
            }
            catch
            {
                return null; // Token is invalid or expired
            }
        }
    }
}
