using CN_WEB.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModel = CN_WEB.Core.Model.User;

namespace CN_WEB.Repository.Account
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public AccountRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IQueryable<UserModel>> Select()
        {
            IQueryable<UserModel> query = _unitOfWork.Select<UserModel>().AsNoTracking();
            return await Task.FromResult(query);
        }

        #endregion Constructor
    }
}
