namespace FormForge.AssetManagement.AssetPolicy
{
    /// <summary>
    /// Defines an asset policy that provides identification and addressing 
    /// for assets managed by the asset management system.
    /// </summary>
    public interface IAssetPolicy
    {
        /// <summary>
        /// Gets the address of the asset, typically used for locating or loading the asset.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets the unique identifier for the asset.
        /// </summary>
        int Id { get; }
    }
}