using PosSystem.Models.PageSetting;
using Radzen;

namespace PosSystem.Services;

public static class PageSettingService
{
    public static PageSettingModel GetPageSetting(this LoadDataArgs args)
    {
        var pageSettingModel = new PageSettingModel()
        {
            SkipCount = args.Skip.Value,
            PageSize = args.Top.Value,
            SearchValue = args.Filter,
           
        };
        pageSettingModel.Sorting = args.Sorts.Select(x => new SortingModel
        {
            Key = x.Property,
            Order = x.SortOrder.ToString()
        }).FirstOrDefault();
        return pageSettingModel;
    }
    public static IQueryable<T> Sort<T>
        (this IQueryable<T> query, string? sortColumn, string sortDirection)
    {
        if (sortColumn is null) return query;

        var propertyInfo = typeof(T).GetProperties();

        if (propertyInfo.Any(x => x.Name == sortColumn))
        {
            if ((sortDirection.ToLower()=="desc")
                || (sortDirection.ToLower()=="descending"))
            {
                query = query.OrderBy($"{sortColumn} desc");
            }
            else
            {
                query = query.OrderBy($"{sortColumn}");
            }
        }

        return query;
    }
    
}
