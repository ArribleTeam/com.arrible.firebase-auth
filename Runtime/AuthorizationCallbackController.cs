using System;
using Firebase.Auth;

namespace ARRT.Firebase.AuthorizationManagement
{
    public static class AuthorizationCallbackController
    {
        public static Action<FirebaseUser> OnSignIn;
        public static Action<FirebaseUser> OnSignOut;
        public static Action<FirebaseUser> OnStateChange;

        private static FirebaseAuth m_Auth;
        private static FirebaseUser m_User;


        static AuthorizationCallbackController()
        {
            m_Auth = FirebaseAuth.DefaultInstance;
            m_Auth.StateChanged += OnStateChanged;
        }


        private static void OnStateChanged(object sender, EventArgs eventArgs)
        {

            if (m_Auth.CurrentUser != m_User)
            {
                bool signedIn = m_User != m_Auth.CurrentUser && m_Auth.CurrentUser != null;
                if (!signedIn && m_User != null)
                {
                    OnSignOut?.Invoke(m_User);
                    OnStateChange?.Invoke(m_User);
                }

                m_User = m_Auth.CurrentUser;
                if (signedIn)
                {
                    OnSignIn?.Invoke(m_User);
                    OnStateChange?.Invoke(m_User);
                }
            }
        }

    }
}
