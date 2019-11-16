using static Users.Domain.Result;

namespace Users.Domain
{
    public static class DomainError
    {
        public static class PhoneError
        {
            public static ErrorResult InvalidNumber { get; } = Fail("USR100", "Invalid number phone");
            public static ErrorResult NumberNotFound { get; } = Fail("USR101", "Number phone not found");
            public static ErrorResult NumberAlreadyExist { get; } = Fail("USR102", "Number already exist");
        }

        public static class AddressError
        {
            public static ErrorResult InvalidLine { get; } = Fail("USR200", "Invalid line");
            public static ErrorResult InvalidNumber { get; } = Fail("USR201", "Invalid number");
            public static ErrorResult InvalidPostCode { get; } = Fail("USR202", "Invalid post code");
            public static ErrorResult InvalidAddressId { get; } = Fail("USR203", "Invalid address Id");
            public static ErrorResult AddressNotFound { get; } = Fail("USR204", "Address not found");
            public static ErrorResult AddressAlreadyExist { get; } = Fail("USR205", "Address already exist");
        }

        public static class UserError
        {
            public static ErrorResult MissingEmail { get; } = Fail("USR000", "Missing email");
            public static ErrorResult InvalidEmail { get; } = Fail("USR001", "Invalid email");

            public static ErrorResult MissingFirstName { get; } = Fail("USR002", "Missing first name");
            public static ErrorResult InvalidFirstName { get; } = Fail("USR003", "Invalid first name length");

            public static ErrorResult MissingLastNames { get; } = Fail("USR002", "Missing last names");
            public static ErrorResult InvalidLastNames { get; } = Fail("USR003", "Invalid last names length");

            public static ErrorResult InvalidBirthDay { get; } = Fail("USR004", "Invalid birth day");

            public static ErrorResult UserNotFound { get; } = Fail("USR005", "User not found");

            public static ErrorResult EmailAlreadyExist { get; } = Fail("USR006", "Email already exist");
        }
    }
}
