using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
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

                try
                {
                    await this.propertiesRepository.UpsertPropertiesAsync(result.Run, result.ValidProperties);
                    Log.Information("Successfully persisted {ValidCount} ingested properties.", result.ValidProperties.Count);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Failed to persist {ValidCount} ingested properties.");
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
