using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour {
    //Класс описывает поведение части тела червячка.

    public int owner = 0; //Владелец части тела или номер команды. 

    //Параметры отвечающие за движение червячка.
    public float moveForce = 10;
    public float[] friction = {1, 1, 1, 1, 1};
    public float[] mass = {10, 1, 5, 1, 5};  

    public int numPartsAtStart = 4; 
    public GameObject growFood;
    public float growFoodChance = 0.5F;
    public float foodMoveTime = 0.5F;
    public float protectionTime = 1F;
    public int health = 3;

    [HideInInspector]
    public int partType = 0;
    [HideInInspector]
    public int foodEaten = 0;
    [HideInInspector]
    public Part parentPart, childPart;

    //Компоненты, которые использует скрипт.
    Rigidbody2D rb;
    Controls controls;
    Animator animator;
    SpriteRenderer sprite;
    DistanceJoint2D partJoint;
    FixedJoint2D itemJoint;

    bool canMoveFood = true, isProtected = false;
    Weapon headWeapon, assWeapon;
    bool canFreeHead = true, canFreeAss = true;

    public void SetAs(int partType, int owner, int layout)
    {
        //Метод настраивает часть тела.

        this.partType = partType;
        this.owner = owner;
        gameObject.layer = owner + 8;
        GetComponent<Controls>().layout = layout;
        rb.mass = mass[partType];

        switch (partType)
        {
            case 0: //если голова
                GetComponent<DistanceJoint2D>().enabled = false;
                parentPart = null;
                sprite.sortingOrder = 4;
                break;
            case 1: //если тело до сердца
                GetComponent<DistanceJoint2D>().enabled = true;       
                break;
            case 2: //если сердце
                GetComponent<DistanceJoint2D>().enabled = true;
                break;
            case 3: //если тело после сердца
                GetComponent<DistanceJoint2D>().enabled = true;
                break;
            case 4: //если жопа
                GetComponent<DistanceJoint2D>().enabled = true;
                childPart = null;
                break;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = GetComponent<Controls>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        partJoint = GetComponent<DistanceJoint2D>();
        itemJoint = GetComponent<FixedJoint2D>();

        SetAs(partType, owner, controls.layout);
    }

    public void ConnectTo(GameObject obj)
    {
        //Функция присоединяет эту часть тела к другой.

        partJoint.connectedBody = obj.GetComponent<Rigidbody2D>();
        parentPart = obj.GetComponent<Part>();
        obj.GetComponent<Part>().childPart = GetComponent<Part>();
    }

    void Grow()
    {
        //Функция растит частичку из задницы.

        if (partType == 2)
        {
            GameObject newObj = Instantiate(gameObject, transform.position, Quaternion.identity);
            newObj.GetComponent<Part>().ConnectTo(gameObject);
            SetAs(1, owner, controls.layout);
            newObj.GetComponent<Part>().SetAs(2, owner, controls.layout);
        }
    }

    void CreateBodyFromHead()
    {
        //Функция создает червяка при инициализации

        if (partType == 0)
        {
            GameObject prevObj = gameObject;
            bool heartLower = true;
            for (int i = 0; i < numPartsAtStart; i++)
            {
                int prevPartType = partType;
                partType = 1;
                GameObject newObj = Instantiate(gameObject, transform.position, Quaternion.identity);
                partType = prevPartType;
                newObj.GetComponent<Part>().ConnectTo(prevObj);
                if (i < numPartsAtStart / 2) newObj.GetComponent<Part>().SetAs(1, owner, controls.layout);
                if (i == numPartsAtStart / 2) newObj.GetComponent<Part>().SetAs(2, owner, controls.layout);
                if (i > numPartsAtStart / 2) newObj.GetComponent<Part>().SetAs(3, owner, controls.layout);
                prevObj = newObj;
            }
            prevObj.GetComponent<Part>().SetAs(4, owner, controls.layout);
        }   
    }  

    void Start()
    {
        CreateBodyFromHead();
    }

    void MoveHeadAndAss()
    {
        //Функция двигает голову или задницу в зависимости от нажатых клавиш.

        //Функция добавляет трение к частичке.
        rb.AddForce(new Vector2(-friction[partType] * rb.mass * rb.velocity.x, -friction[partType] * rb.mass * rb.velocity.y), ForceMode2D.Force);

        if (1 <= partType && partType <= 3) return; //Тело и сердце не управляются.

        //Считываем нажатие клавиш.
        Vector2 dir;
        if (partType == 0) dir = controls.HeadMoves();
        else dir = controls.AssMoves();
        dir.Normalize();

        //Двигаем частичку.
        rb.AddForce(new Vector2(moveForce * rb.mass * dir.x, moveForce * rb.mass * dir.y), ForceMode2D.Force);

        //Повернем голову или задницу по направлению движения
        if (dir.SqrMagnitude() != 0)
        {
            float angle = Mathf.Acos(dir.x);
            float sign = Mathf.Sign(dir.y);
            //rb.AddTorque((angle - transform.rotation.z) * 5, ForceMode2D.Force);
            //transform.rotation = Quaternion.Euler(0, 0, sign * angle / Mathf.PI * 180 - 90);
        }
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            if (partType == 1)
            {
                GetComponent<DistanceJoint2D>().enabled = false;
                SetAs(0, owner, controls.layout);
                ParentsToFloor();
            }
        }
    }

    public void ParentsToFloor()
    {
        gameObject.layer = LayerMask.NameToLayer("Floor");
        if (parentPart != null) parentPart.ParentsToFloor();
    }

    public void ChildsToFloor()
    {

    }

    void Draw()
    {
        //Функция полностью отрисовывает частичку.

        animator.SetBool("isHead", partType == 0);
        animator.SetBool("isAss", partType == 4);
        animator.SetBool("isHeart", partType == 2);

        if (isProtected) sprite.color = Color.green;
        else
        {
            if (foodEaten == 0) sprite.color = Color.white;
            if (foodEaten == 1) sprite.color = Color.yellow;
            if (foodEaten == 2) sprite.color = Color.green;
            if (foodEaten == 3) sprite.color = Color.red;
            if (foodEaten == 4) sprite.color = Color.blue;
        }
        switch (health)
        {
            case 3:
                sprite.color = Color.white;
                break;
            case 2:
                sprite.color = Color.yellow;
                break;
            default:
                sprite.color = Color.red;
                break;
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Кушаем еду.
        if (partType == 0 && foodEaten == 0)
        { 
            if (collision.gameObject.tag == "GrowFood") if (TakeFood(1)) Destroy(collision.gameObject);
            if (collision.gameObject.tag == "ProtectionFood") if (TakeFood(2)) Destroy(collision.gameObject);
            if (collision.gameObject.tag == "DangerousFood") if (TakeFood(3)) Destroy(collision.gameObject);
            if (collision.gameObject.tag == "ProtectGateFood") if (TakeFood(4)) Destroy(collision.gameObject);
        }

        //Если ударился мяч под эффектом.
        if (collision.gameObject.tag == "Ball")
        {
            switch (collision.gameObject.GetComponent<Ball>().smell)
            {
                case 3:
                    if (!isProtected) DestroyChildren();
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Holder" && partType == 0 && controls.Bite() && headWeapon == null)
        {
            itemJoint.enabled = true;
            itemJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            headWeapon = collision.gameObject.transform.parent.GetComponent<Weapon>();
            headWeapon.TakeUp(owner);
            canFreeHead = false;
        }

        if (collision.gameObject.tag == "Holder" && partType == 4 && controls.Fart() && assWeapon == null)
        {
            itemJoint.enabled = true;
            itemJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            assWeapon = collision.gameObject.transform.parent.GetComponent<Weapon>();
            assWeapon.TakeUp(owner);
            canFreeHead = false;
        }
    }

    void FreeHeadAndAss()
    { 
        if (partType == 0 && canFreeHead && headWeapon != null && controls.Bite())
        {
            itemJoint.enabled = false;
            headWeapon.Drop();
            headWeapon = null;
        }
        if (partType == 4 && canFreeAss && assWeapon != null && controls.Fart())
        {
            itemJoint.enabled = false;
            assWeapon.Drop();
            assWeapon = null;
        }
        canFreeHead = true;
        canFreeAss = true;
    }

    public void DestroyChildren()
    {
        //Функция уничтожает частичку и заставляет сделать это частички потомки.

        if (childPart != null) childPart.DestroyChildren();
        if (partType == 1)
        {
            childPart.ConnectTo(parentPart.gameObject);
            if (Random.value < growFoodChance) Instantiate(growFood, transform.position, Quaternion.identity);
            Destroy(gameObject);       
        }      
    }

    public void ProtectAll()
    {
        //Функция защищает частичку и заставляет сделать это всех родителей.

        if (parentPart != null) parentPart.ProtectAll();
        isProtected = true;
        StartCoroutine("WaitAndRemoveProtection");
    }

    IEnumerator WaitAndRemoveProtection()
    {
        yield return new WaitForSeconds(protectionTime);
        isProtected = false;
    }

    public bool GiveFood()
    {
        //Функция отдает еду потомку, если он готов ее взять.

        if (canMoveFood && childPart != null && childPart.TakeFood(foodEaten))
        {
            foodEaten = 0;
            return true;
        }
        else return false;
    }
    
    public bool TakeFood(int foodType)
    {
        //Функция берет еду от родителя.

        if (foodEaten == 0)
        {
            canMoveFood = false;
            StartCoroutine("WaitAndCanMoveFood");
            foodEaten = foodType;           
            return true;
        }
        else return false;
    }

    IEnumerator WaitAndCanMoveFood()
    {
        yield return new WaitForSeconds(foodMoveTime);
        canMoveFood = true;
    }
  
    void Fart()
    {
        //Функция обрабатывает пуканье задницы.

        if (partType == 4 && controls.Fart())
        {
            switch (foodEaten)
            {
                case 1:
                    foodEaten = 0;
                    Grow();
                    break;
                case 2:
                    foodEaten = 0;
                    ProtectAll();
                    break;
                case 3:
                    foodEaten = 0;
                    GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>().SetDangerous();
                    break;
                case 4:
                    foodEaten = 0;
                    GameObject.FindGameObjectWithTag("Gate" + owner).GetComponent<Gate>().Protect();
                    break;
            }
        }        
    }

    void Update()
    {
        MoveHeadAndAss();
        GiveFood();
        Fart();
        FreeHeadAndAss();
        CheckHealth();
        Draw();
    }
}
