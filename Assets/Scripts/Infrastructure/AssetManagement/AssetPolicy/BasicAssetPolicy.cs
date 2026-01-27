namespace FormForge.AssetManagement.AssetPolicy
{
    /// <inheritdoc />
    public class BasicAssetPolicy : IAssetPolicy
    {
        public string Address { get; }
        
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAssetPolicy"/> class 
        /// with the specified asset address.
        /// </summary>
        /// <param name="address">The address of the asset.</param>
        public BasicAssetPolicy(string address)
        {
            Address = address;
            Id = Address.GetHashCode();
        }
    }
}