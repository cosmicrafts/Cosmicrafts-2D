using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_Tutorial_Menu : MonoBehaviour
{
    public Button[] Main_Elements = new Button[8];

    public scr_UIPlayerEditor PE;

    int Progress = 0;

    public void StartMenuTutorial()
    {
        for (int i = 0; i < Main_Elements.Length; i++)
            Main_Elements[i].enabled = false;

        transform.GetChild(2).gameObject.SetActive(true);
        Progress = 2;
    }

    public void AddProgress()
    {
        transform.GetChild(Progress).gameObject.SetActive(false);
        Progress++;
        if (Progress < transform.childCount)
        {
            transform.GetChild(Progress).gameObject.SetActive(true);
        } else
        {
            for (int i = 0; i < Main_Elements.Length; i++)
                Main_Elements[i].enabled = true;
        }
    }

}
