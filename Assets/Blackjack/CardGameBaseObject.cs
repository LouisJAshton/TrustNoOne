using System;
using LouisAshton.Singletons;
using UnityEngine;

public class CardGameBaseObject : Singleton<CardGameBaseObject>
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawSphere(transform.position, 0.1f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}
