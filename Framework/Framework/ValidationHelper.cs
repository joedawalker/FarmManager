using System;

namespace Framework
{
    public class ValidationHelper
    {
        public static bool IsValidId( int id )
        {
            return id > 0;
        }
    }
}
