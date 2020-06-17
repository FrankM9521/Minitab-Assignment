using Minitab.Assignment.DomainModels;
using Minitab.Assignment.Entities;
using System;
using System.Linq;

namespace Minitab.Assignment.CrmStub.Mappers
{
    public static class CustomerMappers
    {
        public static CustomerEntity ToCustomerEntity(this CustomerDomainModel customerDomainModel)
        {
            var customerId = Guid.NewGuid();
            
            var entity =  new CustomerEntity
            {
                CustomerEmail = customerDomainModel.CustomerEmail,
                CustomerName = customerDomainModel.CustomerName,
                CustomerId = customerId
            };

            var address = customerDomainModel.Address?.ToAddressEntity();
            
            if (address != null)
            {
                entity.Address.Add(address);
            }
            
            return entity;
        }

        public static AddressEntity ToAddressEntity(this AddressDomainModel addressDomainModel)
        {
            return new AddressEntity
            {
                  City = addressDomainModel.City,
                  Country = addressDomainModel.Country,
                  Line1 = addressDomainModel.Line1,
                  PostalCode = addressDomainModel.PostalCode,
                  State = addressDomainModel.State,
                  AddressId = Guid.NewGuid()
            };
        }

        public static CustomerDomainModel ToCustomerDomainModel(this CustomerEntity customerEntity)
        {
            return new CustomerDomainModel
            {
                CustomerEmail = customerEntity.CustomerEmail,
                CustomerName = customerEntity.CustomerName,
                Address = customerEntity.Address.FirstOrDefault()?.ToAddressEntity()
            };
        }

        public static AddressDomainModel ToAddressEntity(this AddressEntity addressEntityl)
        {
            return new AddressDomainModel
            {
                City = addressEntityl.City,
                Country = addressEntityl.Country,
                Line1 = addressEntityl.Line1,
                PostalCode = addressEntityl.PostalCode,
                State = addressEntityl.State
            };
        }
    }
}
