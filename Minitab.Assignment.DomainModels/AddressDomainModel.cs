namespace Minitab.Assignment.DomainModels
{
    /// <summary>
    /// Address domain model. 
    /// The domain model matches the model the CRM accepts
    /// Although this mirrors the data contract, seperating the data contract from
    /// the domain allows changes to either that would not affect the other
    /// </summary>
    public class AddressDomainModel
    {
        public string Line1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
