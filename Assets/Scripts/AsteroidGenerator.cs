using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; 
    public int fieldSize = 500;          
    public int asteroidCount = 5;      
    public float minScale = 0.5f;
    public float maxScale = 5.0f;

    void Start()
    {
        GenerateField();
    }

    void GenerateField()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere * fieldSize;

            GameObject prefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            GameObject asteroid = Instantiate(prefab, randomPos, Quaternion.identity);
            
            asteroid.transform.rotation = Random.rotation;

            float scale = Random.Range(minScale, maxScale);
            asteroid.transform.localScale = new Vector3(scale, scale, scale);
            
            asteroid.transform.SetParent(this.transform);
        }
    }
}