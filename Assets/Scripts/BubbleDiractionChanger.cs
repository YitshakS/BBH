using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDiractionChanger : MonoBehaviour
{
    public bool isVisible; // האם משני כיוון הבועות נראים או לא
    public Vector2 diraction; // כיוון תנועת שינוי הבועה

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = isVisible;
    }
}
