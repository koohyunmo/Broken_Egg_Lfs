using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : MonoBehaviour, IStoreListener
{
    [Header("Product ID")]
    public readonly string productId_test_id = "test_id";
    public readonly string productId_egg_01 = "eggpack1";
    public readonly string productId_egg_02 = "eggpack2";
    public readonly string productId_egg_03 = "eggpack3";

    [Header("Cache")]
    private IStoreController storeController; //���� ������ �����ϴ� �Լ� ������
    private IExtensionProvider storeExtensionProvider; //���� �÷����� ���� Ȯ�� ó�� ������

    Action _iapAction;

    private void Start()
    {
        InitUnityIAP(); //Start ������ �ʱ�ȭ �ʼ�
    }

    /* Unity IAP�� �ʱ�ȭ�ϴ� �Լ� */
    private void InitUnityIAP()
    {

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /* ���� �÷��� ��ǰ�� �߰� */
        builder.AddProduct(productId_test_id, ProductType.Consumable, new IDs() { { productId_test_id, GooglePlay.Name } });
        builder.AddProduct(productId_egg_01, ProductType.Consumable, new IDs() { { productId_egg_01, GooglePlay.Name } });
        builder.AddProduct(productId_egg_02, ProductType.Consumable, new IDs() { { productId_egg_02, GooglePlay.Name } });
        builder.AddProduct(productId_egg_03, ProductType.Consumable, new IDs() { { productId_egg_03, GooglePlay.Name } });
        //builder.AddProduct(productId_test_id2, ProductType.Consumable, new IDs() { { productId_test_id2, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);
    }

    /* �����ϴ� �Լ� */
    public void Purchase(string productId, Action iapAction)
    {
        Debug.Log(productId);

        Product product = storeController.products.WithID(productId); //��ǰ ����

        if (product != null && product.availableToPurchase) //��ǰ�� �����ϸ鼭 ���� �����ϸ�
        {
            _iapAction -= iapAction;
            _iapAction += iapAction;
            Debug.Log("����?");
            storeController.InitiatePurchase(product); //���Ű� �����ϸ� ����

        }
        else //��ǰ�� �������� �ʰų� ���� �Ұ����ϸ�
        {
            Debug.Log("��ǰ�� ���ų� ���� ���Ű� �Ұ����մϴ�");
        }
    }

    #region Interface
    /* �ʱ�ȭ ���� �� ����Ǵ� �Լ� */
    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�");

        storeController = controller;
        storeExtensionProvider = extension;
    }

    /* �ʱ�ȭ ���� �� ����Ǵ� �Լ� */
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�");
    }

    /* ���ſ� �������� �� ����Ǵ� �Լ� */
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("���ſ� �����߽��ϴ�");
    }

    /* ���Ÿ� ó���ϴ� �Լ� */
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("���ſ� �����߽��ϴ�");

        if (args.purchasedProduct.definition.id == productId_test_id)
        {
            /* test_id ���� ó�� */
            Debug.Log($"{productId_test_id}���� Invoke");
            _iapAction?.Invoke();
            _iapAction = null;
        }
        else if (args.purchasedProduct.definition.id == productId_egg_01)
        {
            /* test_id ���� ó�� */
            Debug.Log($"{productId_egg_01}���� Invoke");
            _iapAction?.Invoke();
            _iapAction = null;
        }
        else if (args.purchasedProduct.definition.id == productId_egg_02)
        {
            /* test_id ���� ó�� */
            Debug.Log($"{productId_egg_02}���� Invoke");
            _iapAction?.Invoke();
            _iapAction = null;
        }
        else if (args.purchasedProduct.definition.id == productId_egg_03)
        {
            /* test_id ���� ó�� */
            Debug.Log($"{productId_egg_03}���� Invoke");
            _iapAction?.Invoke();
            _iapAction = null;
        }

        return PurchaseProcessingResult.Complete;
    }
    #endregion
}
