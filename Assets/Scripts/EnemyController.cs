using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed; // מהירות האויב
    public Vector2 diraction; // כיוון תנועת האויב
    Rigidbody2D rb2d; // (נוקשות גוף האויב (חוקי הפיסיקה

    // הפונקציה נקראית לפני הפרם הראשון ומעדכנת אותו
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // (נוקשות גוף האויב (חוקי הפיסיקה
    }
    
    // הפונקציה של עדכון הפיזיקה נקראית לפני כל פרם ומעדכנת אותו
    void Update()
    {
        rb2d.velocity = new Vector2(diraction.x * speed, rb2d.velocity.y); // עדכון מהירות האויב
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlatformEnd")) // אם האויב הגיע לסוף הפלטפורמה עליה הוא עומד
            Flip(); // הוא יסובב לצד השני

        if(other.CompareTag("Bubble"))
            Destroy(this.gameObject);
    }

    void Flip()
    {
        diraction *= -1; // שינוי כיוון האויב
      
        transform.localScale = new Vector2(diraction.x, transform.localScale.y); // עדכון תמונת האויב כלפי ימין או כלפי שמאל

        // עדכון תמונת האויב - אפשרויות נוספות לכתיבה

        // transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        // Vector2 newScale = transform.localScale;
        // newScale.x *= -1;
        // transform.localScale = newScale;
    }
}