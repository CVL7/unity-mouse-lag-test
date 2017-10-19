using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAxisRawMouseFollow : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
