using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;//타워 생성을 위한 프리팹
    public Weapon[] weapon;//레벨별 타워(무기)정보

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;
        public float damage;
        public float slow;//0.2 = 20%
        public float buff; //0.2 = +20%
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
