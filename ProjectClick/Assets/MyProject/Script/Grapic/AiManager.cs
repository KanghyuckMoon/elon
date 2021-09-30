using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AiManager : MonoBehaviour
{
    [SerializeField]
    private Image enemyAIUImage;
    [SerializeField]
    private Image playerUImage;
    [SerializeField]
    private List<Sprite> enemySprites;
    [SerializeField]
    private List<Sprite> playerSprites;
    [SerializeField]
    private Image Background;
    [SerializeField]
    private Image playerHPUI;
    [SerializeField]
    private Image enemyHPUI;
    [SerializeField]
    private UIManager uIManager;
    [SerializeField]
    private BGMManager bGM;
    [SerializeField]
    private NestedScrollManager scrollManager;

    private float enemyHp;
    private float enemyMaxHp;
    private float enemyDamage;
    private float playerHp;
    private float playerMaxHp;
    private float playerDamage;
    private float playerCriticalPersent;
    private long step = 0;

    [SerializeField]
    private Image koImage;
    [SerializeField]
    private AudioClip[] audioClip;
    private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(Battle());
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator Battle()
    {
        while(true)
        {
            ResetState();
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                if(playerHp <= 0 || enemyHp <= 0)
                {
                    break;
                }
                //인간의 공격
                HumanAttack();
                yield return new WaitForSeconds(0.75f);
                if (playerHp <= 0 || enemyHp <= 0)
                {
                    break;
                }
                //AI의 공격
                AIAttack();
                yield return new WaitForSeconds(0.75f);
            }
            if (playerHp > 0)
            {
                Win();
                yield return new WaitForSeconds(1);
                ChangeAI();
            }
        }
    }

    private void HumanAttack()
    {
        playerUImage.rectTransform.DOAnchorPosX(600, 0.25f).OnComplete(() => playerUImage.rectTransform.DOAnchorPosX(-600, 0.25f));
        enemyHp -= playerDamage;
        float random = Random.Range(0.0f, 1.0f);
        if(random <= playerCriticalPersent)
        {
            Debug.Log("Critical");
            enemyHp -= playerDamage * (GameManager.Instance.CurrentData.clickUpAIList[2].amount + 1);
        }
        else
        {
            enemyHp -= playerDamage;
        }
        enemyAIUImage.rectTransform.DOShakeAnchorPos(0.25f, 20);
        Background.rectTransform.DOShakeAnchorPos(0.25f, 20);
        enemyHPUI.fillAmount = enemyHp / enemyMaxHp;
        if(!bGM.ReturnisEvent() && scrollManager.targetIndex == 3)
        {
            audioSource.clip = audioClip[0];
            audioSource.Play();
        }
    }
    private void AIAttack()
    {
        enemyAIUImage.rectTransform.DOAnchorPosX(-600,0.25f).OnComplete(() => enemyAIUImage.rectTransform.DOAnchorPosX(600, 0.25f)); ;
        playerHp -= enemyDamage;
        playerUImage.rectTransform.DOShakeAnchorPos(0.25f, 20);
        Background.rectTransform.DOShakeAnchorPos(0.25f, 20);
        playerHPUI.fillAmount = playerHp / playerMaxHp;
        if (!bGM.ReturnisEvent() && scrollManager.targetIndex == 3)
        {
            audioSource.clip = audioClip[0];
            audioSource.Play();
        }
    }

    private void Win()
    {
        koImage.gameObject.SetActive(true);
        uIManager.AchievementsCheak();
        step = ++GameManager.Instance.CurrentData.neuralink;
        if (!bGM.ReturnisEvent() && scrollManager.targetIndex == 3)
        {
            audioSource.clip = audioClip[1];
            audioSource.Play();
        }
    }

    private void ChangeAI()
    {
        enemyAIUImage.sprite = enemySprites[Random.Range(0, 3)];
    }

    private void ResetState()
    {
        koImage.gameObject.SetActive(false);
        playerMaxHp = (100 * (GameManager.Instance.CurrentData.clickUpAIList[1].amount + 1));
        playerHp = playerMaxHp;
        playerDamage = 10 + GameManager.Instance.CurrentData.clickUpAIList[0].amount;
        playerCriticalPersent = (float)GameManager.Instance.CurrentData.clickUpAIList[3].amount / 100;
        enemyMaxHp = (step + 1) * 120;
        enemyHp = enemyMaxHp;
        enemyDamage = 10 + (step) * (step);
        playerHPUI.fillAmount = playerHp / playerMaxHp;
        enemyHPUI.fillAmount = enemyHp / enemyMaxHp;
    }
}
