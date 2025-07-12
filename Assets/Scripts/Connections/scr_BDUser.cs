using System.Collections.Generic;

public class scr_BDUser {

    public List<scr_UnitProgress> MyUnits = new List<scr_UnitProgress>();
    public List<scr_DataMatch> HistoryMatchs = new List<scr_DataMatch>();

    public List<string>[] MyDeck = new List<string>[3];

    public string[] Name_deck = new string[] { "", "", "" };

    public int idAccount = 0;
    public int idUser = 0;
    public string Name = "";
    public string Email = "";
    public int Level = 0;
    public int Range = 0;
    public int LRange = 1;
    public string Avatar = "Player";
    public int IDAvatar = 0;
    public int IdCurrentTitle = -1;
    public int Xp = 0;
    public int RankPoints = 0;
    public int IdClan = -1;
    public string Clan = "<No Clan>";
    public string Deck = "";
    public string Region = "No def";
    public int VLeague = 0;
    public int LLeague = 0;
    public int DLeague = 0;
    public int VIa = 0;
    public int LIa = 0;
    public int DIa = 0;
    public int Camp_Progress = 0;
    public string FavoriteDeck = "";
}
