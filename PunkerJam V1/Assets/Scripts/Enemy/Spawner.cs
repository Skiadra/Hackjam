using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;

    [SerializeField] private float spawnInterval = 3.5f;

    [SerializeField] private float NextStage = 120f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(spawnInterval,enemy1Prefab));
    }

    private void FixedUpdate() {
        NextStage -= 1 * Time.deltaTime;
        
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-10f, 10f), Random.Range(-1f, 4.5f), 0), Quaternion.identity);

        // Wave 2
        if(NextStage < 80f && NextStage > 40f)
        {
            Debug.Log("Wave 2");
            StartCoroutine(spawnEnemy(interval,enemy1Prefab));
            StartCoroutine(spawnEnemy(4.0f,enemy2Prefab));

        }else if (NextStage < 40f) 
        {
            Debug.Log("Wave 3");
            StartCoroutine(spawnEnemy(3.0f,enemy1Prefab));
            StartCoroutine(spawnEnemy(3.5f,enemy2Prefab));
        }
        // Wave 1
        else {
            StartCoroutine(spawnEnemy(interval,enemy));
        }     
    }
    
}
