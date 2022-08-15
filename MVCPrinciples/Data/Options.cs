namespace MVCPrinciples.Data;

public class Options
{
    public const string SectionName = "CustomAppConfig";

    public int ProductsDisplayQuantity { get; set; }
    public string DbConnectionString { get; set; }
}