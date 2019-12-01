using UnityEngine;

public class MenuController : MonoBehaviour
{

    public MainMenu mainMenu;
    public GameObject battleTypeSelectionMenu;
    public GameObject manageMenu;
    public SelectTeamMenuController selectTeamBattleMenu;
    public GameObject selectTeamTrainingMenu;
    public GameObject inventoryMenu;
    public GameObject storeMenu;
    public GameObject teamEditor;

    private GameObject[] allMenus;

    private int battleType;

    private void Start()
    {
        allMenus = new[]
        {
            mainMenu.gameObject, battleTypeSelectionMenu, manageMenu, selectTeamBattleMenu.gameObject,
            selectTeamTrainingMenu, inventoryMenu, storeMenu, teamEditor
        };
        
        ShowMainMenu();
    }

    private void HideAll()
    {
        foreach (var menu in allMenus) 
            menu.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAll();
        mainMenu.gameObject.SetActive(true);
    }

    public void ShowBattleTypeSelection()
    {
        HideAll();
        battleTypeSelectionMenu.SetActive(true);
    }

    public void ShowManageMenu()
    {
        HideAll();
        manageMenu.SetActive(true);
    }

    public void ShowBattleTeamSelection()
    {
        HideAll();
        selectTeamBattleMenu.gameObject.SetActive(true);
        selectTeamBattleMenu.battleType = battleType;
    }

    public void ShowTrainingTeamSelection()
    {
        HideAll();
        selectTeamTrainingMenu.SetActive(true);
    }

    public void SetBattleType(int battleType)
    {
        this.battleType = battleType;
    }

    public void ShowInventory()
    {
        HideAll();
        inventoryMenu.SetActive(true);
    }

    public void ShowTeamEditor()
    {
        HideAll();
        teamEditor.SetActive(true);
    }

}