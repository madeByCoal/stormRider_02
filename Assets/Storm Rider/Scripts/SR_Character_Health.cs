using UnityEngine;
using System.Collections;

public class SR_Character_Health : MonoBehaviour
{

    public GameObject explosion;
    public float yOffset;
    private SceneFadeInOut sceneFadeInOut;
    private float timer;
    public float resetAfterDeathTime = 0.5f;
    public float health = 100f;
    private SR_Character_Controller_3 Vcontroller;
    private SpriteRenderer spriteRenderer;
    private SR_Sound_Controller soundControler;
    public GameObject energyPrefab;
    private GameObject energyBar;
    private RectTransform energyRect;
    private Animator anim;

    void Awake()
    {
        sceneFadeInOut = GameObject.FindGameObjectWithTag("fader").GetComponent<SceneFadeInOut>();
        Vcontroller = GetComponent<SR_Character_Controller_3>();
        soundControler = GetComponent<SR_Sound_Controller>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CreateEnergyBar();
        anim = GetComponent<Animator>();
    }

    void CreateEnergyBar()
    {
        energyBar = GameObject.Instantiate(energyPrefab);
        GameObject UI = GameObject.Find("UI");
        energyBar.transform.SetParent(UI.transform, false);
        energyRect = energyBar.GetComponent<RectTransform>();
    }
    void SetBarPosition()
    {
        Vector3 BarPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(BarPosition);
        energyRect.anchoredPosition = screenPos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Obstacle":
                health -= 10f;
                break;
            case "Player":
                //health -= 2f;
                break;
            default:
                break;
        }
    }

    void Update()
    {

        SetBarPosition();
        // If health is less than or equal to 0...
        if (health <= 0f)
        {
            // Destroy the player
            Die();
            //LevelReset ();
            Destroy(gameObject);
        }
    }

    public void ApplyDamage(float damage)
    {
        anim.SetTrigger("Hit");
        //health -= damage;
    }

    public void Die()
    {
        OnExplode();
        spriteRenderer.enabled = false;
        Vcontroller.enabled = false;
        soundControler.enabled = false;
    }

    void OnExplode()
    {
        // Create a quaternion with a random rotation in the z-axis.
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Instantiate the explosion where the rocket is with the random rotation.
        Instantiate(explosion, transform.position, randomRotation);
    }

    void LevelReset()
    {
        // Increment the timer.
        timer += Time.deltaTime;

        //If the timer is greater than or equal to the time before the level resets...
        if (timer >= resetAfterDeathTime)
        {
            sceneFadeInOut.EndScene();
        }
    }
}
