using Newtonsoft.Json;
using SongsProvider.Spotify;
using System.Collections.Generic;
using System.Net.Http;

namespace Server.OAuthService
{
    public class OAuthService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _endpointUrl;
        private const string _grantType = "client_credentials";

        public OAuthService(string clientId, string clientSecret, string endpointUrl)
        { 
            _clientId = clientId;
            _clientSecret = clientSecret;
            _endpointUrl = endpointUrl;
        }

        public Token GetToken()
        {
            var httpClient = new HttpClient();
            var form = new Dictionary<string, string>() {
                { "grant_type", _grantType },
                { "client_id", _clientId },
                { "client_secret", _clientSecret }
            };
            var responce = httpClient.PostAsync(_endpointUrl, new FormUrlEncodedContent(form)).Result;
            var jsonContent = responce.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<Token>(jsonContent);

            return token;
        }
    }
}
