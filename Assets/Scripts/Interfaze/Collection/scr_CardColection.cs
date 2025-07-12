using UnityEngine;
using UnityEngine.UI;

public class scr_CardColection : MonoBehaviour
{
    public string s_name; //Nombre de la nave
    public string s_idname;
    public string s_type;
    public int i_Cost; // costo (se usara mas adelante)
    public int i_rarity;
    public int i_Level;
    public int i_IndexDeck;
    public bool InDeck = false;// indica si esta en el deck
    public bool empty = false; //indica si es un solot vacio
    public Image SP_mysprite; //sprite de la nave
    public Text TXT_cost;
    public Text TXT_lvl;
    public Text TXT_Name;
    scr_UIDeckEditor ManagerCards;

    private void Awake()
    {
        ManagerCards = FindObjectOfType<scr_UIDeckEditor>();
    }

    public void SetUnit(string idunit)
    {
        s_idname = idunit;
        UpdateInfo();
    }

    public void SwitchCards() //selecciona la carta y crea una carta clon en la seccion de cartas seleccionadas (se agrega a la lista de dek del jugador)
    {
        if (ManagerCards.go_remplace.InDeck)//Pasamos carta del deck a la coleccion
        {
            scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc][ManagerCards.CardPosition] = ManagerCards.go_selected.s_idname;
            scr_StatsPlayer.PlayerAvUnits[scr_StatsPlayer.idc].Remove(ManagerCards.go_selected.s_idname);
            scr_StatsPlayer.PlayerAvUnits[scr_StatsPlayer.idc].Add(ManagerCards.go_remplace.s_idname);
            ManagerCards.SwitchCardsInUI(ManagerCards.go_remplace.s_idname, ManagerCards.go_selected.s_idname);
        }
        else //Pasamos carta de la coleccion al deck
        {
            scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc][ManagerCards.CardPosition] = ManagerCards.go_remplace.s_idname;
            scr_StatsPlayer.PlayerAvUnits[scr_StatsPlayer.idc].Remove(ManagerCards.go_remplace.s_idname);
            scr_StatsPlayer.PlayerAvUnits[scr_StatsPlayer.idc].Add(ManagerCards.go_selected.s_idname);
            ManagerCards.SwitchCardsInUI(ManagerCards.go_selected.s_idname, ManagerCards.go_remplace.s_idname);
        }
        ManagerCards.go_remplace = null;
        ManagerCards.CardPosition = -1;
        ManagerCards.OrderSelectedCards();
    }

    public void BeginDrag()
    {
        if (!ManagerCards.to_drop && !empty && !ManagerCards.LoadingCards)
        {
            ManagerCards.go_remplace = this;
            if  (InDeck)
                ManagerCards.CardPosition = i_IndexDeck;
            ManagerCards.to_drop = true;
            ManagerCards.go_DragShip = Instantiate(scr_Resources.DragUnit, transform.position, Quaternion.identity);
            ManagerCards.go_DragShip.GetComponent<SpriteRenderer>().sprite = SP_mysprite.sprite;
            ManagerCards.go_DragShip.GetComponent<scr_DragUnit>().in_interfaz = true;
            ManagerCards.go_DragShip.transform.localScale = new Vector2(0.25f, 0.25f);
        }
    }

    public void DropShip()
    {
        if (ManagerCards.go_DragShip != null)
        {
            Destroy(ManagerCards.go_DragShip);
            ManagerCards.to_drop = false;
            ManagerCards.go_DragShip = null;
            if (ManagerCards.go_selected != null && ManagerCards.go_remplace != null && ManagerCards.go_remplace.InDeck!= ManagerCards.go_selected.InDeck)
            {
                if (ManagerCards.go_selected.InDeck && ManagerCards.CardPosition == -1)
                    ManagerCards.CardPosition = ManagerCards.go_selected.i_IndexDeck;
                SwitchCards();
            }
        }
    }

    public void SetSelected()
    {
        ManagerCards.go_selected = gameObject.GetComponent<scr_CardColection>();
    }

    public void DeSelect()
    {
        ManagerCards.go_selected = null;
    }

    public void UpdateInfo()
    {
        s_name = scr_GetStats.GetPropUnit(s_idname, "Name");
        s_type = scr_GetStats.GetTypeUnit(s_idname);
        int.TryParse(scr_GetStats.GetPropUnit(s_idname, "Cost"), out i_Cost);
        int.TryParse(scr_GetStats.GetPropUnit(s_idname, "Rarity"), out i_rarity);
        if (TXT_cost != null)
        {
            TXT_cost.text = i_Cost.ToString();
        }
        if (TXT_lvl != null)
        {
            i_Level = 1;
            int id = scr_StatsPlayer.FindIdUnit(s_idname);
            if (id != -1)
                i_Level = scr_StatsPlayer.MyUnits[id].Level;
            TXT_lvl.text = scr_Lang.GetText("txt_mn_info2_unit") + " " + i_Level.ToString();
        }
        if (TXT_Name != null)
        {
            TXT_Name.text = s_name;
        }
        Sprite ico = Resources.Load<Sprite>("Units/Iconos/" + scr_GetStats.GetPropUnit(s_idname, "Icon"));
        if (ico != null)
            SP_mysprite.sprite = ico;
        else
            SP_mysprite.sprite = ManagerCards.NoIco;
    }

    public void ShowInfo()
    {
        ManagerCards.ShowInfoCard(s_idname);
    }

    public void Clear()
    {
        empty = true;
    }
}
