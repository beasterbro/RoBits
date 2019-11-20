using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//A script to manage the store interface
public class StoreController : MonoBehaviour
{
    
    [SerializeField] public ItemToolTip itemToolTip;
    [SerializeField] public BuyMenu BuyOptionMenu;

    private PartInfo[] allParts;
    
    public PartInfo[] AllParts => allParts;
    
    private List<PartInfo> cpu = new List<PartInfo>();
    private List<PartInfo> armor = new List<PartInfo>();
    private List<PartInfo> weapons = new List<PartInfo>();
    private List<PartInfo> bodytype = new List<PartInfo>();
    private List<PartInfo> transport = new List<PartInfo>();
    private List<PartInfo> sensor = new List<PartInfo>();
    
    
    //Shows the tool tip for the part that was clicked on in the store
    public void ShowTooltip(PartInfo partInfo)
    {
        if (partInfo != null)
        {
            itemToolTip.ShowPartInfo(partInfo);
        }
    }

    //Shows the buy menu for the part that was clicked
    public void ShowBuyMenu(int partID)
    {
        BuyOptionMenu.ShowBuyMenu(partID);
    }

    //Called when the Cancel Button is pressed and hides the buy menu and the part info
    public void CancelBuyMenu()
    {
        BuyOptionMenu.CancelBuyMenu();
        HideTooltip();
    }

    //Shorts the parts by type and displays them in the store
    void SortPartsByType()
    {
        foreach (var part in allParts)
        {
            PartType type = part.PartType;
            switch (type)
            {
                case PartType.Cpu:
                    cpu.Add(part);
                    break;
                case PartType.Armor:
                    armor.Add(part);
                    break;
                case PartType.Weapon:
                    weapons.Add(part);
                    break;
                case PartType.BodyType:
                    bodytype.Add(part);
                    break;
                case PartType.Sensor:
                    sensor.Add(part);
                    break;
                case PartType.Transport:
                    transport.Add(part);
                    break;
                default:
                    break;
            }
        }
    }

    //Hides the tool tip (part information)
    public void HideTooltip()
    {
        
        itemToolTip.HideToolTip();
        
    }

    //Displays the the part information for the inputted partID
    public void showPartSpecs(int partID)
    {
        if (100 <= partID && partID <= 199)
        {
            //Get the body parts array
            ShowTooltip(bodytype[partID - 100]);
        }
        
        if (200 <= partID && partID <= 299)
        {
            //Get the armor parts array
            ShowTooltip(armor[partID - 200]);
        }
        
        if (300 <= partID && partID <= 399)
        {
            //Get the weapon parts array
            ShowTooltip(weapons[partID - 300]);
        }
        
        if (400 <= partID && partID <= 499)
        {
            //Get the sensors parts array
            ShowTooltip(sensor[partID - 400]);
        }
        
        if (500 <= partID && partID <= 599)
        {
            //Get the transport parts array
            ShowTooltip(transport[partID - 500]);
        }
        
        if (600 <= partID && partID <= 699)
        {
            //Get the CPU parts array
            ShowTooltip(cpu[partID - 600]);
        }
        
       }

    //Instantiates they parts and adds them to the store based on type
    private async void Start()
    {
        DataManager.Instance.EstablishAuth("lucaspopp0@gmail.com");
        await DataManager.Instance.FetchInitialData();
        allParts = DataManager.Instance.AllParts;
        SortPartsByType();
        
    }
}