using System.Collections;  
using System.Collections.Generic; 
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private int totalShots;
    private float timer;
    private Animator animator;
    
    [SerializeField] 
    public float frequency;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > frequency){
            timer = 0;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot(){

        animator.SetTrigger("shoot");

        yield return new WaitForSeconds(0.42f);

        Instantiate(bullet, bulletPos.position, Quaternion.identity); 
        totalShots+=1;
     
    }
}
