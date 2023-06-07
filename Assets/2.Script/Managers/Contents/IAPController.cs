using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

using UnityEngine;
using UnityEngine.Purchasing;
using EasyMobile;

public class IAPController : MonoBehaviour, IStoreListener
{
    [Header("Product ID")]
    public readonly string productId_test_id = "test_id";
    public const string productIDConsumable = "consumable";
    
    public const string productId_EggPack1 = "eggpack1";
    public const string productId_EggPack2 = "eggpack2";
    public const string productId_EggPack3 = "eggpack3";

    [Header("Cache")]
    private IStoreController storeController; //구매 과정을 제어하는 함수 제공자
    private IExtensionProvider storeExtensionProvider; //여러 플랫폼을 위한 확장 처리 제공자

    Action _iapAction;

    private void Start()
    {
        //InitUnityIAP(); //Start 문에서 초기화 필수
        bool isInitialized = InAppPurchasing.IsInitialized(); //Easy Mobile 초기화 여부 확인
    }

    #region Easy Mobile
    private void Awake()
    {
        if(!RuntimeManager.IsInitialized())
            RuntimeManager.Init();
    }

    private void OnEnable()
    {
        InAppPurchasing.PurchaseCompleted += PurchasingCompleted;
        InAppPurchasing.PurchaseFailed += PurchasingFailed;
    }

    private void OnDisable()
    {
        InAppPurchasing.PurchaseCompleted -= PurchasingCompleted;
        InAppPurchasing.PurchaseFailed -= PurchasingFailed;
    }

    public void Purchasing(string productId,Action iapAction)
    {
        _iapAction = iapAction;
        switch (productId)
        {
            case productId_EggPack1:
                InAppPurchasing.Purchase(EM_IAPConstants.Product_Egg_Pack1);
                break;
            case productId_EggPack2:
                InAppPurchasing.Purchase(EM_IAPConstants.Product_Egg_Pack2);
                break;
            case productId_EggPack3:
                InAppPurchasing.Purchase(EM_IAPConstants.Product_Egg_Pack3);
                break;
        }
    }

    private void PurchasingFailed(IAPProduct product, string arg2)
    {
        NativeUI.Alert("Purchase Failed", "Purchase failed: " + product.Name + "\n" + arg2);
    }

    private void PurchasingCompleted(IAPProduct product)
    {
        _iapAction?.Invoke();
        _iapAction = null;
    }
    #endregion

    /* Unity IAP를 초기화하는 함수 */
    private void InitUnityIAP()
    {

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /* 구글 플레이 상품들 추가 */
        //builder.AddProduct(productId_test_id, ProductType.Consumable, new IDs() { { productId_test_id, GooglePlay.Name } });
        //builder.AddProduct(productId_test_id2, ProductType.Consumable, new IDs() { { productId_test_id2, GooglePlay.Name } });
        builder.AddProduct(productIDConsumable, ProductType.Consumable, new IDs() { { productId_EggPack1, GooglePlay.Name } });
        builder.AddProduct(productIDConsumable, ProductType.Consumable, new IDs() { { productId_EggPack2, GooglePlay.Name } });
        builder.AddProduct(productIDConsumable, ProductType.Consumable, new IDs() { { productId_EggPack3, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);
    }

    /* 구매하는 함수 */
    public void Purchase(string productId,Action iapAction)
    {
        Debug.Log(productId);

        Product product = storeController.products.WithID(productId); //상품 정의

        if (product != null && product.availableToPurchase) //상품이 존재하면서 구매 가능하면
        {
            _iapAction -= iapAction;
            _iapAction += iapAction;
            Debug.Log("구매?");
            storeController.InitiatePurchase(product); //구매가 가능하면 진행

        }
        else //상품이 존재하지 않거나 구매 불가능하면
        {
            Debug.Log("상품이 없거나 현재 구매가 불가능합니다");
        }
    }

    #region Interface
    /* 초기화 성공 시 실행되는 함수 */
    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        Debug.Log("초기화에 성공했습니다");

        storeController = controller;
        storeExtensionProvider = extension;
    }

    /* 초기화 실패 시 실행되는 함수 */
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화에 실패했습니다");
    }

    /* 구매에 실패했을 때 실행되는 함수 */
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("구매에 실패했습니다");
    }

    /* 구매를 처리하는 함수 */
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("구매에 성공했습니다");

        if (args.purchasedProduct.definition.id == productId_test_id)
        {
            /* test_id 구매 처리 */
            Debug.Log("구매 Invoke");
            _iapAction?.Invoke();
        }
        _iapAction?.Invoke();
        //else if (args.purchasedProduct.definition.id == productId_test_id2)
        {
            /* test_id2 구매 처리 */
        }

        return PurchaseProcessingResult.Complete;
    }
    #endregion
}
