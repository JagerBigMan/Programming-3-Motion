using UnityEngine;

public class TorqueTest : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float torque = 3f;
    public ForceMode2D mode = ForceMode2D.Force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) rigidbody.AddTorque(+torque, mode);
        if (Input.GetKey(KeyCode.RightArrow)) rigidbody.AddTorque(-torque, mode);
    }
}
