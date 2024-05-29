using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //scrolling
        transform.Translate(Vector2.left * GameManager.Instance.GetScrollSpeed() * Time.deltaTime);
    }
}
