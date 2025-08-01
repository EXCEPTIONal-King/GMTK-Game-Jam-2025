using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField]
    float speed = 5;
    [SerializeField]
    float threshold = 0.01f;

    [SerializeField]  // TODO: remove when grid is in place
    Vector3[] destinations;
    int currentIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, destinations[currentIndex]) < threshold)
        {
            currentIndex++;
            if (currentIndex >= destinations.Length)
            {
                currentIndex = 0;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destinations[currentIndex], Time.deltaTime * speed);
        }
    }

    public void SetPoints(params Vector3[] points)
    {
        destinations = points;
        // TODO: this will eventually depend on the grid system methinks
        // but I'll just set the points in the inspector for now
    }
}
