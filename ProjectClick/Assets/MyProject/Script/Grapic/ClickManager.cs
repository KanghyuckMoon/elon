using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private NestedScrollManager nestedScroll;

    [SerializeField]
    private GameObject clickEffectPrefeb;

    [SerializeField]
    private StockManager stockManager;

    [SerializeField]
    private UIManager uIManager;

    private Vector2 mousePosition;

    [SerializeField]
    private Text heartText;
    [SerializeField]
    private Text dogeCoinText;
    [SerializeField]
    private Text ufoText;
    [SerializeField]
    private Text neuralinkText;

    private bool endLevelUp;
    [SerializeField]
    private Image Elononsdaf;

    [SerializeField]
    private List<Sprite> elonImage = new List<Sprite>();

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips;

    [SerializeField]
    private Sprite[] clickEffectSprite;
    private int clickcount = 0;
    private float clickTime = 0;
    private bool macronot = false;
    private int macrocount = 0;
    [SerializeField]
    private Text macrotext;
    [SerializeField]
    private GameObject macroObject = null;

    private void Start()
    {
        UpdataUI();
        InvokeRepeating("UpSecondHeart",0f,1f);
        audioSource = GetComponent<AudioSource>();
        UpgradeElonImage();
    }

    public void OnClick()
    {
        if (macronot) return;
        UpMoney();
        UpdataUI();
        uIManager.AchievementsCheak();
    }

    private void UpMoney()
    {
        clickcount++;
        switch(nestedScroll.targetIndex)
        {
            case 0:
                UpHeart();
                Pooling();
                break;
            case 1:
                UpDogeCoin();
                Pooling();
                break;
            case 2:
                UpUfo();
                break;
            case 3:
                UpAi();
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        clickTime += Time.deltaTime;
        if(clickTime > 1)
        {
            if(clickcount > 25)
            {
                macrocount++;
                macronot = true;
                macroObject.SetActive(true);
                Invoke("MacroFalse", 5);
                if(macrocount > 3)
                {
                    GameManager.Instance.CurrentData.heart = 0;
                    GameManager.Instance.CurrentData.ufo = 0;
                    GameManager.Instance.CurrentData.neuralink = 0;
                    GameManager.Instance.CurrentData.dogecoin = 0;
                    macrocount = 0;
                }
            }
            clickcount = 0;
            clickTime = 0;
        }
    }

    private void MacroFalse()
    {
        macronot = false;
        macroObject.SetActive(false);
    }    

    private void UpHeart()
    {
        GameManager.Instance.CurrentData.heart += GameManager.Instance.CurrentData.clickUpLevelList[0].amount + 1;
        GameManager.Instance.CurrentData.heart += (long)((GameManager.Instance.CurrentData.clickUpLevelList[0].amount + 1) *
            GameManager.Instance.CurrentData.clickUpLevelList[1].amount * 0.1f);
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    public void UpSecondHeart()
    {
        GameManager.Instance.CurrentData.heart += GameManager.Instance.CurrentData.clickUpLevelList[2].amount;
        GameManager.Instance.CurrentData.heart += (long)((GameManager.Instance.CurrentData.clickUpLevelList[2].amount) *
            GameManager.Instance.CurrentData.clickUpLevelList[3].amount * 0.1f);
        UpdataUI();
    }

    private void UpDogeCoin()
    {
        GameManager.Instance.CurrentData.dogecoin += (long)((GameManager.Instance.CurrentData.heart * (stockManager.value / 100)) * (GameManager.Instance.CurrentData.clickUpLevelList[2].amount + 1));
        GameManager.Instance.CurrentData.heart *=  (long)(GameManager.Instance.CurrentData.clickUpGloballList[1].amount * 0.001f);
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }
    private void UpUfo()
    {

    }
    private void UpAi()
    {

    }


    public void UpdataUI()
    {
        heartText.text = string.Format("{0}", GameManager.Instance.CurrentData.heart);
        dogeCoinText.text = string.Format("{0}", GameManager.Instance.CurrentData.dogecoin);
        ufoText.text = string.Format("{0}", GameManager.Instance.CurrentData.ufo);
        neuralinkText.text = string.Format("{0}", GameManager.Instance.CurrentData.neuralink);
    }


    private GameObject Pooling()
    {
        GameObject obj = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).gameObject.activeSelf)
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.GetChild(i).position = mousePosition;
                obj = transform.GetChild(i).gameObject;
                obj.SetActive(true);
                if (nestedScroll.targetIndex == 1 && stockManager.value <= 1)
                {
                    obj.GetComponent<Image>().sprite = clickEffectSprite[2];
                }
                else
                {
                    obj.GetComponent<Image>().sprite = clickEffectSprite[nestedScroll.targetIndex];
                }
                obj.GetComponent<ClickEffect>().Show();
                return obj;
            }
        }

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        obj = Instantiate(clickEffectPrefeb, mousePosition, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        if (nestedScroll.targetIndex == 1 && stockManager.value <= 1)
        {
            obj.GetComponent<Image>().sprite = clickEffectSprite[2];
        }
        else
        {
            obj.GetComponent<Image>().sprite = clickEffectSprite[nestedScroll.targetIndex];
        }
        obj.GetComponent<ClickEffect>().Show();
        return obj;
    }

    public void UpgradeElonImage()
    {
        if (endLevelUp) return;
        if(GameManager.Instance.CurrentData.clickUpLevelList[3].amount > 9)
        {
            Elononsdaf.sprite = elonImage[4];
        }
        else if(GameManager.Instance.CurrentData.clickUpLevelList[2].amount > 9)
        {

            Elononsdaf.sprite = elonImage[3];
        }
        else if (GameManager.Instance.CurrentData.clickUpLevelList[1].amount > 9)
        {

            Elononsdaf.sprite = elonImage[2];
        }
        else if (GameManager.Instance.CurrentData.clickUpLevelList[0].amount > 9)
        {

            Elononsdaf.sprite = elonImage[1];
        }
        Elononsdaf.SetNativeSize();
    }
}
