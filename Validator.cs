﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdKR
{
    public interface IValidator
    {
        string PreValidation(DataHolder<string> dataHolder);
        bool PostValidation(double[] array);
        bool AccetableRange(DataHolder<double> dataHolder);
    }
    public class Validator : IValidator
    {
        private bool ValidateData(string value)
        {
            return double.TryParse(value, out double result);

        }
        public bool PostValidation(double[] array)
        {
            foreach (var item in array)
            {
                if (Double.IsNaN(item))
                {
                    return false;
                }
            }
            return true;
        }
        public string PreValidation(DataHolder<string> dataHolder)
        {
            string errors = "";
            if (!ValidateData(dataHolder.LeftBorder))
            {
                errors += "Incorect value (left border) field  " + Environment.NewLine;
            }
            if (!ValidateData(dataHolder.RightBorder))
            {
                errors += "Incorect value (right border) field  " + Environment.NewLine;
            }
            if (!ValidateData(dataHolder.Step))
            {
                errors += "Incorect value (step) field  " + Environment.NewLine;
            }
            else if (Double.Parse(dataHolder.Step) <= 0)
            {
                errors += "Step can`t be <= 0 " + Environment.NewLine;
                dataHolder.Step.ToString();
            }
            if (!ValidateData(dataHolder.A))
            {
                errors += "Incorect value (a) field  " + Environment.NewLine;
            }
            return errors;
        }
        public bool AccetableRange(DataHolder<double> dataHolder)
        {
            if (dataHolder.LeftBorder > dataHolder.RightBorder)
                return false;
            if ((dataHolder.RightBorder - dataHolder.LeftBorder) < 2 * dataHolder.Step)
                return false;
            return true;
        }
    }
}
