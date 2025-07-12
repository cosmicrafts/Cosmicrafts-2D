using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class m_options : MonoBehaviour {

    public Toggle BarradeVida;
    public Toggle NumerosDeDaño;
    public Toggle CargaCircular;
    public Toggle Music;
    public Toggle Fxs;
    public Toggle FullScreen;
    public Slider ZoomSpeed;
    public Slider MoveSpeed;
    public Slider ShakeCamera;
    public Dropdown Resolution;
    public Dropdown Leng;
    public Dropdown Graphics;

    string[] OP_Graphics_Lan = new string[3]{ "txt_mn_opts34", "txt_mn_opts33", "txt_mn_opts32" };

    void Start()
    {

#if UNITY_STANDALONE_WIN

        if (!scr_StatsPlayer.Op_Fullscr)
        {
            FullScreen.isOn = false;
        }

        Resolution.value = scr_StatsPlayer.OP_Resolution;

#endif
#if UNITY_ANDROID || UNITY_IOS

        FullScreen.gameObject.SetActive(false);
        Resolution.gameObject.SetActive(false);

#endif
        List<string> G_List = new List<string>();

        for (int i = 0; i < OP_Graphics_Lan.Length; i++)
            G_List.Add(scr_Lang.GetText(OP_Graphics_Lan[i]));

        Graphics.AddOptions(G_List);

        if (!scr_StatsPlayer.Op_DMGText)
        {
            NumerosDeDaño.isOn = false;
        }
        if (!scr_StatsPlayer.Op_UIHPBar)
        {
            BarradeVida.isOn = false;
        }
        if (!scr_StatsPlayer.Op_Music)
        {
            Music.isOn = false;
        }
        if (!scr_StatsPlayer.Op_SoundFxs)
        {
            Fxs.isOn = false;
        }
        if (!scr_StatsPlayer.Op_RadialFill)
        {
            CargaCircular.isOn = false;
        }

        ZoomSpeed.value = scr_StatsPlayer.Op_CameraSen;
        MoveSpeed.value = scr_StatsPlayer.Op_CameraMov;
        ShakeCamera.value = scr_StatsPlayer.Op_ShakeCamera;
        Leng.value = scr_StatsPlayer.Op_Leng;
        Graphics.value = scr_StatsPlayer.OP_Graphics;
    }

    public static void ChangeResolution()
    {
        int type = scr_StatsPlayer.OP_Resolution;
        switch (type)
        {
            case 1:
                {
                    Screen.SetResolution(1440, 900, scr_StatsPlayer.Op_Fullscr);
                }
                break;
            case 2:
                {
                    Screen.SetResolution(1600, 900, scr_StatsPlayer.Op_Fullscr);
                }
                break;
            case 3:
                {
                    Screen.SetResolution(1920, 1080, scr_StatsPlayer.Op_Fullscr);
                }
                break;
            default:
                {
                    Screen.SetResolution(1366, 768, scr_StatsPlayer.Op_Fullscr);
                }
                break;
        }
    }

    public void SetGraphics()
    {
        scr_StatsPlayer.OP_Graphics = Graphics.value;
        Scr_Database.SaveDataPlayer();
    }

    public void SetResValue()
    {
        scr_StatsPlayer.OP_Resolution = Resolution.value;
        Scr_Database.SaveDataPlayer();
        ChangeResolution();
    }

    public void SetFullScr()
    {
        Screen.fullScreen = FullScreen.isOn;
        scr_StatsPlayer.Op_Fullscr = FullScreen.isOn;
        Scr_Database.SaveDataPlayer();
    }

    public void SetDMGbool()
    {
        scr_StatsPlayer.Op_DMGText = NumerosDeDaño.isOn;
        Scr_Database.SaveDataPlayer();
    }

    public void SetHPBarbool()
    {
        scr_StatsPlayer.Op_UIHPBar = BarradeVida.isOn;
        Scr_Database.SaveDataPlayer();
    }

    public void SetRadiallFillboll()
    {
        scr_StatsPlayer.Op_RadialFill = CargaCircular.isOn;
        Scr_Database.SaveDataPlayer();
    }

    public void SetMusicbool()
    {
        scr_StatsPlayer.Op_Music = Music.isOn;
        Scr_Database.SaveDataPlayer();

        if (scr_StatsPlayer.Op_Music)
        {
            if (!scr_Music.AudioBegin)
            {
                scr_Music.AudioBegin = true;
                scr_Music.as_audio.Play();
            }
        }
        else
        {
            if (scr_Music.AudioBegin)
            {
                scr_Music.AudioBegin = false;
                scr_Music.as_audio.Stop();
            }
        }
    }

    public void SetFxsbool()
    {
        scr_StatsPlayer.Op_SoundFxs = Fxs.isOn;
        Scr_Database.SaveDataPlayer();
    }

    public void SetCameraSpeed()
    {
        scr_StatsPlayer.Op_CameraMov = MoveSpeed.value;
        Scr_Database.SaveDataPlayer();
    }

    public void SetShakeCamera()
    {
        scr_StatsPlayer.Op_ShakeCamera = ShakeCamera.value;
        Scr_Database.SaveDataPlayer();
    }

    public void SetCameraZoomSpeed()
    {
        scr_StatsPlayer.Op_CameraSen = ZoomSpeed.value;
        Scr_Database.SaveDataPlayer();
    }

    public void SetLeng()
    {
        scr_StatsPlayer.Op_Leng = Leng.value;
        scr_Lang.setLanguage();
        Scr_Database.SaveDataPlayer();
        List<string> G_List = new List<string>();

        for (int i = 0; i < OP_Graphics_Lan.Length; i++)
            G_List.Add(scr_Lang.GetText(OP_Graphics_Lan[i]));

        Graphics.ClearOptions();
        Graphics.AddOptions(G_List);
    }
}
