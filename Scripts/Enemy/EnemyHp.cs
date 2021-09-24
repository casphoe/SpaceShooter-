using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : CCompo
{

    public Camera uiCamera; //ui카메라를 담을 변수
    public Canvas canvas; //캔버스를 담을 변수

    private RectTransform rectParent;
    private RectTransform rectHp;
    public Transform enemyTr;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;

        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate() //update 이후 실행함
    {

        /*
         * NullReferenceException: Object reference not set to an instance of an object
EnemyHp.LateUpdate () (at Assets/SpaceShooter/Scripts/Enemy/EnemyHp.cs:28)
            
         * 위에 오류가 발생하는 원인은 게임상 카메라에 main 카메라를 설정을 안했기 때문에 발생한 문제였습니다.
         * 
         */
        var screenPos = Camera.main.WorldToScreenPoint(enemyTr.position + new Vector3(0f,3f,0f));
        var localpos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localpos);
        //스크린좌표에서 캔버스에서 사용할 수 있는 좌표로 변경

        rectHp.localPosition = localpos;
    }
}
