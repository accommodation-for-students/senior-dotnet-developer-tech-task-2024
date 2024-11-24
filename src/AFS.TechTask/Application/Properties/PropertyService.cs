using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using Microsoft.Extensions.Options;
using Serilog;

namespace AFS.TechTask.Application.Properties
{
    /// <summary>
    /// Application service for interacting with properties
    /// </summary>
    public class PropertyService : IPropertyService
    {
        private readonly FeatureFlagsOptions featureFlags;

        private readonly IPropertyIngestService propertyIngest;
        private readonly IPropertiesRepository propertiesRepository;

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyService"/> class.
        /// </summary>
        public PropertyService(IPropertyIngestService propertyIngestService, IPropertiesRepository propertiesRepository, IOptions<FeatureFlagsOptions> featureFlags) 
        {
            this.propertyIngest = propertyIngestService;
            this.propertiesRepository = propertiesRepository;
            this.featureFlags = featureFlags.Value;
        }

        /// <summary>
        /// Ingest properties from an external source and persist if valid.
        /// </summary>
        public async Task RunIngestPropertiesJobAsync()
        { 
            if (!featureFlags.EnablePropertyIngest)
            {
                return;
            }

            Log.Information("Starting ingest properties job.");

            PropertyIngestResult result = await this.propertyIngest.IngestPropertiesAsync();

            if (result.Success)
            {
                Log.Information("Successfully validated {ValidCount} properties, validation failed for: {InvalidCount} properties.", result.ValidProperties.Count, result.InvalidProperties.Count);

                int index = 1;
                foreach (Property property in result.ValidProperties)
                {
                    try
                    {
                        int propertyId = await this.propertiesRepository.InsertPropertyAsync(result.Run, property);
                        Log.Information("Successfully persisted property number {Index} with Id: {PropertyId}.", index, propertyId);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Failed to persist property number {Index}.", index);
                    }

                    index += 1;
                }
            }
            else
            {
                Log.Warning("Failed to ingest properties, validation failed for: {InvalidCount}.", result.InvalidProperties.Count);
                // Generate an error report, etc. 
            }
        }
    }
}
