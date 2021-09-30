using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ClickEffect : MonoBehaviour
{
    [SerializeField]
    private float fllowTime = 1f;
    [SerializeField]
    private float size = 1f;
    [SerializeField]
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Show()
    {
        image.color = new Color(1f, 1f, 1f, 1f);

        transform.localScale = Vector2.zero;
        transform.DOScale(size, fllowTime);
        image.DOFade(0f, fllowTime).OnComplete(() => SetActiveFalse());
    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
