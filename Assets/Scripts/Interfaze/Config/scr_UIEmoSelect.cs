using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_UIEmoSelect : MonoBehaviour {

    public string[] Emotes = new string[8];

    public Dropdown[] DD_MyEmotes = new Dropdown[4];

    int Current = 0;

	// Use this for initialization
	void Start () {

        SetDrops();
    }


    public void SelectEmote(int index)
    {
        Current = index;
        scr_StatsPlayer.Emotes[Current] = DD_MyEmotes[Current].captionText.text;
        SetDrops();
        Scr_Database.SaveDataPlayer();
    }

    void SetDrops()
    {
        List<string> AvEmotes = new List<string>();
        for (int i = 0; i < Emotes.Length; i++)
            AvEmotes.Add(Emotes[i]);

        for (int i=0; i<4; i++)
        {
            DD_MyEmotes[i].ClearOptions();
            AvEmotes.Remove(scr_StatsPlayer.Emotes[i]);
        }

        Dropdown.OptionDataList Av = new Dropdown.OptionDataList();

        for (int i=0; i<AvEmotes.Count; i++)
        {
            Dropdown.OptionData newdata = new Dropdown.OptionData();
            newdata.text = AvEmotes[i];
            Av.options.Add(newdata);
        }

        for (int i = 0; i < 4; i++)
        {
            Dropdown.OptionData selected = new Dropdown.OptionData();
            selected.text = scr_StatsPlayer.Emotes[i];
            if (i>0) { Av.options.RemoveAt(0); }
            Av.options.Insert(0, selected);
            DD_MyEmotes[i].AddOptions(Av.options);
            DD_MyEmotes[i].value = 0;
        }

    }
}
