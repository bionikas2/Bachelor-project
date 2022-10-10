using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Follow,
    Die,
    Attack
};

public enum EnemyType
{
    Melee,
    Ranged,
    Boss
};

public enum BossAttack
{

    Attack1,
    Attack2,

}

public class EnemyController : MonoBehaviour
{

    public Animator animator;

    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;

    public BossAttack bossAttack;

    public int health;
    public float range;
    public float speed;
    public float attackRange = 3;
    public float bulletSpeed;
    public float coolDown;
    private bool coolDownAttack = false;
    public bool notInRoom = false;
    private Vector3 randomDir;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState)
        {
            
            case(EnemyState.Idle):
            break;
            case(EnemyState.Follow):
                Follow();
            break;
            case(EnemyState.Die):
            break;
            case(EnemyState.Attack):
                Attack();
            break;
        }

        if(!notInRoom)
        {
            if(IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Follow;
            }
            else if(!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Idle;
            }
            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        switch(enemyType)
        {
        case(EnemyType.Melee):                  
            if(transform.position.x < player.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(2, 2, 2);
            }
            else if(transform.position.x > player.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(-2, 2, 2);
            }
            break;
        case(EnemyType.Ranged):
            if(transform.position.x < player.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(2, 2, 2);
            }
            else if(transform.position.x > player.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(-2, 2, 2);
            }
            break;
        }
    }

    int i = 0;

    void Attack()
    {
        if(!coolDownAttack)
        {
            switch(enemyType)
            {
                case(EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    FindObjectOfType<AudioManager>().Play("Woosh");
                    StartCoroutine(CoolDown());
                break;
                case(EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    FindObjectOfType<AudioManager>().Play("bow");
                    StartCoroutine(CoolDown());
                break;
                case(EnemyType.Boss):

                    if(i==10)
                    {

                    bossAttack = (BossAttack)Random.Range(0,2);
                    i = 0;
                    }
                    BossController();
                    i++;
                break;
            }
        }
    }

    void BossController()
    {

        
        Vector3[] attack2cord = new Vector3[8];

        attack2cord[0] = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+10,gameObject.transform.position.z);
        attack2cord[1] = new Vector3(gameObject.transform.position.x+10, gameObject.transform.position.y+10,gameObject.transform.position.z);
        attack2cord[2] = new Vector3(gameObject.transform.position.x+10, gameObject.transform.position.y,gameObject.transform.position.z);
        attack2cord[3] = new Vector3(gameObject.transform.position.x+10, gameObject.transform.position.y-10,gameObject.transform.position.z);
        attack2cord[4] = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y-10,gameObject.transform.position.z);
        attack2cord[5] = new Vector3(gameObject.transform.position.x-10, gameObject.transform.position.y-10,gameObject.transform.position.z);
        attack2cord[6] = new Vector3(gameObject.transform.position.x-10, gameObject.transform.position.y,gameObject.transform.position.z);
        attack2cord[7] = new Vector3(gameObject.transform.position.x-10, gameObject.transform.position.y+10,gameObject.transform.position.z);

        

        switch(bossAttack)
        {
            case(BossAttack.Attack1):

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;

                bullet.GetComponent<BulletController>().GetPlayer(player.transform);

                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;

                bullet.GetComponent<BulletController>().isEnemyBullet = true;
                
                FindObjectOfType<AudioManager>().Play("Woosh");
                
                StartCoroutine(CoolDown());

            break;

            //----------------------------------------------------------------------
            case(BossAttack.Attack2):
                foreach(Vector3 v in attack2cord)
                {
                    GameObject bullet1 = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;

                    GameObject p = new GameObject();

                    p.transform.position = v;

                    bullet1.GetComponent<BulletController>().GetPlayer(p.transform);

                    bullet1.AddComponent<Rigidbody2D>().gravityScale = 0;

                    bullet1.GetComponent<BulletController>().isEnemyBullet = true;

                    FindObjectOfType<AudioManager>().Play("Woosh");

                    StartCoroutine(CoolDown());

                }

            break;
        }
    }
    
    
    
    public void DamageEnemy(int damage)
    {
        health -= damage;

        if(health <= 0 && enemyType != EnemyType.Boss)
        {
            Death();
        }
        else if(health <= 0 && enemyType == EnemyType.Boss)
        {
            FindObjectOfType<Timer>().Finish();
            SceneManager.LoadScene(7);
            Death();
        }

    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public void Death()
    {
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
    }
}
