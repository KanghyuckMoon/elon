using UnityEngine;
using UnityEngine.UI;


public class UpgradePanel : MonoBehaviour
{
    [SerializeField]
    private ClickManager clickManager;
    [SerializeField]
    private NestedScrollManager nestedScroll;
    [SerializeField]
    private ItemList itemlist;
    [SerializeField]
    private Text itemNameText = null;
    [SerializeField]
    private Text itemTextExplanation = null;
    [SerializeField]
    private Text amountText = null;
    [SerializeField]
    private Text priceText = null;
    [SerializeField]
    private Text priceTextx10 = null;
    [SerializeField]
    private GameObject buttonx10 = null;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Image BackgroundImage;
    [SerializeField]
    private UIManager uIManager = null;
    private string typePriceString;

    private ClickUpLevel clickup = null;

    public void SetValue(ClickUpLevel data_clickup)
    {
        gameObject.SetActive(true);
        this.clickup = data_clickup;
        SetPriceType();
        UpdateValuse();
    }

    public void UpdateValuse()
    {
        itemNameText.text = clickup.upgradeName;
        itemTextExplanation.text = clickup.upgradeExplanation;
        SetPrice(1);
        SetTextPrice(priceText, 1);
        MaxPriceCheak(priceText, 0);
        itemImage.sprite = Resources.Load(clickup.imageAddress, typeof(Sprite)) as Sprite;

        if(clickup.eventindex == 0)
        {
            amountText.text = string.Format("X{0}", clickup.amount);
            BackgroundImage.color = new Color(0.5f,0.57f,1);
            SetTextPrice(priceTextx10, 12.5f);
            MaxPriceCheak(priceTextx10, 9);
        }
    }

    private void SetTextPrice(Text textPrice, float multiplyPrice)
    {
        textPrice.text = string.Format("{0}{1}", (long)(clickup.price * multiplyPrice), typePriceString);
    }

    private void SetPriceType()
    {
        switch (clickup.type)
        {
            case 0:
                typePriceString = "애정";
                break;

            case 1:
                typePriceString = "코인";
                break;

            case 2:
                typePriceString = "기술";
                break;

            case 3:
                typePriceString = "칩";
                break;
        }
        if(clickup.eventindex == 1)
        {
            amountText.gameObject.SetActive(false);
            priceTextx10.gameObject.SetActive(false);
            buttonx10.gameObject.SetActive(false);
            if(clickup.amount > 0)
            {
                StartOpenUp();
            }
        }
    }

    private void MaxPriceCheak(Text text,int plusAmount)
    {
        if(clickup.amount + plusAmount >= clickup.maxamount)
        {
            text.text = string.Format("MAX");
        }
    }

    public void OnClickBuy(int plusAmount)
    {
        Data money = GameManager.Instance.CurrentData;
        if ((clickup.amount + plusAmount) > clickup.maxamount) return;
        switch (clickup.type)
        {
            case 0:
                if (money.heart < clickup.price * plusAmount) return;
                money.heart -= clickup.price * plusAmount;
                break;

            case 1:
                if (money.dogecoin < clickup.price * plusAmount) return;
                money.dogecoin -= clickup.price * plusAmount;
                break;

            case 2:
                if (money.ufo < clickup.price * plusAmount) return;
                money.ufo -= clickup.price * plusAmount;
                break;

            case 3:
                if (money.neuralink < clickup.price * plusAmount) return;
                money.neuralink -= clickup.price * plusAmount;
                break;
        }
        PurchaseMoney(plusAmount);
    }

    private void PurchaseMoney(int value)
    {
        clickup.amount += 1 * value;
        uIManager.AchievementsCheak();
        clickManager.UpdataUI();
        clickManager.UpgradeElonImage();


        if(clickup.eventindex == 1)
        {
            nestedScroll.ActivePanel();
            itemlist.SizeMinuse();
            gameObject.SetActive(false);
        }
        else
        {
            SetPrice(value);
            UpdateValuse();
        }

    }

    private void SetPrice(int value)
    {
        clickup.price = clickup.referenceprice;
        for (int i = 0; i < clickup.amount + value - 1; i++)
        {
        clickup.price = (long)(clickup.price * 1.1f);
        }
    }

    public void StartOpenUp()
    {
        clickManager.UpdataUI();
        itemlist.SizeMinuse();
        gameObject.SetActive(false);
    }
}
