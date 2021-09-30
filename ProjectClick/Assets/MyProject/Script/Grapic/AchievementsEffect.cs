using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AchievementsEffect : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;
    private List<bool> clearlist = new List<bool>();
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        clearlist = GameManager.Instance.CurrentData.clearachievementList;
    }

    public void ClearAchievements(int index)
    {
        if (clearlist[index]) return;
        else
        {
            image.sprite = Resources.Load(GameManager.Instance.CurrentData.achievementsList[index].imageAddress, typeof(Sprite)) as Sprite;
            text.text = GameManager.Instance.CurrentData.achievementsList[index].achievementsName;
            rectTransform.DOAnchorPosY(880, 1).OnComplete(() => GetComponent<RectTransform>().DOAnchorPosY(900, 1).OnComplete(() => GetComponent<RectTransform>().DOAnchorPosY(2000, 1)));
            GameManager.Instance.CurrentData.clearachievementList[index] = true;
            clearlist[index] = true;
        }
    }
}
