using UnityEngine;
using UnityEngine.UI;

public class scr_StarOrb : MonoBehaviour {

    [HideInInspector]
    public scr_DataOrb MyData;

    [HideInInspector]
    public bool RedyToOpen = false;

    public Text txt_Progress;

    [HideInInspector]
    public int ReqStars = 100;

    public void InitOrbe(scr_DataOrb _data, int _reqstars)
    {
        MyData = _data;
        ReqStars = _reqstars;
        if (MyData.Stars >= ReqStars)
        {
            MyData.Stars = ReqStars;
            RedyToOpen = true;
            txt_Progress.text = scr_Lang.GetText("txt_mn_info57");
        }
        else
        {
            txt_Progress.text = MyData.Stars.ToString() + "/" + ReqStars.ToString()+" "+scr_Lang.GetText("txt_mn_info45");
        }
    }

    public void RestartOrb()
    {
        RedyToOpen = false;
        MyData.Stars -= ReqStars;
        txt_Progress.text = MyData.Stars.ToString() + "/" + ReqStars.ToString() + " " + scr_Lang.GetText("txt_mn_info45");
    }

    private void OnEnable()
    {
        
    }
}
