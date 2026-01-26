namespace FormForge.Core.Services
{
    /// <summary>
    /// Represents the service life space.
    /// </summary>
    public enum ServiceLifespan
    {
        /// <summary>
        /// <para>Represents a singleton service.</para>
        /// <para>The instance of a lazy singleton service is created when it is first requested.</para>
        /// </summary>
        /// <remarks>
        /// A singleton service is created only once and the same instance is used throughout the application.
        /// </remarks>
        LazySingleton,
        /// <summary>
        /// Represents a transient service.
        /// </summary>
        /// <remarks>
        /// A transient service is created each time it is requested.
        /// </remarks>
        Transient,
        /// <summary>
        /// Represents a singleton service.
        /// </summary>
        /// <remarks>
        /// A singleton service is created only once and the same instance is used throughout the application.
        /// </remarks>
        Singleton
    }
}
