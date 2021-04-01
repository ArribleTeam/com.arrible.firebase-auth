
#if USE_FIREBASE_AUTH
using System;
using System.Linq;
using UnityEngine;
using Firebase.Auth;

namespace ARRT.Firebase.AuthorizationManagement
{
    public static class Authorization
    {
        public static bool isAuthorizedUser { get => user != null; }
        public static FirebaseUser user { get => auth.CurrentUser; }

        private static FirebaseAuth auth { get => FirebaseAuth.DefaultInstance; }


        public static void SignOut()
        {
            auth.SignOut();
        }

        public static void SignInAnonymously()
        {
            if (isAuthorizedUser)
            {
                Debug.Log("User is already logged in!");
                return;
            }

            auth.SignInAnonymouslyAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogWarning("SignInAnonymouslyAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogWarning("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }

                var newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            });
        }

        public static void SignInWith<T> (T value) where T:ISignIn
        { 
            if (isAuthorizedUser)
            {
                Debug.Log("User is already logged in!");
                return;
            }

            value.SignIn(auth);
        }

        public static void SignInWithEmailAndPassword(string email, string password)
        {
            if (isAuthorizedUser)
            {
                Debug.Log("User is already logged in!");
                return;
            }

            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogWarning("SignInWithEmailAndPassword was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogWarningFormat("SignInWithEmailAndPassword encountered an error: {0}", task.Exception);
                    return;
                }

                var newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            });
        }

        public static void CreateUserWithEmailAndPassword(string email, string password)
        {
            if (isAuthorizedUser)
            {
                Debug.Log("User is already logged in!");
                return;
            }

            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith((task) =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPassword was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPassword encountered an error: " + task.Exception);
                    return;
                }

                var newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            });
        }

        public static void ExistsUser(string email, Action<bool> response)
        {
            auth.FetchProvidersForEmailAsync(email).ContinueWith((task) =>
            {
                if (task.Result.ToList().Count <= 0)
                {
                    response?.Invoke(false);
                    return;
                }
                
                response?.Invoke(true);
            });
        }
    }
}
#endif
