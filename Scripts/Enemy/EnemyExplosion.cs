using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : CCompo
{
    // Start is called before the first frame update
    void Start()
    {
        Function.LateCallFunc(this, 0.5f, (CCompo) =>
        {
            gameObject.SetActive(false);
        });
    }

   
}
