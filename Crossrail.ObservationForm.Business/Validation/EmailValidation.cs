using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Crossrail.ObservationForm.Business.Validation
{
    public class EmailValidation
    {
        public static bool IsValidEmail(string email)
        {
            MailAddress mailAddress;
            return TryParseEmail(email, out mailAddress);
        }

        public static bool TryParseEmail(string email, out MailAddress mailAddress)
        {
            mailAddress = null;

            try
            {
                mailAddress = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                //Ignore exceptions for mail address parsing.
                return false;
            }
        }
    }
}
