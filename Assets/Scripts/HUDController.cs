using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class HUDController : MonoBehaviour
{
    public VisualTreeAsset visualTreeAsset;
    VisualElement rootVisualElement;
    private ProgressBar HPBar;
    public GameBehaviour gameManager;
    
    private int _health;

    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameObject.Find("Game_Manager").GetComponent<GameBehaviour>();

        rootVisualElement=GetComponent<UIDocument>().rootVisualElement;

        HPBar=rootVisualElement.Q<ProgressBar>("progressBar");
        
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate(){
        _health=gameManager.HP;
        UpdateHUD();
    }
    
    void UpdateHUD(){
        HPBar.title="Health:"+_health;
        HPBar.value=_health;
    }
}
