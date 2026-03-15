using UnityEngine;

public class UpgradeBuffUi : MonoBehaviour
{
    [SerializeField] private GameObject buffPrefab;

    void Start()
    {
        foreach (Upgrade upgrade in UpgradeManager.instance.Upgrades)
        {
            GameObject nBuff = Instantiate(buffPrefab, transform);
            UpgradeBuffItem buffItem = nBuff.GetComponent<UpgradeBuffItem>();
            buffItem.SelectedUpgrade = upgrade;
        }
    }
}