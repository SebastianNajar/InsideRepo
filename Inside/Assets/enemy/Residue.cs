using UnityEngine;

public class Residue : MonoBehaviour
{
    public void cleanResidue(){
        Debug.Log("residue cleaned");
        Destroy(gameObject);
    }
}
