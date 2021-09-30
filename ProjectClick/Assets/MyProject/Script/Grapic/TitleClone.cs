using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleClone : MonoBehaviour
{
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private float fllowTime = 1f;
    [SerializeField]
    private float size;

    [SerializeField]
    private bool endAnimationOn;
    [SerializeField]
    private string AnimationName;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (endAnimationOn)
        {
            animator = GetComponent<Animator>();
            animator.enabled = false;
            Invoke("AnimationOn", fllowTime);
        }
        transform.DOMove(parent.position, fllowTime);
        transform.DOScale(size, fllowTime);
        
    }

    private void AnimationOn()
    {
        animator.enabled = true;
        animator.SetBool(AnimationName, true);
    }

}
