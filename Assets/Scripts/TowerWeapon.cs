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
    private Transform spawnPoint;//�߻�ü ���� ��ġ
    [SerializeField]
    private WeaponType weaponType;

    [Header("Achor")]
    [SerializeField]
    private GameObject projectilePrefab; //�߻�ü ������

    [Header("Sword")]
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private LayerMask targerLayer;
    
    //[SerializeField]
    //private float attackRate = 0.5f; //����
    //[SerializeField]
    //private float attackRange = 2.0f;//���� ����
    //[SerializeField]
    //private int attackDamage = 1; //��
    private int level = 0; //Ÿ�� ����
    private WeaponState weaponState = WeaponState.SearchTarget; //Ÿ�� ������ ����
    private SpriteRenderer spriteRenderer;
    private Tower_Spawner tower_Spawner;
    private Transform attackTarget = null; //���� ���
    private EnemySpawner enemySpawner; //���ӿ� �����ϴ� �� ���� ȹ���
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
        //���� ���¸� WeaponState.SearchTarget���� ����
        //ChangeState(WeaponState.SearchTarget);
        //���� �Ӽ��� ĳ��, �������� ��
        if(weaponType == WeaponType.Cannon||weaponType == WeaponType.Archor|| weaponType == WeaponType.Wizard)
        {
            ChangeState(WeaponState.SearchTarget);
        }
    }
    public void ChangeState(WeaponState newState)
    {
        //������ ������̴� ���� ����
        StopCoroutine(weaponState.ToString());
        //���� ����
        weaponState = newState;
        //���ο� ���� ���
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
        //�������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�
        //���� = arctan(y/x)
        //x,y ������ ���ϱ�
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        //x,y �������� �������� ���� ���ϱ�
        //������ radian �����̹Ƿ� Mathf.Rad2Deg�� ���� �� ������ ����
        float degree = Mathf.Atan2(dy,dx)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,degree);
    }
    private IEnumerator SearchTarget()
    {
        while(true){
            //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
            //float closesDistSqr = Mathf.Infinity;
            ////�� �������� �� ����Ʈ�� �ִ� �ʿ� �����ϴ� ��� �� �˻�
            //for(int i = 0;i<enemySpawner.EnemyList.Count;++i) {
            //    float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //    //���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
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
            ////1.target�� �ִ��� �˻�(�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ���� ��)
            //if(attackTarget == null)
            //{
            //    ChangeState(WeaponState.SearchTarget); break;
            //}
            ////2. target�� ���� ���� �ȿ� �ִ��� �˻�(���� ������ ����� ���ο� �� Ž��)
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
            //3, attackRate �ð���ŭ ���
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. ����(�߻�ü ����)
            SpawnProjectile();
        }
    }
    private IEnumerator TryAttackLaser()
    {
        //������, ������ Ÿ�� ȿ�� Ȱ��ȭ
        EnableLaser();
        while (true)
        {
            //Ÿ���� ���� �������� �˻�
            if (IsPossibleToAttackTarget() == false)
            {
                //��Ȱ��ȭ
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
        //���� ������ �ִ� ���� ã�� ���� �ּ� �Ÿ��� �ִ��� ũ�� ����
        float closesetDistSqr = Mathf.Infinity;
        //�� �������� ����Ʈ�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
        for(int i = 0;i<enemySpawner.EnemyList.Count;i++)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, ���� ���� �˻��� ������ �Ÿ��� ������
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
        //Ÿ���� �ִ��� �˻�
        if(attackTarget == null)
        {
            return false;
        }
        //Ÿ���� ���� ���� �ȿ� �ִ��� �˻�
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
        //������ �߻�ü���� ���ݴ��(attackTarget)���� ����
        //clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);
        //���ݷ� = �⺻ ��+���� �߰���
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

        //���� �������� ���� ���� ������ ���� �� �� ���� ����Ÿ�ٰ� ������ ������Ʈ�� ����
        for(int i=0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                //���� ��������
                lineRenderer.SetPosition(0, spawnPoint.position);
                //��ǥ����
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                //Ÿ�� ȿ�� ��ġ ����
                hitEffect.position = hit[i].point;
                ////�� ü�� ����(�ʴ� ����ŭ ����)
                //attackTarget.GetComponent<EnemyHp>().TakeDamage(towerTemplate.weapon[level].damage*Time.deltaTime);
                float damage = towerTemplate.weapon[level].damage + AddedDamage;
                attackTarget.GetComponent<EnemyHp>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }
    public bool Upgrade()
    {
        //Ÿ�� ���׷��̵忡 �ʿ��� ��尡 ������� �˻�
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }
        //Ÿ�� ���� ����
        level++;
        //���� ����
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        //��� ����
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        //���� �Ӽ��� �������
        if(weaponType == WeaponType.Wizard)
        {
            //������ ���� ������ ���� ����
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        return true;
    }
    public void Sell()
    {
        //�� ����
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        //�ٽ� �Ǽ� �����ϰ� ����
        ownerTile.IsBuildTower = false;
        //Ÿ�� �ı�
        Destroy(gameObject);
    }
}
