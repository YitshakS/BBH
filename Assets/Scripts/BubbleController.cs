using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    float originalSpeed; // המהירות המקורית של הבועה
    public float speed; // המהירות הנוכחית של הבועה
    public float shootSpeed; // המהירות הגבוהה של הבועה בזמן יצירתה
    public float enemyCapturedSpeed; // מהירות הבועה בזמן שאויב לכוד בתוכה

    public Vector2 diraction; // כיוון תנועת הבועה
    Rigidbody2D rb2d; // (נוקשות גוף הבועה (חוקי הפיסיקה
    public float secondsBetweenScales; // שניות בין היריה להפיכת מהירות הבועה וקנה המידה של הבועה למקוריים
    bool isSpawnFinish; // אם יצירת הבועה לא הסתיימה לא תתחיל האנימציה שמגדילה מקטינה אותה

    public Sprite grayBubble; // תמונת הבועה האפורה
    public Sprite transparentBubble; // תמונת הבועה השקופה
    public Sprite enemyTrapped; // תמונת האויב נלכד בבועה

    public int floatingBubblesLayerID; // שכבת בועות צפות, יש לה אינטרקציה עם שכבת משני כיוון בועות

    Vector2 originalScale; // הגודל המקורי של הבועה
    float bubbleYoffset; // גובה הבועה

    float screenTopY; // הגבול העליון של המסך
    float screenBottomY; // הגבול התחתון של המסך

    Coroutine animateBubbleSpawnRoutine; // מחזיק את היחוס לפונקצית האנימציה של השרצת בועה
    Coroutine animateBubbleDestroyOverTimeRoutine; // מחזיק את היחוס של פונקצית השמדת הבועה לאחר זמן

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // (נוקשות גוף הבועה (חוקי הפיסיקה

        bubbleYoffset = GetComponent<SpriteRenderer>().bounds.extents.y; // חצי מהגובה הבועה
        screenTopY = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y; // הפינה השמאלית העליונה
        screenBottomY = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y; // הפינה השמאלית התחתונה

        originalSpeed = speed; // שמירת מהירות הבועה המקורית כדי להשיב אותה לאחר גמר היצירה שמאיצה במהירות
        originalScale = transform.localScale; // שמירת גודל הבועה המקורית כדי להשיב אותה לאחר האנימציה שמגדילה אותה

        // הכוללות השהיה IEnumerator קריאה לפונקציות
        animateBubbleSpawnRoutine = StartCoroutine(AnimateBubbleSpawn());
        StartCoroutine(AnimateBubbleSmallBig());
        animateBubbleDestroyOverTimeRoutine = StartCoroutine(AnimateBubbleDestroyOverTime());
    }

    IEnumerator AnimateBubbleSpawn() // אנימצית יצירת בועה
    {
        isSpawnFinish = false;
        speed = shootSpeed; // שינוי מהירות הבועה למהירה יותר ביצירתה
        for (int i = 3; i > 0; i--)
        {
            transform.localScale = originalScale / i; // הגדלת הבועה בכל איטרציה
            yield return new WaitForSeconds(secondsBetweenScales);
            speed /= 2; // האצת המהירות בזמן הגדלת הבועה
        }
        isSpawnFinish = true;
        // בסוף הלולאה הבועה תהיה בגודלה המקורי
        speed = originalSpeed; // והמהירות תהיה המהירות המקורית
        diraction = Vector2.up; // והכיוון שלה יהיה למעלה
        gameObject.layer = floatingBubblesLayerID; // ובשכבה של הבועות הצפות כדי שרק לאחר האנימציה התנגשות במשני כיוון בועות תשפיע
    }

    void StopBubbleSpawnAnimate()
    {
        StopCoroutine(animateBubbleSpawnRoutine);
        isSpawnFinish = true;
        diraction = Vector2.up; // הכיוון למעלה
        speed = originalSpeed; // המהירות המקורית
        transform.localScale = originalScale; // הגודל המקורי
        gameObject.layer = floatingBubblesLayerID; // שינוי לשכבה של הבועות הצפות כדי שהתנגשות במשני כיוון בועות לא תשפיע
    }

    IEnumerator AnimateBubbleSmallBig() // אנימציה של הבועה לאחר יצירתה
    {
        Vector2 smallScale = originalScale / 1.05f;
        while (true)
        {
            if (isSpawnFinish) // אם יצירת הבועה הסתיימה
            {
                // היא תקטן ותגדל לסירוגין
                transform.localScale = smallScale;
                yield return new WaitForSeconds(0.2f);
                transform.localScale = originalScale;
                yield return new WaitForSeconds(0.2f);
            }
            else
                yield return null; // אחרת, אם יצירת הבועה לא הסתיימה, לא תבוצע השהיה
        }
    }

    IEnumerator AnimateBubbleDestroyOverTime()
    {
        yield return new WaitForSeconds(5f); // המתנה 5 שניות
        GetComponent<SpriteRenderer>().sprite = grayBubble; // הפיכת תמונת הבועה לאפורה
        yield return new WaitForSeconds(5f); // המתנה 5 שניות
        GetComponent<SpriteRenderer>().sprite = transparentBubble; // הפיכת תמונת הבועה לשקופה
        yield return new WaitForSeconds(5f); // המתנה 5 שניות
        Destroy(this.gameObject); // השמדת הבועה
    }

    void Update()
    {
        if (transform.position.y > screenTopY + bubbleYoffset) // אם הבועה עוברת את הגבול העליון של המסך
            transform.position = new Vector2(transform.position.x, screenBottomY - bubbleYoffset); // היא תופיע בגבול התחתון של המסך ומשם תמשיך לעלות
    }

    void FixedUpdate()
    {
        rb2d.velocity = diraction * speed; // הזזת הבועה
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BubbleDiractionChanger")) // אם הבועה מתנגשת באלמנטים המשנים כיוון בועות
            diraction = other.GetComponent<BubbleDiractionChanger>().diraction; // כיוון הבועה משתנה בהתאם

        if (other.CompareTag("Player")) // אם הבועה מתנגשת בשחקן
            Destroy(this.gameObject); // היא מושמדת

        if (other.CompareTag("Wall")) // אם הבועה מתנגשת בקיר
            StopBubbleSpawnAnimate();

        if (other.CompareTag("Enemy")) // אם הבועה מתנגשת באויב
        {
            StopBubbleSpawnAnimate();
            StopCoroutine(animateBubbleDestroyOverTimeRoutine);
            speed = enemyCapturedSpeed;
            GetComponent<SpriteRenderer>().sprite = enemyTrapped; // הפיכת תמונת הבועה לאויב בתוכה
        }
    }
}
