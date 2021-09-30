using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTr;

    public Slider tabSlider;
    public RectTransform[] BtnRect, BtnImageRect;

    [SerializeField]
    private int SIZE = 4;
    float[] pos = new float[4];
    float distance, curPos, targetPos;
    bool isDrag;
    public int targetIndex;
    [SerializeField]
    private List<Sprite> tabsprites = new List<Sprite>();
    [SerializeField]
    private Image tabimage;
    [SerializeField]
    private RectTransform TabBar;




    void Start()
    {
        SIZE = GameManager.Instance.CurrentData.size;
        ActiveGameManagertoPanel();
    }

    public void ActivePanel()
    {
        if (SIZE >= 4) return;
        SIZE++;
        GameManager.Instance.CurrentData.size = SIZE;
        contentTr.GetChild(SIZE - 1).gameObject.SetActive(true);
        BtnRect[SIZE - 1].gameObject.SetActive(true);
        BtnImageRect[SIZE - 1].gameObject.SetActive(true);
        SetChildList();
        targetIndex++;
        curPos = SetPos();
        VerticalScrollUp();
    }

    public void ActiveGameManagertoPanel()
    {
        SIZE = GameManager.Instance.CurrentData.size;
        for (int i = 0; i < SIZE; i++)
        {
            contentTr.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < 4; i++)
        {
            BtnRect[i].gameObject.SetActive(false);
            BtnImageRect[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < SIZE; i++)
        {
            BtnRect[i].gameObject.SetActive(true);
            BtnImageRect[i].gameObject.SetActive(true);
        }

        if (SIZE == 1)
        {
            pos[0] = 0;
            distance = 0;
        }
        else
        {
            distance = 1f / (SIZE - 1);
            for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
        }
    }

    public void SetChildList()
    {
        SIZE = 0;
        for(int i = 0; i < contentTr.childCount; i++)
        {
            if(contentTr.GetChild(i).gameObject.activeSelf)
            {
                SIZE++;
            }
        }
        for(int i = 0; i < 4;i++)
        {
            BtnRect[i].gameObject.SetActive(false);
            BtnImageRect[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < SIZE; i++)
        {
            BtnRect[i].gameObject.SetActive(true);
            BtnImageRect[i].gameObject.SetActive(true);
        }

        if (SIZE == 1)
        {
            pos[0] = 0;
            distance = 0;
        }
        else
        {
            distance = 1f / (SIZE - 1);
            for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
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
                    //Debug.Log(pos[i] + distance * 0.5f);
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
                if (SIZE == 1) return;
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
                if (SIZE == 1) return;
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

        VerticalScrollUp();
    }

    void VerticalScrollUp()
    {
        // 목표가 수직스크롤이고, 옆에서 옮겨왔다면 수직스크롤을 맨 위로 올림
        for (int i = 0; i < SIZE; i++)
            if (contentTr.GetChild(i).GetComponent< ScrollScript>() && curPos != pos[i] && targetPos == pos[i])
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
    }


    void Update()
    {
        tabSlider.value = scrollbar.value;
        tabimage.sprite = tabsprites[targetIndex];

        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);

            // 목표 버튼은 크기가 커짐
            for (int i = 0; i < SIZE; i++)
            {
                BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? (1440/SIZE) + (10 * SIZE * (SIZE - 1 <= 0 ? 1:SIZE-1)) : (1440 / SIZE) - (10 * SIZE), BtnRect[i].sizeDelta.y);
                TabBar.sizeDelta = new Vector2((1440 / SIZE) + (10 * SIZE * (SIZE - 1 <= 0 ? 1 : SIZE - 1)),-34);
                tabSlider.GetComponent<RectTransform>().sizeDelta = new Vector2( ReturnToTabSliderSize(),226.4f);
            }
        }


        if (Time.time < 0.1f) return;

        for (int i = 0; i < SIZE; i++)
        {
            // 버튼 아이콘이 부드럽게 버튼의 중앙으로 이동, 크기는 1, 텍스트 비활성화
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = new Vector3(0.8f,0.8f,1);
            //bool textActive = false;

            // 선택한 버튼 아이콘은 약간 위로 올리고, 크기도 키우고, 텍스트도 활성화
            if (i == targetIndex)
            {
                BtnTargetPos.y = -23f;
                BtnTargetScale = new Vector3(1.4f, 1.4f, 1);
                //textActive = true;
            }

            BtnImageRect[i].anchoredPosition3D = Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);
            BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, BtnTargetScale, 0.25f);
            //BtnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }
    }

    private float ReturnToTabSliderSize()
    {
        switch(SIZE)
        {
            case 1:
                return 0;
            case 2:
                return 700;
            case 3:
                return 900;
            case 4:
                return 960;
            default:
                return 0;
        }
    }


    public void TabClick(int n)
    {
        curPos = SetPos();
        targetIndex = n;
        targetPos = pos[n];
        VerticalScrollUp();
    }
}
