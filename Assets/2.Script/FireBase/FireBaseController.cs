using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
using System;

public class FireBaseController : MonoBehaviour
{
    private Firebase.FirebaseApp app;
    private FirebaseAuth auth;
    private FirebaseUser user;

    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if(dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FIreBaseInit();
            }
            else
            {
                Debug.LogError("CheckAndFixDependenciesAsync Failed");
            }
        });

        StartCoroutine(c_wait());

    }


    IEnumerator c_wait()
    {
        yield return new WaitForSeconds(0.5f);
        SignIn();
    }



    private void FIreBaseInit()
    {
        app = Firebase.FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        Debug.Log("FireBase Init");

    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if(senderAuth != null)
        {
            user = senderAuth.CurrentUser;
            if(user != null)
            {
                Debug.Log(user.UserId);
            }
        }
    }

    public void SignIn()
    {
        SigninAnonymous();
    }

    private Task SigninAnonymous()
    {
        return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread
            (
            task => 
            {
                if(task.IsFaulted)
                {
                    Debug.LogError("Sign In Failed");
                }
                else if(task.IsCompleted)
                {
                    Debug.Log("Signin complete");
                }
            });

    }
}
