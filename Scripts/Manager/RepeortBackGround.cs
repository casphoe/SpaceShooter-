using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//질문 할거
public class RepeortBackGround : MonoBehaviour
{

    private const float ScrollSpeed = 1f; //스크롤 할 속도를 상수(절대로 변하지 않는 숫자)로 지정

    private Material BackGround;

    // Start is called before the first frame update
    void Start()
    {
        BackGround = GetComponent<Renderer>().material; //Renderer라는 컴포넌트의 Material정보를 받아옵니다.
    }

    // Update is called once per frame
    void Update()
    {
        float newOffsety = BackGround.mainTextureOffset.y + ScrollSpeed * Time.deltaTime;

        Vector2 newOffset = new Vector2(0, newOffsety);

        BackGround.mainTextureOffset = newOffset;
    }
}
