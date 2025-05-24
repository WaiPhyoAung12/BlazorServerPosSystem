using PosSystem.Models.PageSetting;
using System.Security.Claims;

namespace PosSystem.Services.Shared;

public static class DevCode
{
    public static string GetImageName(this string imageName)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        return $"{timestamp}-{imageName}";
    }
    public static PageSettingModel GetPageSettingResponse(this PageSettingModel pageSettingModel, int totalRowCount)
    {
        PageSettingModel pageSetting = pageSettingModel;
        var totalPageNo = totalRowCount / pageSettingModel.PageSize;
        if (totalRowCount % pageSettingModel.PageSize > 0) totalPageNo++;
        pageSetting.TotalRowCount = totalRowCount;
        pageSetting.TotalPageCount = totalPageNo;
        return pageSetting;
    }

    public static int ToInt<T>(this T? value)
    {
        return Convert.ToInt32(value);
    }
    
}
