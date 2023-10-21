using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdKR
{
    public class DataHolder<T>
    {
        public DataHolder(T leftBorder, T rightBorder, T step, T a)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Step = step;
            A = a;
        }
        public T LeftBorder { get; private set; }
        public T RightBorder { get; private set; }
        public T Step { get; private set; }
        public T A { get; private set; }

        public string GetStringData()
        {
            return $"{LeftBorder} {RightBorder} {Step} {A}";
        }
    }
}
