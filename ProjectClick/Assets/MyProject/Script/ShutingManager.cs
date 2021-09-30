using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShutingManager : MonoBehaviour
{
    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private Image subPlayerImage;
    [SerializeField]
    private Image enemyImage;
    [SerializeField]
    private Image enemyHpImage;
    [SerializeField]
    private Sprite[] enemySprites;
    [SerializeField]
    private Transform poolManager;

    [SerializeField]
    private LayserBeem Layser;
    [SerializeField]
    private Transform canvas;

    private long step = 0;
    private float enemyHp = 0;
    private float maxenemyHp = 100;
    private float playersDamage = 0;
    private float playersSkillDamage = 0;
    private float subPlayersDamage = 0;

    private bool isClick = false;
    public float minClickTime = 1;
    private float ClickTime = 0;
    private float delayTime = 0;
    private float skillcooltime = 0;
    [SerializeField]
    private Image skilcoolTimeImage;
    [SerializeField]
    private Image Background;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField]
    private NestedScrollManager scrollManager;
    [SerializeField]
    private BGMManager bGm;
    [SerializeField]
    private Sprite[] sprites;
    private Data data;

    private void Start()
    {
        data = GameManager.Instance.CurrentData;
        enemyHp = maxenemyHp;
        audioSource = GetComponent<AudioSource>();
        SetDamages();
    }

    private void SetDamages()
    {
        playersDamage = 10 * data.clickUpSpaceList[0].amount + 1;
        playersSkillDamage = 100 * data.clickUpSpaceList[1].amount;
        playersDamage += (long)((10 * data.clickUpSpaceList[0].amount + 1) *
            data.clickUpSpaceList[3].amount * 0.1f);
        if (data.clickUpSpaceList[2].amount == 0)
        {
            subPlayerImage.gameObject.SetActive(false);
        }
        else
        {

            subPlayerImage.gameObject.SetActive(true);
            subPlayersDamage = 5 * data.clickUpSpaceList[2].amount;
            subPlayersDamage += (long)((5 * data.clickUpSpaceList[2].amount + 1) *
                data.clickUpSpaceList[3].amount * 0.1f);
        }

    }

    private void Update()
    {
        if (scrollManager.targetIndex != 2) return;
        if (Input.GetMouseButtonDown(0)) ButtonDown();
        if (Input.GetMouseButtonUp(0)) ButtonUp();
        if(skillcooltime <= 2)
        {
            skilcoolTimeImage.fillAmount = skillcooltime * 0.5f;
        skillcooltime += Time.deltaTime;
        }
        if (isClick)
        {
            ClickTime += Time.deltaTime;
            delayTime += Time.deltaTime;
            SetDamages();
            if (minClickTime < ClickTime && delayTime > 0.1f)
            {
                LayserPool(0);
                if (data.clickUpSpaceList[2].amount > 0)
                {
                    LayserPoolSub();
                }
                Invoke("DamagedEnemy", 1);
                delayTime = 0;
            }
        }
        else
        {
            ClickTime = 0;
            delayTime = 0;
        }

    }

    public void ButtonUp()
    {
        isClick = false;
        if (minClickTime >= ClickTime)
        {
            if (skillcooltime < 2) return;
            LayserPool(1);
            skillcooltime = 0;
            Invoke("DamagedEnemyBySkill", 1);
        }
    }
    public void ButtonDown()
    {
        isClick = true;
    }

    private void LayserPool(int i)
    {
        LayserBeem newLayser = null;
        if (poolManager.childCount > 0)
        {
            newLayser = poolManager.GetChild(0).GetComponent<LayserBeem>();
        }
        else
        {
            newLayser = Instantiate(Layser, canvas.transform);
        }

        newLayser.transform.position = transform.position;
        newLayser.transform.SetParent(canvas);
        newLayser.Show(poolManager);
        newLayser.GetComponent<Image>().sprite = sprites[i];
        audioSource.clip = audioClips[0];
        if(!bGm.ReturnisEvent())
        {
        audioSource.Play();
        }
        Invoke("DamagedEnemy", 0.5f);
    }

    private void LayserPoolSub()
    {
        LayserBeem newLayser = null;
        if (poolManager.childCount > 0)
        {
            newLayser = poolManager.GetChild(0).GetComponent<LayserBeem>();
        }
        else
        {
            newLayser = Instantiate(Layser, canvas.transform);
        }

        newLayser.transform.position = subPlayerImage.transform.position;
        newLayser.transform.SetParent(canvas);
        newLayser.Show(poolManager);
        newLayser.GetComponent<Image>().sprite = sprites[0];
        Invoke("DamagedEnemy", 0.5f);
    }

    private void DamagedEnemy()
    {
        enemyHp -= playersDamage;
        enemyHp -= subPlayersDamage;
        enemyHpImage.fillAmount = enemyHp / maxenemyHp;
        Background.rectTransform.DOShakeAnchorPos(0.1f, 10).OnComplete(() => Background.rectTransform.DOAnchorPos(new Vector2(0,-242), 0.2f));
        enemyImage.rectTransform.DOShakeAnchorPos(0.1f, 10).OnComplete(() => enemyImage.rectTransform.DOAnchorPos(new Vector2(0, 441), 0.2f)); ;
        if (enemyHp <= 0)
        {
            step++;
            maxenemyHp = 100 + data.ufo * data.ufo;
            enemyImage.sprite = enemySprites[3];
            Invoke("InvokeSetEnemy", 0.1f);
        }
    }

    private void DamagedEnemyBySkill()
    {
        enemyHp -= playersDamage * 10;
        enemyHp -= subPlayersDamage * 5;
        enemyHpImage.fillAmount = enemyHp / maxenemyHp;
        Background.rectTransform.DOShakeAnchorPos(0.25f, 50).OnComplete(() => Background.rectTransform.DOAnchorPos(new Vector2(0, -242), 0.2f));
        enemyImage.rectTransform.DOShakeAnchorPos(0.25f, 50).OnComplete(() => enemyImage.rectTransform.DOAnchorPos(new Vector2(0, 441), 0.2f)); ;
        if (enemyHp <= 0)
        {
            step = ++data.shutingwin;
            maxenemyHp = 100 + step * step;
            enemyImage.sprite = enemySprites[3];
            Invoke("InvokeSetEnemy",0.1f);
        }
        audioSource.clip = audioClips[1];
        if (!bGm.ReturnisEvent())
        {
            audioSource.Play();
        }
    }

    private void InvokeSetEnemy()
    {
        enemyHp = maxenemyHp;
        enemyHpImage.fillAmount = enemyHp / maxenemyHp;
        data.ufo++;
        enemyImage.sprite = enemySprites[Random.Range(0, 3)];
    }
}
