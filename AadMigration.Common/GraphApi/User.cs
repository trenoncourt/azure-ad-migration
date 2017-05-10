using System.Collections.Generic;

namespace AadMigration.Common.GraphApi
{
    public class User
    {
        public bool AccountEnabled { get; set; }
        public IEnumerable<Signinname> SignInNames { get; set; }
        public string CreationType { get; set; }
        public string DisplayName { get; set; }
        public string MailNickname { get; set; }
        public Passwordprofile PasswordProfile { get; set; }
        public string UserPrincipalName { get; set; }
    }

    public class Passwordprofile
    {
        public bool ForceChangePasswordNextLogin { get; set; }
        public string Password { get; set; }
    }

    public class Signinname
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}