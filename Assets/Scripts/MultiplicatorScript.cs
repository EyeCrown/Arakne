using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplicatorScript : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private TextMeshPro tmp;
    [SerializeField] private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DerstroyTimer());
        //SetValue(GameManager.Instance.multiplier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue( int val)
    {
        value = val;
        tmp.text = "x" + value;
    }

    IEnumerator DerstroyTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
