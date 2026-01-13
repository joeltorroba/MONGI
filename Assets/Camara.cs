using UnityEngine;

public class Camara : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject jugador;
    public float speed = 100;
    private float giroX = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;

        giroX -= y;
        giroX = Mathf.Clamp(giroX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(giroX, 0, 0);
        jugador.transform.Rotate(Vector3.up * x);


    }
}
