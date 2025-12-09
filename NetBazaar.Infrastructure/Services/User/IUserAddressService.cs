using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.DTOs.User;
using NetBazaar.Domain.Entities.Users;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBazaar.Application.Interfaces.User
{
    public class UserAddressService : IUserAddressService
    {
        private readonly INetBazaarDbContext _context;
        private readonly IMapper _mapper;

        public UserAddressService(INetBazaarDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserAddressDto>> GetAddressesAsync(string userId)
        {
            var addresses = await _context.UserAddresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.Id)
                .ToListAsync();

            return _mapper.Map<List<UserAddressDto>>(addresses);
        }

        public async Task<UserAddressDto?> GetAddressByIdAsync(int id, string userId)
        {
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            return _mapper.Map<UserAddressDto?>(address);
        }

        public async Task AddNewAddressAsync(string userId, CreateOrUpdateUserAddressDto dto)
        {
            // 1) اگر آدرس جدید باید Default باشد، تمام Defaultهای قبلی را Unset کنید
            if (dto.IsDefault)
            {
                var existingDefaults = await _context.UserAddresses
                    .Where(a => a.UserId == userId && a.IsDefault)
                    .ToListAsync();

                foreach (var addr in existingDefaults)
                    addr.UnsetDefault();
            }

            // 2) اضافه کردن آدرس جدید
            var address = _mapper.Map<UserAddress>(dto);
            address.SetUserId(userId);

            _context.UserAddresses.Add(address);

            // 3) ذخیره تغييرات
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(int id, string userId, CreateOrUpdateUserAddressDto dto)
        {
            // 1) پیدا کردن آدرس با بررسی مالکیت
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                throw new KeyNotFoundException("آدرس مورد نظر یافت نشد یا شما دسترسی ندارید.");

            // 2) اگر آدرس جدید باید Default باشد و قبلاً Default نبوده
            if (dto.IsDefault && !address.IsDefault)
            {
                var existingDefaults = await _context.UserAddresses
                    .Where(a => a.UserId == userId && a.Id != id && a.IsDefault)
                    .ToListAsync();

                foreach (var addr in existingDefaults)
                    addr.UnsetDefault();
            }
            address.UpdateInfo(dto.ReceiverName, dto.PhoneNumber, dto.PostalCode, dto.AddressText, dto.IsDefault);
            // 4) ذخیره تغییرات
            _context.UserAddresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(int id, string userId)
        {
            // 1) پیدا کردن آدرس با بررسی مالکیت
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                throw new KeyNotFoundException("آدرس مورد نظر یافت نشد یا شما دسترسی ندارید.");

            // 2) بررسی اینکه آیا آدرس حذف‌شده پیش‌فرض است یا نه
            bool wasDefault = address.IsDefault;

            // 3) حذف آدرس
            _context.UserAddresses.Remove(address);
            await _context.SaveChangesAsync();

            // 4) اگر آدرس حذف‌شده پیش‌فرض بود، اولین آدرس دیگر را پیش‌فرض کنیم
            if (wasDefault)
            {
                var firstAddress = await _context.UserAddresses
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.Id)
                    .FirstOrDefaultAsync();

                if (firstAddress != null)
                {
                    firstAddress.SetAsDefault();
                    _context.UserAddresses.Update(firstAddress);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task SetDefaultAddressAsync(int id, string userId)
        {
            // 1) پیدا کردن آدرس مورد نظر با بررسی مالکیت
            var targetAddress = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (targetAddress == null)
                throw new KeyNotFoundException("آدرس مورد نظر یافت نشد یا شما دسترسی ندارید.");

            // 2) اگر قبلاً پیش‌فرض نیست، آن را پیش‌فرض کنیم
            if (!targetAddress.IsDefault)
            {
                // 3) غیرفعال کردن تمام پیش‌فرض‌های قبلی
                var existingDefaults = await _context.UserAddresses
                    .Where(a => a.UserId == userId && a.IsDefault)
                    .ToListAsync();

                foreach (var addr in existingDefaults)
                    addr.UnsetDefault();

                // 4) تنظیم آدرس مورد نظر به عنوان پیش‌فرض
                targetAddress.SetAsDefault();

                // 5) ذخیره تغییرات
                await _context.SaveChangesAsync();
            }
        }
    }
}