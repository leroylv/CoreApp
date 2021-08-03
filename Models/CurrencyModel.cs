using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Models
{
    public partial record CurrencyModel
    { 
        #region Ctor

        public CurrencyModel()
        {
            ExchangeRates = new List<ExchangeRate>();
        }

        #endregion

        #region Properties

        public List<ExchangeRate> ExchangeRates { get; set; }

        #endregion
    }
}
