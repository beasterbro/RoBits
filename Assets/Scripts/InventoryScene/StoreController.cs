using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
    
    
    public void ShowTooltip(PartInfo partInfo)
    {
        if (partInfo != null)
        {
            itemToolTip.ShowPartInfo(partInfo);
        }
    }

    public void ShowBuyMenu(int partID)
    {
        BuyOptionMenu.ShowBuyMenu(partID);
    }

    public void CancelBuyMenu()
    {
        BuyOptionMenu.CancelBuyMenu();
        HideTooltip();
    }

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

    public void HideTooltip()
    {
        
        itemToolTip.HideToolTip();
        
    }

    public void showPartSpecs(int i)
    {
        if (100 <= i && i <= 199)
        {
            //Get the body parts array
            ShowTooltip(bodytype[i - 100]);
        }
        
        if (200 <= i && i <= 299)
        {
            //Get the armor parts array
            ShowTooltip(armor[i - 200]);
        }
        
        if (300 <= i && i <= 399)
        {
            //Get the weapon parts array
            ShowTooltip(weapons[i - 300]);
        }
        
        if (400 <= i && i <= 499)
        {
            //Get the sensors parts array
            ShowTooltip(sensor[i - 400]);
        }
        
        if (500 <= i && i <= 599)
        {
            //Get the transport parts array
            ShowTooltip(transport[i - 500]);
        }
        
        if (600 <= i && i <= 699)
        {
            //Get the CPU parts array
            ShowTooltip(cpu[i - 600]);
        }
        
       }

    private async void Start()
    {
        DataManager.Instance.EstablishAuth("lucaspopp0@gmail.com");
        await DataManager.Instance.FetchInitialData();
        allParts = DataManager.Instance.AllParts;
        SortPartsByType();
        
    }
}