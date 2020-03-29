using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Todo.Gravatar
{
    /// <summary>
    /// Gravatar client which uses Http protocol.
    /// </summary>
    public class HttpGravatarClient : IGravatarClient
    {
        private HttpClient _httpClient;

        /// <summary>
        /// Constructs HttpGravatarClient object.
        /// </summary>
        /// <param name="client">HttpClient to inject.</param>
        public HttpGravatarClient(HttpClient client)
        {
            _httpClient = client;
        }

        /// <inheritdoc/>
        public async Task<string> GetNameAsync(string email)
        {
            string hashEmail = GravatarHasher.GetHash(email);
            HttpResponseMessage response = await _httpClient.GetAsync(hashEmail);
            HttpResponseMessage userData = await _httpClient.GetAsync(response.RequestMessage.RequestUri);
            string pageContent = await userData.Content.ReadAsStringAsync();
            string userName = ExtractUserName(pageContent);
            return userName;
        }

        private string ExtractUserName(string pageContent)
        {
            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContent);
            HtmlNode value = pageDocument.DocumentNode.SelectSingleNode("(//h2[contains(@class,'fn')])[1]");
            return value.InnerText;
        }
    }
}
