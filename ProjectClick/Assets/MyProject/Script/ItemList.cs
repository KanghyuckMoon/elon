using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemList : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTr;

    public Slider tabSlider;
    [SerializeField]
    public RectTransform[] BtnImageRect;

    [SerializeField]
    private int SIZE = 0;
    float[] pos = new float[6];
    float distance, curPos, targetPos;
    bool isDrag;
    [SerializeField]
    int targetIndex;




    void Start()
    {
        // 거리에 따라 0~1인 pos대입
        //SetChildList();
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++)
        {   
            pos[i] = distance * i;
        }
    }

    public void SizeMinuse()
    {
        SIZE--;
        BtnImageRect[4].gameObject.SetActive(false);
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++)
        {
            pos[i] = distance * i;
        }
    }

    float SetPos()
    {
        // 절반거리를 기준으로 가까운 위치를 반환
        for (int i = 0; i < SIZE; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                if (scrollbar.value > 1)
                {
                    return targetPos;
                }
                else
                {
                    targetIndex = i;
                    return pos[i];
                }
            }
        return 0;
    }


    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();

    public void OnDrag(PointerEventData eventData) => isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        // 절반거리를 넘지 않아도 마우스를 빠르게 이동하면
        if (curPos == targetPos)
        {
            // ← 으로 가려면 목표가 하나 감소
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                if (scrollbar.value > 1)
                {
                }
                else
                {
                    --targetIndex;
                    targetPos = curPos - distance;
                }

            }

            // → 으로 가려면 목표가 하나 증가
            else if (eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                if (scrollbar.value > 1)
                {
                }
                else
                {
                    targetPos = curPos + distance;

                }
            }
        }
    }


    void Update()
    {

        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
        }


        if (Time.time < 0.1f) return;

        for (int i = 0; i < SIZE; i++)
        {
            if (i == targetIndex)
            {
                BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, new Vector3(2f, 2f, 1), 0.25f);
            }
            else
            {
                BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, new Vector3(1,1,1), 0.25f);
            }

        }
    }
}
