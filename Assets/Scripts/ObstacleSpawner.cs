using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    private IEnumerator SpawnObstacle()
    {
        while (true) //siempre generar obstaculos entre minTime y MaxTime
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            float minTime = 1f;
            float MaxTime = 1.7f;
            float randomTime = Random.Range(minTime, MaxTime); 
            
            Instantiate(obstacles[randomIndex], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }    
    }

    //collider que destruye objetos fuera de la pantalla
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Destroy(collision.gameObject);
    }
}
