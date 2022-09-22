using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecoverPassword : MonoBehaviour
{
    [SerializeField]
    private InputField _emailInputField;
    [SerializeField] GameObject notification;
    public void ResetPassword()
    {
        var auth = FirebaseAuth.DefaultInstance;
        string emailAddress = _emailInputField.text;
        StartCoroutine(LoginCoroutine(emailAddress));
    }
    private IEnumerator LoginCoroutine(string emailAddress)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var passwordTask = auth.SendPasswordResetEmailAsync(emailAddress);

        yield return new WaitUntil(() => passwordTask.IsCompleted);

        if (passwordTask.IsCanceled)
        {
            Debug.LogError("SendPasswordResetEmailAsync was canceled.");
        }
        if (passwordTask.IsFaulted)
        {
            Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + passwordTask.Exception);
        }
        else
        {
            notification.SetActive(true);
            Debug.Log("Password reset email sent successfully.");
        }
       
    }

}
