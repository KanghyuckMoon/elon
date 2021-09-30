using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LayserBeem : MonoBehaviour
{

    private Image layserImage;

    public void Show(Transform poolManager)
    {


        layserImage = GetComponent<Image>();
        layserImage.gameObject.SetActive(true);
        layserImage.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        RectTransform rectTransform = GetComponent<RectTransform>();
        float targetPositionY = rectTransform.anchoredPosition.y + 1200f;

        layserImage.DOFade(0f, 1f).OnComplete(() => Despawn(poolManager));
        rectTransform.DOAnchorPosY(targetPositionY, 1f);
    }

    private void Despawn(Transform poolManager)
    {
        layserImage.DOFade(1f, 0f);
        transform.SetParent(poolManager);
        layserImage.gameObject.SetActive(false);
    }
}
