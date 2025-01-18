using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameBehaviour : MonoBehaviour
{
    public int EnemyCount;
    public List<GameObject> Enemies;
    private int _playerHP=5;
    private bool _isPaused=false;
    private bool _inputLocked=false;
    public VisualElement escMenu;
    public VisualElement LoseMenu;
    public int HP{
        get{
            return _playerHP;
        }
        set{
            _playerHP=value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale=1;
        var root=GameObject.Find("PlayerHUD").GetComponent<UIDocument>().rootVisualElement;
        escMenu=root.Q<VisualElement>("EscMenu");
        LoseMenu=root.Q<VisualElement>("GameLoseMenu");
        var resume=escMenu.Q<Button>("ResumeButton");
        var quit=escMenu.Q<Button>("QuitButton");
        var restart=LoseMenu.Q<Button>("RestartButton");
        resume.clicked+=ResumeGame;
        quit.clicked+=ToStartMenu;
        restart.clicked+=RestartScene;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Debug.Log("ESC PRESSED!");
            if(_isPaused)
            ResumeGame();
            else
            PauseGame();
        }
        if(_playerHP<=0){
            LoseMenu.style.display=DisplayStyle.Flex;
            Time.timeScale=0;
        }

    }
    void LateUpdate(){

    }

    void ResumeGame(){
        escMenu.style.display=DisplayStyle.None;
        Time.timeScale=1f;
        _isPaused=false;
    }

    void PauseGame(){
        escMenu.style.display=DisplayStyle.Flex;
        Time.timeScale=0f;
        _isPaused=true;
    }

    void ToStartMenu(){
        SceneManager.LoadScene("Startmenu");
    }
    void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}

