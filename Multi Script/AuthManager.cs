using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }

    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public Button signInButton;

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;
    public static FirebaseUser User;

    public GameObject loadingIcon;
    // Start is called before the first frame update
    void Start()
    {
        // 시작 할 때, sign in 버튼 비활성화
        signInButton.interactable = false;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;
            if (result != DependencyStatus.Available)
            {
                Debug.LogError(result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                IsFirebaseReady = true;
                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
            }
            // Firebase가 ready상태이면 버튼 활성화
            signInButton.interactable = IsFirebaseReady;
        });
    }

    public void SignIn()
    {
        // 1. Firebase가 ready상태가 아닐 경우
        // 2. 이미 Sign-In이 진행중일 경우
        // 3. User가 이미 할당 된 경우
        // 모두 버튼을 눌렀을 때 아무일도 일어나지 않게 함.
        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
            return;

        IsSignInOnProgress = true;
        signInButton.interactable = false; // 로그인 진행 중엔 sign in 버튼 비활성화
        loadingIcon.SetActive(true);

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
        {
            IsSignInOnProgress = false;
            signInButton.interactable = true;

            if (task.IsFaulted)//로그인 실패시
                Debug.LogError(task.Exception);
            else if (task.IsCanceled) // task가 캔슬됬을 경우
                Debug.LogError("Sign-in canceled");
            else
            {
                User = task.Result;
                Debug.Log(User.Email);
                SceneManager.LoadScene("Lobby");
            }
        });
    }

}
