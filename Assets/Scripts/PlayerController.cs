using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; // מהירות השחקן
    public Vector2 diraction; // כיוון תנועת השחקן

    public float jumpForce; // עוצמת הקפיצה
    public float groundRadius; // הרדיוס שממנו נבדוק נקודה מתחת לשחקן האם הטווח הזה נוגע בקרקע
    public LayerMask whatIsGround; // האם האובייקט מגודרת כשכבת קרקע, רק ממנה יכול השחקן לקפוץ
    public Transform[] groundPoints; // מערך נקודות מתחת לשחקן שמשמשות לבדוק האם הוא דורך על קרקע

    public bool canShoot; // האם השחקן יכול לירות
    public float shootDelay; // פרק זמן בין יריה ליריה
    public GameObject buublePrefab; // מאפשר להגדיר באמצעות האינספקטור את הפריפאב של הבועה שהשחקן יורה
    public Transform bubbleSpawnPoint; // נקודה ליד השחקן שממנה נוצרת הבועה
    
    Rigidbody2D rb2d; // (נוקשות גוף השחקן (חוקי הפיסיקה

    // הפונקציה נקראית לפני הפרם הראשון ומעדכנת אותו
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // (נוקשות גוף השחקן (חוקי הפיסיקה
    }

    // הפונקציה נקראית לפני כל פרם ומעדכנת אותו
    void Update()
    {      
        // לחיצה על חץ למעלה כשהשחקן על הקרקע גורמת לשחקן לקפוץ
        if (CheckIsGrounded() && Input.GetKeyDown(KeyCode.UpArrow)) // Input.GetKeyDown("up")
            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // הוספת כח לקפיצה

        // שמאלי גורמת לשחקן לירות בועות Ctrl לחיצה על
        if (Input.GetKey(KeyCode.LeftControl) && canShoot)
            StartCoroutine(SpawnBubble());
    }
    
    // בדיקה האם השחקן על קרקע
    bool CheckIsGrounded ()
    {
        // מעבר על כל הנקודות
        for (int i = 0; i < groundPoints.Length; i++)
            // בדיקה האם נקודה נוגעת בטווח רדיוס מעל מה שמוגדר כשכבת קרקע
            if (Physics2D.OverlapCircle(groundPoints[i].position, groundRadius, whatIsGround))
                return true; // מספיק שנקודה אחת נוגעת בקרקע
        return false; // אם אף נקודה לא נוגעת
    }

    // ירי בועה
    IEnumerator SpawnBubble ()
    {
        // יצירת בועה על פי הבועה בפריפאב במיקום שבו מוגדר אלמנט הבועה ליד השחקן
        GameObject bubble = Instantiate(buublePrefab, bubbleSpawnPoint.position, Quaternion.identity);

        bubble.GetComponent<BubbleController>().diraction = diraction; // כיוון הבועה ככיוון השחקן
        canShoot = false;
        yield return new WaitForSeconds(shootDelay); // השהיה בין ירירה ליריה
        canShoot = true;
    }

    // הפונקציה של עדכון הפיזיקה נקראית לפני כל פרם ומעדכנת אותו
    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal"); // קליטת לחיצת חיצים שמאלי או ימני
        if (inputX < 0) // אם נלחץ חץ שמאלי
            diraction = Vector2.left;
        if (inputX > 0) // אם נלחץ חץ ימני
            diraction = Vector2.right;
        
        rb2d.velocity = new Vector2(inputX * speed, rb2d.velocity.y); // עדכון מהירות השחקן

        transform.localScale = new Vector2(diraction.x, transform.localScale.y); // עדכון תמונת השחקן כלפי ימין או כלפי שמאל
    }
}