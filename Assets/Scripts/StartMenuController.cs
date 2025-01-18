using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartMenuBehaviour : MonoBehaviour
{
    public VisualTreeAsset startMenuUXML;  // 将 UXML 文件拖入此处
    public string firstLevelSceneName = "Level1";  // 设置第一个关卡的场景名称

    void Start()
    {
        // 获取 UI 根节点
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

        // 克隆 UXML 文件到 UI 根节点
        var startMenu = startMenuUXML.CloneTree();
        rootVisualElement.Add(startMenu);

        // 获取按钮控件
        Button StartButton = rootVisualElement.Q<Button>("StartButton");
        Button SettingsButton=rootVisualElement.Q<Button>("SettingsButton");
        Button quitButton = rootVisualElement.Q<Button>("ExitButton");

        // 为 New Game 按钮添加点击事件
        StartButton.clicked += () => LoadFirstLevel();

        // 为 Quit 按钮添加点击事件（可选）
        quitButton.clicked += () => QuitGame();

        StartButton.RegisterCallback<MouseEnterEvent>(evt=>{
            StartButton.AddToClassList("Button-Hover");
        });

        SettingsButton.RegisterCallback<MouseEnterEvent>(evt=>{
            SettingsButton.AddToClassList("Button-Hover");
        });

        quitButton.RegisterCallback<MouseEnterEvent>(evt=>{
            quitButton.AddToClassList("Button-Hover");
        });

        StartButton.RegisterCallback<MouseLeaveEvent>(evt=>{
            StartButton.RemoveFromClassList("Button-Hover");
        });

        SettingsButton.RegisterCallback<MouseLeaveEvent>(evt=>{
            SettingsButton.RemoveFromClassList("Button-Hover");
        });

        quitButton.RegisterCallback<MouseLeaveEvent>(evt=>{
            quitButton.RemoveFromClassList("Button-Hover");
        });

    }

    void LoadFirstLevel()
    {
        // 加载游戏的第一个关卡
        Debug.Log("Start Loading!");
        SceneManager.LoadScene(firstLevelSceneName);
    }

    void QuitGame()
    {
        // 退出游戏（在编辑器中无效）
        Application.Quit();
    }
}
