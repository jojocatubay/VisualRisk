using MvvmExample.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VisualRiskNPV.Model;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Configuration;

namespace VisualRiskNPV.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel() // Constructor
        {
            NPVModelList = new ObservableCollection<NetPresentationValueModel>();
            InitializeTestValue();
        }


        #region " Command "

        private ICommand _calculateCommand;
        public ICommand CalculateCommand
        {
            get
            {
                if (_calculateCommand == null)
                    _calculateCommand = new RelayCommand(param => OnCalculate());

                return _calculateCommand;
            }
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                    _clearCommand = new RelayCommand(param => OnClear());

                return _clearCommand;
            }
        }

        #endregion " Command "

        #region " Method "

        public void OnCalculate()
        {
            try
            {
                var initialValue = Convert.ToDouble(InitialValue.ToString().Contains("-") ? InitialValue : Convert.ToDecimal("-" + InitialValue));
                var diff = UpperBoundRate - LowerBoundRate;
                int numberOfIncrement = Convert.ToInt32(diff / Increment);
                decimal lbr = LowerBoundRate;
                decimal ValueRate = Increment;
                for (int i = 1; i <= numberOfIncrement; i++)
                {
                    //CashFlow 1
                    var discountFactor1 = 1 / Math.Pow(Convert.ToDouble(1 + lbr), 1);
                    var presentValue1 = discountFactor1 * Convert.ToDouble(CashFlow1);

                    //CashFlow 2
                    var discountFactor2 = 1 / Math.Pow(Convert.ToDouble(1 + lbr), 2);
                    var presentValue2 = discountFactor2 * Convert.ToDouble(CashFlow2);

                    //CashFlow 2
                    var discountFactor3 = 1 / Math.Pow(Convert.ToDouble(1 + lbr), 3);
                    var presentValue3 = discountFactor3 * Convert.ToDouble(CashFlow3);

                    var totalPresentValue = initialValue + presentValue1 + presentValue2 + presentValue3;

                    NetPresentationValueModel netPV = new NetPresentationValueModel();

                    lbr += Increment;
                    ValueRate += Increment;

                    netPV.InitialValue = InitialValue;
                    netPV.DiscountRate = lbr;
                    netPV.NPV = Convert.ToDecimal(totalPresentValue);
                    netPV.CashFlowAmount = InitialValue + Convert.ToDecimal(totalPresentValue);
                    netPV.DateCreated = DateTime.Now;

                    NPVModelList.Add(netPV);
                    this.SaveNPV(netPV.InitialValue, netPV.CashFlowAmount, netPV.DiscountRate, netPV.NPV);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Fill all filelds with correct values.","Net Presentation Value");
            }

            

        }
        private void OnClear()
        {
            NetPresentationValueModel netPresentationValue = new NetPresentationValueModel();
            this.LowerBoundRate = 0;
            this.UpperBoundRate = 0;
            this.Increment = 0;
            this.InitialValue = 0;
            this.CashFlow1 = 0;
            this.CashFlow2 = 0;
            this.CashFlow3 = 0;
        }
        private void InitializeTestValue()
        {
            NetPresentationValueModel netPresentationValue = new NetPresentationValueModel();
            this.LowerBoundRate = 3.65m;
            this.UpperBoundRate = 3.70m;
            this.Increment = 0.010m;
            this.InitialValue = 100000;
            this.CashFlow1 = 10000;
            this.CashFlow2 = 10000;
            this.CashFlow3 = 10000;
        }


        private void BindDatagrid()
        {
            SqlConnection sqlConn = new SqlConnection();
        }

        //private void Retrive()
        //{
        //    string connectionString = @"Data Source=4035-60040;Initial Catalog=VisualRisk;Integrated Security=SSPI;";
        //    SqlConnection sqlConn = new SqlConnection(connectionString);
        //    try
        //    {
        //        if (sqlConn.State == ConnectionState.Closed)
        //            sqlConn.Open();
        //        SqlCommand sqlCmd = new SqlCommand();
        //        sqlCmd.CommandText = "Use VisualRisk; SELECT * FROM Npv";
        //        sqlCmd.Connection = sqlConn;
        //        SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        sqlConn.Close();
        //    }
        //}

        public void SaveNPV(decimal initialValue, decimal cashFlowAmount, decimal discountRate, decimal nPV )
        {
            string connectionString = @"Data Source=4035-60040;Initial Catalog=VisualRisk;Integrated Security=SSPI;";

            SqlConnection sqlConn = new SqlConnection(connectionString);
            SqlCommand sqlCmd = sqlConn.CreateCommand();
            sqlCmd.CommandText = "INSERT INTO [dbo].[Npv]  ([InitialValue],[CashFlowAmount],[DiscountRate],[NPV],[DateCreated])  VALUES(@initialValue,@cashFlowAmount,@discountRate,@nPV ,getdate())";

            sqlCmd.Parameters.AddWithValue("@initialValue", initialValue);
            sqlCmd.Parameters.AddWithValue("@cashFlowAmount", cashFlowAmount);
            sqlCmd.Parameters.AddWithValue("@discountRate", discountRate);
            sqlCmd.Parameters.AddWithValue("@nPV", nPV);

            try
            {
                sqlConn.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
            //MessageBox.Show("Data Saved Successfully.");
        }

        #endregion

        #region " Properties "

        public decimal _LowerBoundRate;
        public decimal LowerBoundRate
        {
            get { return _LowerBoundRate; }
            set
            {
                _LowerBoundRate = value;
                RaisePropertyChanged("LowerBoundRate");
            }
        }

        public decimal _UpperBoundRate;
        public decimal UpperBoundRate
        {
            get { return _UpperBoundRate; }
            set
            {
                _UpperBoundRate = value;
                RaisePropertyChanged("UpperBoundRate");
            }
        }

        public decimal _Increment;
        public decimal Increment
        {
            get { return _Increment; }
            set
            {
                _Increment = value;
                RaisePropertyChanged("Increment");
            }
        }

        public decimal _InitialValue;
        public decimal InitialValue
        {
            get { return _InitialValue; }
            set
            {
                _InitialValue = value;
                RaisePropertyChanged("InitialValue");
            }
        }

        public decimal _CashFlow1;
        public decimal CashFlow1
        {
            get { return _CashFlow1; }
            set
            {
                _CashFlow1 = value;
                RaisePropertyChanged("CashFlow1");
            }
        }

        public decimal _CashFlow2;
        public decimal CashFlow2
        {
            get { return _CashFlow2; }
            set
            {
                _CashFlow2 = value;
                RaisePropertyChanged("CashFlow2");
            }
        }

        public decimal _CashFlow3;

        public decimal CashFlow3
        {
            get { return _CashFlow3; }
            set
            {
                _CashFlow3 = value;
                RaisePropertyChanged("CashFlow3");
            }
        }

        private NetPresentationValueModel netPVModel;
        public NetPresentationValueModel NetPVModel
        {
            get { return netPVModel; }
            set
            {
                netPVModel = value; RaisePropertyChanged("NetPVModel");
            }
        }

        private ObservableCollection<NetPresentationValueModel> nPVModel;
        public ObservableCollection<NetPresentationValueModel> NPVModelList
        {
            get { return nPVModel; }
            set
            {
                nPVModel = value; RaisePropertyChanged("NPVModelList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion " Properties "



    }
}
