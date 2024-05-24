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
    private Transform spawnPoint;//�߻�ü ���� ��ġ
    [SerializeField]
    private WeaponType weaponType;

    [Header("Achor")]
    [SerializeField]
    private GameObject projectilePrefab; //�߻�ü ������
    
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
            //3, attackRate �ð���ŭ ���
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. ����(�߻�ü ����)
            SpawnProjectile();
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
    public bool Upgrade()
    {
        //Ÿ�� ���׷��̵忡 �ʿ��� ��尡 ������� �˻�
        if (playerGold.CurrentGold < 100)
        {
            return false;
        }
        //Ÿ�� ���� ����
        level++;
        //���� ����
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;

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
