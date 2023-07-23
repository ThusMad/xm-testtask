using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmPriceAgg.DAL.Models
{
    public class Ohlc
    {
        public float Close { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Open { get; set; }
        public long Timestamp { get; set; }
        public float Volume { get; set; }
    }

    public class Data
    {
        public List<Ohlc> Ohlc { get; set; }
        public string Pair { get; set; }
    }

    public class BitstampKlineResponse
    {
        public Data Data { get; set; }
    }
}
