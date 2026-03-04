namespace CompanyName.MyMeetings.API.Modules.UserAccess
{
    /// <summary>
    /// Request model for registering a new user account.
    /// </summary>
    public class RegisterNewUserRequest
    {
        /// <summary>
        /// The login username for the new account.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// The password for the new account.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The email address for the new account.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The URL link used to confirm the registration.
        /// </summary>
        public string ConfirmLink { get; set; }
    }
}
