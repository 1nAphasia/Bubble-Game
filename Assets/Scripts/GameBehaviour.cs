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
    public VisualElement escMenu;
    public int HP{
        get{
            return _playerHP;
        }
        set{
            _playerHP=value;
            Debug.LogFormat("Lives:{0}",_playerHP);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        var root=GameObject.Find("PlayerHUD").GetComponent<UIDocument>().rootVisualElement;
        escMenu=root.Q<VisualElement>("EscMenu");
        var resume=escMenu.Q<Button>("ResumeButton");
        var quit=escMenu.Q<Button>("QuitButton");
        resume.clicked+=ResumeGame;
        quit.clicked+=ToStartMenu;
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
}
