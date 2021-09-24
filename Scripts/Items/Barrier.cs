using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : CCompo
{
    private float Speed;
    Vector3 pos;
    
    public int Score;

    // Start is called before the first frame update
    void Start()
    {
        ItemManager.instance.ItemStat("Barrier");
        pos = transform.position;
        Speed = 18f;
        Score = ItemManager.instance.Score;
    }

    // Update is called once per frame
    void Update()
    {
        pos.y -= Speed * Time.deltaTime;
        transform.position = pos;
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
