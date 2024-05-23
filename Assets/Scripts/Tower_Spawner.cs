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
    private EnemySpawner enemySpawner;// ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����..
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;
    private int towerType;//Ÿ�� �Ӽ�
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;
        //��ư�� �ߺ��ؼ� ������ ���� ����
        if(isOnTowerButton == true)
        {
            return;
        }
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //���� ���� ������ �Ǽ� x
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            //�� ��� �����´ٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        //�Ǽ� ��ư �����ٰ� ����
        isOnTowerButton = true;
        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnTowerCancelSystem");
    }
    public void SpawnTower(Transform tileTransform)
    {
        //�Ǽ� ��ư�� ������ ���� Ÿ�� �Ǽ� ����
        if(isOnTowerButton == false)
        {
            return;
        }
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        ////1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� x
        //if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        //{
        //    systemTextViewer.PrintText(SystemType.Money);
        //    return;
        //}
        Tile tile = tileTransform.GetComponent<Tile>();

        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //2. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ Ÿ�� �Ǽ� x
        if(tile.IsBuildTower == true )
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        //�ٽ� Ÿ�� �Ǽ� ��ư�� ������ Ÿ���� ���鵵�� ���� ����
        isOnTowerButton = false;
        //Ÿ���� �Ǽ��Ǿ� �����Ƿ� ����
        tile.IsBuildTower = true;
        //Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�(Ÿ�Ϻ�Ÿ z�� -1�� ��ġ�� ��ġ)
        Vector3 positison = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab,positison,Quaternion.identity);
        //Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(this,enemySpawner, playerGold,tile);

        //���� ��ġ�Ǵ� Ÿ���� ���� Ÿ�� �ֺ��� ��ġ�� ���
        //Ÿ���� ��ġ�����Ƿ� �ӽ�Ÿ�� ����
        Destroy(followTowerClone);
        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StopCoroutine("OnTowerCancelSystem");
    }
    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                //���콺 �ӽ�Ÿ�� ����
                Destroy(followTowerClone);
                break;
            }

            yield return null;
        }
    }
}
