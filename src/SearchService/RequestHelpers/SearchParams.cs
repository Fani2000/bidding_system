namespace SearchService.RequestHelpers;

public class SearchParams
{
    public string? searchTerm { get; set; } = string.Empty;
    public int pageNumber { get; set; } = 1;
    public int pageSize { get; set; } = 4;
    public string? seller { get; set; } = string.Empty;
    public string? winner { get; set; } = string.Empty;
    public string? orderBy { get; set; } = string.Empty;
    public string? filterBy { get; set; } = string.Empty;
}
