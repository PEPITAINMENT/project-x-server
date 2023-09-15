namespace SongsProvider.UrlService
{
    public class UrlBuilder
    {
        private string _baseUrl { get; set; }

        private Dictionary<string, string> _parameters { get; set; } = new Dictionary<string, string>();

        public UrlBuilder(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public string GetUrl()
        {
            if (_parameters.Count > 0)
            {
                var url = $"{_baseUrl}?";
                foreach (var parameter in _parameters)
                {
                    url += $"{parameter.Key}={parameter.Value}&";
                }
                url = url.Remove(url.Length - 1);

                return url;
            }

            return _baseUrl;
        }

        public void AddParameter(string name, string value)
        {
            _parameters[name] = value;
        }

        public void DeleteParameter(string name)
        {
            _parameters.Remove(name);
        }
    }
}
