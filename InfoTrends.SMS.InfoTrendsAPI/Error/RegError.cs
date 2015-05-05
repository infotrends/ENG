using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoTrendsAPI.Error
{
    public class RegError
    {
        public const string INVALID_EMAIL
            = "[Reg100]: Invalid Email";

        public const string EMAIL_ALREADY_EXISTS_IN_OUR_DATABASE
            = "[Reg110]: Email already exists in our database";

        public const string USERNAME_ALREADY_EXISTS_IN_OUR_DATABASE
            = "[Reg120]: Username already exists in our database";

        public const string ERROR_ADDING_PARTICIPANT
            = "[Reg130]: Error Adding Participant";
    }
}
