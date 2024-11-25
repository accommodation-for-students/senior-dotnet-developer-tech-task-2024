using Microsoft.Extensions.Options;
using Serilog;
using System.Text.Json;

namespace AFS.TechTask.Application.Properties.Ingest
{
    /// <summary>
    /// Client for fetching <see cref="PropertyResponse"/> records from an external source.
    /// </summary>
    public class PropertyIngestClient : IPropertyIngestClient
    {
        private readonly PropertyIngestOptions options;

        private readonly HttpClient client;

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyIngestClient"/> class.
        /// </summary>
        public PropertyIngestClient(IOptions<PropertyIngestOptions> options)
        {
            this.options = options.Value;
            this.client = new HttpClient();
        }

        /// <summary>
        /// Retrieve <see cref="PropertyResponse"/> records from an external source.
        /// </summary>
        /// <returns>A collection of <see cref="PropertyResponse"/> records.</returns>
        public async Task<IReadOnlyCollection<PropertyResponse>> GetPropertiesAsync()
        {
            string url = $"{this.options.BaseURL}/api/properties";

            HttpResponseMessage response = await this.client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                IReadOnlyCollection<PropertyResponse> properties = JsonSerializer.Deserialize<IReadOnlyCollection<PropertyResponse>>(content);
                return properties;
            }
            else
            {
                Log.Error("Call to {Url} failed with status code: {StatusCode}", url, response.StatusCode);
                throw new Exception($"Failed to retrieve properties from {url}, status code: {response.StatusCode}");
            }
        }
    }
}
