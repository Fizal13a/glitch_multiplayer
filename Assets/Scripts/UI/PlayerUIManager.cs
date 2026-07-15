using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    
    [Header("Charge")]
    [SerializeField] private Image throwChargeImage;
    private RectTransform throwChargeRect;
    
    [Header("UI Hanger Positions")]
    [SerializeField] private Transform playerHeadTop;

    private void Awake()
    {
        if(throwChargeImage != null) throwChargeRect  = throwChargeImage.GetComponent<RectTransform>();
    }

    public void SetInteractCharge(float charge)
    {
        if(!throwChargeImage.gameObject.activeInHierarchy) throwChargeImage.gameObject.SetActive(true);
        SetUIToWorldPosition(throwChargeRect,playerHeadTop);
        throwChargeImage.fillAmount = Mathf.Clamp01(charge);
    }

    public void ResetInteractCharge()
    {
        throwChargeImage.gameObject.SetActive(false);
        throwChargeImage.fillAmount = 0f;
    }

    #region Helpers

    private void SetUIToWorldPosition(RectTransform uiTransform, Transform worldPosition)
    {
           Vector3 screenPosition =
               playerCamera.WorldToScreenPoint(worldPosition.position);
        
                uiTransform.position = screenPosition;
    }

    #endregion
}