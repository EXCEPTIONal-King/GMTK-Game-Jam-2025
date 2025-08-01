using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField]
    float spinSpeed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Spin();
    }

    void Spin()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}
