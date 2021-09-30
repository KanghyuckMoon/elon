using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private List<EventPanel> eventlist = new List<EventPanel>();
    private List<bool> clearlist = new List<bool>();

    private void Start()
    {
        clearlist = GameManager.Instance.CurrentData.cleareventList;
    }
    public void EventOn(int index)
    {
        if (clearlist[index]) return;
        else
        {
            eventlist[index].gameObject.SetActive(true);
            eventlist[index].Show();    
        }
    }
}
