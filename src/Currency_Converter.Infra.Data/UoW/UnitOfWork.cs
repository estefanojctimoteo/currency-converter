using Currency_Converter.Domain.Interfaces;
using Currency_Converter.Infra.Data.Context;

namespace Currency_Converter.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ConversionContext _context;

        public UnitOfWork(ConversionContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}