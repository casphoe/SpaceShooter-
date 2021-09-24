using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Follower : CCompo //자식으로 생성함
{
    public Vector3 followPos;
    public Transform parent;
    public Queue<Vector3> parentPos;
    public float FollowDealy;

    private void Start()
    {
        parentPos = new Queue<Vector3>();
        PoolingManager.instance.FollowerBulletManager(45);
        FollowDealy = 6f;
        InvokeRepeating("Shoot", 2f, 1f);
    }

    private void Update()
    {
        Follow();
    }

    private void Shoot()
    {
        /*
         * UnityException: Transform child out of bounds
         * follower 객체에 자식 오브젝트가 없는데 child를 포지션을 찾아서 일어난 오류
         */

        PoolingManager.instance.FollowerBulletCreate(gameObject.transform.position, Vector3.zero);
    }

    private void Follow()
    {
        //Queue = FIFO (first input first out) 먼저 입력된 데이터가 먼저 나가는 자료구조

        if(!parentPos.Contains(parent.position)) //Contains 큐의 값이 포함되고 있는지 확인
        {
            parentPos.Enqueue(parent.position); //Enqueue 큐에 데이터를 저장하는 함수(Input Pos)
        }

        if(parentPos.Count > FollowDealy)
        {
            followPos = parentPos.Dequeue(); //Dequeue 큐에 입력된 데이터를 내보내는 함수(out pos)
        }
        else
        {
            followPos = parent.position;
        }
        transform.position = followPos;
    }
}