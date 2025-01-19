using System.Collections;  
using System.Collections.Generic; 
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject bullet;
    public Transform bulletPos;
    private int totalShots;
    public int maxShots;
    private float timer;
    
    [SerializeField] 
    public float frequency;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > frequency){
            timer = 0;
            shoot();
        }
    }

    void shoot(){
        if(totalShots < maxShots){
            Instantiate(bullet, bulletPos.position, Quaternion.identity); 
            totalShots+=1;
        }
    }
}
