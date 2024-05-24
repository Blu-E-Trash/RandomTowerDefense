using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;//타워 생성을 위한 프리팹
    public Weapon[] weapon;//레벨별 타워(무기)정보

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;   //타워 이미지
        public float damage;    //딜
        public float rate;      //공속
        public float range;     //범위
        public int sell;        //팔면 얼마?
    }
}
