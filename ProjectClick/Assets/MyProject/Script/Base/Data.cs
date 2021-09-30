using System.Collections.Generic;


[System.Serializable]
public class Data
{
    public int size;
    public long heart;
    public long dogecoin;
    public long ufo;
    public long neuralink;
    public long shutingwin;

    public List<ClickUpLevel> clickUpLevelList = new List<ClickUpLevel>();
    public List<ClickUpLevel> clickUpGloballList = new List<ClickUpLevel>();
    public List<ClickUpLevel> clickUpSpaceList = new List<ClickUpLevel>();
    public List<ClickUpLevel> clickUpAIList = new List<ClickUpLevel>();

    public List<Achievements> achievementsList = new List<Achievements>();
    public List<bool> clearachievementList = new List<bool>();
    public List<bool> cleareventList = new List<bool>();
}
