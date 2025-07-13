using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using UnityEngine.Networking;
using System.Linq;
using Facebook.Unity;
using System;

public class Scr_Database : Photon.MonoBehaviour
{

    private ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties;

    static bool startlogin = false;
    public static bool isLoggedIn;
    public static bool isLoggedSocial;
    InputField user_inpt;
    InputField pass_inpt;
    InputField reg_user_inpt;
    InputField reg_pass_inpt;
    InputField reg_email_inpt;
    Text StateTextLog;
    Text StateTextReg;
    scr_ctr_users scr_control;
    scr_FaceBook scr_fb;
    scr_GooglePlay scr_gp;

    [HideInInspector]
    public string SocialEmail = "";
    [HideInInspector]
    public string SocialPass = "";
    [HideInInspector]
    public bool LogWithPlataform = false;

    public static SaveGameFree.Data_player MyData;
    public static string fileName = "PlayerData";
    public static string db_dm = "2W?$.L9*4dS/x";
    bool AutoLog = false;
    string BaseUrl = "http://cosmicrafts.com/CCPHPS";

    string[] STATS;

    float timeout = 0.0f;

    List<string> UDB; //UNIDADES DESCARGADAS DEL PERFIL DE USUARIO

    public static bool DEV_BYPASS = true; // Set to false for real login

    void Awake()
    {
        isLoggedIn = false;
        isLoggedSocial = false;
        // Initialize our game data
        MyData = new SaveGameFree.Data_player();

        // Initialize the Saver with the default configurations
        SaveGameFree.Saver.Initialize();
        //MyData = new SaveGameFree.Data_player();
        MyData = SaveGameFree.Saver.Load<SaveGameFree.Data_player>(fileName);
        scr_StatsPlayer.Op_Leng = MyData.Leng;

        //Init Lang
        scr_Lang.setLanguage();
    }


    void Start()
    {
        scr_StatsPlayer.LastState = 2;
        scr_StatsPlayer.CurrentState = 2;
        scr_StatsPlayer.AllUnits = new List<string>();
        scr_StatsPlayer.PlayerDeck = new List<string>[3];
        scr_StatsPlayer.PlayerAvUnits = new List<string>[3];
        for (int i = 0; i < 3; i++)
        {
            if (scr_StatsPlayer.PlayerDeck[i] == null)
                scr_StatsPlayer.PlayerDeck[i] = new List<string>();
            if (scr_StatsPlayer.PlayerAvUnits[i] == null)
                scr_StatsPlayer.PlayerAvUnits[i] = new List<string>();
        }

        scr_StatsPlayer.MobileDevice = false;

        scr_StatsPlayer.UnitsNotAv = new List<string>();
        UDB = new List<string>();

        scr_control = GetComponent<scr_ctr_users>();
        scr_fb = GetComponent<scr_FaceBook>();

        user_inpt = scr_control.in_user;
        pass_inpt = scr_control.in_password;
        reg_user_inpt = scr_control.in_new_user;
        reg_pass_inpt = scr_control.in_new_pass;
        reg_email_inpt = scr_control.in_new_email;
        StateTextLog = scr_control.Log_State;
        StateTextReg = scr_control.Reg_State;

        //user_inpt.characterLimit = 18;
        pass_inpt.characterLimit = 24;
        reg_user_inpt.characterLimit = 18;
        reg_pass_inpt.characterLimit = 16;
        scr_control.in_new_pass2.characterLimit = 16;

        pass_inpt.inputType = InputField.InputType.Password;
        reg_pass_inpt.inputType = InputField.InputType.Password;
        scr_control.in_new_pass2.inputType = InputField.InputType.Password;

        reg_email_inpt.contentType = InputField.ContentType.EmailAddress;
        reg_email_inpt.characterValidation = InputField.CharacterValidation.EmailAddress;
        reg_email_inpt.characterLimit = 40;

        user_inpt.text = MyData.NameOrEmail;
        pass_inpt.text = MyData.Pasword;
        scr_control.UI_AL.isOn = AutoLog = MyData.AL;
        scr_StatsPlayer.Op_UIHPBar = MyData.HPBarEn;
        scr_StatsPlayer.Op_DMGText = MyData.DMGTexts;
        scr_StatsPlayer.Op_CameraSen = MyData.ZoomSpeed;
        scr_StatsPlayer.Op_CameraMov = MyData.CameraSpeed;
        scr_StatsPlayer.Op_ShakeCamera = MyData.ShakeCamera;
        scr_StatsPlayer.Op_Fullscr = MyData.FullScreen;
        scr_StatsPlayer.Emotes = MyData._Emotes;
        scr_StatsPlayer.Op_Music = MyData.Music;
        scr_StatsPlayer.Op_SoundFxs = MyData.SoundFxs;
        scr_StatsPlayer.OP_Resolution = MyData.Resolution;
        scr_StatsPlayer.Op_RadialFill = MyData.RadialFill;
        scr_StatsPlayer.OP_Graphics = MyData.Graphics;
        LogWithPlataform = MyData.LogWithPlataform;

#if UNITY_IPHONE
    scr_StatsPlayer.MobileDevice = true;
#endif
#if UNITY_ANDROID
        scr_StatsPlayer.MobileDevice = true;
        scr_control.gp.enabled = true;
        scr_control.gp.ActiveGP();
        scr_control.LogWithGP.SetActive(true);
#endif

    if (DEV_BYPASS)
    {
        user_inpt.text = "devuser";
        pass_inpt.text = "devpass";
        StartCoroutine(DevBypassLogin());
        return; // Skip the rest of Start() if bypassing
    }

        if (!scr_StatsPlayer.MobileDevice)
            scr_control.PanelSocialLog.SetActive(false);

        if (scr_StatsPlayer.Op_Music)
        {
            scr_Music.AudioBegin = true;
            scr_Music.as_audio.Play();
        }

#if UNITY_STANDALONE_WIN

        m_options.ChangeResolution();

#endif

        if (user_inpt.text.Length >= 4 && pass_inpt.text.Length >= 4 && !startlogin && AutoLog)
        {
            startlogin = true;
            LogIn(); //aqui se hace el autologin
        }

    }

