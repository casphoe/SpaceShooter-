using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHp : CCompo
{

    public Vector2 pos;

    private void Start()
    {
        PlayerManager.instance.BH = GetComponent<BarrierHp>();
        PlayerManager.instance.IsBarrier = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        pos = PlayerManager.instance.PlayerDir;
        transform.position = pos;
    }

    public void TakeDamage()
    {
        Function.LateCallFunc(this, 0.7f, (CCompo) =>
        {
            PlayerManager.instance.IsBarrier = false;
            gameObject.SetActive(false);
        });
    }
}
