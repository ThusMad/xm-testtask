namespace XmPriceAgg.BLL.Interfaces;

public interface IAggregationAlgorithm
{
    public float Aggregate(params float[] prices);

    public float Aggregate(IEnumerable<float> prices);
}