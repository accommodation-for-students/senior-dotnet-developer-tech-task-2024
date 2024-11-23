namespace AFS.TechTask.Common
{
    /// <summary>
    /// Configurable feature flag options
    /// </summary>
    public class FeatureFlagsOptions
    {
        public const string FeatureFlags = "FeatureFlags";

        /// <summary>
        /// Whether the property ingest feature is enabled.
        /// </summary>
        public bool EnablePropertyIngest { get; set; }
    }
}
