using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Cannon = 0,Archor,Wizard,Sword,}
public enum WeaponState { SearchTarget = 0, TryAttackCannon,TryAttackWizard,TryAttackArchor,TryAttackSword}//AttackToTarget

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

    [Header("Sword")]
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private LayerMask targerLayer;
    
    //[SerializeField]
    //private float attackRate = 0.5f; //공속
    //[SerializeField]
    //private float attackRange = 2.0f;//공격 범위
    //[SerializeField]
    //private int attackDamage = 1; //딜
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
    public int UpgradeCost => Level < MaxLevel? towerTemplate.weapon[level+1].cost : 0;
    public int SellCost => towerTemplate.weapon[level].sell;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public float Slow => towerTemplate.weapon[level].slow;
    public float Buff => towerTemplate.weapon[level].buff;
    public WeaponType WeaponType => weaponType;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int BuffLevel
    {
        set=> buffLevel = Mathf.Max(0, value);
        get => buffLevel;   
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
            //제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
            //float closesDistSqr = Mathf.Infinity;
            ////적 스포너의 적 리스트에 있는 맵에 존재하는 모든 적 검사
            //for(int i = 0;i<enemySpawner.EnemyList.Count;++i) {
            //    float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //    //현재 검사중인 적과의 거리가 공격범위 내에 있고, 현재까지 검사한 적보다 거리가 가까우면
            //    //if(distance <= attackRange && distance<=closesDistSqr)
            //    if(distance < towerTemplate.weapon[level].range && distance<=closesDistSqr)
            //    {
            //        closesDistSqr = distance;
            //        attackTarget = enemySpawner.EnemyList[i].transform;
            //    }
            //}
            attackTarget = FindClosestAttackTarget();
            if(attackTarget != null)
            {
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if(weaponType == WeaponType.Archor)
                {
                    ChangeState(WeaponState.TryAttackArchor);
                }
                else if (weaponType == WeaponType.Wizard)
                {
                    ChangeState(WeaponState.TryAttackWizard);
                }
            }

            yield return null;
        }
    }
    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            ////1.target이 있는지 검사(다른 발사체에 의해 제거, Goal 지점까지 이동해 삭제 등)
            //if(attackTarget == null)
            //{
            //    ChangeState(WeaponState.SearchTarget); break;
            //}
            ////2. target이 공격 범위 안에 있는지 검사(공격 범위를 벗어나면 새로운 적 탐색)
            //float distance = Vector3.Distance(attackTarget.position,transform.position);
            //if (distance > towerTemplate.weapon[level].range)
            //{
            //    attackTarget = null;
            //    ChangeState(WeaponState.SearchTarget);
            //    break;
            //}
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
    private IEnumerator TryAttackLaser()
    {
        //레이저, 레이저 타격 효과 활성화
        EnableLaser();
        while (true)
        {
            //타겟을 공격 가능한지 검사
            if (IsPossibleToAttackTarget() == false)
            {
                //비활성화
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            SpawnLaser();

            yield return null;
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
    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range,targerLayer);

        //같은 방향으로 여러 개의 광선을 쏴서 그 중 현재 공격타겟과 동일한 오브젝트를 검출
        for(int i=0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                //선의 시작지점
                lineRenderer.SetPosition(0, spawnPoint.position);
                //목표지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                //타격 효과 위치 설정
                hitEffect.position = hit[i].point;
                ////적 체력 감소(초당 딜만큼 감소)
                //attackTarget.GetComponent<EnemyHp>().TakeDamage(towerTemplate.weapon[level].damage*Time.deltaTime);
                float damage = towerTemplate.weapon[level].damage + AddedDamage;
                attackTarget.GetComponent<EnemyHp>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }
    public bool Upgrade()
    {
        //타워 업그레이드에 필요한 골드가 충분한지 검사
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }
        //타워 레벨 증가
        level++;
        //외형 변경
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        //골드 차감
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        //무기 속성이 마법사면
        if(weaponType == WeaponType.Wizard)
        {
            //레벨에 따라 레이저 굵기 설정
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

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
