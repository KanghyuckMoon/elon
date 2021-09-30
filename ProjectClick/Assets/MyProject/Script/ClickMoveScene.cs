using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickMoveScene : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private GameObject language;
    [SerializeField]
    private Text languageText;
    [SerializeField] Image progressBar;
    private bool click;
    public bool getstart;

    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        StartCoroutine(LoadScene());
    }

    private void Update()
    {
        if (!getstart) return;
        if(Input.GetMouseButtonDown(0))
        {
            if (click) return;
            click = true;
            language.SetActive(true);
        }
    }

    public void SelectLauguage()
    {
        languageText.text = "지원되는 언어가 아닙니다.";
        Invoke("ResetLauguage",1);
    }
    private void ResetLauguage()
    {
        languageText.text = "언어를 선택해주세요";
    }
    IEnumerator LoadScene()
    {
        yield return null; 
        AsyncOperation op = SceneManager.LoadSceneAsync("ClickScene");
        op.allowSceneActivation = false; 
        float timer = 0.0f; 
        while (!op.isDone)
        {
            yield return null; 
            if(click)
            {
                timer += Time.deltaTime * 0.1f;
                if (op.progress < 0.9f)
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                    if (progressBar.fillAmount >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                    if (progressBar.fillAmount == 1.0f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}
