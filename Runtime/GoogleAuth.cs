using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Google;

namespace ARRT.Firebase.AuthorizationManagement
{
    public class GoogleAuth : ISignIn
    {
        private string m_ClientId;


        public GoogleAuth(string clientId)
        {
            m_ClientId = clientId;
        }

        public void SignIn(FirebaseAuth auth)
        {
            RequestCredential(m_ClientId, (credential) =>
            {
                SignInWithCredential(auth, credential);
            });
        }


        private void SignInWithCredential(FirebaseAuth auth, Credential credential)
        {
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {

                    Debug.LogError("SignInWithCredential was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {

                    Debug.LogError("SignInWithCredential encountered an error: " + task.Exception);
                    return;
                }

                var newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            });
        }

        private void RequestCredential(string clientID, Action<Credential> response)
        {
            Debug.Log("Trying to request credentials.");
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                WebClientId = m_ClientId,
                UseGameSignIn = false,
                RequestIdToken = true,
                RequestEmail = true,
            };

            if (GoogleSignIn.DefaultInstance != null)
                GoogleSignIn.DefaultInstance.SignOut();

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                            Debug.LogFormat("The request failed: status[{0}], msg[{1}]", error.Status, error.Message);
                        }
                    }
                }
                else if (task.IsCanceled)
                {
                    Debug.Log("Request canceled!");
                }
                else
                {
                    var credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
                    response?.Invoke(credential);
                    Debug.Log("Credentials were accepted successfully!");
                }
            });
        }

    }
}
