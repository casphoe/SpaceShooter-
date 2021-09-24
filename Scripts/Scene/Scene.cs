using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : CCompo
{
    public override void Awake()
    {
        base.Awake();
    }

    public virtual void Update()
    {
        /*
         * 유니티의 모든 게임 객체는 자기 자신이 포함 되어있는 씬에 접근하는 것이 가능
         * (모든 게임 객체는 scene 프로퍼티를 지니고 있음)
         * 
         */

        var StartScene = this.gameObject.scene;

        if(Input.GetKeyDown(KeyCode.Escape) && !StartScene.name.Equals(CDefine.SCENE_NAME_SPACESHOOT_STARTSCENE))
        {
            SceneLoader.instance.LoadScene(CDefine.SCENE_NAME_SPACESHOOT_STARTSCENE);
        }
    }
}
