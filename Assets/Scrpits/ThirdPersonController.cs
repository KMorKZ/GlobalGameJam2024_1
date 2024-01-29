using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator anim;
    public Slider repSlider;
    public TextMeshProUGUI repText;

    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool cursorIsLocked = true;
    bool lockCursor = true;
    bool canWalk;
    bool isJoking;
    [SerializeField]private float sphereRadius = 0.1f;
    [SerializeField]private float jokeRadius = 0.1f;
    [SerializeField]private Vector3 sphereCheckOffset = new Vector3(0, -2, 0);
    [SerializeField]private LayerMask sphereCheckMask;
    [SerializeField]private LayerMask npcCheckMask;
    public List<NPC> NPC = new List<NPC>();
    private List<NPC> collidedNPC = new List<NPC>();

    private float gravityForce = -981f;
    private Vector3 currentVelocity = Vector3.zero;
    public int reputation = 0;
    private void Start()
    {
        canWalk = true;
        isJoking = false;
        foreach( GameObject go in GameObject.FindGameObjectsWithTag("NPC"))
        {
            NPC.Add(go.GetComponent<NPC>());
        
        }
    }
    private void Update()
    {
        repSlider.value = reputation;
        repText.text = reputation.ToString();

        Walking();

        if (isJoking)
        {
            Debug.Log("jokingFUF");
            foreach(NPC npc in NPC)
            {

                Debug.Log("badum");
                if (npc.isCollided) return;
                if(npc.DistanceToPlayer() < jokeRadius)
                {
                    npc.isCollided = true;
                    collidedNPC.Add(npc);
                    npc.LookAtPlayer();
                }
            }

        }

        UpdateCursorLock();
    }
    private void Walking()
    {
        if (!cursorIsLocked)
        {
            anim.SetBool("isRunning", false);
            return;
        }
        float horizontal = 0;
        float vertical = 0;
        if (canWalk)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0F, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);

        }
    }

    public void PlayAnimation(Joke joke)
    {
        
        StartCoroutine(Delay(joke));

    }
    IEnumerator Delay(Joke joke)
    {
        string animName = joke.animationName;
        float time = joke.cooldownTime;
        string jokeType = joke.jokeType;
        int sum = 0;
        canWalk = false;
        if (animName == "isDancing")
        {
            anim.SetInteger("isDancing", joke.jokeIndex);
        }
        else if (animName == "isJoking")
        {
            anim.SetBool(animName, true);
        }
        isJoking = true;
       
        yield return new WaitForSeconds(time);

        if (animName == "isDancing")
        {
            anim.SetInteger("isDancing", 0);
        }
        else if (animName == "isJoking")
        {
            anim.SetBool(animName, false);
        }


        if (collidedNPC.Count > 0)
        {
            Debug.Log("Collided");
            foreach (NPC npc in collidedNPC)
            {
                Debug.Log("Reacted");
                sum += npc.Reaction(jokeType);
                npc.isCollided = false;
                npc.StopLooking();
            }
            collidedNPC.RemoveRange(0, collidedNPC.Count);

        }
        isJoking = false;
        canWalk = true;
        reputation += sum;
    }

    private void FixedUpdate()
    {
        if (Physics.OverlapSphere(transform.position + sphereCheckOffset, sphereRadius, sphereCheckMask).Length == 0)
        {
            currentVelocity.y = -20;
        }
        else
        {
            currentVelocity.y += gravityForce * Time.fixedDeltaTime;
        }

        controller.Move(currentVelocity * Time.fixedDeltaTime);
        
    }
    
    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        if (lockCursor)
            InternalLockUpdate();
    }

    public void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            cursorIsLocked = false;
        else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            cursorIsLocked = true;

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
