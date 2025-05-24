using Microsoft.AspNetCore.Components;
using MudBlazor;
using PosSystem.Constant;
using PosSystem.Models.Product;
using PosSystem.Models.Sale;
using PosSystem.Models.SaleDetails;
using PosSystem.Services.Dialog;
using PosSystem.Services.Product;
using PosSystem.Services.Sale;
using PosSystem.Services.Shared;

namespace PosSystem.Pages.Sale;

public partial class CreateSale
{
    [Inject]
    IProductService productService { get; set; } = default!;

    [Inject]
    IDialogServiceProvider dialogService { get; set; }=default!;

    [Inject]
    ISaleService saleService { get; set; } = default!;
    public int? BarCode { get; set; }
    public SaleRequestModel Model { get; set; } = new();
    public ProductModel ProductModel { get; set; } = new();
    public SaleDetailsModel SaleDetailsModel { get; set; } = new();


    private bool ShowCashForm = true;

    private bool ShowOtherForm = false;
    private async Task GetProduceByBarCode()
    {
        if (BarCode is null)
            return;

        var response=await productService.GetProductByBarCode((int)BarCode);

        if (response.IsSuccess)
        {
            ProductModel = response.Data!;
            SaleDetailsModel = new()
            {
                ProductName=ProductModel.ProductName,
                ProductId = ProductModel.Id,
                UnitPrice = ProductModel.Price,
                Quantity = 1,
                SubTotal = ProductModel.Price * 1,
            };
        }
        StateHasChanged();
    }

    private void CalculateTotalPrice()
    {
        SaleDetailsModel.SubTotal=SaleDetailsModel.UnitPrice*SaleDetailsModel.Quantity;
        StateHasChanged();
    }

    private void SubmitCategory()
    {
        if (SaleDetailsModel.ProductId is 0)
            return;

        var existingProduct=Model.SaleDetailsList.FirstOrDefault(x=>x.ProductId==SaleDetailsModel.ProductId);
        if(existingProduct != null)
        {
            existingProduct.Quantity += SaleDetailsModel.Quantity;
            existingProduct .SubTotal+= SaleDetailsModel.SubTotal;
        }
        else
        {
            Model.SaleDetailsList.Add(SaleDetailsModel);
            Model.PaymentType = EnumPaymentTypes.Cash.ToInt();
        }

        BarCode = null;
        SaleDetailsModel = new();
        ProductModel = new();
        StateHasChanged();

    }
    private async void DeleteProduct(SaleDetailsModel model)
    {
        var confirmDelete = await dialogService.ShowConfirmDeleteDialogAsync("Sale");

        if(confirmDelete.Canceled) return;

        var existingProduct = Model.SaleDetailsList.FirstOrDefault(x => x.ProductId == model.ProductId);
        if(existingProduct !=null)
            Model.SaleDetailsList.Remove(existingProduct);

        StateHasChanged();
    }

    private async Task Reset()
    {
        var confirmDelete = await dialogService.ShowConfirmDeleteDialogAsync("Sale");

        if (confirmDelete.Canceled) return;

        Model = new();
        StateHasChanged();
    }

    private async Task SubmitSale()
    {
        Model.TotalCount = Model.SaleDetailsList.Sum(x => x.Quantity);
        Model.TotalAmount = Model.SaleDetailsList.Sum(x => x.SubTotal);
        var result = await saleService.PurchaseProduct(Model);
        if (result.IsSuccess)
             dialogService.ShowDialogAsync(result.Message, "Sale");
    }
    private void PaymentTypeChoose()
    {
        if (Model.PaymentType == EnumPaymentTypes.Cash.ToInt())
        {
            ShowCashForm = true;
            ShowOtherForm = false;
        }
        else
        {
            ShowOtherForm = true;
            ShowCashForm= false;
        }
        StateHasChanged();
    }
    private void CalculateChange()
    {
        var totalAmount = Model.SaleDetailsList.Sum(x => x.SubTotal);
        if (Model.AmountPaid < totalAmount)
            dialogService.ShowConfirmDialogAsync("Total amount must be greater than customer paid amount!","Sale");
        else
            Model.Changes = Model.AmountPaid - totalAmount;

        StateHasChanged();
    }
}