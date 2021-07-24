using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class ComponentBluetooth : MonoBehaviour
{
    public static ComponentBluetooth Instance { get; private set; }
    public TMP_Text connectionTest;
    public bool IsConnected;
    public string dataRecived = "";
    public event EventHandler seDesconecto;

    private float time = 0.0f;
    public float interpolationPeriod = 0.025f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            BluetoothService.CreateBluetoothObject();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        IsConnected = false;
       
        if (!IsConnected)
        {
            IsConnected = BluetoothService.StartBluetoothConnection("moto e5 plus");
            Debug.Log(IsConnected ? "Successful connection" : "Failed connection");
        }
    }

    public bool estaConectado()
    {
        return this.IsConnected;
    }

    public bool reconnect()
    {
       
        if (!IsConnected)
        {
            IsConnected = BluetoothService.StartBluetoothConnection("moto e5 plus");     

        }
        return IsConnected;


    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            time = 0.0f;
            connectionTest.text = IsConnected.ToString();
            if (IsConnected)
            {
                try
                {
                    string datain = BluetoothService.ReadFromBluetooth();
                    if (datain.Length > 1)
                    {
                        dataRecived = datain;
                        Debug.Log(dataRecived);
                    }
                }
                catch (Exception e)
                {
                    dataRecived = "000000000000";
                    IsConnected = false;
                    seDesconecto?.Invoke(this, EventArgs.Empty);
                }
            }
        }
       

    }
    public void disconnect()
    {
        /* if (device != null)
             device.close();*/
        if (IsConnected)
        {
            BluetoothService.StopBluetoothConnection();
        }
    }

}