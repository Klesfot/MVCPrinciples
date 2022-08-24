namespace MVCPrinciples.Data;

public class Options
{
    public const string SectionName = "CustomAppConfig";

    public virtual int ProductsDisplayQuantity { get; set; }
    public virtual string DbConnectionString { get; set; }
}