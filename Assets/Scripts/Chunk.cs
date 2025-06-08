using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector2Int Coord { get; private set; }
    private float animationSpeed;
    private Coroutine scaleRoutine;

    public void Initialize(Vector2Int coord, float speed)
    {
        gameObject.SetActive(true);
        Coord = coord;
        animationSpeed = speed;
        transform.localScale = Vector3.zero;
        StartCoroutine(ScaleUp());
    }

    public void Release()
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(ScaleDown());
    }

    IEnumerator ScaleUp()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);
            progress += Time.deltaTime * animationSpeed;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    IEnumerator ScaleDown()
    {
        float progress = 0f;
        Vector3 startScale = transform.localScale;
        while (progress < 1f)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            progress += Time.deltaTime * animationSpeed;
            yield return null;
        }
        Destroy(gameObject);
    }
}
