using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxually.TechnicalTest.Core.Interfaces
{
    public interface ITaxuallyHttpClient
    {
        public Task PostAsync<TRequest>(string url, TRequest request);
    }
}
