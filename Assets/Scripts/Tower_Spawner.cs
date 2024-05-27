using UnityEditor;
using UnityEngine;
using System.Collections;

public class Tower_Spawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    [SerializeField]
    private EnemySpawner enemySpawner;// 현재 맵에 존재하는 적 리스트 정보를 얻기 위해..
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private int towerType;//타워 속성
    
    public void ReadyToSpawnTower()
    {
        //타워를 랜덤으로 지정
        towerType = 2;//Random.Range(0,4);
        //버튼을 중복해서 누르는 것을 방지
        if(isOnTowerButton == true)
        {
            Debug.Log("버튼 눌려져 있음");
            return;
        }
        //타워 건설 가능 여부 확인
        //지울 돈이 없으면 건설 x
        if (30 > playerGold.CurrentGold)
        {
            //돈 없어서 못짓는다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        //건설 버튼 눌렀다고 설정
        isOnTowerButton = true;
        systemTextViewer.PrintText(SystemType.Lcation);
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
        playerGold.CurrentGold -= 30;
        //선택한 타일의 위치에 타워 건설(타일보타 z축 -1의 위치에 배치)
        Vector3 positison = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, positison, Quaternion.identity);
        switch (towerType)
        {
            case 0:
                systemTextViewer.PrintText(SystemType.Archor);
                break;
            case 1:
                systemTextViewer.PrintText(SystemType.Cannon); break;
            case 2:
                systemTextViewer.PrintText(SystemType.Sword); break;
            case 3:
                systemTextViewer.PrintText(SystemType.Wizard); break;
        }
        //타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(clone.GetComponent<Tower_Spawner>(),enemySpawner, playerGold,tile);
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
                //타워 건설 취소 메세지 나오게 하기
                break;
            }

            yield return null;
        }
    }
}
