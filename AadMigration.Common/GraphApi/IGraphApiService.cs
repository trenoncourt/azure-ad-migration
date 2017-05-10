﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace AadMigration.Common.GraphApi
{
    public interface IGraphApiService
    {
        Task<IEnumerable<User>> GetUsersAsync(string token);

        Task AddUserAsync(User user, string token);
    }
}