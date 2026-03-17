using System;
using UnityEngine;

public class Mouser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnMouseEnter()
    {
        Debug.LogWarning("Mouser::OnMouseEnter");
    }
}
