using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossL : CCompo
{
    public ParticleSystem[] PlayPartcle = new ParticleSystem[3];

    private ParticleSystem BL;
    private BoxCollider2D BossLaserCollider;
    

    private void Start()
    {
        BL = GetComponent<ParticleSystem>();
        BossLaserCollider = gameObject.transform.parent.GetComponent<BoxCollider2D>();

        Function.LateCallFunc(this, 0.3f, (CCompo) =>
        {
            for(int i = 0; i < PlayPartcle.Length; i++)
            {
                PlayPartcle[i].Play();
            }
            BL.Play();
            BossLaserCollider.enabled = true;
        });
    }
}
