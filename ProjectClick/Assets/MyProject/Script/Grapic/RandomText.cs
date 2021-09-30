using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomText : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private List<string> randomTextList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = RandomtoText();
    }

    private string RandomtoText()
    {
        int i = Random.Range(0,randomTextList.Count);
        return randomTextList[i];
    }
}
