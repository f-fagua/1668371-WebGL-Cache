using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackReciever : MonoBehaviour
{
    public void DestroyCube()
    {
        Destroy(gameObject, 3f);
    }
}
