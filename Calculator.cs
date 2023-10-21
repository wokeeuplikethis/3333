    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdKR
{
    public interface ICalculator
    {
        GraphData GetGraphData(DataHolder<double> dataHolder);
    }
    public class Calculator : ICalculator
    {
        public GraphData GetGraphData(DataHolder<double> dataHolder)
        {
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();



            for (double x = dataHolder.LeftBorder; x <= dataHolder.RightBorder; x += dataHolder.Step)
            {
                xs.Add(x);
                ys.Add(Math.Pow(dataHolder.A, 3) / Math.Pow(dataHolder.A, 2) + Math.Pow(x, 2));               
            }
            return new GraphData(xs.ToArray(), ys.ToArray());
        }
    }
}
