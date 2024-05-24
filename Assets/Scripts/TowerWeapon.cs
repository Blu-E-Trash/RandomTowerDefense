using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Cannon = 0,Archor,Wizard,Sword,}
public enum WeaponState { SearchTarget = 0, TryAttackCannon}//AttackToTarget

public class TowerWeapon : MonoBehaviour
{
    [Header("Archors")]
    [SerializeField]
    private TowerTemplate towerTemplate;
    [SerializeField]
    private Transform spawnPoint;//발사체 생성 위치
    [SerializeField]
    private WeaponType weaponType;

    [Header("Achor")]
    [SerializeField]
    private GameObject projectilePrefab; //발사체 프리팹
    
    private int level = 0; //타워 레벨
    private WeaponState weaponState = WeaponState.SearchTarget; //타워 무기의 상태
    private SpriteRenderer spriteRenderer;
    private Tower_Spawner tower_Spawner;
    private Transform attackTarget = null; //공격 대상
    private EnemySpawner enemySpawner; //게임에 존재하는 적 정보 획득용
    private PlayerGold playerGold;
    private Tile ownerTile;

    private float addedDamage;
    private int buffLevel;

    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int SellCost => towerTemplate.weapon[level].sell;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public WeaponType WeaponType => weaponType;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public void Setup(Tower_Spawner tower_Spawner,EnemySpawner enemySpawner, PlayerGold playerGold,Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.tower_Spawner = tower_Spawner;
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;
        //최초 상태를 WeaponState.SearchTarget으로 설정
        //ChangeState(WeaponState.SearchTarget);
        //무기 속성이 캐논, 레이저일 떄
        if(weaponType == WeaponType.Cannon||weaponType == WeaponType.Archor|| weaponType == WeaponType.Wizard)
        {
            ChangeState(WeaponState.SearchTarget);
        }
    }
    public void ChangeState(WeaponState newState)
    {
        //이전에 재생중이던 상태 종료
        StopCoroutine(weaponState.ToString());
        //상태 변경
        weaponState = newState;
        //새로운 상태 재생
        StartCoroutine(weaponState.ToString());
    }
    private void Update()
    {
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }
    private void RotateToTarget()
    {
        //원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극 좌표계 이용
        //각도 = arctan(y/x)
        //x,y 변위값 구하기
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        //x,y 변위값을 바탕으로 각도 구하기
        //각도가 radian 단위이므로 Mathf.Rad2Deg를 곱해 도 단위를 구함
        float degree = Mathf.Atan2(dy,dx)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,degree);
    }
    private IEnumerator SearchTarget()
    {
        while(true){
            attackTarget = FindClosestAttackTarget();
            if(attackTarget != null)
            {
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if(weaponType == WeaponType.Archor)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if (weaponType == WeaponType.Wizard)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
            }

            yield return null;
        }
    }
    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            //3, attackRate 시간만큼 대기
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. 공격(발사체 생성)
            SpawnProjectile();
        }
    }
   
    private Transform FindClosestAttackTarget()
    {
        //제일 가까이 있는 적을 찾기 위해 최소 거리를 최대한 크게 설정
        float closesetDistSqr = Mathf.Infinity;
        //적 스포너의 리스트에 있는 현재 맵에 존재하는 모든 적 검사
        for(int i = 0;i<enemySpawner.EnemyList.Count;i++)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //현재 검사중인 적과의 거리가 공격범위 내에 있고, 현재 까지 검사한 적보다 거리가 가까우면
            if (distance <= towerTemplate.weapon[level].range && distance <= closesetDistSqr)
            {
                closesetDistSqr= distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
            }
        }
        return attackTarget;
    }
    private bool IsPossibleToAttackTarget()
    {
        //타겟이 있는지 검사
        if(attackTarget == null)
        {
            return false;
        }
        //타겟이 공격 범위 안에 있는지 검사
        float distance = Vector3.Distance(attackTarget.position,transform.position);
        if (distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }
        return true;
    }
    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab,spawnPoint.position,Quaternion.identity);
        //생성된 발사체에게 공격대상(attackTarget)정보 제공
        //clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);
        //공격력 = 기본 공+버프 추가공
        float damage = towerTemplate.weapon[level].damage + AddedDamage;
        clone.GetComponent<Projectile>().Setup(attackTarget, damage);
    }
    public bool Upgrade()
    {
        //타워 업그레이드에 필요한 골드가 충분한지 검사
        if (playerGold.CurrentGold < 100)
        {
            return false;
        }
        //타워 레벨 증가
        level++;
        //외형 변경
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;

        return true;
    }
    public void Sell()
    {
        //돈 증가
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        //다시 건설 가능하게 설정
        ownerTile.IsBuildTower = false;
        //타워 파괴
        Destroy(gameObject);
    }
}
