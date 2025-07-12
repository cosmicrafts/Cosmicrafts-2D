using UnityEngine;

public class scr_MenuManager : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject MenuPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (Scr_Database.isLoggedIn)
        {
            MenuPanel.SetActive(true);
        } else
        {
            LoginPanel.SetActive(true);
        }
    }
}
