using System;

namespace Minitab.Assignment.DataContracts
{
    /// <summary>
    /// Data Contract for a Customet
    /// </summary>
    [Serializable]
    public class Customer
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public Address Address { get; set; }
    }
}
