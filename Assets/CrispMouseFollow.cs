using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CrispMouseFollow : MonoBehaviour
{
    void Update()
    {
        transform.position = CrispMouse.Instance.position;
    }
}
