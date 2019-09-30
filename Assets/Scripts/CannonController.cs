using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Tooltip("Toggle for the correct input values (mobile uses touchscreen)")]
    public bool Touchscreen;
    Vector3 lookPos;
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        Vector3 pos;

        if (Touchscreen)
        {
            pos = Input.GetTouch(0).position;
        }
        else
        {
            pos = Input.mousePosition;
        }

        lookPos = Camera.main.ScreenToWorldPoint(pos);
        Quaternion rot = Quaternion.LookRotation(transform.position - lookPos, Vector3.forward);
        //transform.LookAt(lookPos, Vector3.forward);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }

    private void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 200, 20), lookPos.ToString(), 25);
    }
}
