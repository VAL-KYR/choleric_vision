using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class buttonTestToUnity : MonoBehaviour {

    public float speed;

    private float amountToMove;

    SerialPort sp = new SerialPort("COM3", 9600);

    // Use this for initialization
    void Start() {
        sp.Open();
        sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update() {
        amountToMove = speed * Time.deltaTime;

        if(sp.IsOpen)
        {
            try
            {
                moveCube(sp.ReadByte());
                print(sp.ReadByte());
            }
            catch (System.Exception)
            {
                
            }
        }
    }

    void moveCube(int buttonState)
    {
        if (buttonState == 1)
        {
            transform.Translate(Vector3.up * amountToMove, Space.Self);
        }
    }
}
