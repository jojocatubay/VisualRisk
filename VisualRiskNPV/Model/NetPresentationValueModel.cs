using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace VisualRiskNPV.Model
{
   
    public class NetPresentationValueModel : INotifyPropertyChanged
    {
        public decimal InitialValue { get; set; }
        public decimal CashFlowAmount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal NPV { get; set; }
        public DateTime DateCreated { get; set; }


      
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
