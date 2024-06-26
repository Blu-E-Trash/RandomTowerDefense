using System.Collections;
using UnityEngine;

public enum EnemyDestroyType { kill = 0, Arrive }
public class Enemy : MonoBehaviour
{
    private int             wayPointCount;      //이동 경로 개수
    private Transform[]     wayPoints;           //이동 경로 정보
    private int             currentIndex = 0;   //현재 목표지점 인덕스
    private Movement2D      movement2D;         //오브젝트 이동 제어
    private EnemySpawner    enemySpawner;       //적의 삭제를 본인이 하지 않고 EnemySpawner에 알려서 삭제
    private EnemyHp enemyHp;


    public float MoveSpeed => movement2D.MoveSpeed;
    public float EnemyHp => enemyHp.MaxHP;
    public float Gold => gold;
    [SerializeField]
    private int gold = 10;                      //사망시 획득 골드
    [SerializeField]
    private int point = 10;                          //사망시 획득 포인트
    
    public void Setup(EnemySpawner enemySpawner,Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //적 이동 경로 WayPoint 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        //적의 위치를 첫번째 waypoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        //적의 이동 목표지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        //다음 이동방향 설정
        NextMoveTo();

        while(true){
            //적 오브젝트 회전
            transform.Rotate(Vector3.forward * 10);

            //적의 현재위치와 목표위치의 거리가 0.02*movement2D.MoveSpeed보다 적을 때 if 실행
            //movement2D.MoveSpeed를 곱하는 이유는 속도가 빠르면 한 프레임에 0.02보다 크게 움직이기 때문
            //if조건문에 걸리지 않고 경로를 탈주하는 오브젝트가 발생할 수 있음
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position)<0.02f* movement2D.MoveSpeed)
            {
                //다음 이동 방향 설정
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        //아직 이동할 waypoint가 남아있으면
        if (currentIndex < wayPointCount - 1)
        {
            //적의 위치를 정확하게 목표 위치로 설정
            transform.position = wayPoints[currentIndex].position;
            //이동 방향 설정  -> 다음 목표지점
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        //현재 위치가 마지막 waypoint면
        else
        {
            gold = 0;
            point = 0;
            //적 오브젝트 삭제
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        //에너미 스포너에서 리스트로 적 정보를 관리하기에 Destroy()를 직접 하지 않고
        //에너미 스포너에게 본인이 삭제될 때 필요한 처리를 하도록 DestroyEnemy() 함수 호출
        enemySpawner.DestroyEnemy(type, this, gold, point);
    }
}
