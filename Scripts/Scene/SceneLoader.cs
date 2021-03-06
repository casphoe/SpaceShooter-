using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //씬을 불러오기 위해서 SceneManagement이라는 네임 스페이스를 선언

public class SceneLoader : Singleton<SceneLoader>
{
    // 씬을 로드함
    public void LoadScene(string SceneName, float fDealy = 0.0f, LoadSceneMode c_LoadSceneMode = LoadSceneMode.Single)
    {
        //LoadSceneMode.Single 기존 씬을 제거하고 새로운 씬을 불려옴, LoadSceneMode.Additive은 기존 씬에다가 다른 씬을 추가해줌

        var Erator = this.SceneDoload(SceneName, fDealy, c_LoadSceneMode);

        StartCoroutine(Erator);
    }

    /*
     * SceneManager.LoadScene 함수를 사용하면 특정 씬을 로드하는 것이 가능하다.
     * 단, 해당 함수로 씬을 로드하기 위해서는 반드시 해당 씬이 File -> Build Settings 로딩할 씬이 포함되어 있어야한다.
     */

    public IEnumerator SceneDoload(string SceneName, float fDealy = 0.0f, LoadSceneMode c_LoadSceneMode = LoadSceneMode.Single)
    {
        yield return Function.CreateWaitSecond(fDealy);
        SceneManager.LoadScene(SceneName, c_LoadSceneMode);
    }
}
