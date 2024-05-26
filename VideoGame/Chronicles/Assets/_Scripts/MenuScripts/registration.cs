using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using UnityEditor.VersionControl;


[Serializable]
public class UserDetailsList
{
    public List <UserDetailsList>;
    public UserDetailsList()
    {
        userDetails = new  List<UserDetail>;
        
    }
}


[Serializable]
public class UserDetails {
    public string userName;
    public string password;
}




public class registration : MonoBehaviour
{

    public GameObject UserDetailsListPanel;
    public GameObject UserRegisterPanel;
    public TMP_InputField username;
    public TMP_InputField password;
    public GameObject scrollViewContent;
    public UserDetailsView userDetailsViewPrefab;
    



    public void UserRegistration()
    {
        UserDetails userDetails = new UserDetails();
        userDetails.userName = username.text;
        userDetails.password = password.text;
        username.text = "";
        password.text = "";
        Message.text = "User Added succesfully";
        UserDetailsData.GetInstance().AddUserDetails(userDetails);

    }


    public void ClearPrefab()
    {
        foreach (Transform transf in scrollViewContent.transform)
        {
            Destroy(transf.gameObject);
        }
    }




    public void ShowRegistrationPanel()
    {
        Message.text ="";
        UserDetailsListPanel.SetActive(false);
        UserRegisterPanel.SetActive(true);
    }


    public void ShowUserDetailsListPanel()
    {
        UserDetailsListPanel.SetActive(true);
        UserRegisterPanel.SetActive(false);
    }

    public void DisplayUserDetailsList ()
    {
        ShowUserDetailsListPanel();
        ClearPrefab();

        UserDetailsList userdetailsList = UserDetailsData.GetInstance().GetUserDetailsList();

        foreach (UserDetails userDetails in userdetailsList.userDetailsList)
        {
            userDetailsView userDetailsObj = Instantiate (userDetailsViewPrefab) as UserDetailsView;
            userDetailsObj.gameObject.SetActive(true);
            userDetailsObj.UpdateUserDetails(userDetails);
            userDetailsObj.transform.SetParent(scrollViewContent.transform, false);
        }
    }
}
