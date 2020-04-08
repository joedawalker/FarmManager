namespace EquipmentApi.Classes
{
    public enum UserValidationFailureType
    {
        FirstNameMissing = 0,
        LastNameMissing = 1,
        EmailMissing = 2,
        EmailBadFormat = 3,
        PasswordMissing = 4,
        PasswordBadFormat = 5,
        UserNotFound = 6,
        InvalidId = 7,
        GenericError = 8,
        EmailAlreadyInUse = 9,
        UserIsNull = 10
    }
}
