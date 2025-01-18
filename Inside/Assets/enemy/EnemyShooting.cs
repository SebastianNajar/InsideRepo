using System.Collections;  
using System.Collections.Generic; 
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject bullet;
    public Transform bulletPos;
    private float timer;
    
    [SerializeField] 
    public float frequency;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTimen ;
        if(timer > frequency){
            timer = 0;
            shoot();
        }
    }

    void shoot(){
         Instantiate(bullet, bulletPos.position, Quaternion.identity ); 
    }
}
