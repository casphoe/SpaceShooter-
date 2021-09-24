using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, GameObject> GameObjectList;

    public override void Awake()
    {
        base.Awake();

        GameObjectList = new Dictionary<string, GameObject>();
    }

    // 게임 객체를 반환
    public GameObject GetGameObject(string key,bool IsAutoCreate = true)
    {
        if(IsAutoCreate && !GameObjectList.ContainsKey(key))
        {
            /*
             * Resources.Load 함수는 유니티의 Assests 폴더 하위에 있는 폴더 중에 Resources 이름 지닌 폴더를 기준으로 특정 에셋을 로드하는 역활을 수행
             * (즉, 해당 함수를 통해서 에셋을 로드하기 위해서는 해당 에셋이 반드시 Resources 폴더 하위에 있어야한다.)
             * 
             * ps.
             *    해당 함수에 넘겨지는 파일 경로는 Resource 하위부터 시작하는 경로를 의미
             *    Resources 폴더 까지의 경로는 자동으로 유니티 인식함
             *    
             *    또한 해당 경로에서 파일의 확장자는 명시하지 않는다.
             */
            var oGameObject = Resources.Load<GameObject>(key);
            GameObjectList.Add(key, oGameObject);
        }
        return GameObjectList[key];
    }

    // 게임 객체를 추가
    public void AddGameObject(string key, GameObject oGameobject)
    {
        if(!GameObjectList.ContainsKey(key))
        {
            GameObjectList.Add(key, oGameobject);
        }
    }

    public void RemoveGameObject(string key)
    {
        if(GameObjectList.ContainsKey(key))
        {
            var oGameObject = GameObjectList[key];
            GameObjectList.Remove(key);
            /*
             * Resource.UnloadAssest 함수는 메모리상에 로드 된 에셋을 제거하는 역활을 수행한다.
             * (즉, 이미 한번 로드 된 에셋은 사용하지 않더라도 메모리 상에 계속 존재하기 때문에 더이상 필요 없을 경우 해당
             * 함수를 사용해서 명시적으로 메모리 상에서 제거 해줘야한다.)
             * 
             */
            Resources.UnloadAsset(oGameObject);
        }
    }
}
