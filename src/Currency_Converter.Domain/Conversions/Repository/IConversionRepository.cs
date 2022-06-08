using System;
using Currency_Converter.Domain.Interfaces;
using System.Collections.Generic;
using Currency_Converter.Domain.Conversions.Commands;

namespace Currency_Converter.Domain.Conversions.Repository
{
    public interface IConversionRepository : IRepository<Conversion>
    {
        
    }
}