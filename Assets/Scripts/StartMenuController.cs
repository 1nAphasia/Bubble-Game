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
        Button newGameButton = rootVisualElement.Q<Button>("NewGameButton");
        Button quitButton = rootVisualElement.Q<Button>("QuitButton");

        // 为 New Game 按钮添加点击事件
        newGameButton.clicked += () => LoadFirstLevel();

        // 为 Quit 按钮添加点击事件（可选）
        quitButton.clicked += () => QuitGame();
    }

    void LoadFirstLevel()
    {
        // 加载游戏的第一个关卡
        SceneManager.LoadScene(firstLevelSceneName);
    }

    void QuitGame()
    {
        // 退出游戏（在编辑器中无效）
        Application.Quit();
    }
}
