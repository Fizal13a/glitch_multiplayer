using UnityEngine;
using UnityEngine.UI;

public class CustomerUIManager : MonoBehaviour
{
    [Header("Customer Patience")] 
    [SerializeField] private Image customerWaitImage;
    [SerializeField] private RectTransform customerWaitImageRect;
    [SerializeField] private Transform customerCounterTransform;
    
    public void SetCustomerPatienceLevel(float charge)
    {
        if (!customerWaitImage.gameObject.activeInHierarchy)
        {
            customerWaitImage.fillAmount = 0f;
            customerWaitImage.gameObject.SetActive(true);
        }
        
        SetUIToWorldPosition(customerWaitImageRect,customerCounterTransform);
        customerWaitImage.fillAmount = Mathf.Clamp01(charge);
    }

    public void ResetCustomerPatienceLevel()
    {
        customerWaitImage.gameObject.SetActive(false);
        customerWaitImage.fillAmount = 0f;
    }
    
    private void SetUIToWorldPosition(RectTransform uiTransform, Transform worldPosition)
    {
        Vector3 screenPosition =
            Camera.main.WorldToScreenPoint(worldPosition.position);
        
        uiTransform.position = screenPosition;
    }
}
