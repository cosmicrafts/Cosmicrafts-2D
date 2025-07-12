using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections;

public class scr_FaceBook : MonoBehaviour
{

    public static List<string> FB_Friends;

    static string SharePromoLink = "http://www.cosmicrafts.com/";
    static string LogoPromoLink = "http://www.cosmicrafts.com/";

    static string StorePage = "";

    public Scr_Database DB;
    public scr_ctr_users UIC;

    [HideInInspector]
    public string UserName = "";
    [HideInInspector]
    public string Email = "";
    [HideInInspector]
    public string Password = "";


    private void Start()
    {
        InitFacebook();
    }

    public void InitFacebook()
    {

#if UNITY_EDITOR
        return;
#endif

        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback);
        }
        else
        {
            FB.ActivateApp();
        }

        if (FB.IsLoggedIn)
        {
            UIC.HideNormalLogin();
            LoadDataUser();
        }
        else
        {
            UIC.in_user.text = "";
            UIC.in_password.text = "";
        }
    }

    void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        } else
        {
            //Error init
            return;
        }

        if (FB.IsLoggedIn)
        {
            UIC.HideNormalLogin();
            LoadDataUser();
        }
    }

    void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            LoadDataUser();
            DB.StartCoroutine(DB.CheckLoginData(Email,Password));
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    void LoadDataUser()
    {
        FB.API("/me?fields=id,name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
        FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, FbGetPicture);
    }

    public void LogGameWithFacebook()
    {
        if (FB.IsLoggedIn)
        {
            DB.LogIn(Email, Password);
        }
    }

    private void FbGetPicture(IGraphResult result)
    {
        if (result.Texture != null)
            UIC.Social_Avatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
    }

    private void FetchProfileCallback(IGraphResult result)
    {
        Scr_Database.isLoggedSocial= false;

        Dictionary<string, object>  FBUserDetails = (Dictionary<string, object>)result.ResultDictionary;

        UserName = FBUserDetails["name"].ToString();
        Password = Scr_Database.GetPasswordFromId(FBUserDetails["id"].ToString());
        Email = FBUserDetails["email"].ToString();

        DB.SocialEmail = Email;
        DB.SocialPass = Password;

        UIC.Social_Name.text = UserName;
        Scr_Database.isLoggedSocial = true;
    }

    public void FacebookLogin()
    {
        if (!UIC.Terms.isOn)
        {
            UIC.AlertTerms.SetActive(true);
            return;
        }

        var permissions = new List<string>() {"public_profile","email","user_friends" };
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    public void FacebookLogOUT()
    {
        FB.LogOut();
    }

    public static void FacebookShare()
    {
        FB.ShareLink(new System.Uri(SharePromoLink),"Check it out!","I love this game",new System.Uri(LogoPromoLink));
    }

    public static void FacebookGameRequest()
    {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Cosmicrafts");
    }

    public static void FacebookInvite()
    {
        FB.Mobile.AppInvite(new System.Uri(StorePage));
    }

    public static void GetFriendsPlaying()
    {
        FB_Friends = new List<string>();
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendslist = (List<object>)dictionary["data"];
            foreach (var dict in friendslist)
            {
                string _friend = (string)((Dictionary<string, object>)dict)["name"];
                FB_Friends.Add(_friend);
            }
        });
    }

}
