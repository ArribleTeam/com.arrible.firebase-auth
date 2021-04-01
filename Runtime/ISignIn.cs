using Firebase.Auth;

namespace ARRT.Firebase.AuthorizationManagement
{
    public interface ISignIn
    {
        void SignIn(FirebaseAuth auth);
    }
}
