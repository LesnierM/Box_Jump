using System;

[Serializable]
public class ScoreData
{
    public string name;
    public int Score;

    public ScoreData(string name, int score)
    {
        this.name = name;
        Score = score;
    }
}
