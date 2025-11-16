using CsvHelper.Configuration;
using walkeasyfinal.Models;

public class ShoeMap : ClassMap<Shoes>
{
    public ShoeMap()
    {
        Map(x => x.ShoesName);
        Map(x => x.Image);
        Map(x => x.Category);
        Map(x => x.Amount);
        Map(x => x.Description);
        // Don't map Id
    }
}
