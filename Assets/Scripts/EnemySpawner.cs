using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject      enemyPrefab; //적 프리팹
    [SerializeField]
    private Transform canvasTransform;
    //[SerializeField]
    //private float           spawnTime; //생성 주기
    [SerializeField]
    private Transform[]     wayPoints; //현재 스테이지의 이동 경로
    [SerializeField]
    private PlayerHp playerHp;
    [SerializeField]
    private PlayerGold playerGold;
    private Wave currentWave;//현재 웨이브 정보
    private int currentEnemyCount; //현재 웨이브에 남아있는 적 숫자(시작시 max, 사망시 -1)
    private List<Enemy>     enemyList;  //현재 맵에 존재하는 모든 적의 정보
    

    //적의 생성과 삭제는 EnemySpawner에서 하기 때문에 Set은 필요 없음
    public List<Enemy> EnemyList => enemyList;
    //현재 웨이브에 남아있는 적, 최대 적 숫자
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        //적 리스트 메모리 할당
        enemyList = new List<Enemy> ();
        //생성 코루틴 함수 호출
        //StartCoroutine("SpawnEnemy");
    }
    public void StartWave(Wave wave)
    {
        //매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;
        //현재 웨이브의 최대 적 숫자를 저장
        currentEnemyCount = currentWave.maxEnemyCount;
        //현재 웨이브 시작
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        //현재 웨이브에서 생성한 적 숫자
        int spawnEnemyCount = 0;

        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //GameObject clone = Instantiate(enemyPrefab); //적 오브젝트 생성
            int enemyIndex = Random.Range(0,currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();// 방금 생성된 적의 enemy컴포넌트
            
            enemy.Setup(this,wayPoints);
            enemyList.Add(enemy);

            //현재 웨이브에서 생성한 적의 숫자 +1
            spawnEnemyCount++;

            //yield return new WaitForSeconds(spawnTime);
            //각 웨이브마다 spawnTime이 다를 수 있기 때문에 현재 웨이브(currentWave)의 spawnTime 사용
            yield return new WaitForSeconds(currentWave.spawnTime);//
        }
    }
    public void DestroyEnemy(EnemyDestroyType type,Enemy enemy,int gold)
    {
        //적이 목표 지점까지 도착했을 때
        if(type == EnemyDestroyType.Arrive)
        {
            playerHp.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.kill)
        {
            playerGold.CurrentGold += gold;
        }
        currentEnemyCount--;
        //리스트에서 사망하는 적 정보 삭제
        enemyList.Remove(enemy);
        //적 오브젝트 삭제
        Destroy(enemy.gameObject);
        
    }
}
