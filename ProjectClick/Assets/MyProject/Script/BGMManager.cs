using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private bool isEvent = false;

    [SerializeField]
    private List<AudioClip> eventAudioList = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> nowAudioList = new List<AudioClip>();
    [SerializeField]
    private NestedScrollManager scroll;

    private int nowmusic = -1;
    private AudioSource audioSource;
    private List<bool> clearlist = new List<bool>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        clearlist = GameManager.Instance.CurrentData.cleareventList;
    }

    private void Update()
    {
        SetMusic(); 
    }

    public bool ReturnisEvent()
    {
        return isEvent;
    }



    public void EndEvent()
    {
        isEvent = false;
        SetMusic();
    }

    public void StartEventMusic(int index)
    {
        if (clearlist[index]) return;
        isEvent = true;
        nowmusic = -1;
        if(eventAudioList[index] == null)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.clip = eventAudioList[index];
            audioSource.Play();
        }
    }

    private void SetMusic()
    {
        if (isEvent) return;
        if (nowmusic == scroll.targetIndex) return;
        else
        {
        nowmusic = scroll.targetIndex;
        audioSource.clip = nowAudioList[nowmusic];
        audioSource.Play();
        }
    }
}
