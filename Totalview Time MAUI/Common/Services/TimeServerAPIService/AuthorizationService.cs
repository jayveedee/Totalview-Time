using System.IdentityModel.Tokens.Jwt;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using TimeServerAPI;
using Totalview_Time_MAUI.Common.Model;

namespace Totalview_Time_MAUI.Common.Services.TimeServerAPIService;

internal class AuthorizationService
{
    public TimeServerAPIService CreateTimeAPIClient(string accessToken, string serverAddress = default)
    {
        if (accessToken == null) throw new ArgumentNullException(nameof(accessToken));

        StoreBasicUserDetails(accessToken);

        WCFDatabaseClient client = new WCFDatabaseClient(WCFDatabaseClient.EndpointConfiguration.BasicHttpBinding_IWCFDatabase);
        ClientBase<IWCFDatabase> clientBase = client;
        clientBase.Endpoint.EndpointBehaviors.Add(new AuthorizationBehavior(accessToken));

        if (!string.IsNullOrEmpty(serverAddress))
        {
            clientBase.Endpoint.Address = new EndpointAddress(serverAddress);
        }

        var timeServerAPIService = new TimeServerAPIService();
        timeServerAPIService.Initialize(client);
        return timeServerAPIService;
    }

    private void StoreBasicUserDetails(string accessToken)
    {
        var formattedAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        var userId = int.Parse(formattedAccessToken.Subject);
        var claims = formattedAccessToken.Claims;

        var name = "";
        foreach (var claim in claims.Where(claim => claim.Type == "name"))
        {
            name = claim.Value;
            break;
        }

        var storage = StorageService.Instance.LoadStorage();
        storage.User = new User(userId, name);
        StorageService.Instance.SaveStorage(storage);
    }
}

internal class AuthorizationClientMessageInspector : IClientMessageInspector
{
    private const string _accessTokenHeaderName = "Token";
    private readonly string _accessToken;

    public AuthorizationClientMessageInspector(string accessToken)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
    }

    public void AfterReceiveReply(ref Message reply, object correlationState)
    {
        // No implementation necessary
    }

    public object BeforeSendRequest(ref Message request, IClientChannel channel)
    {
        if (String.IsNullOrEmpty(_accessToken))
        {
            return null;
        }

        request.Headers.Add(MessageHeader.CreateHeader(_accessTokenHeaderName, "", _accessToken));

        return null;
    }
}

internal class AuthorizationBehavior : IEndpointBehavior
{
    private readonly string _accessToken;
    public AuthorizationBehavior(string accessToken)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
        // No implementation necessary  
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(new AuthorizationClientMessageInspector(_accessToken));
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
        // No implementation necessary  
    }

    public void Validate(ServiceEndpoint endpoint)
    {
        // No implementation necessary  
    }
}