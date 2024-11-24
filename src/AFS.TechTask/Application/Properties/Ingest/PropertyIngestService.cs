using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using Microsoft.Extensions.Options;
using Serilog;

namespace AFS.TechTask.Application.Properties.Ingest
{
    /// <summary>
    /// Service for ingesting <see cref="PropertyResponse"/> from an external source.
    /// </summary>
    public class PropertyIngestService : IPropertyIngestService
    {
        private readonly FeatureFlagsOptions featureFlags;

        private readonly IPropertyIngestClient client;

        /// <summary>
        /// Instantialises a new instance of the <see cref="PropertyIngestService"/> class.
        /// </summary>
        public PropertyIngestService(IPropertyIngestClient propertyIngestClient, IOptions<FeatureFlagsOptions> featureFlags)
        {
            this.client = propertyIngestClient;
            this.featureFlags = featureFlags.Value;
        }

        /// <summary>
        /// Retrieve and validate property records from an external source.
        /// </summary>
        /// <returns>A <see cref="PropertyIngestResult"/> containing validated and invalid property records.</returns>
        public async Task<PropertyIngestResult> IngestPropertiesAsync()
        {
            if (!featureFlags.EnablePropertyIngest)
            {
                return PropertyIngestResult.InvalidResult(DateTime.Now);
            }

            try
            {
                IReadOnlyCollection<PropertyResponse> importedProperties = await this.client.GetPropertiesAsync();

                List<Property> validProperties = new();
                List<InvalidPropertyIngest> invalidImports = new();

                foreach (PropertyResponse import in importedProperties)
                {
                    try
                    {
                        Property property = PropertyFactory.Create(import);
                        validProperties.Add(property);
                    }
                    catch (Exception e)
                    {
                        Log.Warning(e, "Property ingest failed to import Property: {Property}", import);
                        invalidImports.Add(new InvalidPropertyIngest(import, e));
                    }
                }

                bool succeeded = importedProperties.Any() && validProperties.Any();

                return new PropertyIngestResult(DateTime.Now, succeeded, validProperties, invalidImports); // TODO: Inject a time provider
            }
            catch (Exception e) 
            {
                Log.Error(e, "Failed to ingest properties.");
                return PropertyIngestResult.InvalidResult(DateTime.Now);
            }
        }
    }
}
