using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    private InputField userNameIF;
    private InputField passWordIF;
    private Button LoginButton;

    private LoginRequest loginRequest;

    private void Start()
    {
        loginRequest = this.GetComponent<LoginRequest>();

        userNameIF = transform.Find("UserNameIF").GetComponent<InputField>();
        passWordIF = transform.Find("PassWordIF").GetComponent<InputField>();
        LoginButton = transform.Find("login").GetComponent<Button>();
    }
}
