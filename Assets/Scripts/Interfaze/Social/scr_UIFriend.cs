using UnityEngine;
using UnityEngine.UI;

public class scr_UIFriend : MonoBehaviour {

    public Text NameFriend;
    public Text Level;
    public Image Avatar;
    public Image StatusLabel;

    int Status = 0;

    public void ShowProfile()
    {
        scr_UIPlayerEditor editor = FindObjectOfType<scr_UIPlayerEditor>();
        if (editor!=null)
        {
            scr_UIProfile PUI = editor.UIProfile;
            if (!PUI.gameObject.activeSelf)
            {
                PUI.gameObject.SetActive(true);
                PUI.SetUser(NameFriend.text, Status);
                PUI.UpdateProfileUIInfo();
                transform.root.GetComponent<ChatNewGui>().Chat.SetActive(false);
            }
        }

    }

    public void OnFriendStatusUpdate(int status, bool gotMessage, object message)
    {
        Debug.Log("Status:" + status.ToString());
        Status = status;
        StatusLabel.color = scr_StatsPlayer.GetColorStatusUser(Status);
    }

    public void SelectFriend()
    {
        ChatNewGui chat = FindObjectOfType<ChatNewGui>();
        if (chat!=null)
        {
            chat.SetDestiny(NameFriend.text);
        }
    }

}

