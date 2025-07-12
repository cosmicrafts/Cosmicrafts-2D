using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class scr_Tutorial : MonoBehaviour
{
    int Progress;

    public GameObject[] Steps = new GameObject[4];

    public Text Title_UI;

    public scr_CardsCntrl CC;

    [HideInInspector]
    public bool WhaitForSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        Progress = 0;
        if (!scr_StatsPlayer.Tutorial)
            gameObject.SetActive(false);
        else
        {
            Steps[0].SetActive(true);
            Title_UI.text = scr_Lang.GetText("txt_tutorial_00");
            for (int i = 0; i < 4; i++)
                CC.Cards[i].Is_Lock = true;
        }
            
    }

    public void AddProgress()
    {
        WhaitForSpawn = false;
        Steps[Progress].SetActive(false);
        Progress++;
        if (Progress<Steps.Length)
        {
            Steps[Progress].SetActive(true);
        }
        switch(Progress)
        {
            case 5:
                {
                    CC.Cards[0].Is_Lock = false;
                    WhaitForSpawn = true;
                }
                break;
            case 6:
                {
                    CC.Cards[0].Is_Lock = true;
                }
                break;
            case 7:
                {
                    CC.Cards[0].Is_Lock = false;
                    WhaitForSpawn = true;
                }
                break;
            case 8:
                {
                    CC.Cards[0].Is_Lock = true;
                }
                break;
            case 11:
                {
                    CC.Cards[0].Is_Lock = false;
                    WhaitForSpawn = true;
                }
                break;
            case 12:
                {
                    CC.Cards[0].Is_Lock = true;
                }
                break;
            case 14:
                {
                    CC.Cards[0].Is_Lock = false;
                    WhaitForSpawn = true;
                }
                break;
            case 15:
                {
                    CC.Cards[0].Is_Lock = true;
                }
                break;
            case 22:
                {
                    for (int i = 0; i < 4; i++)
                        CC.Cards[i].Is_Lock = false;
                }
                break;
        }
    }
}
