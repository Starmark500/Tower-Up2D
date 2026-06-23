// using - это как "взять книгу с полки"
// UnityEngine - это большая книга с готовыми инструментами для игр
using UnityEngine;

// public class - это как "создать нового робота"
// PlayerController1 - имя нашего робота (можно любое)
// MonoBehaviour - это как "робот умеет работать в Unity"
public class PlayerController1 : MonoBehaviour
{
    // [Header] - это красивая надпись в Unity, просто для удобства
    [Header("Movement")]
    // public float - это число с плавающей точкой (дробное число)
    // speed = 5f - скорость игрока, f значит float (число с запятой)
    public float speed = 5f;        // скорость бега: чем больше, тем быстрее
    public float jumpForce = 12f;   // сила прыжка: чем больше, тем выше
    
    [Header("Ground Check")]
    // Transform - это позиция точки в пространстве
    // groundCheck - точка у ног игрока для проверки земли
    public Transform groundCheck;
    // радиус круга для проверки земли (как фонарик светит)
    public float groundCheckRadius = 0.2f;
    // LayerMask - это как маска, которая видит только слой "Земля"
    public LayerMask groundLayer;
    
    // private - это секретная переменная, видна только внутри робота
    // Rigidbody2D - это физика для 2D игр (гравитация, столкновения)
    private Rigidbody2D rb;
    // bool - это выключатель: true (да) или false (нет)
    private bool isGrounded;  // стоит ли игрок на земле?
    private float moveX;      // куда двигаться: -1 (влево), 0 (стоять), 1 (вправо)
    
    // void Start() - это как "проснуться утром"
    // Код здесь выполняется ОДИН раз при старте игры
    void Start()
    {
        // GetComponent - "найди деталь в роботе"
        // Мы ищем Rigidbody2D и сохраняем в переменную rb
        rb = GetComponent<Rigidbody2D>();
        
        // if (rb != null) - "если деталь существует"
        // != значит "не равно", null значит "ничего"
        if (rb != null)
        {
            // rb.gravityScale - настройка силы притяжения
            // 3f значит "в 3 раза сильнее обычного" (как на Луне или Земле)
            rb.gravityScale = 3f;
            
            // Запрещаем вращение по оси Z (чтобы игрок не падал боком)
            // RigidbodyConstraints2D.FreezeRotation - "заморозить вращение"
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            // Interpolate - делает движение более плавным
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    // void Update() - выполняется КАЖДЫЙ кадр (60 раз в секунду)
    // Здесь проверяем кнопки и обновляем состояние
    void Update()
    {
        // Input.GetAxisRaw - "какую кнопку нажал?"
        // "Horizontal" - стрелки влево/вправо или A/D
        // Raw - значит "сразу", без плавности (-1, 0 или 1)
        moveX = Input.GetAxisRaw("Horizontal");
        
        // if (groundCheck != null) - "если точка проверки земли существует"
        if (groundCheck != null)
        {
            // Physics2D.OverlapCircle - "нарисуй невидимый круг и проверь"
            // groundCheck.position - ГДЕ рисовать круг (у ног)
            // groundCheckRadius - РАЗМЕР круга
            // groundLayer - ЧТО искать (только слой "Земля")
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,   // где проверяем
                groundCheckRadius,      // размер круга
                groundLayer            // что ищем
            );
        }
        
        // && - это "И" (должны выполниться ОБА условия)
        // Input.GetButtonDown - кнопка нажата ТОЛЬКО в этом кадре
        // "Jump" - кнопка прыжка (обычно Пробел)
        // isGrounded - стоим на земле (true/false)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // rb.linearVelocity - скорость движения объекта
            // new Vector2 - новая скорость: (по X, по Y)
            // rb.linearVelocity.x - оставляем текущую скорость по X
            // jumpForce - новая скорость по Y (вверх)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        
        // ПОВОРОТ СПРАЙТА (картинки игрока)
        // if (moveX > 0) - "если двигаемся вправо"
        if (moveX > 0)
            // transform.localScale - размер и направление объекта
            // Vector3(1, 1, 1) - нормальный размер, смотрит вправо
            transform.localScale = new Vector3(1, 1, 1);
        // else if - "иначе если"
        else if (moveX < 0)  // "если двигаемся влево"
            // Vector3(-1, 1, 1) - зеркально отражен по X, смотрит влево
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // void FixedUpdate() - как Update, но для ФИЗИКИ
    // Выполняется с постоянной скоростью (50 раз в секунду)
    void FixedUpdate()
    {
        // ДВИЖЕНИЕ ПО ГОРИЗОНТАЛИ
        // moveX * speed - направление умножаем на скорость
        // Если moveX = 1 (вправо), скорость = 5
        // Если moveX = -1 (влево), скорость = -5
        // Если moveX = 0 (стоим), скорость = 0
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        // rb.linearVelocity.y - оставляем текущую скорость по Y (падение/прыжок)
    }
    
    // void OnDrawGizmosSelected() - рисует ПОДСКАЗКИ в редакторе Unity
    // Видно только когда объект выбран
    void OnDrawGizmosSelected()
    {
        // if (groundCheck != null) - если точка проверки существует
        if (groundCheck != null)
        {
            // Gizmos.color - цвет подсказки
            Gizmos.color = Color.red;  // красный цвет
            // Gizmos.DrawWireSphere - нарисуй прозрачный круг
            // groundCheck.position - где рисовать
            // groundCheckRadius - какого размера
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}