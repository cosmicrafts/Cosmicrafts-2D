using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class scr_ctr_users : MonoBehaviour {


	public Button btn_Reg, btn_SignIn, btn_Back, btn_Create, btn_Forget, btn_Connect, btn_Confirm;
	public InputField in_user, in_password, in_new_user, in_new_email , in_new_pass, in_new_pass2;
    Scr_Database ControlSql;
    public scr_FaceBook fb;
    public scr_GooglePlay gp;
	public Text Log_State, Reg_State, Name_State;
    public Animator UI_Reg, UI_Log;
    public Toggle UI_AL;
    public GameObject PanelUserReg;
    public GameObject PanelSocialLog;
    public GameObject Social_Message;
    public GameObject DataLogin_Grup;
    public GameObject PanelUserLogin;
    public GameObject PanelNoConnection;
    public GameObject Help_Login;
    public GameObject AlertTerms;
    public GameObject MenuPanel;

    public GameObject LogWithGP;

    public Image Social_Avatar;
    public Text Social_Name;

    public Toggle Terms;

    [HideInInspector]
    public bool islog = true;

    WaitForSeconds Delay1s = new WaitForSeconds(1f);

    bool findFirstSelectable = false;

    public Text ConsoleCanvas;

    /*
string output = "";
string stack = ""; 

void OnEnable()
{
   Application.logMessageReceived += HandleLog;
}

void OnDisable()
{
   Application.logMessageReceived -= HandleLog;
}

void HandleLog(string logString, string stackTrace, LogType type)
{
   output = logString;
   stack = stackTrace;
   ConsoleCanvas.text += logString+"\n"+stackTrace+ "\n\n";
}
*/

    void Start()
    {

        ControlSql = GetComponent<Scr_Database>();
        if (!scr_Conection.CheckInternetConnection())
        {
            PanelNoConnection.SetActive(true);
            PanelUserLogin.SetActive(false);
        }
    }

    private void Update()
    {
        PcCommands();
    }

    public void CheckConnection()
    {
        if (scr_Conection.CheckInternetConnection())
        {
            PanelNoConnection.SetActive(false);
            PanelUserLogin.SetActive(true);
            fb.InitFacebook();
        }
    }

    public void HideNormalLogin()
    {
        DataLogin_Grup.SetActive(false);
        Social_Message.SetActive(true);
        Help_Login.SetActive(false);
        PanelSocialLog.SetActive(false);
    }

    public void ShowNormalLogin()
    {
        DataLogin_Grup.SetActive(true);
        Social_Message.SetActive(false);
        Help_Login.SetActive(true);
        PanelSocialLog.SetActive(false);
        in_user.text = "";
        in_password.text = "";
        btn_SignIn.interactable = true;
        if (scr_StatsPlayer.MobileDevice)
            PanelSocialLog.SetActive(true);
    }

    public void fn_show_reg_form()
	{
        StopAllCoroutines();
        StartCoroutine(GoToRegister());
	}

	public void fn_show_log_form()
    {
        StopAllCoroutines();
        StartCoroutine(GoToLogin());
    }

    public void SelectSignInButton()
    {
        btn_SignIn.Select();
    }

    void PcCommands()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (EventSystem.current != null)
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;

                //try and find the first selectable if there isn't one currently selected
                //only do it if the findFirstSelectable is true
                //you may not always want this feature and thus
                //it is disabled by default
                if (selected == null && findFirstSelectable)
                {
                    Selectable found = (Selectable.allSelectables.Count > 0) ? Selectable.allSelectables[0] : null;

                    if (found != null)
                    {
                        //simple reference so that selected isn't null and will proceed
                        //past the next if statement
                        selected = found.gameObject;
                    }
                }

                if (selected != null)
                {
                    Selectable current = (Selectable)selected.GetComponent("Selectable");

                    if (current != null)
                    {
                        Selectable nextDown = current.FindSelectableOnDown();
                        Selectable nextUp = current.FindSelectableOnUp();
                        Selectable nextRight = current.FindSelectableOnRight();
                        Selectable nextLeft = current.FindSelectableOnLeft();

                        if (nextDown != null)
                        {
                            nextDown.Select();
                        }
                        else if (nextRight != null)
                        {
                            nextRight.Select();
                        }
                        else if (nextUp != null)
                        {
                            nextUp.Select();
                        }
                        else if (nextLeft != null)
                        {
                            nextLeft.Select();
                        }
                    }
                }
            }
        }
    }

    IEnumerator GoToRegister()
    {
        islog = false;
        Reg_State.text = "";
        reset_texts();

        btn_SignIn.interactable = false;
        btn_Forget.interactable = false;
        UI_Log.SetTrigger("Exit");

        PanelUserReg.SetActive(false);

        yield return Delay1s;
        UI_Reg.gameObject.SetActive(true);
        yield return Delay1s;
        UI_Log.gameObject.SetActive(false);

        btn_Back.interactable = true;
        btn_Create.interactable = true;
        btn_Connect.interactable = true;
        AlertTerms.SetActive(false);
        Terms.interactable = false;
        Terms.isOn = false;
    }

    IEnumerator GoToLogin()
    {
        islog = true;
        Log_State.text = "";
        ControlSql.CancelRegister();
        reset_texts();

        btn_Back.interactable = false;
        btn_Create.interactable = false;
        btn_Connect.interactable = false;
        UI_Reg.SetTrigger("Exit");

        yield return new WaitForSeconds(1f);
        UI_Log.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        UI_Reg.gameObject.SetActive(false);

        btn_SignIn.interactable = true;
        btn_Forget.interactable = true;

        DataLogin_Grup.SetActive(!Scr_Database.isLoggedSocial);
        Social_Message.SetActive(Scr_Database.isLoggedSocial);

        Help_Login.SetActive(!Scr_Database.isLoggedSocial);

        in_user.text = Scr_Database.MyData.NameOrEmail;
    }

    public void GoToChangePassword()
    {
        Application.OpenURL("http://cosmicrafts.com/Re_Password.php");
    }

    public void reset_texts()
	{
        in_user.text = "";
		in_password.text = "";
        in_new_user.text = "";
        in_new_email.text = "";
        in_new_pass.text = "";
        in_new_pass2.text = "";
    }
}
