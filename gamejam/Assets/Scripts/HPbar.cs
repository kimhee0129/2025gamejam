using UnityEngine;

public class HPbar : MonoBehaviour
{
    public Transform fill;

    public void set_value(float p)
    {
        fill.localScale = new Vector3(4.93f * p, 1.03f, 1f);
    }
}
