using System;
using System.Collections.Generic;
using System.Text;

namespace UserService
{
    public enum UserValidationFailureType
    {
        FirstNameMissing = 0,
        LastNameMissing = 1,
        EmailMissing = 2,
        EmailBadFormat = 3,
        PasswordMissing = 4,
        PasswordBadFormat = 5
    }
}
