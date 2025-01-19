using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public GameObject[] bridgeParts;
    public bool open = false;

    private void Update()
    {
        if (open)
        {
            StartCoroutine(ConstructBridge());
            open = false;
        }
    }

    IEnumerator ConstructBridge()
    {
        for(int i = 0; i < bridgeParts.Length; i++)
        {
            bridgeParts[i].SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
