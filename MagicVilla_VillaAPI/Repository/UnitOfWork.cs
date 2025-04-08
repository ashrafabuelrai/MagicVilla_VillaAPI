using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace MagicVilla_VillaAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IVillaRepository Villa { get; private set; }
        public IVillaNumberRepository VillaNumber { get; private set; }
        public IUserRepository User { get; private set; }

        public UnitOfWork(ApplicationDbContext db,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper,IConfiguration configuration)   
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            Villa = new VillaRepository(_db);
            VillaNumber = new VillaNumberRepository(_db);
            User = new UserRepository(_db,_userManager,_roleManager,_mapper,configuration);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
