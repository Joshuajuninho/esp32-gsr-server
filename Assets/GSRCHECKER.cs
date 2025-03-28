using UnityEngine;
using System.IO.Ports;


public class SphereController : MonoBehaviour
{
    public float gsrValue;
    private SerialPort serialPort;

    private Renderer sphereRenderer;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();

        // 🔴 Change "COM3" to your actual port (check in Arduino IDE)
        serialPort = new SerialPort("COM3", 115200);
        serialPort.Open();
    }

    void Update()
    {
        ReadGSRData();
        UpdateSphereColor();
    }

    void ReadGSRData()
    {
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine();
            if (float.TryParse(data, out float gsr))
            {
                gsrValue = gsr; // Store real GSR value
            }
        }
    }

    void UpdateSphereColor()
    {
        float normalizedGSR = Mathf.InverseLerp(0.5f, 5f, gsrValue);
        Color newColor = Color.Lerp(Color.blue, Color.red, normalizedGSR);
        sphereRenderer.material.color = newColor;

        Debug.Log("GSR Value: " + gsrValue);
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}

