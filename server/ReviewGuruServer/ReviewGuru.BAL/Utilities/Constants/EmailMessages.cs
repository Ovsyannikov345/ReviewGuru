using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Utilities.Constants
{
    public static class EmailMessages
    {
        public static readonly string WelcomeMessage = $"Dear user, we wanted to take a moment to personally thank you for joining our media review service.\n" +
                                                       "\nBest regards, Review Guru team.";

        private static readonly string _emailVerificationMessage = "You've just created an account on Review Guru.\n" +
                                                                   "Please verify your email address by clicking the link below\n" +
                                                                   "{0}\n" +
                                                                   "\nBest regards, Review Guru team.";

        public static string GetVerificationMessage(string verificationLink)
        {
            return string.Format(_emailVerificationMessage, verificationLink);
        }
    }
}
