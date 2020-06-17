namespace Minitab.Assignment.DomainModels
{
    /// <summary>
    /// Customer domain model. 
    /// The domain model matches the model the CRM accepts
    /// Although this mirrors the data contract, seperating the data contract from
    /// the domain allows changes to either that would not affect the other
    /// </summary>
    public class CustomerDomainModel
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public AddressDomainModel Address { get; set; }
    }
}
