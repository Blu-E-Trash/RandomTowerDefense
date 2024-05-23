using UnityEditor;
using UnityEngine;
using System.Collections;

public class Tower_Spawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    //[SerializeField]
    //private GameObject towerPrefab;
    //[SerializeField]
    //private int towerBuildGold = 50;
    [SerializeField]
    private EnemySpawner enemySpawner;// 현재 맵에 존재하는 적 리스트 정보를 얻기 위해..
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;
    private int towerType;//타워 속성
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;
        //버튼을 중복해서 누르는 것을 방지
        if(isOnTowerButton == true)
        {
            return;
        }
        //타워 건설 가능 여부 확인
        //지울 돈이 없으면 건설 x
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            //돈 없어서 못짓는다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        //건설 버튼 눌렀다고 설정
        isOnTowerButton = true;
        //타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }
    public void SpawnTower(Transform tileTransform)
    {
        //건설 버튼을 눌렀을 때만 타워 건설 가능
        if(isOnTowerButton == false)
        {
            return;
        }
        //타워 건설 가능 여부 확인
        ////1. 타워를 건설할 만큼 돈이 없으면 타워 건설 x
        //if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        //{
        //    systemTextViewer.PrintText(SystemType.Money);
        //    return;
        //}
        Tile tile = tileTransform.GetComponent<Tile>();

        //타워 건설 가능 여부 확인
        //2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 x
        if(tile.IsBuildTower == true )
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        //다시 타워 건설 버튼을 눌러서 타워를 만들도록 변수 설정
        isOnTowerButton = false;
        //타워가 건설되어 있으므로 설정
        tile.IsBuildTower = true;
        //타워 건설에 필요한 골드만큼 감소
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        //선택한 타일의 위치에 타워 건설(타일보타 z축 -1의 위치에 배치)
        Vector3 positison = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab,positison,Quaternion.identity);
        //타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(this,enemySpawner, playerGold,tile);

        //새로 배치되는 타워가 버프 타워 주변에 배치될 경우
        //타워를 배치했으므로 임시타워 삭제
        Destroy(followTowerClone);
        //타워 건설을 취소할 수 있는 코루틴 함수 중지
        StopCoroutine("OnTowerCancelSystem");
    }
    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                //마우스 임시타워 삭제
                Destroy(followTowerClone);
                break;
            }

            yield return null;
        }
    }
}
