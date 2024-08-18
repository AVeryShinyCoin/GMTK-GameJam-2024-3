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

    public void ReleasePattern(float rot, Vector3 velocity) // velocity.z = speed
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rot);
        foreach (GameObject gob in breakAsteroids)
        {
            gob.transform.SetParent(null);
            gob.SetActive(true);
        }
        Destroy(gameObject);
    }
}
