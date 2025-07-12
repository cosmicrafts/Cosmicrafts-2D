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
        if (DragUnit == null) { Debug.LogError("Error Loading DRAG UNIT prefab"); }

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
	
}
