using MongoDB.Driver;
using NetBazaar.Application.DTOs.Visitor;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using NetBazaar.Application.Interfaces.Visitor;
using NetBazaar.Domain.Entities.VisitorContext;
using System;

namespace NetBazaar.Infrastructure.Services.Visitors
{
    public class SaveVisitorInfoService : ISaveVisitorInfoService
    {
        private readonly IMongoCollection<NetBazaar.Domain.Entities.VisitorContext.Visitor> _collection;

        public SaveVisitorInfoService(IMongoDbContext<NetBazaar.Domain.Entities.VisitorContext.Visitor> dbContext)
        {
            _collection = dbContext?.GetCollection()
                ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Execute(RequestSaveVisitorInfoDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var visitor = MapToVisitorEntity(request);
            _collection.InsertOne(visitor);
        }

        private static NetBazaar.Domain.Entities.VisitorContext.Visitor MapToVisitorEntity(RequestSaveVisitorInfoDTO request)
        {
            return new NetBazaar.Domain.Entities.VisitorContext.Visitor
            {
                //Id = Guid.NewGuid().ToString(),
                Browser = CreateVisitorVersion(request.Browser),
                CurrentUrl = request.CurrentUrl,
                Device = CreateVisitorDevice(request.Device),
                HttpMethod = request.HttpMethod,
                IpAddress = request.IpAddress,
                OperatingSystem = CreateVisitorVersion(request.OperatingSystem),
                PhysicalPath = request.PhysicalPath,
                ReferrerUrl = request.ReferrerUrl,
                ReferringAction = request.ReferringAction,
                ReferringController = request.ReferringController,
                VisitedAt = request.VisitedAt,
                IsBot = request.IsBot,
            };
        }

        private static VisitorVersion CreateVisitorVersion(VisitorVersionDto versionDto)
        {
            if (versionDto == null)
                return null;

            return new VisitorVersion
            {
                Family = versionDto.Family,
                Version = versionDto.Version
            };
        }

        private static VisitorDevice CreateVisitorDevice(VisitorDeviceDto deviceDto)
        {
            if (deviceDto == null)
                return null;

            return new VisitorDevice
            {
                Brand = deviceDto.Brand,
                Model = deviceDto.Model,
                DeviceType = deviceDto.Family // Note: Mapping Family to DeviceType
            };
        }
    }
}