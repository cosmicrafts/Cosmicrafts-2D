using UnityEngine;

public class scr_Resources : MonoBehaviour {

    public static GameObject Bullet;
    public static GameObject DragUnit;
    public static GameObject Explo;
    public static GameObject Efect;
    public static GameObject Portal;
    public static GameObject UIDmg;
    public static GameObject AreaSelect;
    public static GameObject Command_Icon;

    public AudioClip[] _Shoots = new AudioClip[5];

    public static AudioClip[] SD_Shoots = new AudioClip[5];
    // Use this for initialization
    void Awake() {

        for (int i = 0; i < _Shoots.Length; i++)
            SD_Shoots[i] = _Shoots[i];

        Bullet = Resources.Load("Statics/Bullet") as GameObject;
        if (Bullet == null) { Debug.LogError("Error Loading Bullet prefab"); }

        DragUnit = Resources.Load("Statics/DragUnit") as GameObject;
        if (DragUnit == null) { 
            Debug.LogWarning("DragUnit prefab not found, creating dynamic DragUnit");
            DragUnit = CreateDynamicDragUnit();
        }

        Explo = Resources.Load("Statics/Explosion") as GameObject;
        if (Explo == null) { Debug.LogError("Error Loading Explosion prefab"); }

        Efect = Resources.Load("Statics/EfectPower") as GameObject;
        if (Efect == null) { Debug.LogError("Error Loading EfectPower prefab"); }

        Portal = Resources.Load("Statics/Portal") as GameObject;
        if (Portal == null) { Debug.LogError("Error Loading Portal prefab"); }

        UIDmg = Resources.Load("Statics/UIdmg") as GameObject;
        if (UIDmg == null) { Debug.LogError("Error Loading UIdmg prefab"); }

        AreaSelect = Resources.Load("Statics/Selection") as GameObject;
        if (AreaSelect == null) { Debug.LogError("Error Loading AreaSelect prefab"); }

        Command_Icon = Resources.Load("Statics/Comand_Icon") as GameObject;
        if (Command_Icon == null) { Debug.LogError("Error Loading Comand_Icon prefab"); }
    }

    private GameObject CreateDynamicDragUnit()
    {
        // Create a dynamic DragUnit GameObject
        GameObject dragUnit = new GameObject("DynamicDragUnit");
        
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
