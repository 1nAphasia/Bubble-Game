using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class HUDController : MonoBehaviour
{
    public VisualTreeAsset visualTreeAsset;
    VisualElement rootVisualElement;
    public GameBehaviour gameManager;
    public int bubbleChoice;
    private Player _player;
    private ProgressBar _HPBar;
    private VisualElement _bubColorElement;
    private int _health;

    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameObject.Find("Game_Manager").GetComponent<GameBehaviour>();
        _player=GameObject.Find("Player").GetComponent<Player>();
        rootVisualElement=GetComponent<UIDocument>().rootVisualElement;
        _HPBar=rootVisualElement.Q<ProgressBar>("HPBar");
        _bubColorElement=rootVisualElement.Q<VisualElement>("BubColor");
        bubbleChoice=_player.bubble_choice;
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate(){
        bubbleChoice=_player.bubble_choice;
        _health=gameManager.HP;
        UpdateHUD();
    }
    
    void UpdateHUD(){
        switch(bubbleChoice)
        {
            case 0:{
                _bubColorElement.style.backgroundColor=new StyleColor(Color.blue);
                break;
            }            
            case 1:{
                _bubColorElement.style.backgroundColor=new StyleColor(Color.red);
                break;
            }

        }
        
        _HPBar.title="Health:"+_health;
        _HPBar.value=_health;
    }
}
