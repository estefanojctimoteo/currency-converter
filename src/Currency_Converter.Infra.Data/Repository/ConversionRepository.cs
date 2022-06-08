using Dapper;
using System.Linq;
using System.Collections.Generic;
using Currency_Converter.Domain.Conversions;
using Currency_Converter.Domain.Conversions.Commands;
using Currency_Converter.Domain.Conversions.Repository;
using Currency_Converter.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Currency_Converter.Infra.Data.Repository
{
    public class ConversionRepository : Repository<Conversion>, IConversionRepository
    {
        public ConversionRepository(ConversionContext context) : base(context)
        {
        }
    }
}