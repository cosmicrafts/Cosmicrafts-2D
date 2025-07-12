using System;

namespace SaveGameFree
{
    [Serializable]
    public class Data_player
    {

        public string NameOrEmail = "";
        public string Pasword = "";
        public bool AL = false;

        public bool DMGTexts = false;
        public bool HPBarEn = true;
        public bool RadialFill = true;
        public float ZoomSpeed = 0.1f;
        public float CameraSpeed = 4f;
        public float ShakeCamera = 0.5f;
        public bool FullScreen = false;
        public bool Music = true;
        public bool SoundFxs = true;
        public int Leng = 0;
        public int Resolution;
        public int Graphics = 2;
        public bool LogWithPlataform = false;

        public string[] _Emotes = new string[4] { "Hello", "GG", "XD", "Nice Play" };

        //store data
        public string[] Store_UnitsBundle = new string[4] { "", "", "", "" };
        public string[] Store_SkinsPromo = new string[4] { "", "", "", "" };
        public bool[] Store_CreateDaily = new bool[4] { true, true, true, true };

    }

}


