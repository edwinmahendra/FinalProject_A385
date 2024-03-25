using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_A3850_DAL.Models;
using FinalProject_A3850_DAL.Models.Dto.Req;
using FinalProject_A3850_DAL.Models.Dto.Res;

namespace FinalProject_A3850_DAL.Repositories.Service.Interface
{
    public interface ICarService
    {
        //tambahkan interface untuk CRUD user
        void AddBrand(ReqBrandDto brand);
        List<ResBrandDto> GetBrand();
        ResBrandDto GetBrand(int id);
        void UpdateBrand(ReqBrandDto brand);
        void DeleteBrand(int id);

        Task<PrcOutMess> InsertOrUpdateBrandAsync(ReqBrandDto brand);
        Task<List<ResBrandDto>> GetBrandInfoFromView();

        List<ResTypeDto> GetType();
        ResTypeDto GetType(int id);
        void DeleteType(int id);
        Task<PrcOutMess> InsertOrUpdateTypeAsync(ReqTypeDto type);
        Task<List<ResTypeDto>> GetTypeInfoFromView();

        List<ResUsageDto> GetUsage();
        ResUsageDto GetUsage(int id);
        void DeleteUsage(int id);
        Task<PrcOutMess> InsertOrUpdateUsageAsync(ReqUsageDto usage);
        Task<List<ResUsageDto>> GetUsageInfoFromView();

        List<ResModelsDto> GetModel();
        void UpdateModel(int id, ReqModelDto model);
        ResModelsDto GetModel(int id);
        void DeleteModel(int id);
        Task<PrcOutMess> InsertOrUpdateModelAsync(ReqModelDto model);
        Task<List<ResModelsDto>> GetModelInfoFromView();
        IEnumerable<ResModelsDto> GetAllBrands();
        List<ResModelsDto> GetModels();

        Task<PrcOutMess> ExecPrcModel(ReqModelDto models, string status);

        Task<PrcOutMess> ExecPrcCar(ReqCarDto car, string status);
        List<ResCarDto> GetCar();
        ResCarDto GetCar(Guid carId);
        Task<List<ResCarInfoDto>> GetCarWithView();
        void UpdateCar(Guid id, ReqCarDto model);
        void DeleteCar(Guid id);

        Task<PrcOutMess> InsertOrUpdateBuyAsync(ReqBuyDto buy);
        List<ResBuyDto> GetPurchase();
        ReqBuyDto GetPurchase(Guid id);
        Task<List<MstBuy>> GetPurchaseWithView();
        void DeletePurchase(Guid id);

        Task<PrcOutMess> InsertOrUpdateCustomerAsync(ReqCustomerDto customer);
        List<ResCustomerDto> GetCustomer();
        ResCustomerDto GetCustomer(Guid id);
        Task<List<ResCustomerDto>> GetCustomerWithView();
        void DeleteCustomer(Guid id);
    }
}