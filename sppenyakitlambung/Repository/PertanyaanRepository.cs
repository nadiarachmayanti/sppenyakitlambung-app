using System;
namespace sppenyakitlambung.Repository
{
     class PertanyaanRepository
    {
        private static readonly Lazy<PertanyaanRepository>
            lazy =
            new Lazy<PertanyaanRepository>
            ();
        private PertanyaanRepository()
        {
            
        }
    }
}