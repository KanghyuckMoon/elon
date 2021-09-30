using UnityEngine;
using UnityEngine.UI;

public class AchievementsPanel : MonoBehaviour
{
    [SerializeField]
    private Image achienvementsPanel;
    [SerializeField]
    private Text achienvementsName;
    [SerializeField]
    private Text achienvementsExText;

    private Achievements achievements;

    public void SetValue(Achievements achievements)
    {
        this.achievements = achievements;
        UpdateValuse();
    }

    public void UpdateValuse()
    {
        achienvementsName.text = achievements.achievementsName;
        achienvementsExText.text = achievements.achievementsExplanation;
        achienvementsPanel.sprite = Resources.Load(achievements.imageAddress, typeof(Sprite)) as Sprite;
        CheakImage();
    }

    public void CheakImage()
    {
        if(achievements.clear)
        {
            achienvementsPanel.color = new Color(1, 1, 1, 1);
        }
        else
        {
            achienvementsPanel.color = new Color(0, 0, 0, 1);
        }
    }
}
