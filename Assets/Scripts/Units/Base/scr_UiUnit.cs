using UnityEngine;
using UnityEngine.UI;

public class scr_UiUnit : Photon.MonoBehaviour {

    public Image UI_Hp;

    public Image UI_Duration;

    public Image Image_Selection;

    public GameObject MyCanvas;

    [HideInInspector]
    public float i_HealToShow = 0;

    [HideInInspector]
    public float i_DmgToShow = 0;

    [HideInInspector]
    public float f_DmgEffectToShow = 0f;

    [HideInInspector]
    public bool GetCritical = false;

    public scr_Unit MyUS;

    float Delay_Update = 0.25f;

    public Color c_Full = new Color();
    public Color c_Haf = new Color();
    public Color c_Low = new Color();
    public Color c_Enemy = new Color();
    public Color c_Inmortal = new Color();

    // Update is called once per frame
    void Update () {

        LoopEvents();
    }

    void LoopEvents()
    {

        if (!scr_StatsPlayer.Op_DMGText)
            return;

        Delay_Update -= Time.deltaTime;
        if (Delay_Update > 0f) { return; } else { Delay_Update = 0.25f; }

        if (i_DmgToShow > 0)
        {
            float color = -1f;
            if (GetCritical) { GetCritical = false; color = 0f; }

            if (scr_MNGame.GM.b_InNetwork)
                photonView.RPC("FloatDmgRPC", PhotonTargets.AllBufferedViaServer, i_DmgToShow, color, "", true);
            else
                FloatDmg(i_DmgToShow, color, "", true);
            i_DmgToShow = 0;
        }
        if (f_DmgEffectToShow > 0)
        {
            float color = 0.075f;

            if (scr_MNGame.GM.b_InNetwork)
                photonView.RPC("FloatDmgRPC", PhotonTargets.AllBufferedViaServer, f_DmgEffectToShow, color, "", true);
            else
                FloatDmg((int)f_DmgEffectToShow, color, "", true);
            f_DmgEffectToShow = 0;
        }
        if (i_HealToShow > 0)
        {
            if (scr_MNGame.GM.b_InNetwork)
                photonView.RPC("FloatDmgRPC", PhotonTargets.AllBufferedViaServer, i_HealToShow, 0.3f, "", false);
            else
                FloatDmg(i_HealToShow, 0.3f, "", false);
            i_HealToShow = 0;
        }

    }

    public void InitUI()
    {
        if (!scr_StatsPlayer.Op_UIHPBar)
        {
            UI_Hp = null;
        } else
        {
            UI_Hp.transform.parent.gameObject.SetActive(true);
        }
        if (MyUS.f_Duration>0f)
        {
            UI_Duration.transform.parent.gameObject.SetActive(true);
        }
        SetColorTeam();
    }

    public void EnableUI(bool _enable)
    {
        if (UI_Hp)
            UI_Hp.transform.parent.gameObject.SetActive(_enable);
        if (UI_Duration)
            UI_Duration.gameObject.SetActive(_enable);
    }

    public void SetColorTeam()
    {
        if (!scr_StatsPlayer.Op_UIHPBar)
            return;

        if (MyUS.Im_Aliad)
            SetHpColorPorcent();
        else
            UI_Hp.color = c_Enemy;
    }

    void SetHpColorPorcent()
    {
        float porcent_health = MyUS.f_hp / MyUS.f_Maxhp;

        if (porcent_health < 0.25f)
        {
            UI_Hp.color = c_Low;
        }
        else if (porcent_health < 0.5f)
        {
            UI_Hp.color = c_Haf;
        } else { UI_Hp.color = c_Full; }
    }

    public void SetColorBar(Color _color)
    {
        if (!scr_StatsPlayer.Op_UIHPBar)
            return;

        UI_Hp.color = _color;
    }

    public void SetHpBar(float _porcent)
    {
        if (!scr_StatsPlayer.Op_UIHPBar)
            return;

        UI_Hp.fillAmount = _porcent;

        if (MyUS.Im_Aliad)
            SetHpColorPorcent();
    }

    public void SetDurationBar(float _porcent)
    {
        if (!UI_Duration)
            return;

        UI_Duration.fillAmount = _porcent;
    }

    public void ShowEvade()
    {
        if (scr_MNGame.GM.b_InNetwork)
            photonView.RPC("FloatDmgRPC", PhotonTargets.AllBufferedViaServer, 0f, 0.2f, scr_Lang.GetText("txt_game_info21"), true);
        else
            FloatDmg(0f, 0.9f, scr_Lang.GetText("txt_game_info21"), true);
    }

    [PunRPC]
    public void FloatDmgRPC(float dmg, float huecolor, string OpText, bool gravity)
    {
        FloatDmg(dmg, huecolor, OpText, gravity);
    }

    public void FloatDmg(float dmg, float huecolor, string OpText, bool gravity)
    {
        if (!scr_StatsPlayer.Op_DMGText)
            return;

        scr_UIdmg uidmg = Instantiate(scr_Resources.UIDmg, MyCanvas.transform).GetComponent<scr_UIdmg>();
        if (huecolor >= 0f)
        {
            uidmg.txt_dmg.color = Color.HSVToRGB(huecolor, 0.75f, 1f);
        }
        else
        {
            float v = Mathf.Abs(huecolor);
            uidmg.txt_dmg.color = new Color(v, v, v);
        }
        uidmg.gravity = gravity;
        if (!gravity)
            uidmg.hspeed = 0;

        if (OpText != "")
            uidmg.txt_dmg.text = OpText;
        else
            uidmg.dmg = (int)dmg;
    }
}
