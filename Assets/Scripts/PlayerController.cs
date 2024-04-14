using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject attack;
    new Rigidbody rigidbody;
    float xVel = 0;
    float yVel = 0;

    public float sideSpeed = 3;
    public float jumpSpeed = 10;
    public float fastFallSpeed = 5;
    public bool touchingGround = false;
    bool releasedUp = true;
    bool attackOnCooldown = false;

    public int doubleJump = 1;
    public bool fastFell = false;
    float maxHealth = 10;
    float currentHealth = 10;
    public Image healthBar;
    public TextMeshProUGUI scoreText;
    private SacrificeManager sacrificeManager;
    bool dead = false;
    bool justHit = false;
    public Material mat;
    public Material hitMat;
    public float range = 5;
    bool wallJumping = false;
    float wallJumpingFactor = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        scoreText.gameObject.SetActive(false);
        sacrificeManager = GameObject.Find("SacrificeManager").GetComponent<SacrificeManager>();
    }

    private void OnCollisionEnter(Collision other) {
        touchingGround = true;
        doubleJump = 1;
        if (other.gameObject.tag == "Enemy"){
            //Invicibility period
            if (!other.gameObject.GetComponent<Enemy>().dead){
                GetHit();
            }
            
        }
        
    }

    public void Heal(float amount){
        currentHealth += amount;
        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void GiveMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }

    public void GiveRange(float amount)
    {
        range += amount;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Projectile"){
            GetHit();
            Destroy(other.gameObject);
        }
    }

    private void ResetJustHit(){
        justHit = false;
        GetComponent<Renderer>().material = mat;
    }

    private void GetHit(){
        if (justHit){
            return;
        }
        GetComponent<Renderer>().material = hitMat;
        currentHealth--;
        justHit = true;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0){
            Death();
        }
        else{
            Invoke(nameof(ResetJustHit), 0.25f);
        }
    }

    private void Death(){
        scoreText.gameObject.SetActive(true);
        scoreText.text = "Score : " + sacrificeManager.kills;
        dead = true;
    }

    public void Restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit(){
        Application.Quit(0);
    }

    private void OnCollisionExit(Collision other) {
        touchingGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead){
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            scoreText.gameObject.SetActive(!scoreText.gameObject.activeSelf);

            if (scoreText.gameObject.activeSelf){
                Time.timeScale = 0;
            }
            else{
                Time.timeScale = 1;
            }
        }

        HandleMovement();
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (attackOnCooldown){
            return;
        }

        //UP
        if (Input.GetAxis("VerticalSecondary") > 0)
        {
            Attack(0, 1);
        }
        //RIGHT
        else if (Input.GetAxis("HorizontalSecondary") > 0)
        {
            Attack(1, 0);
        }
        //DOWN
        else if (Input.GetAxis("VerticalSecondary") < 0)
        {
            Attack(0, -1);
        }
        //LEFT
        else if (Input.GetAxis("HorizontalSecondary") < 0)
        {
            Attack(-1, 0);
        }
        
    }

    private void ResetAttackCooldown(){
        attackOnCooldown = false;
    }

    private void Attack(float xScaleFactor, float yScaleFactor)
    {
        attackOnCooldown = true;
        Invoke(nameof(ResetAttackCooldown), 0.5f);
        GameObject spawnedAttack = Instantiate(attack, transform.position + new Vector3(-0.25f, -0.25f, 0), Quaternion.identity);
        spawnedAttack.GetComponent<HitboxController>().SetScaleFactor(xScaleFactor, yScaleFactor);
        spawnedAttack.transform.SetParent(transform);
    }

    public bool IsFullHealth(){
        return currentHealth == maxHealth;
    }

    private void ResetWallJumping(){
        wallJumping = false;
    }
    
    private void HandleMovement()
    {
        float velLeft = 0;
        if (Input.GetAxis("Horizontal") < 0)
        {
            velLeft = -sideSpeed;
        }

        float velRight = 0;
        if (Input.GetAxis("Horizontal") > 0)
        {
            velRight = sideSpeed;
        }

        if (wallJumping){
            velRight = wallJumpingFactor;
            velLeft = 0;
        }


        yVel = rigidbody.velocity.y;
        if (touchingGround)
        {
            fastFell = false;
            if (Input.GetAxis("Jump") > 0 && releasedUp)
            {
                //Chekc if walljump
                Vector3 raycastOrigin = transform.position;
                Vector3 raycastDirection = Vector3.down;
                float raycastLength = 1f;
                RaycastHit hit;
                if (!Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastLength))
                {
                    //walljump
                    wallJumping = true;
                    //Walljump direction
                    Vector3 raycastRight = Vector3.right;
                    Vector3 raycastLeft = Vector3.left;

                    if (Physics.Raycast(raycastOrigin, raycastRight, out hit, raycastLength)){
                        wallJumpingFactor = -1;
                    }
                    else if (Physics.Raycast(raycastOrigin, raycastLeft, out hit, raycastLength)){
                        wallJumpingFactor = 1;
                    }
                    CancelInvoke(nameof(ResetWallJumping));
                    Invoke(nameof(ResetWallJumping), 0.1f);
                    velRight = wallJumpingFactor;
                    velLeft = 0;
                }

                //normal jump
                yVel = jumpSpeed;
                releasedUp = false;
            }
            else if (Input.GetAxis("Jump") <= 0){
                releasedUp = true;
            }
        }
        else
        {
            if (Input.GetAxis("Jump") > 0 && releasedUp && doubleJump > 0)
            {
                doubleJump--;
                yVel = jumpSpeed;
                releasedUp = false;
            }
            else if (Input.GetAxis("Jump") <= 0){
                releasedUp = true;
            }

            if (rigidbody.velocity.y < 0 && Input.GetAxis("Vertical") < 0 && !fastFell){
                yVel = -fastFallSpeed;
                fastFell = true;
            }
        }

        xVel = velLeft + velRight;
        
        rigidbody.velocity = new Vector3(xVel, yVel, 0);
    }
}

