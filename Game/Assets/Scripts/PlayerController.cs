using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    public Animator animator;
    public float speed;
    Rigidbody2D rigidbody;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        fireDelay = GameController.FireRate;

        speed = GameController.MoveSpeed;

        float horizontal = Input.GetAxis("Horizontal");

        float vertical = Input.GetAxis("Vertical");         
    
        animator.SetFloat("Speed",Mathf.Abs(horizontal));

        if(horizontal < 0)
        {

            gameObject.transform.localScale = new Vector3(2, 2, 2);

        }

        if(horizontal > 0)
        {

            gameObject.transform.localScale = new Vector3(-2, 2, 2);

        }

        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);

        float shootHor = Input.GetAxis("ShootHorizontal");

        float shootVert = Input.GetAxis("ShootVertical");

        if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVert);
            lastFire = Time.time;
        }

    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
        );
        if(x>0 && y==0)
        {bullet.transform.Rotate(0, 0, 0);}
        else if(x<0 && y==0)
        {bullet.transform.Rotate(0, 180, 0);}

        else if(x>0 && y>0)
        {bullet.transform.Rotate(0, 0, 45);}
        else if(x>0 && y<0)
        {bullet.transform.Rotate(0, 0, -45);}

        else if(y>0 && x==0)
        {bullet.transform.Rotate(0, 0, 90);}
        else if(y<0 && x==0)
        {bullet.transform.Rotate(0, 0, -90);}

        else if(x<0 && y>0)
        {bullet.transform.Rotate(0, 180, 45);}
        else if(x<0 && y<0)
        {bullet.transform.Rotate(0, 180, -45);}

        FindObjectOfType<AudioManager>().Play("fIreballRelease");

    }

}
