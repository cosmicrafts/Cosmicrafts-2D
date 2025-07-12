using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi;
using UnityEngine.Networking;

public class scr_GooglePlay : MonoBehaviour
{

    public Scr_Database DB;
    public scr_ctr_users UIC;

    [HideInInspector]
    public bool OkGP_Log = false;

    [HideInInspector]
    public string UserName = "";
    [HideInInspector]
    public string Email = "";
    [HideInInspector]
    public string IdUser = "";
    [HideInInspector]
    public string Password = "";
#if UNITY_ANDROID
    PlayGamesClientConfiguration config;
#endif
    // Start is called before the first frame update
    public void ActiveGP()
    {
#if UNITY_ANDROID
        config = new PlayGamesClientConfiguration.Builder().AddOauthScope("email").RequestEmail().Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;

        try
        {
            PlayGamesPlatform.Activate();

        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        if (DB.LogWithPlataform)
        {
            //scr_control.ConsoleCanvas.text = "Start Log GP \n";
            GP_Start_Login();
        }
#endif
    }

    public bool IsAuthenticated()
    {
        return Social.localUser.authenticated;
    }

    public void GP_Start_Login()
    {
        Scr_Database.isLoggedSocial = false;

        if (Social.localUser.authenticated)
        {
            LoadDataUser();
            Scr_Database.isLoggedSocial = true;
            UIC.HideNormalLogin();
            return;
        }

        Social.localUser.Authenticate((bool sucess) =>
        {
            if (sucess)
            {
                LoadDataUser();
                Scr_Database.isLoggedSocial = true;
                UIC.HideNormalLogin();
            }
            else
            {
                Debug.Log("error login google play: " + Social.localUser.state);
            }
        });
    }

    public void GP_Login()
    {
        if (!UIC.Terms.isOn)
        {
            UIC.AlertTerms.SetActive(true);
            return;
        }

        Scr_Database.isLoggedSocial = false;

        if (Social.localUser.authenticated)
        {
            LoadDataUser();
            Scr_Database.isLoggedSocial = true;
            DB.StartCoroutine(DB.CheckLoginData(Email, Password));
            return;
        }
        
        Social.localUser.Authenticate((bool sucess) =>
        {
            if (sucess)
            {
                LoadDataUser();
                Scr_Database.isLoggedSocial = true;
                DB.StartCoroutine(DB.CheckLoginData(Email, Password));
            } else
            {
                Debug.Log("error login google play: "+ Social.localUser.state);
            }
        });
    }

    void LoadDataUser()
    {
#if UNITY_ANDROID
        UserName = ((PlayGamesLocalUser)Social.localUser).userName;
        Email = ((PlayGamesLocalUser)Social.localUser).Email;
        IdUser = ((PlayGamesLocalUser)Social.localUser).id;
        Password = Scr_Database.GetPasswordFromId(IdUser);
        //string url_avatar = ((PlayGamesLocalUser)Social.localUser).AvatarURL;
        //((PlayGamesLocalUser)Social.localUser).LoadImage();
        //UIC.ConsoleCanvas.text = url_avatar+" \n";
        //UIC.in_user.text = url_avatar;
        StartCoroutine(GetAvatar());
        
        //UIC.ConsoleCanvas.text = UserName+"\n"+ Email+ "\n"+ Password;
        
        DB.SocialEmail = Email;
        DB.SocialPass = Password;
        UIC.Social_Name.text = UserName;
#endif
    }



    IEnumerator GetAvatar()
    {

        /*
        yield return new WaitUntil(() => ((PlayGamesLocalUser)Social.localUser).image!=null);
        UIC.Social_Avatar.sprite = Sprite.Create(((PlayGamesLocalUser)Social.localUser).image, new Rect(0, 0, 128, 128), new Vector2());
        */
#if UNITY_ANDROID
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(((PlayGamesLocalUser)Social.localUser).AvatarURL);
        yield return request.SendWebRequest();

        Texture2D _avatar;

        if (request.isNetworkError || request.isHttpError)
        {
            _avatar= Texture2D.blackTexture;
            Debug.Log(request.error);
        }
        else
        {
            _avatar = DownloadHandlerTexture.GetContent(request);
            //UIC.ConsoleCanvas.text = "Avatar ok";
        }
        UIC.Social_Avatar.sprite = Sprite.Create(_avatar, new Rect(0, 0, 128, 128), new Vector2());

#else
        yield return null;
#endif
    }

    public void LogOut()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.SignOut();
        UIC.ShowNormalLogin();
        DB.SetUsePlataform(false);
#endif
    }
}
