namespace Models.Users.Validator;

// Represents user fields
[Flags]
public enum UserFields {
  FirstName = 1,
  LastName = 2,
  Username = 4,
  Password = 8,

  Create = FirstName | LastName | Username | Password,
  Put = FirstName | LastName | Username,
  All = Create
}
