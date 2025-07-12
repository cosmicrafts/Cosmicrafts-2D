using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_Practice : MonoBehaviour
{
    public GameObject DeckButtonsSwitch;

    public scr_DragPractice DragPractice;

    [HideInInspector]
    public List<GameObject> UnitsTest;

    // Start is called before the first frame update
    void Start()
    {
        UnitsTest = new List<GameObject>();
        if (!scr_StatsPlayer.Practice)
        {
            Destroy(DragPractice.gameObject);
            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndPractice()
    {
        SceneManager.LoadScene("Menu-Interfas");
    }


    //Rules
    public void SwitchInmortalBases(bool _inmortal)
    {
        if (scr_MNGame.GM.Station0)
        {
            scr_MNGame.GM.Station0.IsInmune = _inmortal;
        }
        if (scr_MNGame.GM.Station1)
        {
            scr_MNGame.GM.Station1.IsInmune = _inmortal;
        }
    }

    public void SwitchInmortalAllied(bool _inmortal)
    {
        scr_MNGame.GM.InmortalAllied = _inmortal;
    }

    public void SwitchInmortalEnemys(bool _inmortal)
    {
        scr_MNGame.GM.InmortalEnnemys = _inmortal;
    }

    public void SwitchInivitorsActive(bool _active)
    {
        scr_MNGame.GM.InivitorsActive = _active;
    }

    public void SwitchInfiniteEnergy(bool _infinite)
    {
        scr_MNGame.GM.InfiniteEnergy = _infinite;
        if (_infinite)
            scr_MNGame.GM.AddResources(100f);
    }

    public void SwitchFreezeDeck(bool _active)
    {
        scr_MNGame.GM.FreezeDeck = _active;
    }

    public void ShowDeckEditor(bool _active)
    {
        DeckButtonsSwitch.SetActive(_active);
    }

    public void SwitchFreezeUnits(bool _active)
    {
        scr_MNGame.GM.FreezeUnits = _active;
    }

    public void SwitchFreeSpawn(bool _active)
    {
        scr_MNGame.GM.FreeSpawn = _active;
    }

    public void SwitchSpawnAsEnemy(bool _active)
    {
        scr_MNGame.GM.SpawnAsEnemy = _active;
    }

    //Generate
    public void GenInivitorsUnits()
    {
        for (int i=0; i< scr_MNGame.GM.Inividores.Length; i++)
        {
            if (scr_MNGame.GM.Inividores[i])
                scr_MNGame.GM.Inividores[i].MyGenUnits.GenOfflineMinions();
        }
    }

    public void GenEnnemysUnits()
    {
        for (int i = 0; i < scr_MNGame.GM.Inividores.Length; i++)
        {
            if (scr_MNGame.GM.Inividores[i])
            {
                if (scr_MNGame.GM.Inividores[i].i_Team==1)
                    scr_MNGame.GM.Inividores[i].MyGenUnits.GenOfflineMinions();
            }
        }
    }

    public void GenAlliedUnits()
    {
        for (int i = 0; i < scr_MNGame.GM.Inividores.Length; i++)
        {
            if (scr_MNGame.GM.Inividores[i])
            {
                if (scr_MNGame.GM.Inividores[i].i_Team == 0)
                    scr_MNGame.GM.Inividores[i].MyGenUnits.GenOfflineMinions();
            }
        }
    }

    public void GenTestEnemy()
    {
        if (UnitsTest.Count<10)
        {
            DragPractice.team_spawn = 1;
            DragPractice.gameObject.SetActive(true);
            DragPractice.ToDelete = false;
            DragPractice.MySprite.color = Color.white;
        }
    }

    public void GenTestAllied()
    {
        if (UnitsTest.Count < 10)
        {
            DragPractice.team_spawn = 0;
            DragPractice.gameObject.SetActive(true);
            DragPractice.ToDelete = false;
            DragPractice.MySprite.color = Color.white;
        }
    }

    //Functions

    public void DeleteTargets()
    {
        for (int i=0; i< UnitsTest.Count; i++)
        {
            if (UnitsTest[i])
            {
                Destroy(UnitsTest[i]);
            }
        }

        UnitsTest.Clear();
    }

    public void DeleteAllieds()
    {
        scr_Unit[] AllUnits = FindObjectsOfType<scr_Unit>();
        foreach (scr_Unit _unit in AllUnits)
        {
            if (_unit.i_Team==0 && !_unit.IsInitial)
            {
                _unit.AddDamage(-1f, false);
            }
        }
    }

    public void DeleteEnemys()
    {
        scr_Unit[] AllUnits = FindObjectsOfType<scr_Unit>();
        foreach (scr_Unit _unit in AllUnits)
        {
            if (_unit.i_Team == 1 && !_unit.IsInitial)
            {
                _unit.AddDamage(-1f, false);
            }
        }
    }

    public void SwitchDeleteMode(bool _active)
    {
        scr_MNGame.GM.DeleteOnClick = _active;
        if (scr_MNGame.GM.DeleteOnClick)
        {
            DragPractice.gameObject.SetActive(true);
            DragPractice.ToDelete = true;
            DragPractice.MySprite.color = Color.red;
        } else
        {
            DragPractice.ToDelete = false;
            DragPractice.MySprite.color = Color.white;
            DragPractice.gameObject.SetActive(false);
        }
    }

    public void RestartPractice()
    {
        SceneManager.LoadScene("Mapa_vsIA");
    }
}
