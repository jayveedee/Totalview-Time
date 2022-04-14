using IdentityModel.OidcClient.Browser;
using System.Diagnostics;

namespace Totalview_Time_Smartclient.MVVM.Model.Services.Authentication
{
    public class AuthenticationBrowser : IdentityModel.OidcClient.Browser.IBrowser
    {
        public AuthenticationBrowser() { }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(options.EndUrl));

                return new BrowserResult
                {
                    Response = ToRawIdentityURL(options.EndUrl, result)
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                return new BrowserResult()
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.ToString()
                };
            }
        }

        private string ToRawIdentityURL(string redirectURL, WebAuthenticatorResult result)
        {
            var parameters = result.Properties.Select(pair => $"{pair.Key}={pair.Value}");
            var values = string.Join("&", parameters);

            return $"{redirectURL}#{values}";
        }
    }

}

