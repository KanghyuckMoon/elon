using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockManager : MonoBehaviour
{
    [SerializeField]
    private LineRenderer stock;
    public float value = 0;
    private float reapeattime = 0.05f;

    private void Start()
    {
        stock = GetComponent<LineRenderer>();
        StartCoroutine(stockGraph());
    }

    private IEnumerator stockGraph()
    {
        while(true)
        {
            for(int i = 1; i < 10; i++)
            {
                stock.SetPosition(i, new Vector2(100 * i, stock.GetPosition(i + 1).y));
            }
            value = Random.Range(-100, 200 + (GameManager.Instance.CurrentData.clickUpGloballList[3].amount * 5));
            stock.SetPosition(10, new Vector2(1000, value));
            if(value >= 0)
            {
                stock.startColor = Color.red;
                stock.endColor = Color.red;
            }
            else
            {
                stock.startColor = Color.blue;
                stock.endColor = Color.blue;
            }
            SetRepeatingTIme();
            yield return new WaitForSeconds(reapeattime);
        }
    }

    public void SetRepeatingTIme()
    {
        reapeattime = (GameManager.Instance.CurrentData.clickUpGloballList[0].amount + 4) * 0.025f;
    }
}