    public void LogOutAllSocial()
    {
        if (FB.IsLoggedIn)
        {
            FB.LogOut();
        }

#if UNITY_ANDROID
        if (scr_control.gp.IsAuthenticated())
        {
            scr_control.gp.LogOut();
        }
#endif

        scr_control.ShowNormalLogin();
    }

    public static CultureInfo GetCurrentCultureInfo()
    {
        SystemLanguage currentLanguage = Application.systemLanguage;
        CultureInfo correspondingCultureInfo = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.EnglishName.Equals(currentLanguage.ToString()));
        return CultureInfo.CreateSpecificCulture(correspondingCultureInfo.TwoLetterISOLanguageName);
    }

    public static string GetPasswordFromId(string s)
    {
        string result = s.Substring(s.Length / 2);
        result = "tiap" + result;
        char[] arr = result.ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }

    public IEnumerator CheckLoginData(string Email, string Password)
    {
        yield return new WaitUntil(() => isLoggedSocial);
        scr_BDUpdate.f_CheckEmailExist(Email);
        yield return new WaitUntil(() => scr_BDUpdate.ExistEmail > -1);
        if (scr_BDUpdate.ExistEmail == 1)
        {
            scr_control.fn_show_log_form();
            scr_control.PanelSocialLog.SetActive(false);
            SaveLogDataInDevice(Email, Password);
        }
        else if (scr_BDUpdate.ExistEmail == 0)
        {
            ChooseUserName();
        }
        else
        {
            FB.LogOut();
            if (scr_gp.enabled)
                scr_gp.LogOut();
        }
        scr_BDUpdate.ExistEmail = -1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (reg_user_inpt.enabled)
            {
                Registrar();
            }
        }

        if (timeout > 0)
        {
            timeout -= 1 * Time.deltaTime;
            if (timeout <= 0)
            {
                timeout = 0.0f;
                StateTextLog.text = scr_Lang.GetText("txt_stats_error1_log");
                StateTextLog.color = Color.red;
                if (scr_control.islog)
                {
                    StopCoroutine("IELogIn");
                    ActiveInputsLog();
                }
                else
                {
                    CancelRegister();
                }
            }
        }
    }

    public static void SaveDataPlayer()
    {
        MyData.DMGTexts = scr_StatsPlayer.Op_DMGText;
        MyData.HPBarEn = scr_StatsPlayer.Op_UIHPBar;
        MyData._Emotes = scr_StatsPlayer.Emotes;
        MyData.CameraSpeed = scr_StatsPlayer.Op_CameraMov;
        MyData.ZoomSpeed = scr_StatsPlayer.Op_CameraSen;
        MyData.FullScreen = scr_StatsPlayer.Op_Fullscr;
        MyData.Music = scr_StatsPlayer.Op_Music;
        MyData.SoundFxs = scr_StatsPlayer.Op_SoundFxs;
        MyData.Leng = scr_StatsPlayer.Op_Leng;
        MyData.Resolution = scr_StatsPlayer.OP_Resolution;
        MyData.RadialFill = scr_StatsPlayer.Op_RadialFill;
        MyData.ShakeCamera = scr_StatsPlayer.Op_ShakeCamera;
        MyData.Graphics = scr_StatsPlayer.OP_Graphics;
        SaveGameFree.Saver.Save(MyData, fileName);
    }

    public static void SaveDatFile()
    {
        SaveGameFree.Saver.Save(MyData, fileName);
    }

    public static bool IsValidEmailAddress(string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
            return true;
        else
            return false;
    }

    public static bool CheckNOSPCH(string str)
    {
        Regex regex = new Regex(@"[^a-zA-z0-9_]");
        Match match = regex.Match(str);
        if (match.Success)
            return false;
        else
            return true;
    }

    public static bool CheckNOSPCHWA(string str)
    {
        Regex regex = new Regex(@"[^a-zA-z0-9áéíóúÁÉÍÓÚñÑ_]");
        Match match = regex.Match(str);
        if (match.Success)
            return false;
        else
            return true;
    }

    public static bool CheckValidPassWord(string pass)
    {
        Regex regex = new Regex(@"[^a-zA-z0-9_*!()@#]");
        Match match = regex.Match(pass);
        if (match.Success)
            return false;
        else
            return true;
    }

    public void ComprobarDatosIniciales()
    {
        bool okdata = true;
        StateTextReg.text = "";
        //check password 
        if (reg_pass_inpt.text != scr_control.in_new_pass2.text)
        {
            StateTextReg.text = scr_Lang.GetText("txt_stats_error4_singup"); ;
            okdata = false;
        }
        else if (!CheckValidPassWord(reg_pass_inpt.text))
        {
            StateTextReg.text = scr_Lang.GetText("txt_stats_error5_singup"); ;
            okdata = false;
        }
        //check Email
        if (reg_pass_inpt.text.Length < 6)
        {
            StateTextReg.text = scr_Lang.GetText("txt_stats_error6_singup"); ;
            okdata = false;
        }
        if (!IsValidEmailAddress(reg_email_inpt.text))
        {
            StateTextReg.text = scr_Lang.GetText("txt_stats_error7_singup"); ;
            okdata = false;
        }

        if (!scr_control.Terms.isOn && okdata)
        {
            scr_control.AlertTerms.SetActive(true);
            return;
        }

        if (okdata)
        {
            ChooseUserName();
        }
        else
        {
            StateTextReg.color = Color.red;
        }
    }

    public void ChooseUserName()
    {
        reg_pass_inpt.interactable = false;
        scr_control.in_new_pass2.interactable = false;
        reg_email_inpt.interactable = false;
        scr_control.btn_Create.interactable = false;
        scr_control.btn_Connect.interactable = false;
        scr_control.btn_Back.interactable = false;

        scr_control.btn_Confirm.interactable = true;
        scr_control.PanelUserReg.SetActive(true);
        scr_control.PanelUserReg.GetComponent<Animator>().Play("Enter");
    }

    public void Registrar()
    {
        if (DEV_BYPASS)
        {
            StartCoroutine(DevBypassLogin());
            return;
        }
        bool okdata = true;
        scr_control.Name_State.text = "";
        //check user name
        if (reg_user_inpt.text.Length < 4)
        {
            scr_control.Name_State.text = scr_Lang.GetText("txt_stats_error2_singup");
            okdata = false;
        }
        else if (!CheckNOSPCH(reg_user_inpt.text))
        {
            scr_control.Name_State.text = scr_Lang.GetText("txt_stats_error3_singup"); ;
            okdata = false;
        }

        if (okdata)
        {
            if (isLoggedSocial)
            {
                Registrar(reg_user_inpt.text, SocialPass, SocialEmail);
            } else
            {
                Registrar(reg_user_inpt.text, reg_pass_inpt.text, reg_email_inpt.text);
            }
        }
        else
        {
            scr_control.Name_State.color = Color.red;
        }
    }

    public void Registrar(string _name, string password, string email)
    {
        scr_control.btn_Confirm.interactable = false;
        StartCoroutine(IERegistrar(_name, password, email));
    }

    public void LogIn()
    {
        if (DEV_BYPASS)
        {
            StartCoroutine(DevBypassLogin());
            return;
        }
        StateTextLog.text = "";
        bool okdata = true;

        string _name_or_pass = user_inpt.text;
        string _pass = pass_inpt.text;

        if (!isLoggedSocial)
        {
            okdata = CheckInputsLogin();
        } else
        {
            _name_or_pass = SocialEmail;
            _pass = SocialPass;
        }

        if (okdata)
        {
            LogIn(_name_or_pass, _pass);
        }
        else
        {
            StateTextLog.color = Color.red;
        }

    }

    bool CheckInputsLogin()
    {
        bool okdata = true;
        if (isLoggedIn)
        {

            StateTextLog.text = scr_Lang.GetText("txt_stats_error3_log");
            okdata = false;
        }
        if (user_inpt.text.Length < 4)
        {
            StateTextLog.text = scr_Lang.GetText("txt_stats_error2_singup");
            okdata = false;
        }
        if (pass_inpt.text.Length < 6)
        {
            StateTextLog.text = scr_Lang.GetText("txt_stats_error6_singup");
            okdata = false;
        }
        if (!CheckNOSPCH(user_inpt.text))
        {
            if (!IsValidEmailAddress(user_inpt.text))
            {
                StateTextLog.text = scr_Lang.GetText("txt_stats_error3_singup");
                okdata = false;
            }
        }
        if (!CheckValidPassWord(pass_inpt.text))
        {
            StateTextLog.text = scr_Lang.GetText("txt_stats_error5_singup");
            okdata = false;
        }
        return okdata;
    }

    public void LogIn(string _name, string password)
    {
        StartCoroutine(IELogIn(_name, password));
        user_inpt.interactable = false;
        pass_inpt.interactable = false;
        scr_control.UI_AL.interactable = false;
        scr_control.btn_SignIn.interactable = false;
        scr_control.btn_Reg.interactable = false;
    }

    void ActiveInputsReg()
    {
        reg_user_inpt.interactable = true;
        reg_pass_inpt.interactable = true;
        scr_control.in_new_pass2.interactable = true;
        reg_email_inpt.interactable = true;
        scr_control.btn_Back.interactable = true;
        scr_control.btn_Create.interactable = true;
        scr_control.btn_Connect.interactable = true;
        StateTextReg.text = "";
    }

    void ActiveInputsLog()
    {
        user_inpt.interactable = true;
        pass_inpt.interactable = true;
        scr_control.btn_SignIn.interactable = true;
        scr_control.btn_Reg.interactable = true;
        scr_control.UI_AL.interactable = true;
    }

    public static void EndGameDB()
    {
        if (scr_StatsPlayer.id != 0)
        {
            WWWForm sendLoginInfo = new WWWForm();
            sendLoginInfo.AddField("IdAC", scr_StatsPlayer.id);
            UnityWebRequest getData = UnityWebRequest.Post("http://l7000711.ferozo.com/CCBDPHPCRLT/sqlLogOut.php", sendLoginInfo);

            getData.SendWebRequest();
        }
    }

    public void CancelRegister()
    {
        StopCoroutine("IERegistrar");
        ActiveInputsReg();
        timeout = 0.0f;
    }

    public void SaveLogDataInDevice(string name_or_email, string password)
    {
        MyData.NameOrEmail = name_or_email;
        MyData.Pasword = password;
        SaveGameFree.Saver.Save(MyData, fileName);
    }

    public void SetUsePlataform(bool use_plataform)
    {
        MyData.LogWithPlataform = use_plataform;
        SaveGameFree.Saver.Save(MyData, fileName);
    }

    IEnumerator IERegistrar(string _name, string password, string email)
    {
        timeout = 10.0f;

        //Formulario
        scr_control.Name_State.text = scr_Lang.GetText("txt_stats_info2_log");
        scr_control.Name_State.color = Color.cyan;
        WWWForm sendData = new WWWForm();
        sendData.AddField("dw", db_dm);

        sendData.AddField("NikeName", _name);
        sendData.AddField("Password", password);
        sendData.AddField("Email", email);
        sendData.AddField("MyRegion", GetCurrentCultureInfo().Name);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSignUp.php", sendData);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        //Pudimos conectar con servidor?
        scr_control.PanelUserReg.GetComponent<Animator>().SetTrigger("Exit");

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            isLoggedIn = false;
            FB.LogOut();
            ActiveInputsReg();
            timeout = 0.0f;
            StateTextReg.text = scr_Lang.GetText("txt_stats_error4_log");
            StateTextReg.color = Color.red;
            StopCoroutine("IERegistrar");
        }

        string result = getData.downloadHandler.text;

        //Check
        if (result.Length > 0)
        {
            if (result.Substring(0, 2) == "OK")
            {
                SaveLogDataInDevice(_name, password);
                Debug.Log("User created");
                ActiveInputsReg();
                isLoggedIn = false;
                timeout = 0.0f;
                scr_control.fn_show_log_form();
            }
            else if (result.Length > 4)
            {
                if (result.Substring(0, 5) == "ERROR")
                {
                    ActiveInputsReg();
                    StateTextReg.text = result.Substring(6);
                    StateTextReg.color = Color.red;
                    //Debug.LogError(result.Substring(6));
                    timeout = 0.0f;
                }
            }
        }
    }

    IEnumerator IELogIn(string _name_or_email, string password)
    {
        timeout = 10.0f;
        //Formulario
        StateTextLog.text = scr_Lang.GetText("txt_stats_info4_log");
        StateTextLog.color = Color.cyan;
        WWWForm sendLoginInfo = new WWWForm();
        sendLoginInfo.AddField("dw", db_dm);

        sendLoginInfo.AddField("User", _name_or_email);
        sendLoginInfo.AddField("Password", password);
        sendLoginInfo.AddField("IdDevice", SystemInfo.deviceUniqueIdentifier);
        

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSignIn.php", sendLoginInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            isLoggedIn = false;
            ActiveInputsLog();
            timeout = 0.0f;
            StateTextLog.text = scr_Lang.GetText("txt_stats_error4_log");
            StateTextLog.color = Color.red;
            StopCoroutine("IELogIn");
        }

        string canLogIn = getData.downloadHandler.text;

        string isok = "";

        if (canLogIn.Length > 1)
        {
            isok = canLogIn.Substring(0, 2);
        }
        //Checamos si fue exitoso
        if (isok == "OK")
        {
            MyData.NameOrEmail = _name_or_email;
            MyData.Pasword = password;
            MyData.AL = scr_control.UI_AL.isOn;
            if (scr_control.gp.enabled)
            {
                if (scr_control.gp.IsAuthenticated())
                {
                    MyData.LogWithPlataform = true;
                    //scr_control.ConsoleCanvas.text = "LOG GP SAVE";
                }
                    
            }
            SaveGameFree.Saver.Save(MyData, fileName);

            if (scr_Conection.ConnectToPhoton())
            {
                //Coneccion de Photon Exitosa
                STATS = canLogIn.Split('•');
                SetStats();
                //Bienvenida
                
                StateTextLog.text = scr_Lang.GetText("txt_stats_info5_log");
                StateTextLog.color = Color.green;
                isLoggedIn = true;
                timeout = 0.0f;
                
                yield return new WaitForSeconds(1f);
                LoadAllXmlUnitsAndEnter();
            } else
            {
                StateTextLog.color = Color.red;
                StateTextLog.text = scr_Lang.GetText("txt_stats_error6_log") + " " + canLogIn;
                ActiveInputsLog();
                isLoggedIn = false;
                timeout = 0.0f;
            }

        }
        else
        {
            Debug.LogWarning(canLogIn);
            StateTextLog.text = scr_Lang.GetText("txt_stats_error6_log") + " " + canLogIn;
            timeout = 0.0f;
            if (canLogIn.Length > 0)
                if (canLogIn.Substring(0, 2) == "NO")
                {
                    StateTextLog.text = scr_Lang.GetText("txt_stats_error5_log");
                }
                else if (canLogIn.Length > 4)
                {
                    if (canLogIn.Substring(0, 5) == "ERROR")
                    {
                        StateTextLog.text = canLogIn.Substring(6);
                    }
                }
            StateTextLog.color = Color.red;
            ActiveInputsLog();
            isLoggedIn = false;
        }
    }

    IEnumerator DevBypassLogin()
    {
        yield return new WaitForSeconds(0.5f); // Simulate delay
        isLoggedIn = true;
        MyData.NameOrEmail = user_inpt.text;
        MyData.Pasword = pass_inpt.text;
        SaveGameFree.Saver.Save(MyData, fileName);

        // --- DEV: Populate dummy units and decks with 8 real XML IdNames ---
        string[] devUnitNames = {
            "U_Ship_01", "U_Ship_02", "U_Ship_03", "U_Ship_04",
            "U_Ship_05", "U_Ship_06", "U_Ship_07", "U_Ship_08"
        };

        scr_StatsPlayer.MyUnits.Clear();
        scr_StatsPlayer.AllUnits.Clear();
        for (int i = 0; i < 3; i++)
        {
            if (scr_StatsPlayer.PlayerDeck[i] == null)
                scr_StatsPlayer.PlayerDeck[i] = new List<string>();
            if (scr_StatsPlayer.PlayerAvUnits[i] == null)
                scr_StatsPlayer.PlayerAvUnits[i] = new List<string>();
            scr_StatsPlayer.PlayerDeck[i].Clear();
            scr_StatsPlayer.PlayerAvUnits[i].Clear();
        }

        // Fill deck 0 with 8 units
        for (int i = 0; i < devUnitNames.Length; i++)
        {
            var unit = new scr_UnitProgress();
            unit.Id = i + 1;
            unit.Name = devUnitNames[i];
            unit.Level = 1;
            unit.Reinf = 0;
            unit.Skins = "Classic";
            unit.CurrentSkin = "Classic";
            scr_StatsPlayer.MyUnits.Add(unit);
            scr_StatsPlayer.AllUnits.Add(unit.Name);

            scr_StatsPlayer.PlayerDeck[0].Add(unit.Name);
            scr_StatsPlayer.PlayerAvUnits[0].Add(unit.Name);
        }

        // Fill decks 1 and 2 with the same units to ensure they have 8 cards each
        for (int deckIndex = 1; deckIndex < 3; deckIndex++)
        {
            for (int i = 0; i < devUnitNames.Length; i++)
            {
                scr_StatsPlayer.PlayerDeck[deckIndex].Add(devUnitNames[i]);
                scr_StatsPlayer.PlayerAvUnits[deckIndex].Add(devUnitNames[i]);
            }
        }

        // --- DEV: Initialize all player stats for UI compatibility ---
        scr_StatsPlayer.id = 999;
        scr_StatsPlayer.iduser = 999;
        scr_StatsPlayer.Name = "DevPlayer";
        scr_StatsPlayer.Email = "dev@local.com";
        scr_StatsPlayer.Level = 10;
        scr_StatsPlayer.Xp = 5000;
        scr_StatsPlayer.Range = 5;
        scr_StatsPlayer.LRange = 1;
        scr_StatsPlayer.RankPoints = 1500;
        scr_StatsPlayer.Neutrinos = 10000;
        scr_StatsPlayer.IDAvatar = 0;
        scr_StatsPlayer.IdCurrentTitle = 0;
        scr_StatsPlayer.IdClan = -1;
        scr_StatsPlayer.Region = "DEV";
        scr_StatsPlayer.WinsStreak = 3;
        scr_StatsPlayer.Deck = "DevDeck";
        scr_StatsPlayer.idc = 0;
        scr_StatsPlayer.VLeague = 10;
        scr_StatsPlayer.LLeague = 5;
        scr_StatsPlayer.DLeague = 2;
        scr_StatsPlayer.VIa = 15;
        scr_StatsPlayer.LIa = 3;
        scr_StatsPlayer.DIa = 1;
        scr_StatsPlayer.XpBuff = 1.0f;
        scr_StatsPlayer.XpBuffExpire = System.DateTime.Now.AddDays(7);
        scr_StatsPlayer.LastDateConection = System.DateTime.Now;
        scr_StatsPlayer.FirstConnection = true;
        scr_StatsPlayer.FirstBattle = false;
        scr_StatsPlayer.Afterbattle = false;
        scr_StatsPlayer.Promotion = false;
        scr_StatsPlayer.Downgrade = false;
        scr_StatsPlayer.Tutorial = false;
        scr_StatsPlayer.Practice = false;
        scr_StatsPlayer.New_Levels = 0;
        scr_StatsPlayer.b_LevelUp = false;

        // Initialize Orbes array (required for UpdateCoins())
        scr_StatsPlayer.Orbes = new int[3] { 5, 3, 2 }; // Give some orbs for testing

        // Initialize OpeningOrbs array
        scr_StatsPlayer.OpeningOrbs = new scr_DataOrb[1];
        scr_StatsPlayer.OpeningOrbs[0] = new scr_DataOrb();
        scr_StatsPlayer.OpeningOrbs[0].Stars = 50;
        scr_StatsPlayer.OpeningOrbs[0].Type = 3;

        // Initialize MyTitles and MyAchiv arrays
        scr_StatsPlayer.MyTitles = new bool[11];
        scr_StatsPlayer.MyAchiv = new List<scr_Achievements>();
        for (int i = 0; i < 11; i++)
        {
            scr_StatsPlayer.MyTitles[i] = false;
            scr_Achievements NewAchiv = new scr_Achievements();
            NewAchiv.InitAchiv(i, i, scr_Lang.GetTitleName(i), scr_Lang.GetTitleDescription(i));
            scr_StatsPlayer.MyAchiv.Add(NewAchiv);
        }
        scr_StatsPlayer.MyTitles[0] = true; // Give first title

        // Initialize other required lists
        if (scr_StatsPlayer.Friends == null) scr_StatsPlayer.Friends = new List<string>();
        if (scr_StatsPlayer.NewFriends == null) scr_StatsPlayer.NewFriends = new List<string>();
        if (scr_StatsPlayer.FriendsData == null) scr_StatsPlayer.FriendsData = new List<scr_BDUser>();
        if (scr_StatsPlayer.MySkins == null) scr_StatsPlayer.MySkins = new List<string>();
        if (scr_StatsPlayer.HistoryMatchs == null) scr_StatsPlayer.HistoryMatchs = new List<scr_DataMatch>();
        if (scr_StatsPlayer.UnitsNotAv == null) scr_StatsPlayer.UnitsNotAv = new List<string>();

        // Initialize deck names
        scr_StatsPlayer.Name_deck[0] = "Dev Deck";
        scr_StatsPlayer.Name_deck[1] = "Deck 2";
        scr_StatsPlayer.Name_deck[2] = "Deck 3";

        // Initialize missions (required for end-game processing)
        scr_Missions.InitMissions();

        // Set language for dev bypass mode (default to English, but can be changed)
        scr_StatsPlayer.Op_Leng = MyData.Leng; // Use saved language preference
        if (scr_StatsPlayer.Op_Leng == 0) // If not set, default to English
        {
            scr_StatsPlayer.Op_Leng = 0; // 0 = English, 1 = Spanish
        }
        
        // Re-initialize language system with the correct language
        scr_Lang.setLanguage();

        // -------------------------------------------

        StateTextLog.text = "DEV LOGIN SUCCESS";
        StateTextLog.color = Color.green;
        LoadAllXmlUnitsAndEnter();
    }

    void LoadAllXmlUnitsAndEnter()
    {
        //
        scr_GetStats.LoadXmlShips();
        XmlNodeList allUnits = scr_GetStats.XmlUnits.SelectNodes("/Units/Unit");
        Debug.Log("Saltamos consideracion de existencia de nave del jugador en deck");
        for (int i = 0; i < 3; i++)
        {
            scr_StatsPlayer.PlayerAvUnits[i].Clear();

            for (int j = 0; j < allUnits.Count; j++) //Creamos una carta por Nave en la escena
            {
                string nameunit = allUnits[j].Attributes["IdName"].InnerText;
                if (scr_GetStats.GetPropUnit(nameunit, "ExistInCollection") == "0")
                    continue;

                if (!UDB.Contains(nameunit))
                {
                    if (i == 0)//only add units at first round
                        scr_StatsPlayer.UnitsNotAv.Add(nameunit);
                }
                //else
                {
                    if (!scr_StatsPlayer.PlayerDeck[i].Contains(nameunit))
                    {
                        scr_StatsPlayer.PlayerAvUnits[i].Add(nameunit);
                    }
                }
                if (i == 0)
                    scr_StatsPlayer.AllUnits.Add(nameunit);

            }
        }

        //Otorgamos Rareza a cada unidad
        for (int i = 0; i < scr_StatsPlayer.MyUnits.Count; i++)
        {
            int rarity = 0;
            string rs = scr_GetStats.GetPropUnit(scr_StatsPlayer.MyUnits[i].Name, "Rarity");
            int.TryParse(rs, out rarity);
            scr_StatsPlayer.MyUnits[i].Rarity = rarity;
        }

        //Dar todos los parametros al jugador

        /*PhotonNetwork.playerName = scr_StatsPlayer.Name;
        expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "idC", scr_StatsPlayer.id }, { "exp_ganada", 0 }, { "Test", 120 }, { "Name", scr_StatsPlayer.Name }, { "idAvatar", scr_StatsPlayer.IDAvatar } };
        PhotonNetwork.player.SetCustomProperties(expectedCustomRoomProperties);*/
        scr_control.MenuPanel.SetActive(true);
        Destroy(gameObject);
        // PhotonNetwork.LoadLevel("Mapa_Base_");
    }

    void SetStats()
    {

        //Inicializamos variables de stats
        scr_StatsPlayer.id = int.Parse(STATS[1]);
        scr_StatsPlayer.IdCurrentTitle = int.Parse(STATS[2]);
        scr_StatsPlayer.IdClan = int.Parse(STATS[3]);
        scr_StatsPlayer.IDAvatar = int.Parse(STATS[4]);
        scr_StatsPlayer.Region = STATS[5];
        scr_StatsPlayer.Xp = int.Parse(STATS[6]);
        scr_StatsPlayer.Level = int.Parse(STATS[7]);
        scr_StatsPlayer.Range = int.Parse(STATS[8]);
        scr_StatsPlayer.LRange = int.Parse(STATS[9]);
        scr_StatsPlayer.RankPoints = int.Parse(STATS[10]);
        string _data_missions = STATS[11];
        string _data_achivments = STATS[12];
        string _titles_data = STATS[13];
        string[] _np = STATS[14].Split('-');
        string[] _npia = STATS[15].Split('-');
        scr_StatsPlayer.Neutrinos = int.Parse(STATS[16]);
        string _orbes = STATS[17];
        string _time_orbs = STATS[18];
        string _data_xpbuff = STATS[19];
        scr_StatsPlayer.iduser = int.Parse(STATS[20]);
        scr_StatsPlayer.Name = STATS[21];
        scr_StatsPlayer.Email = STATS[22];
        scr_StatsPlayer.Deck = STATS[23];
        int id_deck = int.Parse(STATS[24]);
        string str_allUnits = STATS[25];
        string _data_friends = STATS[26];
        string _data_friends_request = STATS[27];
        string[] datetime = STATS[28].Split(' ');
        scr_StatsPlayer.WinsStreak = int.Parse(STATS[29]);

        //Pothon profile
        PhotonNetwork.player.NickName = scr_StatsPlayer.Name;
        expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "idC", scr_StatsPlayer.id }, { "exp_ganada", 0 }, { "Test", 120 }, { "Name", scr_StatsPlayer.Name }, { "idAvatar", scr_StatsPlayer.IDAvatar } };
        PhotonNetwork.player.SetCustomProperties(expectedCustomRoomProperties);

        //Seteamos Estadisticas Victorias/Derrotas
        scr_StatsPlayer.VLeague = int.Parse(_np[0]);
        scr_StatsPlayer.LLeague = int.Parse(_np[1]);
        scr_StatsPlayer.DLeague = int.Parse(_np[2]);

        scr_StatsPlayer.VIa = int.Parse(_npia[0]);
        scr_StatsPlayer.LIa = int.Parse(_npia[1]);
        scr_StatsPlayer.DIa = int.Parse(_npia[2]);

        //Seteamos Avatar
        PhotonNetwork.player.CustomProperties.Remove("idAvatar");
        PhotonNetwork.player.CustomProperties.Add("idAvatar", scr_StatsPlayer.IDAvatar);

        //Seteamos ID
        PhotonNetwork.player.CustomProperties.Remove("IdPlayer");
        PhotonNetwork.player.CustomProperties.Add("IdPlayer", scr_StatsPlayer.id);

        //Seteamos Rank Points
        PhotonNetwork.player.CustomProperties.Remove("Rpoints");
        PhotonNetwork.player.CustomProperties.Add("Rpoints", scr_StatsPlayer.RankPoints);

        //Cargamos Aceleradores de experiencia
        if (_data_xpbuff != "")
        {
            string[] _df = _data_xpbuff.Split('|');
            float _xpbuff = 1f;

            float.TryParse(_df[0], out _xpbuff);
            scr_StatsPlayer.XpBuff = _xpbuff;

            string[] _edatetime = _df[1].Split(' ');
            string[] _edate = _edatetime[0].Split('-');
            string[] _etime = _edatetime[1].Split(':');

            int _eyear = 0;
            int _emonth = 0;
            int _eday = 0;

            int _ehour = 0;
            int _eminute = 0;
            int _esec = 0;

            int.TryParse(_edate[0], out _eyear);
            int.TryParse(_edate[1], out _emonth);
            int.TryParse(_edate[2], out _eday);

            int.TryParse(_etime[0], out _ehour);
            int.TryParse(_etime[1], out _eminute);
            int.TryParse(_etime[2], out _esec);

            scr_StatsPlayer.XpBuffExpire = new System.DateTime(_eyear, _emonth, _eday, _ehour, _eminute, _esec);
        }
        else
        {
            scr_StatsPlayer.XpBuffExpire = System.DateTime.Now;
            scr_StatsPlayer.XpBuff = 1f;
        }

        //Cargamos orbes
        scr_StatsPlayer.Orbes = new int[3] { 0, 0, 0};

        if (_orbes.Length > 1)
        {
            string[] _orb = _orbes.Split('|');
            for (int i = 0; i < _orb.Length; i++)
            {
                int _o = 0;
                int.TryParse(_orb[i], out _o);
                scr_StatsPlayer.Orbes[i] = _o;
            }
        }

        //Cargamos Orbes de tiempo
        scr_StatsPlayer.OpeningOrbs = new scr_DataOrb[1];
        if (_time_orbs == "")
        {
            for (int i = 0; i < scr_StatsPlayer.OpeningOrbs.Length; i++)
            {
                scr_StatsPlayer.OpeningOrbs[i] = new scr_DataOrb();
                scr_StatsPlayer.OpeningOrbs[i].Stars = 0;
                scr_StatsPlayer.OpeningOrbs[i].Type = 3;
            }
            scr_BDUpdate.f_SetStarsOrbs(scr_StatsPlayer.id, false);
        }
        else
        {
            string[] to = _time_orbs.Split('|');
            for (int i = 0; i < to.Length; i++)
            {
                string[] _data = to[i].Split('?');
                scr_StatsPlayer.OpeningOrbs[i] = new scr_DataOrb();
                /*
                string[] _datetime = _data[0].Split(' ');
                string[] _date = _datetime[0].Split('-');
                string[] _time = _datetime[1].Split(':');

                int _year = 0;
                int _month = 0;
                int _day = 0;

                int _hour = 0;
                int _minute = 0;
                int _sec = 0;

                int.TryParse(_date[0], out _year);
                int.TryParse(_date[1], out _month);
                int.TryParse(_date[2], out _day);

                int.TryParse(_time[0], out _hour);
                int.TryParse(_time[1], out _minute);
                int.TryParse(_time[2], out _sec);
                */
                scr_StatsPlayer.OpeningOrbs[i].Stars = int.Parse(_data[0]);
                scr_StatsPlayer.OpeningOrbs[i].Type = int.Parse(_data[1]);
                if (i == 0)
                {
                    scr_StatsPlayer.OpeningOrbs[i].Type = 3;
                }
            }
        }

        //Cargamos logors
        int[] progress_achiv;
        progress_achiv = new int[scr_StatsPlayer.MyTitles.Length];

        if (_data_achivments == "")
        {
            for (int i = 0; i < progress_achiv.Length; i++)
            {
                progress_achiv[i] = 0;
            }
        }
        else
        {
            string[] atch = _data_achivments.Split('|');
            progress_achiv = new int[scr_StatsPlayer.MyTitles.Length];
            for (int i = 0; i < scr_StatsPlayer.MyTitles.Length; i++)
            {
                int progress = 0;
                if (i < atch.Length)
                    int.TryParse(atch[i], out progress);
                progress_achiv[i] = progress;
            }
        }

        progress_achiv[0] = 1;

        //Creamos Logros
        for (int i = 0; i < scr_StatsPlayer.MyTitles.Length; i++)
        {
            scr_StatsPlayer.MyTitles[i] = false;
            scr_Achievements NewAchiv = new scr_Achievements();
            NewAchiv.InitAchiv(i, i, scr_Lang.GetTitleName(i), scr_Lang.GetTitleDescription(i));
            scr_StatsPlayer.MyAchiv.Add(NewAchiv);
        }
        //Definimos fases y progresos
        scr_StatsPlayer.MyAchiv[0].SetLevels(new int[1] { 1 }, progress_achiv[0]);

        scr_StatsPlayer.MyAchiv[1].SetLevels(new int[4] { 1, 10, 100, 1000 }, progress_achiv[1]);
        scr_StatsPlayer.MyAchiv[1].AddTitlesRewards(new int[4] { -1, -1, -1, 1 });

        scr_StatsPlayer.MyAchiv[2].SetLevels(new int[3] { 1, 10, 100 }, progress_achiv[2]);
        scr_StatsPlayer.MyAchiv[2].AddTitlesRewards(new int[3] { -1, -1, 2 });

        scr_StatsPlayer.MyAchiv[3].SetLevels(new int[4] { 1, 10, 100, 1000 }, progress_achiv[3]);
        scr_StatsPlayer.MyAchiv[3].AddTitlesRewards(new int[4] { -1, -1, -1, 3 });
        //scr_StatsPlayer.MyAchiv[3].AddOrbsRewards(new int[4] { 1, -1, -1, -1 });

        scr_StatsPlayer.MyAchiv[4].SetLevels(new int[7] { 10, 100, 1000, 10000, 100000, 1000000, 10000000 }, progress_achiv[4]);
        scr_StatsPlayer.MyAchiv[4].AddTitlesRewards(new int[7] { -1, -1, -1, -1, -1, -1, 4 });
        //scr_StatsPlayer.MyAchiv[4].AddNeutrinosRewards(new int[7] { 75, -1, -1, -1, -1, -1, -1 });

        scr_StatsPlayer.MyAchiv[5].SetLevels(new int[7] { 10, 100, 1000, 10000, 100000, 1000000, 10000000 }, progress_achiv[5]);
        scr_StatsPlayer.MyAchiv[5].AddTitlesRewards(new int[7] { -1, -1, -1, -1, -1, -1, 5 });

        scr_StatsPlayer.MyAchiv[6].SetLevels(new int[7] { 10, 100, 1000, 10000, 100000, 1000000, 10000000 }, progress_achiv[6]);
        scr_StatsPlayer.MyAchiv[6].AddTitlesRewards(new int[7] { -1, -1, -1, -1, -1, -1, 6 });

        scr_StatsPlayer.MyAchiv[7].SetLevels(new int[7] { 10, 100, 1000, 10000, 100000, 1000000, 10000000 }, progress_achiv[7]);
        scr_StatsPlayer.MyAchiv[7].AddTitlesRewards(new int[7] { -1, -1, -1, -1, -1, -1, 7 });

        scr_StatsPlayer.MyAchiv[8].SetLevels(new int[7] { 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 }, progress_achiv[8]);
        scr_StatsPlayer.MyAchiv[8].AddTitlesRewards(new int[7] { -1, -1, -1, -1, -1, -1, 8 });

        scr_StatsPlayer.MyAchiv[9].SetLevels(new int[4] { 100, 1000, 10000, 100000 }, progress_achiv[9]);
        scr_StatsPlayer.MyAchiv[9].AddTitlesRewards(new int[4] { -1, -1, -1, 9 });

        scr_StatsPlayer.MyAchiv[10].SetLevels(new int[4] { 10, 100, 1000, 10000 }, progress_achiv[10]);
        scr_StatsPlayer.MyAchiv[10].AddTitlesRewards(new int[4] { -1, -1, -1, 10 });

        //Cargamos mis titulos
        if (_titles_data != "")
        {
            string[] titl = _titles_data.Split('|');
            for (int i = 0; i < titl.Length; i++)
            {
                int idach = 0;
                int.TryParse(titl[i], out idach);
                scr_StatsPlayer.MyTitles[idach] = true;
            }
        }

        //CARGAMOS UNIDADES
        if (str_allUnits.Length > 1)
        {
            string[] str_units = str_allUnits.Split('♦');
            for (int i = 0; i < str_units.Length - 1; i++)
            {
                string[] str_unitStats = str_units[i].Split('■');
                scr_UnitProgress up = new scr_UnitProgress();
                up.Id = int.Parse(str_unitStats[1]);
                up.Name = str_unitStats[0];
                up.Level = int.Parse(str_unitStats[2]);
                up.Reinf = int.Parse(str_unitStats[3]);
                up.Skins = str_unitStats[4];

                UDB.Add(up.Name);
                //skins
                if (up.Skins.Contains("|"))
                {
                    string[] _skins = up.Skins.Split('|');
                    for (int j = 0; j < _skins.Length; j++)
                        scr_StatsPlayer.MySkins.Add(up.Name + ":" + _skins[j]);
                }
                else
                {
                    scr_StatsPlayer.MySkins.Add(up.Name + ":" + up.Skins);
                }

                up.CurrentSkin = str_unitStats[5];
                scr_StatsPlayer.MyUnits.Add(up);
            }
        }

        //Seteamos Deck
        scr_StatsPlayer.idc = id_deck;
        string str_deck = scr_StatsPlayer.Deck;
        string[] str_decks = str_deck.Split('?');
        for (int j = 0; j < 3; j++)
        {
            scr_StatsPlayer.idc = j;
            string[] str_deckunits = str_decks[j].Split('-');
            if (str_decks.Length > 1)
                for (int i = 0; i < str_deckunits.Length; i++)
                {
                    if (i == 0)
                    {
                        scr_StatsPlayer.Name_deck[j] = str_deckunits[i];
                    }
                    else
                    {
                        scr_StatsPlayer.PlayerDeck[j].Add(str_deckunits[i]);
                    }

                }
        }
        scr_StatsPlayer.idc = id_deck;

        //Cargamos amigos
        if (_data_friends.Length > 0)
        {
            string[] str_friends = _data_friends.Split('?');
            if (str_friends.Length > 0)
            {
                for (int i = 0; i < str_friends.Length; i++)
                {
                    if (!scr_StatsPlayer.Friends.Contains(str_friends[i]))
                        scr_StatsPlayer.Friends.Add(str_friends[i]);
                }
            }
        }
        //Cargamos Solicitudes de amistad
        if (_data_friends_request.Length > 0)
        {
            string[] str_friends = _data_friends_request.Split('?');
            if (str_friends.Length > 0)
            {
                for (int i = 0; i < str_friends.Length; i++)
                {
                    if (!scr_StatsPlayer.NewFriends.Contains(str_friends[i]))
                        scr_StatsPlayer.NewFriends.Add(str_friends[i]);
                }
            }
        }

        //Cargamos ultima coneccion
        string[] date = datetime[0].Split('-');
        string[] time = datetime[1].Split(':');

        int year = 0;
        int month = 0;
        int day = 0;

        int hour = 0;
        int minute = 0;
        int sec = 0;

        int.TryParse(date[0], out year);
        int.TryParse(date[1], out month);
        int.TryParse(date[2], out day);

        int.TryParse(time[0], out hour);
        int.TryParse(time[1], out minute);
        int.TryParse(time[2], out sec);

        if (year == 0)
        {
            scr_StatsPlayer.LastDateConection = System.DateTime.Now;
        }
        else
        {
            scr_StatsPlayer.LastDateConection = new System.DateTime(year, month, day, hour, minute, sec);
        }

        //Misiones diarias y semanales
        scr_Missions.InitMissions();

        if (_data_missions.Length > 1)
        {
            string[] _missions = _data_missions.Split('|');
            for (int i = 0; i < _missions.Length-1; i++)
            {
                int _m = 0;
                int.TryParse(_missions[i], out _m);
                if (i < 5)
                {
                    scr_Missions.MD_Progress[i] = _m;
                }
                else
                {
                    scr_Missions.MW_Progress[i - 5] = _m;
                }

            }
            int _dc = 0;
            int.TryParse(_missions[_missions.Length - 1], out _dc);
            scr_Missions.DaysCounter = _dc;
        }

        scr_Missions.CheckExpire();
        scr_Missions.CheckProgressComplete();
        scr_Missions.Claim_Day = scr_Missions.Day_Complete;
        scr_Missions.Claim_Week = scr_Missions.Week_Complete;

    }

    void OnFailedToConnectToPhoton()
    {
        Debug.LogWarning("Error Photon Connection");
    }

    void OnConnectedToPhoton()
    {
        Debug.Log("Conected to Photon");
    }
}
