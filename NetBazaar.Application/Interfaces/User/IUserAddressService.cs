using NetBazaar.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.User
{
    public interface IUserAddressService
    {
        Task<List<UserAddressDto>> GetAddressesAsync(string userId);
        Task<UserAddressDto?> GetAddressByIdAsync(int id, string userId);
        Task AddNewAddressAsync(string userId, CreateOrUpdateUserAddressDto dto);
        Task UpdateAddressAsync(int id, string userId, CreateOrUpdateUserAddressDto dto);
        Task DeleteAddressAsync(int id, string userId);
        Task SetDefaultAddressAsync(int id, string userId);
    }

}
