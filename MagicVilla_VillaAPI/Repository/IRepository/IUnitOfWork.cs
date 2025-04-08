namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        IVillaNumberRepository VillaNumber { get; }
        IUserRepository User { get; }
        void Save();
    }
}
