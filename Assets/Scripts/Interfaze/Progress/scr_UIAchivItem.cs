using UnityEngine;
using UnityEngine.UI;

public class scr_UIAchivItem : MonoBehaviour {

    [HideInInspector]
    public int ID;
    public Text NameAchiv;
    public Image IconShow;
    public GameObject Completed;

    public void InitItem(int _id, string _name, bool _selected, bool _completed)
    {
        ID = _id;
        NameAchiv.text = _name;
        if (_selected)
        {
            NameAchiv.color = Color.yellow;
            IconShow.color = Color.yellow;
        }
        if (_completed)
            Completed.SetActive(true);
        else
            Completed.SetActive(false);
    }

    public void SelectItem()
    {
        NameAchiv.color = Color.yellow;
        IconShow.color = Color.yellow;
    }

    public void DeSelectItem()
    {
        NameAchiv.color = Color.white;
        IconShow.color = Color.white;
    }
}
