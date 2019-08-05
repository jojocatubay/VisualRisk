using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisualRiskNPV;
using VisualRiskNPV.Model;
using VisualRiskNPV.ViewModel;

namespace VisualRiskNVPTest
{
    [TestClass]
    public class UnitTest1
    {
        private MainViewModel mainViewModel;
        [TestMethod]
        public void OnCalculate()
        {
            //Bound and Rates
            mainViewModel.LowerBoundRate = 10;
            mainViewModel.UpperBoundRate = 20;
            mainViewModel.Increment = Convert.ToDecimal(0.25);
            mainViewModel.InitialValue = 10000;
            //CashFlow
            mainViewModel.CashFlow1 = 10000;
            mainViewModel.CashFlow2 = 15000;
            mainViewModel.CashFlow3 = 20000;

            var initialValue = Convert.ToDouble(mainViewModel.InitialValue.ToString().Contains("-") ? mainViewModel.InitialValue : Convert.ToDecimal("-" + mainViewModel.InitialValue));
            var diff = mainViewModel.UpperBoundRate - mainViewModel.LowerBoundRate;
            int numberOfIncrement = Convert.ToInt32(diff / mainViewModel.Increment);
            var totalCashFlow = mainViewModel.CashFlow1 + mainViewModel.CashFlow2 + mainViewModel.CashFlow3;

            var ValueRate = Convert.ToDecimal(mainViewModel.Increment);
            for (int i = 0; i < numberOfIncrement; i++)
            {
                var cfr = totalCashFlow / ValueRate;
                var Npv = Convert.ToDecimal(initialValue) + Convert.ToDecimal(cfr);
                Console.WriteLine(ValueRate + " || " + Npv);
                ValueRate += mainViewModel.Increment;
            }
        }

    }
}
