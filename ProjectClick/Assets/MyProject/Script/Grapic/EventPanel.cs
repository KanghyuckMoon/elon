using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EventPanel : MonoBehaviour
{
    //���� ����Ʈ
    [SerializeField]
    private List<string> evenettext = new List<string>();
    //�ִϸ��̼� ����Ʈ
    [SerializeField]
    private List<string> eventanimation = new List<string>();
    [SerializeField]
    private List<float> eventanimationtime = new List<float>();
    [SerializeField]
    private List<float> eventtexttime = new List<float>();
    //���� ���� ����Ʈ
    [SerializeField]
    private List<int> evenetint = new List<int>();
    private Animator animator;
    [SerializeField]
    private Text evenettextcomponent;
    private int count = 0;
    private int animationcount = 0;
    private int textcount = 0;
    [SerializeField]
    private BGMManager bGMManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        if (count >= evenetint.Count)
        {
            bGMManager.EndEvent();
            gameObject.SetActive(false);
            return;
        }
        switch(evenetint[count])
        {
            case 0:
                //�ؽ�Ʈ ��� �Լ�
                TextAnimate();
                break;

            case 1:
                //�ִϸ��̼� ��� �Լ�
                Animate();
                break;
        }
        count++;
    }

    private void Animate()
    {
        animator.Play(eventanimation[animationcount]);
        Invoke("Show", eventanimationtime[animationcount]);
        animationcount++;
    }

    private void TextAnimate()
    {
        evenettextcomponent.DOText(evenettext[textcount],evenettext[textcount].Length * 0.1f);
        Invoke("Show", evenettext[textcount].Length * 0.1f + eventtexttime[textcount]);
        textcount++;
    }
}
