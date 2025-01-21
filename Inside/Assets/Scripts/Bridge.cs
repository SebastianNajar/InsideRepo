using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public bool open = false;
    public GameObject[] bridgeParts;
    public Collider2D[] colliders;
    public GameObject[] enemies;

    private void Update()
    {
        if (EnemiesAreNull()) open = true;

        //If the bridge is opened construct the bridge
        if (open)
        {
            StartCoroutine(ConstructBridge());
            DisableColliders();
            open = false;
        }
    }

    //For every bridge piece in the bridge, set them active one by one
    IEnumerator ConstructBridge()
    {
        for(int i = 0; i < bridgeParts.Length; i++)
        {
            bridgeParts[i].SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
    }

    //For every Collider attached to this bridge, disable them so the player can pass through the gap
    private void DisableColliders()
    {
        foreach(Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    //Checks to see if all enemies in an array are null
    private bool EnemiesAreNull()
    {
        foreach (GameObject enemy in enemies)
        {
            if(enemy != null)
            {
                return false;
            }
        }
        return true;
    }
}
