using System;
namespace Publisher.Domain
{
    public static class DomainError
    {
        public static class PublisherError
        {
            public static ErrorResult NotFound { get; } = Result.Fail("PSH001", "Publisher not found");

            public static ErrorResult NameIsEmpty { get; } = Result.Fail("PSH002", "Name is empty");

            public static ErrorResult InvalidName { get; } = Result.Fail("PSH003", "Name is invalid");
            
        }
    }
}
