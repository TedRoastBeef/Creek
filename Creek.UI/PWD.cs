using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Creek.UI
{
    public static class Pwd
    {
        [DllImport("credui")]
        private static extern CredUIReturnCodes CredUIPromptForCredentials(ref CREDUI_INFO creditUR, string targetName,
                                                                           IntPtr reserved1, int iError,
                                                                           StringBuilder userName, int maxUserName,
                                                                           StringBuilder password, int maxPassword,
                                                                           [MarshalAs(UnmanagedType.Bool)] ref bool
                                                                               pfSave, CREDUI_FLAGS flags);

        /// <summary>
        /// Fragt nach Benutzername und Passwort
        /// </summary>
        /// <param name="Title">Fenstertitel</param>
        /// <param name="Message">Fensternachricht</param>
        /// <param name="name">Benutzername</param>
        /// <param name="pass">Passwort</param>
        /// <returns>true, wenn erfolgreich</returns>
        public static Credentials askCred(string Title, string Message)
        {
            var returns = new Credentials();
            // Setup the flags and variables
            StringBuilder userPassword = new StringBuilder(), userID = new StringBuilder();
            var credUI = new CREDUI_INFO {pszCaptionText = Title, pszMessageText = Message};
            credUI.cbSize = Marshal.SizeOf(credUI);
            bool save = false;
            const CREDUI_FLAGS flags = CREDUI_FLAGS.ALWAYS_SHOW_UI | CREDUI_FLAGS.GENERIC_CREDENTIALS;

            // Prompt the user
            CredUIReturnCodes returnCode = CredUIPromptForCredentials(ref credUI, Application.ProductName, IntPtr.Zero,
                                                                      0, userID, 100, userPassword, 100, ref save, flags);

            returns.Username = userID.ToString();
            returns.Password = userPassword.ToString();

            returns.Success = returnCode == CredUIReturnCodes.NO_ERROR;

            return returns;
        }

        #region Nested type: CREDUI_FLAGS

        [Flags]
        private enum CREDUI_FLAGS
        {
            INCORRECT_PASSWORD = 0x1,
            DO_NOT_PERSIST = 0x2,
            REQUEST_ADMINISTRATOR = 0x4,
            EXCLUDE_CERTIFICATES = 0x8,
            REQUIRE_CERTIFICATE = 0x10,
            SHOW_SAVE_CHECK_BOX = 0x40,
            ALWAYS_SHOW_UI = 0x80,
            REQUIRE_SMARTCARD = 0x100,
            PASSWORD_ONLY_OK = 0x200,
            VALIDATE_USERNAME = 0x400,
            COMPLETE_USERNAME = 0x800,
            PERSIST = 0x1000,
            SERVER_CREDENTIAL = 0x4000,
            EXPECT_CONFIRMATION = 0x20000,
            GENERIC_CREDENTIALS = 0x40000,
            USERNAME_TARGET_CREDENTIALS = 0x80000,
            KEEP_USERNAME = 0x100000,
        }

        #endregion

        #region Nested type: CREDUI_INFO

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct CREDUI_INFO
        {
            public int cbSize;
            private readonly IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            private readonly IntPtr hbmBanner;
        }

        #endregion

        #region Nested type: CredUIReturnCodes

        private enum CredUIReturnCodes
        {
            NO_ERROR = 0,
            ERROR_CANCELLED = 1223,
            ERROR_NO_SUCH_LOGON_SESSION = 1312,
            ERROR_NOT_FOUND = 1168,
            ERROR_INVALID_ACCOUNT_NAME = 1315,
            ERROR_INSUFFICIENT_BUFFER = 122,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_INVALID_FLAGS = 1004,
        }

        #endregion

        #region Nested type: Credentials

        public class Credentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool Success { get; set; }
        }

        #endregion
    }
}