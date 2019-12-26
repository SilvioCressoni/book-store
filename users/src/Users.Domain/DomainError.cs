using static Users.Domain.Result;

namespace Users.Domain
{
    public static class DomainError
    {
        public static class PhoneError
        {
            public static ErrorResult MissingNumber { get; } = Fail("USR100", "Missing number phone");
            public static ErrorResult InvalidNumber { get; } = Fail("USR101", "Invalid number phone");
            public static ErrorResult NumberNotFound { get; } = Fail("USR102", "Number phone not found");
            public static ErrorResult NumberAlreadyExist { get; } = Fail("USR103", "Number already exist");
        }

        public static class AddressError
        {
            public static ErrorResult MissingLine { get; } = Fail("USR200", "Missing Line");
            public static ErrorResult InvalidLine { get; } = Fail("USR201", "Invalid line");
            public static ErrorResult InvalidNumber { get; } = Fail("USR202", "Invalid number");
            public static ErrorResult MissingPostCode { get; } = Fail("USR203", "Missing Post-Code");
            public static ErrorResult InvalidPostCode { get; } = Fail("USR204", "Invalid Post-Code");
            public static ErrorResult InvalidAddressId { get; } = Fail("USR205", "Invalid address Id");
            public static ErrorResult AddressNotFound { get; } = Fail("USR206", "Address not found");
            public static ErrorResult AddressAlreadyExist { get; } = Fail("USR207", "Address already exist");
            public static ErrorResult InvalidUserId { get; } = Fail("USR208", "Invalid Id, it should be a Guid or UUID");
        }

        public static class UserError
        {
            public static ErrorResult MissingEmail { get; } = Fail("USR000", "Missing email");
            public static ErrorResult InvalidEmail { get; } = Fail("USR001", "Invalid email");

            public static ErrorResult MissingFirstName { get; } = Fail("USR002", "Missing first name");
            public static ErrorResult InvalidFirstName { get; } = Fail("USR003", "Invalid first name length");

            public static ErrorResult MissingLastNames { get; } = Fail("USR002", "Missing last names");
            public static ErrorResult InvalidLastNames { get; } = Fail("USR003", "Invalid last names length");

            public static ErrorResult UserNotFound { get; } = Fail("USR005", "User not found");

            public static ErrorResult EmailAlreadyExist { get; } = Fail("USR006", "Email already exist");
            public static ErrorResult InvalidUserId { get; } = Fail("USR007", "Invalid User Id, it should be a Guid or UUID");
        }

        public static class GetError
        {
            public static ErrorResult InvalidTake { get; } = Fail("USR300", "Take should be great or equal to 0");
            public static ErrorResult InvalidSkip { get; } = Fail("USR301", "Skip should be great or equal to 0");
        }
    }
}
