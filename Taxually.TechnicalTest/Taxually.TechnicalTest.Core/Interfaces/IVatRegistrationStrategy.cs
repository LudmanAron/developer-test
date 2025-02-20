using System;

using Taxually.TechnicalTest.Core.Models;
namespace Taxually.TechnicalTest.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IVatRegistrationStrategy
    {
        Task RegisterVatAsync(VatRegistrationRequest request);
    }
}
