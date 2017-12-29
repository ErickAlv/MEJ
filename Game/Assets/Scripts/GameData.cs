﻿public class GameData{
    private static GameData instance;
    private int score = 0;

    private GameData()
    {
        Paused = false;
        if (instance != null)
            return;
        instance = this;
    }

    public static GameData Instance
    {
        get
        {
            if(instance== null)
            {
                instance = new GameData();
            }
            return instance;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }

    public int Lives { get; set; }

    public int Keys { get; set; }
    public bool Paused
    {
        get;
        set;
    }

}
