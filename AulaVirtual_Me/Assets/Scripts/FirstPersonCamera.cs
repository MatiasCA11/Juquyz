using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Sensibilidad del mouse
    public Transform playerBody;          // Transform del cuerpo del jugador

    private float xRotation = 0f;         // Rotación en el eje X

    void Start()
    {
        // Bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtener la entrada del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Ajustar la rotación vertical (eje X) limitando el ángulo
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplicar la rotación vertical al objeto de la cámara
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Aplicar la rotación horizontal al cuerpo del jugador
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
