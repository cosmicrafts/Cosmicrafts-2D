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
            // Ensure scr_Resources is initialized
            if (scr_Resources.DragUnit == null)
            {
                // Find or create scr_Resources component
                scr_Resources resources = FindObjectOfType<scr_Resources>();
                if (resources == null)
                {
                    GameObject resourcesGO = new GameObject("scr_Resources");
                    resources = resourcesGO.AddComponent<scr_Resources>();
                }
                
                // Force initialization if DragUnit is still null
                if (scr_Resources.DragUnit == null)
                {
                    Debug.LogWarning("DragUnit still null, creating dynamic one");
                    scr_Resources.DragUnit = CreateDynamicDragUnitForEditor();
                }
            }
            
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
        string iconPath = "Units/Iconos/" + scr_GetStats.GetPropUnit(s_idname, "Icon");
        Sprite ico = Resources.Load<Sprite>(iconPath);
        if (ico != null)
            SP_mysprite.sprite = ico;
        else
        {
            Debug.LogWarning("Unit icon not found for unit '" + s_idname + "'. Icon path: " + iconPath + ". Using fallback icon.");
            SP_mysprite.sprite = ManagerCards.NoIco;
        }
    }

    public void ShowInfo()
    {
        ManagerCards.ShowInfoCard(s_idname);
    }

    public void Clear()
    {
        empty = true;
    }

    private GameObject CreateDynamicDragUnitForEditor()
    {
        // Create a dynamic DragUnit GameObject for the deck editor
        GameObject dragUnit = new GameObject("DynamicDragUnit_Editor");
        
        // Add the required components
        dragUnit.AddComponent<scr_DragUnit>();
        
        // Add SpriteRenderer for the main sprite
        SpriteRenderer mainSprite = dragUnit.AddComponent<SpriteRenderer>();
        mainSprite.sortingOrder = 100; // Ensure it renders on top
        
        // Create child objects for the different sprite components
        GameObject spriteBase = new GameObject("SpriteBase");
        spriteBase.transform.SetParent(dragUnit.transform);
        SpriteRenderer baseSprite = spriteBase.AddComponent<SpriteRenderer>();
        baseSprite.sortingOrder = 99;
        
        GameObject negative = new GameObject("Negative");
        negative.transform.SetParent(dragUnit.transform);
        SpriteRenderer negSprite = negative.AddComponent<SpriteRenderer>();
        negSprite.sortingOrder = 98;
        negSprite.color = Color.red;
        
        // Add CircleCollider2D for trigger detection
        CircleCollider2D collider = dragUnit.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.5f;
        
        // Assign the sprite renderers to the DragUnit component
        scr_DragUnit dragUnitScript = dragUnit.GetComponent<scr_DragUnit>();
        dragUnitScript.MySprite = mainSprite;
        dragUnitScript.SpriteBase = baseSprite;
        dragUnitScript.Negative = negSprite;
        
        // Set default sprite (will be overridden when used)
        Sprite defaultSprite = Resources.Load<Sprite>("Units/Iconos/U_Ship_01");
        if (defaultSprite != null)
        {
            mainSprite.sprite = defaultSprite;
            baseSprite.sprite = defaultSprite;
        }
        
        return dragUnit;
    }
}
