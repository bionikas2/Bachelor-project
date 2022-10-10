using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;

    public bool isBossThrowable = false;

    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector3 playerPos;


    EnemyType enemy;
    // Start is called before the first frame update
    void Start() 
    {
        StartCoroutine(DeathDelay());
        if(!isEnemyBullet)
        { 
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
        if(isEnemyBullet)
        {
            if(playerPos.x < transform.position.x){
                Vector2 toVector = playerPos - transform.position;
                float angleToTarget = Vector2.Angle(transform.up, toVector);
                transform.localRotation = Quaternion.Euler(Vector3.forward * angleToTarget);
        }
        else
        {

            Vector2 toVector = playerPos - transform.position;
            float angleToTarget = Vector2.Angle(transform.up, toVector);

            transform.localRotation = Quaternion.Euler(-Vector3.forward * angleToTarget); 

        }
        }
    }

    void Update()
    {
        if(isEnemyBullet)
        {
            curPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            if(curPos == lastPos)
            {
                Destroy(gameObject);
            }
            lastPos = curPos;

        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy" && !isEnemyBullet && !isBossThrowable)
        {
            col.gameObject.GetComponent<EnemyController>().DamageEnemy(1);
            FindObjectOfType<AudioManager>().Play("fireballHit");
            Destroy(gameObject);
        }

        if(col.tag == "Wall" && !isEnemyBullet && !isBossThrowable)
        {
            FindObjectOfType<AudioManager>().Play("fireballHit");
            Destroy(gameObject);
        }
        //---------------------------------------------------------
        if(col.tag == "Player" && isEnemyBullet && !isBossThrowable)
        {
            GameController.DamagePlayer(1);
            FindObjectOfType<AudioManager>().Play("arrowHit");
            Destroy(gameObject);
        }

        if(col.tag == "Wall" && isEnemyBullet && !isBossThrowable)
        {
            FindObjectOfType<AudioManager>().Play("arrowHit");
            Destroy(gameObject);
        }
        //-------------------------------------------------------------
        if(col.tag == "Player" && isBossThrowable && !isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            FindObjectOfType<AudioManager>().Play("");
            Destroy(gameObject);
        }

        if(col.tag == "Wall" && isBossThrowable && !isEnemyBullet)
        {
            FindObjectOfType<AudioManager>().Play("");
            Destroy(gameObject);
        }
    }
}
