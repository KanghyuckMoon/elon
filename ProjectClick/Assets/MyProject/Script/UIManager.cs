using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject upgradePanelLevelTemplate = null;
    [SerializeField]
    private GameObject upgradePanelGlobalTemplate = null;
    [SerializeField]
    private GameObject upgradePanelSpaceTemplate = null;
    [SerializeField]
    private GameObject upgradePanelAITemplate = null;
    [SerializeField]
    private GameObject achievements = null;
    [SerializeField]
    private GameObject achievementsbackground = null;
    [SerializeField]
    private AchievementsEffect achievementsEffect;
    [SerializeField]
    private EventManager eventManager;
    [SerializeField]
    private BGMManager bGMManager;

    private List<UpgradePanel> upgradePanelList = new List<UpgradePanel>();
    private List<AchievementsPanel> achievementsPanelList = new List<AchievementsPanel>();
    private bool achievementsOnoff = false;
    private bool guideOnoff = false;

    [SerializeField]
    private Text guideText;
    [SerializeField]
    private GameObject guide;
    [SerializeField]
    private string[] guidetextlist;
    [SerializeField]
    private NestedScrollManager scrollManager;
    [SerializeField]
    private GameObject endGamePanel;
    private bool endGameOn;

    private void Start()
    {
        CreatePanel();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            endGameOn = !endGameOn;
            endGamePanel.SetActive(endGameOn);
        }
    }

    public void OnClickEndGame()
    {
        Application.Quit();
    }
    public void OnClickEndGameOff()
    {
        endGameOn = false;
        endGamePanel.SetActive(endGameOn);
    }

    private void CreatePanel()
    {
        Data data = GameManager.Instance.CurrentData;
        ForeachInClickupList(data.clickUpLevelList, upgradePanelLevelTemplate);
        ForeachInClickupList(data.clickUpGloballList, upgradePanelGlobalTemplate);
        ForeachInClickupList(data.clickUpSpaceList, upgradePanelSpaceTemplate);
        ForeachInClickupList(data.clickUpAIList, upgradePanelAITemplate);
        ForeachInClickupListAchievenments();
    }

    private void ForeachInClickupList(List<ClickUpLevel> data,GameObject templeateObj)
    {
        GameObject newPanel = null;
        UpgradePanel newPanelComponent = null;
        foreach (ClickUpLevel clickup in data)
        {
            newPanel = Instantiate(templeateObj, templeateObj.transform.parent);
            newPanelComponent = newPanel.GetComponent<UpgradePanel>();
            newPanelComponent.SetValue(clickup);
            upgradePanelList.Add(newPanelComponent);
        }
    }
    private void ForeachInClickupListAchievenments()
    {
        GameObject newPanel = null;
        AchievementsPanel newPanelComponent = null;
        foreach (Achievements clickup in GameManager.Instance.CurrentData.achievementsList)
        {
            newPanel = Instantiate(achievements, achievements.transform.parent);
            newPanelComponent = newPanel.GetComponent<AchievementsPanel>();
            newPanelComponent.SetValue(clickup);
            newPanel.SetActive(true);
            achievementsPanelList.Add(newPanelComponent);
        }
    }

    public void AchievementsCheak()
    {
        Data data = GameManager.Instance.CurrentData;

        AchievementsEachCheak(data,0,0,0,0,false); // ���� �Ϸиӽ�ũ
        AchievementsEachCheak(data, 0, 0, 9, 1, true); // �ֱ� �Ϸиӽ�ũ
        AchievementsEachCheak(data, 0, 1, 9, 2, true); // ����� �÷��̾�
        AchievementsEachCheak(data, 0, 2, 9, 3, true); // ���ڰ� �ɰž�
        AchievementsEachCheak(data, 0, 3, 9, 4, true); // �ȳ��ϼ��� �Ϸиӽ�ũ�Դϴ�
        AchievementsEachCheak(data, 0, 4, 0, 5, true); // �����

        AchievementsEachCheak(data, 1, 3, 0, 6, false); // ���� �ְ��� �ڵ��� ����Ŀ
        AchievementsEachCheak(data.dogecoin, -10000, 7, false, true); // ����
        AchievementsEachCheak(data.dogecoin, 10000, 8, false, false); // ����
        AchievementsEachCheak(data, 1, 4, 0, 9, true); // ȭ�������ϱ�

        AchievementsEachCheak(data.ufo, 0, 10, false, false); // ����
        AchievementsEachCheak(data, 2, 2, 0, 11, true); // ���� �����ݾ�
        AchievementsEachCheak(data, 2, 4, 0, 12, true); // �߸��� �Ǽ�

        AchievementsEachCheak(data.neuralink, 1, 13, true, false); // �η��� ������Ų��
        AchievementsEachCheak(data, 3, 2, 0, 14, false); // ���̿����̷и�
        AchievementsEachCheak(data, 3, 3, 0, 15, true); // 21���� �ʸ��� ���
        AchievementsEachCheak(data, 3, 4, 0, 16, true); // ����

        for (int i = 0; i < achievementsPanelList.Count;i++)
        {
            achievementsPanelList[i].CheakImage();
        }
    }
    private void AchievementsEachCheak(Data data,int type, int index, int value, int eventindex, bool eventOn)
    {
        switch(type)
        {
            case 0:
                if(data.clickUpLevelList[index].amount > value)
                {
                    GameManager.Instance.CurrentData.achievementsList[eventindex].clear = true;
                    achievementsEffect.ClearAchievements(eventindex);
                    if(eventOn)
                    {
                        eventManager.EventOn(eventindex);
                        bGMManager.StartEventMusic(eventindex);
                    }
                    GameManager.Instance.CurrentData.cleareventList[eventindex] = true;
                }
                break;
            case 1:
                if (data.clickUpGloballList[index].amount > value)
                {
                    GameManager.Instance.CurrentData.achievementsList[eventindex].clear = true;
                    achievementsEffect.ClearAchievements(eventindex);
                    if (eventOn)
                    {
                        eventManager.EventOn(eventindex);
                        bGMManager.StartEventMusic(eventindex);
                    }
                    GameManager.Instance.CurrentData.cleareventList[eventindex] = true;
                }
                break;
            case 2:
                if (data.clickUpSpaceList[index].amount > value)
                {
                    GameManager.Instance.CurrentData.achievementsList[eventindex].clear = true;
                    achievementsEffect.ClearAchievements(eventindex);
                    if (eventOn)
                    {
                        eventManager.EventOn(eventindex);
                        bGMManager.StartEventMusic(eventindex);
                    }
                    GameManager.Instance.CurrentData.cleareventList[eventindex] = true;
                }
                break;
            case 3:
                if (data.clickUpAIList[index].amount > value)
                {
                    GameManager.Instance.CurrentData.achievementsList[eventindex].clear = true;
                    achievementsEffect.ClearAchievements(eventindex);
                    if (eventOn)
                    {
                        eventManager.EventOn(eventindex);
                        bGMManager.StartEventMusic(eventindex);
                    }
                    GameManager.Instance.CurrentData.cleareventList[eventindex] = true;
                }
                break;
        }
    }
    private void AchievementsEachCheak(long data, int value, int eventindex, bool eventOn, bool minuseOn)
    {
        if(minuseOn)
        {
            if (data < value)
            {
                GameManager.Instance.CurrentData.achievementsList[eventindex].clear = true;
                achievementsEffect.ClearAchievements(eventindex);
                if (eventOn)
                {
                    eventManager.EventOn(eventindex);
                    bGMManager.StartEventMusic(eventindex);
                }
                GameManager.Instance.CurrentData.cleareventList[eventindex] = true;
            }
        }
        else
        {
            if (data > value)
            {
                GameManager.Instance.CurrentData.achievementsList[eventindex].clear = true;
                achievementsEffect.ClearAchievements(eventindex);
                if (eventOn)
                {
                    eventManager.EventOn(eventindex);
                    bGMManager.StartEventMusic(eventindex);
                }
                GameManager.Instance.CurrentData.cleareventList[eventindex] = true;
            }
        }   
    }


    public void ClickOnOffAhieven()
    {
        achievementsOnoff = !achievementsOnoff;
        if (achievementsOnoff)
        {
            achievementsbackground.SetActive(true);
            guide.SetActive(false);
            guideOnoff = false;
        }
        else
        {
            guide.SetActive(false);
            achievementsbackground.SetActive(false);
        }
    }
    public void ClickOnOffGuide()
    {
        guideOnoff = !guideOnoff;
        if (guideOnoff)
        {
            achievementsbackground.SetActive(false);
            guide.SetActive(true);
            guideText.text = guidetextlist[scrollManager.targetIndex].Replace("\\n", "\n");
            achievementsOnoff = false;
        }
        else
        {
            guide.SetActive(false);
            achievementsbackground.SetActive(false);
        }
    }
}
