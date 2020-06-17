Minitab Assignmennt - Address Validation

The 3 automated tests are in Minitab.Assignmnent.Api.Tests

The file containing the test is CustomerControllerTests

WhenACustomerWithAValidAddressIsAdded_ShouldInsertTheCustomerAndAddress()
-- Validates that both the Customer and the Address have been added

WhenACustomerWithnInValidAddressIsAdded_ShouldOnlyInsertTheCustomer()
-- Validates that only the Customer has been added

WhenOneCustomerHasInvalidAddressAndAnotherHasValid_ShouldOnlyInsertTheValidCustomersAddress()
-- Validates that the Customer with the valid Address has been fullly added and that the customer with the
    invalid address did not have the address added

Since any error conditions were being ignored, the only two test cases are with a valid address and with an invalid address
The tests use an in-memory SQL Lite database

Swagger is enabled and when run the application opens directly to it
================================================================================
Solution Structure

Minitab.Assignment.Api
    Web API for handling post Customer requests. A Get by email and Clear endpoints were added to validate end to end test results

Minitab.Assignment.Services
    Service layer that is responsible for address validation. The address validation service is injected into the AddressService using
    an IAddressValidation interface to make changing address validation services easier

Minitab.Assignment.CrmStub
    Mock CRM which is an EF core Repository. SQL Lite is being used as the database provider

For the 3 layers, different classes were used for each layer
   Minitab.Assignment.DataContracts - Web API data contracts
   Minitab.Assignment.DomainModels - Models used by the service layer
   Minitab.Assignment.Entities - Used in the repository layer

   At each layer, models were mapped using extension methods. I prefer this to automapper

   The servce layer has some tests,  Minitab.Assignment.Services.Tests, which validate adddresses and XMLSerialization.
   Although they are valid tests, mosts were used during development

   Any questions or issues
   Frank.Malinowski.Net@gmail.com