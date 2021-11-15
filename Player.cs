using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour {

    public Camera cam; 
    // BOOL 
    private const bool B = true;

    public Animator walk; 
    
    private Vector3 _moveDelta;
    private Vector2 mousePos;
    
    // HINTS
    public Text hint0; 
    
   // TEXT dialogue
    public GameObject wizardText0;
    
    // LIVES and HP
    private float lives; 
    [FormerlySerializedAs("EnemyHP")] public float enemyHp;
    
    // AUDIO DIALOGUE
    [FormerlySerializedAs("player_0_dialogue0")]
    public AudioSource player0Dialogue0;
    [FormerlySerializedAs("player0Dialogue1_MilkCarton")] public AudioSource player0Dialogue1MilkCarton;
    public AudioSource collectAppleAudioSource;
    public AudioSource interactWithWizard; 

    // Consumables 
    public GameObject apple;
    [FormerlySerializedAs("_milkCarton")] public GameObject milkCarton;

    public GameObject witch;
    
    // Health & XP 
    public Text healthText;
    public float healthInteger = 1000f;

    // ENEMY and Player
    [FormerlySerializedAs("Enemy")] public GameObject enemy;
    public GameObject player; 
    

    public new Rigidbody2D rigidbody; 
    
    // Experience
    public Text xpText;
    public int xp;

    // Player Level
    public Text plText;
    public Text pointsToNextLevel;
    public int pointsToNextLevelInt;
    
    

    private void PlayerLevelConfig() {
        if (pointsToNextLevelInt == 0) return; {
            plText.text = "SURVIVOR";
            pointsToNextLevelInt = 150;
            pointsToNextLevel.text = "POINTS TO NEXT LEVEL " + pointsToNextLevelInt;
        }
    }
    
    private void EnemyXP_HIGH(int damageInflictedOnEnemy)
    {
        if (xp >= 1000)
            enemyHp -= (damageInflictedOnEnemy - 1);
    }

    private void EnemyXP_LOW(int damageInflictedLow) {
        if (xp <= 400) {
            enemyHp -= (damageInflictedLow - 25);
        }
    }
        

    private void Start() {
        
        
        // NUMBER VALUES
        enemyHp = 1000f;
        xp = 0;
        pointsToNextLevelInt = 100; 
        
        // TEXT
        plText.text = "ROOKIE";
        pointsToNextLevel.fontSize = 38;
        pointsToNextLevel.text = "POINTS TO NEXT LEVEL " + pointsToNextLevelInt;
        hint0.enabled = false;
        hint0.text = "To deal damage, click the left mouse button 10 times, since the your ability does 100 damage";
        
        
        // SET ACTIVE
        milkCarton.SetActive(false);
        wizardText0.SetActive(false);
        apple.SetActive(true);

        // GET THE COMPONENT <AUDIO SOURCE> 
        player0Dialogue0 = GetComponent<AudioSource>();
        
        
        // CHECK IF GAME WORKS
        Debug.Log("The game works G");
    }

    private void Update() {
    	// These functions are called every frame
        MovePlayer();
        PlayerLevelConfig();
        EscapeMenu();
        KillGuardSneakily();
        Die(); 
    }
    

    private void KillGuardSneakily() {
        if (Input.GetMouseButtonDown(0)) {
            enemyHp -= 100;
                                                
            if (enemyHp == 0) {
                Destroy(enemy);
            }
        }
    }
    
    

    private void MovePlayer() {
        
        walk.Play(0);

        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        Debug.Log("CURRENT POSITION" + transform.position);


        _moveDelta = new Vector3(x, y, 0);

        if (_moveDelta.x > 0) {
            transform.localScale = Vector3.one;
        }
        else if (_moveDelta.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.Translate(_moveDelta * Time.deltaTime * 10);
    

     x = Input.GetAxisRaw("Horizontal");
     y = Input.GetAxisRaw("Vertical");

        Debug.Log("CURRENT POSITION" + transform.position);

        _moveDelta = new Vector3(x, y, 0);

        if (_moveDelta.x > 0) {
            transform.localScale = Vector3.one;
        }

        if (_moveDelta.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        transform.Translate(_moveDelta * Time.deltaTime, 0);

}

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Wizard")) {
            Debug.Log(transform.position);
            
            milkCarton.SetActive(B);
            
            // HEALTH + XP 
            healthText.text = "HP " + healthInteger;
            xpText.text = "XP " + xp;
            healthInteger += 10f;
            xp += Random.Range(200, 500);
            
            Destroy(c.gameObject);
            WizardDialogue0(1.0f);
            Invoke("PlayDialogue0(1)", 3f);
            xp += 10;
            
            hint0.enabled = true;
        }

        if (c.gameObject.CompareTag("Milk")) {
            Destroy(c.gameObject);
            healthInteger += 20.9f;
            xpText.text = "XP " + xp;
            xp += Random.Range(1, 400);
            
            PlayDialogue1_MilkCarton(1);
        }
    	
    	if (c.gameObject.CompareTag("Guard")) { 
    		DieFromGuards(500);
    	}


        if (c.gameObject.CompareTag("Apple")) {
                 Destroy(c.gameObject);
                 healthInteger++;
                 xp += Random.Range(1, 20);
                 xpText.text = "XP " + xp;
                 SceneManager.LoadScene("Level2");
                apple_Dialogue(1);
                 
                 witch.SetActive(B);
        }

        if (c.gameObject.CompareTag("Shoot")) {
            healthInteger -= 1000f;
            if (healthInteger == 0) {
                Destroy(gameObject);
            }
        }

        if (c.gameObject.CompareTag("Enemy")) {
            EnemyXP_HIGH(100);
            EnemyXP_LOW(500);
    	}

    }
        
    
    // DIALOGUE
    private void PlayDialogue0(int volume) {
        // play the audio "player0Dialogue0" 
        player0Dialogue0.Play(0);
        player0Dialogue0.volume = volume;
    }
    
    private void PlayDialogue1_MilkCarton(int volume1) {
        
        // IF the audio "player0Dialogue0" is playing, stop the milk audio
        if (player0Dialogue0.isPlaying == true) {
            player0Dialogue1MilkCarton.Stop();
        }
        // Else if "player0Dialogue0" is not playing, play the milk audio at a volume of (1, parameter) 
        else if (player0Dialogue0.isPlaying == false) {
            player0Dialogue1MilkCarton.volume = volume1;
            player0Dialogue1MilkCarton.Play(0);
        }

    }

    private void apple_Dialogue(float volume) {
        if (player0Dialogue0.isPlaying && player0Dialogue1MilkCarton == false) {
            collectAppleAudioSource.Play();
        }
        else if (player0Dialogue0 && player0Dialogue1MilkCarton == true) {
            player0Dialogue1MilkCarton.Play();
            player0Dialogue0.Play();
            
            collectAppleAudioSource.Stop();
        }
    }

    private void WizardDialogue0(float volume) {
        interactWithWizard.volume = volume * Time.fixedDeltaTime; 
        interactWithWizard.Play();
    }
    
    private static void EscapeMenu() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Scenes/mainMenu");
        }
    }
    
    private void DieFromGuards(int damageToPlayer) { 
    	healthInteger -= damageToPlayer;
    }
    
    private void Die() { 
    	if (lives <= 0) { // If the amount of lives the player has is equal to zero or below
    		Destroy(player); // It will Destroy the player
    	}
    	// Checks if the players health is less than or equal to 0
    	if (healthInteger <= 0) {
    		Destroy(player); // if the statement is true, it will destroy the player
    	}
    }
}
