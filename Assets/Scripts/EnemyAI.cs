using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;

    public void Rotate(bool TargetVisible)
    {
        if (TargetVisible)
        {
            player = GameObject.FindWithTag("Player");
            transform.up = player.transform.position - transform.position;
        }
        
    }

  
}
