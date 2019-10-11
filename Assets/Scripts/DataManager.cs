// Collects and manages necessary information that needs to be taken from the backend
// to the frontend and vice versa.
public class DataManager
{

    public static DataManager shared = new DataManager();

    private int userCurrency, userXP;
    // TODO: Create TeamInfo and BotInfo class
    // private TeamInfo? userTeams;
    private string currentUserID;
    // TODO: Create InventoryItem class
    // private PartInfo[] inventory;

    public int GetUserCurrency()
    {
        // TODO: Implement
    }

    public void SetUserCurrency(int amount)
    {
        // TODO: Implement
    }

    public int GetUserLevel()
    {
        // TODO: Implement
    }

    public void AddExperienceToUser(int xp)
    {
        // TODO: Implement
    }

    public void GetUserInventory()
    {
        // TODO: Implement
    }

    public void RemoveItemFromUserInventory()
    {
        // TODO: Implement
    }

    public void AddItemToUserInventory()
    {
        // TODO: Implement
    }

    public void GetUserBotTeams()
    {
        // TODO: Implement
    }

    public void UpdateUserBot()
    {
        // TODO: Implement
    }

    public void FetchInitialUserData()
    {
        // TODO: Implement
    }

    public void UpdateUserData()
    {
        // TODO: Implement
    }

    public string GetCurrentUserID()
    {
        // TODO: Implement
    }

}
