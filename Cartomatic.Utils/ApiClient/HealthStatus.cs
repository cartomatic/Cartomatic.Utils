namespace Cartomatic.Utils.ApiClient
{
    /// <summary>
    /// health status
    /// </summary>
    public enum HealthStatus
    {
        /// <summary>
        /// client is ok and responds properly to both standard calls and health check calls
        /// </summary>
        Healthy,
        
        /// <summary>
        /// client is unhealthy - the most recent health check returned non-successful result or timed out
        /// </summary>
        Unhealthy,
        
        /// <summary>
        /// Cient has been marked as dead; usually when it was not possible to obtain a success result from a healthcheck 
        /// </summary>
        Dead
    }
}
