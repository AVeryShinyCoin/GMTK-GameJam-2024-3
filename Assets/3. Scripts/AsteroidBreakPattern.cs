using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AsteroidBreakPattern : MonoBehaviour
{
    [Space(10)]
    public int NumberOfBreaks;
    [SerializeField] Vector2[] breakPositions;
    [SerializeField] float[] breakScale;
    [SerializeField] float[] breakOrientation;
    List<GameObject> breakAsteroids = new();

    public void Init(Vector2 pos, float scale)
    {
        transform.position = pos;
        transform.localScale = new Vector3(scale, scale, scale);
        NumberOfBreaks = breakPositions.Length;
    }
    public void SpawnObjectInPattern(GameObject prefab, int index)
    {
        GameObject gob = Instantiate(prefab);
        gob.SetActive(false);
        gob.transform.SetParent(transform);
        gob.transform.localPosition = breakPositions[index];
        gob.transform.localScale = new Vector3(breakScale[index], breakScale[index], breakScale[index]);
        gob.transform.rotation = Quaternion.Euler(0, 0, breakOrientation[index]);
        breakAsteroids.Add(gob);
    }

    public void ReleasePattern(float rot, Vector2 velocity)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rot);
        foreach (GameObject gob in breakAsteroids)
        {
            gob.transform.SetParent(null);
            gob.SetActive(true);
            Rigidbody2D rb = gob.GetComponent<Rigidbody2D>();
            rb.velocity = velocity;
            float rnd = Random.Range(300, 800);
            int mod = Random.Range(0, 2);
            if (mod == 1) rnd *= -1;
            rb.AddTorque(rnd);
        }
        Destroy(gameObject);
    }
}
