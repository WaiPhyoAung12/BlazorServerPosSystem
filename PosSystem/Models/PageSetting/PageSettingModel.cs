namespace PosSystem.Models.PageSetting;

public class PageSettingModel
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchValue { get; set; }

    public string? SearchTitle { get; set; }
    public SortingModel? Sorting { get; set; }
    public int TotalPageCount { get; set; }
    public int TotalRowCount { get; set; }
    public int SkipCount { get; set; } = 0;
    public bool IsNotValid => this.PageNo < 1 || this.PageSize < 1;
}
public class SortingModel
{
    public string? Key { get; set; }
    public string Order { get; set; } = "ASC";
}
