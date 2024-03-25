using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_A3850_DAL.Models;
using FinalProject_A3850_DAL.Models.Dto.Req;
using FinalProject_A3850_DAL.Models.Dto.Res;
using FinalProject_A3850_DAL.Repositories.Service.Interface;
using Microsoft.Extensions.Configuration;
using NpgsqlTypes;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;

namespace FinalProject_A3850_DAL.Repositories.Service
{
    public class CarService : ICarService
    {
        readonly FinalprojectdbContext _context;
        private readonly IConfiguration _config;

        public CarService(FinalprojectdbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public void AddBrand(ReqBrandDto brandDto)
        {
            if (_context.MstBrands.Any(x => x.Name == brandDto.Name))
            {
                throw new Exception("Nama brand sudah ada");
            }

            var newBrand = new MstBrand
            {
                Name = brandDto.Name,
                Country = brandDto.Country,
                DtAdded = DateTime.Now,
                DtUpdated = DateTime.Now
            };

            _context.MstBrands.Add(newBrand);
            _context.SaveChanges();
        }


        // public void DeleteBrand(int id)
        // {
        //     var brand = _context.MstBrands.Find(id);
        //     if (brand == null)
        //     {
        //         throw new Exception("Brand tidak ditemukan");
        //     }

        //     _context.MstBrands.Remove(brand);
        //     _context.SaveChanges();
        // }

        public void DeleteBrand(int id)
        {
            try
            {
                var dataUser = _context.MstBrands
                    .FirstOrDefault(x => x.Id == id);
                if (dataUser == null)
                {
                    throw new Exception("Data tidak ditemukan");
                }
                _context.Remove(dataUser);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal hapus data: {ex.Message}");
            }
        }

        public void DeleteModel(int id)
        {
            try
            {
                var dataModel = _context.MstModels
                    .FirstOrDefault(x => x.Id == id);
                if (dataModel == null)
                {
                    throw new Exception("Data tidak ditemukan");
                }
                _context.Remove(dataModel);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal hapus data: {ex.Message}");
            }
        }

        public void DeleteType(int id)
        {
            try
            {
                var dataUser = _context.MstTypes
                    .FirstOrDefault(x => x.Id == id);
                if (dataUser == null)
                {
                    throw new Exception("Data tidak ditemukan");
                }
                _context.Remove(dataUser);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal hapus data: {ex.Message}");
            }
        }

        public void DeleteUsage(int id)
        {
            try
            {
                var dataUser = _context.MstUsages
                    .FirstOrDefault(x => x.Id == id);
                if (dataUser == null)
                {
                    throw new Exception("Data tidak ditemukan");
                }
                _context.Remove(dataUser);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal hapus data: {ex.Message}");
            }
        }

        public IEnumerable<ResModelsDto> GetAllBrands()
        {
            return _context.MstBrands.Select(b => new ResModelsDto
            {
                BrandId = b.Id,
                BrandName = b.Name
            }).ToList();

        }

        public List<ResBrandDto> GetBrand()
        {
            return _context.MstBrands
                .OrderBy(b => b.Id)
                .Select(b => new ResBrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Country = b.Country,
                }).ToList();
        }

        public ResBrandDto GetBrand(int id)
        {
            var brand = _context.MstBrands
                .Where(b => b.Id == id)
                .Select(b => new ResBrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Country = b.Country,
                }).FirstOrDefault();

            if (brand == null)
            {
                throw new Exception("Brand tidak ditemukan");
            }

            return brand;
        }

        public async Task<List<ResBrandDto>> GetBrandInfoFromView()
        {
            var brands = new List<ResBrandDto>();
            using (var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT id, name, country FROM vw_brand_info", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            brands.Add(new ResBrandDto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Country = reader.IsDBNull(reader.GetOrdinal("country")) ? null : reader.GetString(reader.GetOrdinal("country")),
                            });
                        }
                    }
                }
            }
            return brands;
        }

        public ResModelsDto GetModel(int id)
        {
            var brand = _context.MstModels
                .Where(b => b.Id == id)
                .Select(b => new ResModelsDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    BrandId = b.BrandId
                }).FirstOrDefault();

            if (brand == null)
            {
                throw new Exception("Model tidak ditemukan");
            }

            return brand;
        }

        public List<ResModelsDto> GetModels()
        {
            return _context.MstModels
                .OrderBy(b => b.Id)
                .Select(b => new ResModelsDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    BrandId = b.BrandId,
                    DtAdded = b.DtAdded ?? DateTime.MinValue,
                    DtUpdated = b.DtUpdated ?? DateTime.MinValue,
                }).ToList();
        }

        public async Task<List<ResModelsDto>> GetModelInfoFromView()
        {
            var models = new List<ResModelsDto>();
            try
            {
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT * FROM vw_model", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                models.Add(new ResModelsDto
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    BrandName = reader.GetString(reader.GetOrdinal("brand")),
                                    BrandId = reader.GetInt32(reader.GetOrdinal("brand_id")), // Fetch and map BrandId
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error executing GetModelInfoFromView: {ex.Message}");
            }
            return models;
        }


        public ResTypeDto GetType(int id)
        {
            var type = _context.MstTypes
                .Where(b => b.Id == id)
                .Select(b => new ResTypeDto
                {
                    Id = b.Id,
                    Name = b.Name,
                }).FirstOrDefault();

            if (type == null)
            {
                throw new Exception("Type tidak ditemukan");
            }

            return type;
        }

        public async Task<List<ResTypeDto>> GetTypeInfoFromView()
        {
            var type = new List<ResTypeDto>();
            using (var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM vw_type", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            type.Add(new ResTypeDto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                            });
                        }
                    }
                }
            }
            return type;
        }

        public ResUsageDto GetUsage(int id)
        {
            var type = _context.MstUsages
                .Where(b => b.Id == id)
                .Select(b => new ResUsageDto
                {
                    Id = b.Id,
                    Name = b.Name,
                }).FirstOrDefault();

            if (type == null)
            {
                throw new Exception("Usage tidak ditemukan");
            }

            return type;
        }

        public async Task<List<ResUsageDto>> GetUsageInfoFromView()
        {
            var type = new List<ResUsageDto>();
            using (var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM vw_usage", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            type.Add(new ResUsageDto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                            });
                        }
                    }
                }
            }
            return type;
        }

        public async Task<PrcOutMess> InsertOrUpdateBrandAsync(ReqBrandDto brand)
        {
            try
            {
                string procedureName = "prc_upsert_brand";
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                PrcOutMess model = new PrcOutMess();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        if (brand.Id > 0)
                        {
                            command.Parameters.AddWithValue("p_id", brand.Id);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("p_id", 0);
                        }
                        command.Parameters.Add(new NpgsqlParameter("p_name", NpgsqlDbType.Varchar) { Value = brand.Name });
                        command.Parameters.Add(new NpgsqlParameter("p_country", NpgsqlDbType.Varchar) { Value = brand.Country });


                        var p_message = new NpgsqlParameter("p_message", NpgsqlDbType.Varchar)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_message);

                        await command.ExecuteNonQueryAsync();

                        model.MESSAGE = p_message.Value.ToString();
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error executing InsertOrUpdateBrandAsync: {ex.Message}");
            }
        }

        public async Task<PrcOutMess> InsertOrUpdateModelAsync(ReqModelDto model)
        {
            try
            {
                string procedureName = "prc_model";
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                PrcOutMess result = new PrcOutMess();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        if (model.Id > 0)
                        {
                            command.Parameters.AddWithValue("p_id", model.Id);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("p_id", 0);
                        }
                        command.Parameters.AddWithValue("p_name", model.Name);
                        command.Parameters.AddWithValue("p_brand_id", model.BrandId);

                        var p_message = new NpgsqlParameter("p_message", NpgsqlDbType.Varchar)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_message);

                        await command.ExecuteNonQueryAsync();

                        result.MESSAGE = p_message.Value.ToString();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error executing InsertOrUpdateModelAsync: {ex.Message}");
            }
        }


        public async Task<PrcOutMess> InsertOrUpdateTypeAsync(ReqTypeDto type)
        {
            try
            {
                string procedureName = "prc_type";
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                PrcOutMess model = new PrcOutMess();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        if (type.Id > 0)
                        {
                            command.Parameters.AddWithValue("p_id", type.Id);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("p_id", 0);
                        }
                        command.Parameters.Add(new NpgsqlParameter("p_name", NpgsqlDbType.Varchar) { Value = type.Name });

                        var p_message = new NpgsqlParameter("p_message", NpgsqlDbType.Varchar)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_message);

                        await command.ExecuteNonQueryAsync();

                        model.MESSAGE = p_message.Value.ToString();
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error executing InsertOrUpdateBrandAsync: {ex.Message}");
            }
        }

        public async Task<PrcOutMess> InsertOrUpdateUsageAsync(ReqUsageDto usage)
        {
            try
            {
                string procedureName = "prc_usage";
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                PrcOutMess model = new PrcOutMess();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        if (usage.Id > 0)
                        {
                            command.Parameters.AddWithValue("p_id", usage.Id);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("p_id", 0);
                        }
                        command.Parameters.Add(new NpgsqlParameter("p_name", NpgsqlDbType.Varchar) { Value = usage.Name });

                        var p_message = new NpgsqlParameter("p_message", NpgsqlDbType.Varchar)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        command.Parameters.Add(p_message);

                        await command.ExecuteNonQueryAsync();

                        model.MESSAGE = p_message.Value.ToString();
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error executing InsertOrUpdateTypeAsync: {ex.Message}");
            }
        }

        public void UpdateBrand(ReqBrandDto brandDto)
        {
            var brand = _context.MstBrands.Find(brandDto.Id);
            if (brand == null)
            {
                throw new Exception("Brand tidak ditemukan");
            }

            brand.Name = brandDto.Name;
            brand.Country = brandDto.Country;
            brand.DtUpdated = DateTime.Now;
            _context.SaveChanges();
        }

        public List<ResCarDto> GetCar()
        {
            return _context.MstCars
                .OrderBy(b => b.Id)
                .Select(b => new ResCarDto
                {
                    Id = b.Id,
                    EngineSize = b.EngineSize,
                    FuelType = b.FuelType,
                    ManufactureYear = b.ManufactureYear,
                    CdChassis = b.CdChassis,
                    CdEngine = b.CdEngine,
                    BrandId = b.BrandId,
                    TypeId = b.TypeId,
                    UsageId = b.UsageId,
                    ModelId = b.ModelId,
                    DtAdded = b.DtAdded ?? DateTime.MinValue,
                    DtUpdated = b.DtUpdated ?? DateTime.MinValue,
                    IdUserAdded = b.IdUserAdded,
                    IdUserUpdated = b.IdUserUpdated,
                }).ToList();
        }

        public ResCarDto GetCar(Guid carId)
        {
            var car = _context.MstCars
                .Where(b => b.Id == carId)
                .Select(b => new ResCarDto
                {
                    Id = b.Id,
                    EngineSize = b.EngineSize,
                    FuelType = b.FuelType,
                    ManufactureYear = b.ManufactureYear,
                    CdChassis = b.CdChassis,
                    CdEngine = b.CdEngine,
                    BrandId = b.BrandId,
                    TypeId = b.TypeId,
                    UsageId = b.UsageId,
                    ModelId = b.ModelId,
                    DtAdded = b.DtAdded ?? DateTime.MinValue,
                    DtUpdated = b.DtUpdated ?? DateTime.MinValue,
                    IdUserAdded = b.IdUserAdded,
                    IdUserUpdated = b.IdUserUpdated,
                })
                .FirstOrDefault();
            if (car == null)
            {
                throw new Exception("Model tidak ditemukan");
            }
            return car;
        }

        public async Task<List<ResCarInfoDto>> GetCarWithView()
        {
            var carInfoList = new List<ResCarInfoDto>();
            using (var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                // Ensure the SQL command text matches the view name
                using (var command = new NpgsqlCommand("SELECT * FROM vw_car_info", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            carInfoList.Add(new ResCarInfoDto
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                EngineSize = reader.GetInt32(reader.GetOrdinal("engine_size")),
                                FuelType = reader.GetString(reader.GetOrdinal("fuel_type")),
                                ManufactureYear = reader.GetInt32(reader.GetOrdinal("manufacture_year")),
                                CdChassis = reader.GetString(reader.GetOrdinal("cd_chassis")),
                                CdEngine = reader.GetString(reader.GetOrdinal("cd_engine")),
                                BrandName = reader.GetString(reader.GetOrdinal("brand_name")), // Updated to reflect view changes
                                ModelName = reader.GetString(reader.GetOrdinal("model_name")), // Updated to reflect view changes
                                TypeName = reader.GetString(reader.GetOrdinal("type_name")), // Updated to reflect view changes
                                UsageName = reader.GetString(reader.GetOrdinal("usage_name")), // Updated to reflect view changes
                                DtAdded = reader.GetDateTime(reader.GetOrdinal("dt_added")),
                                DtUpdated = reader.GetDateTime(reader.GetOrdinal("dt_updated")),
                                IdUserAdded = reader.GetGuid(reader.GetOrdinal("id_user_added")),
                                IdUserUpdated = reader.GetGuid(reader.GetOrdinal("id_user_updated"))
                            });
                        }
                    }
                }
            }
            return carInfoList;
        }


        public void UpdateCar(Guid id, ReqCarDto model)
        {
            try
            {
                _context.MstCars
                    .Where(c => c.Id == id)
                    .ExecuteUpdate(setter => setter
                        .SetProperty(c => c.EngineSize, model.EngineSize)
                        .SetProperty(c => c.FuelType, model.FuelType)
                        .SetProperty(c => c.ManufactureYear, model.ManufactureYear)
                        .SetProperty(c => c.CdChassis, model.CdChassis)
                        .SetProperty(c => c.CdEngine, model.CdEngine)
                        .SetProperty(c => c.BrandId, model.BrandId)
                        .SetProperty(c => c.TypeId, model.TypeId)
                        .SetProperty(c => c.UsageId, model.UsageId)
                        .SetProperty(c => c.ModelId, model.ModelId)
                        .SetProperty(c => c.DtUpdated, DateTime.Now)
                        );

            }
            catch (Exception ex)
            {
                throw new Exception($"Tidak dapat mengubah data car : {ex.Message}");
            }
        }

        public void DeleteCar(Guid id)
        {
            try
            {
                var dataCar = _context.MstCars
                    .FirstOrDefault(x => x.Id == id);

                if (dataCar == null)
                {
                    throw new Exception("Data car tidak ditemukan");
                }

                _context.Remove(dataCar);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal hapus data: {ex.Message}");
            }
        }


        public async Task<PrcOutMess> ExecPrcCar(ReqCarDto car, string status)
        {
            try
            {
                string procedureName = "prc_cars";
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                PrcOutMess model = new PrcOutMess();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        if (car.Id != null)
                        {
                            command.Parameters.AddWithValue("p_id", car.Id);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("p_id", Guid.NewGuid);
                        }
                        command.Parameters.AddWithValue("p_engine_size", car.EngineSize);
                        command.Parameters.AddWithValue("p_fuel_type", car.FuelType);
                        command.Parameters.AddWithValue("p_manufacture_year", car.ManufactureYear);
                        command.Parameters.AddWithValue("p_cd_chassis", car.CdChassis);
                        command.Parameters.AddWithValue("p_cd_engine", car.CdEngine);
                        command.Parameters.AddWithValue("p_brand_id", car.BrandId);
                        command.Parameters.AddWithValue("p_type_id", car.TypeId);
                        command.Parameters.AddWithValue("p_usage_id", car.UsageId);
                        command.Parameters.AddWithValue("p_model_id", car.ModelId);
                        // Tambahkan parameter untuk OUT_MESS
                        command.Parameters.Add(new NpgsqlParameter("stat_mess", NpgsqlTypes.NpgsqlDbType.Varchar, 100)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        });

                        await command.ExecuteNonQueryAsync();

                        // Menerapkan output pada model
                        model.MESSAGE = command.Parameters["stat_mess"].Value != DBNull.Value ? command.Parameters["stat_mess"].Value.ToString() : "No value";
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"ExecPrcCatatan : {ex.Message}");
            }
        }

        public List<ResTypeDto> GetType()
        {
            return _context.MstTypes
                .OrderBy(b => b.Id)
                .Select(b => new ResTypeDto
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList();
        }

        public List<ResUsageDto> GetUsage()
        {
            return _context.MstUsages
                .OrderBy(b => b.Id)
                .Select(b => new ResUsageDto
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList();
        }

        public List<ResModelsDto> GetModel()
        {
            return _context.MstModels
                .OrderBy(b => b.Id)
                .Select(b => new ResModelsDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    BrandId = b.BrandId,
                }).ToList();
        }

        public async Task<PrcOutMess> ExecPrcModel(ReqModelDto models, string status)
        {
            try
            {
                string procedureName = "prc_model";
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                PrcOutMess model = new PrcOutMess();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("id", models.Id);
                        command.Parameters.AddWithValue("p_name", models.Name);
                        command.Parameters.AddWithValue("p_brand_id", models.BrandId);

                        command.Parameters.Add(new NpgsqlParameter("stat_mess", NpgsqlTypes.NpgsqlDbType.Varchar, 100)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        });

                        await command.ExecuteNonQueryAsync();

                        model.MESSAGE = command.Parameters["stat_mess"].Value != DBNull.Value ? command.Parameters["stat_mess"].Value.ToString() : "No value";
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"ExecPrcCatatan : {ex.Message}");
            }
        }

        public void UpdateModel(int id, ReqModelDto model)
        {
            try
            {
                _context.MstModels
                    .Where(c => c.Id == id)
                    .ExecuteUpdate(setter => setter
                        .SetProperty(c => c.Name, model.Name)
                        .SetProperty(c => c.BrandId, model.BrandId)
                        .SetProperty(c => c.DtUpdated, DateTime.Now)
                        );

            }
            catch (Exception ex)
            {
                throw new Exception($"Tidak dapat mengubah data model : {ex.Message}");
            }
        }

        public async Task<PrcOutMess> InsertOrUpdateBuyAsync(ReqBuyDto purchaseDto)
        {
            PrcOutMess result = new PrcOutMess();

            try
            {
                string procedureName = "prc_trn_car_purchases";
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Add parameters from ReqBuyDto
                        command.Parameters.AddWithValue("p_id", purchaseDto.Id != Guid.Empty ? (object)purchaseDto.Id : DBNull.Value);
                        command.Parameters.AddWithValue("p_tenor", purchaseDto.Tenor ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("p_down_payment", purchaseDto.DownPayment ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("p_tax", purchaseDto.Tax ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("p_price", purchaseDto.Price ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("p_payment_status", purchaseDto.PaymentStatus ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("p_car_id", purchaseDto.CarId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("p_cust_id", purchaseDto.CustId ?? (object)DBNull.Value);

                        // Assuming your stored procedure has an output parameter for a message
                        command.Parameters.Add(new NpgsqlParameter("message", NpgsqlTypes.NpgsqlDbType.Varchar)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        });

                        await command.ExecuteNonQueryAsync();

                        // Apply the output to the result model
                        result.MESSAGE = command.Parameters["message"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error in InsertOrUpdateBuyAsync: {ex.Message}");
            }

            return result;
        }


        public ReqBuyDto GetPurchase(Guid id)
        {
            var buy = _context.TrnCarPurchases
                .Where(b => b.Id == id)
                .Select(b => new ReqBuyDto
                {
                    Id = b.Id,
                    Tenor = b.Tenor,
                    DownPayment = b.DownPayment,
                    Tax = b.Tax,
                    Price = b.Price,
                    PaymentStatus = b.PaymentStatus,
                    CarId = b.CarId,
                    CustId = b.CustId,
                    DtAdded = b.DtAdded ?? DateTime.MinValue,
                    DtUpdated = b.DtUpdated ?? DateTime.MinValue,
                    IdUserAdded = b.IdUserAdded,
                    IdUserUpdated = b.IdUserUpdated,
                })
                .FirstOrDefault();
            if (buy == null)
            {
                throw new Exception("Model tidak ditemukan");
            }
            return buy;
        }

        // public async Task<List<MstBuy>> GetPurchaseWithView()
        // {
        //     var buy = new List<MstBuy>();
        //     using (var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection")))
        //     {
        //         await connection.OpenAsync();
        //         using (var command = new NpgsqlCommand("SELECT * FROM VW_CAR_PURCHASE", connection))
        //         {
        //             using (var reader = await command.ExecuteReaderAsync())
        //             {
        //                 while (await reader.ReadAsync())
        //                 {
        //                     buy.Add(new MstBuy
        //                     {
        //                         Id = reader.GetGuid(reader.GetOrdinal("id")),
        //                         Tenor = reader.GetInt32(reader.GetOrdinal("tenor")),
        //                         DownPayment = reader.GetDecimal(reader.GetOrdinal("down_payment")),
        //                         Tax = reader.GetDecimal(reader.GetOrdinal("tax")),
        //                         Price = reader.GetDecimal(reader.GetOrdinal("price")),
        //                         PaymentStatus = reader.GetString(reader.GetOrdinal("payment_status")),
        //                         CarId = reader.GetGuid(reader.GetOrdinal("car_id")),
        //                         CustId = reader.GetGuid(reader.GetOrdinal("cust_id")),
        //                         BrandName = reader.GetString(reader.GetOrdinal("brand_name")),
        //                         ModelName = reader.GetString(reader.GetOrdinal("model_name")),
        //                         CustomerName = reader.GetString(reader.GetOrdinal("customer_name")),
        //                         CustomerEmail = reader.GetString(reader.GetOrdinal("customer_email"))
        //                     });
        //                 }
        //             }
        //         }
        //     }
        //     return buy;

        // }


        public void DeletePurchase(Guid id)
        {
            try
            {
                var dataBeli = _context.TrnCarPurchases
                    .FirstOrDefault(x => x.Id == id);

                if (dataBeli == null)
                {
                    throw new Exception("Data tagihan tidak ditemukan");
                }

                _context.Remove(dataBeli);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal hapus data: {ex.Message}");
            }

        }

        public List<ResBuyDto> GetPurchase()
        {
            return _context.TrnCarPurchases
               .OrderBy(b => b.Id)
               .Select(b => new ResBuyDto
               {
                   Id = b.Id,
                   Tenor = b.Tenor,
                   DownPayment = b.DownPayment,
                   Tax = b.Tax,
                   Price = b.Price,
                   PaymentStatus = b.PaymentStatus,
                   CarId = b.CarId,
                   CustId = b.CustId,
                   DtAdded = b.DtAdded ?? DateTime.MinValue,
                   DtUpdated = b.DtUpdated ?? DateTime.MinValue,
                   IdUserAdded = b.IdUserAdded,
                   IdUserUpdated = b.IdUserUpdated,
               }).ToList();
        }

        // public async Task<List<ResBuyDto>> GetPurchaseWithView()
        // {
        //     var buy = new List<ResBuyDto>();
        //     using (var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection")))
        //     {
        //         await connection.OpenAsync();
        //         using (var command = new NpgsqlCommand("SELECT * FROM VW_CAR_PURCHASE", connection))
        //         {
        //             using (var reader = await command.ExecuteReaderAsync())
        //             {
        //                 while (await reader.ReadAsync())
        //                 {
        //                     buy.Add(new ResBuyDto
        //                     {
        //                         Id = reader.GetGuid(reader.GetOrdinal("id")),
        //                         Tenor = reader.GetInt32(reader.GetOrdinal("tenor")),
        //                         DownPayment = reader.GetDecimal(reader.GetOrdinal("down_payment")),
        //                         Tax = reader.GetDecimal(reader.GetOrdinal("tax")),
        //                         Price = reader.GetDecimal(reader.GetOrdinal("price")),
        //                         PaymentStatus = reader.IsDBNull(reader.GetOrdinal("payment_status")) ? null : reader.GetString(reader.GetOrdinal("payment_status")),
        //                         CarId = reader.GetGuid(reader.GetOrdinal("car_id")),
        //                         CustId = reader.GetGuid(reader.GetOrdinal("cust_id")),
        //                     });
        //                 }
        //             }
        //         }
        //     }
        //     return buy;
        // }

        public List<ResCustomerDto> GetCustomer()
        {
            return _context.MstCustomers
                .OrderBy(b => b.Id)
                .Select(b => new ResCustomerDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Email = b.Email,
                    Address = b.Address,
                    Gender = b.Gender,
                    Occupancy = b.Occupancy,
                    Salary = b.Salary,
                    DtAdded = b.DtAdded ?? DateTime.MinValue,
                    DtUpdated = b.DtUpdated ?? DateTime.MinValue,
                    IdUserAdded = b.IdUserAdded,
                    IdUserUpdated = b.IdUserUpdated,
                }).ToList();
        }

        public async Task<PrcOutMess> InsertOrUpdateCustomerAsync(ReqCustomerDto customer)
        {
            try
            {
                string procedureName = "prc_master_customer";
                string connectionString = _config.GetConnectionString("DefaultConnection") ?? "";

                PrcOutMess model = new PrcOutMess();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Assuming the stored procedure and DTO are designed with these fields
                        command.Parameters.AddWithValue("p_id", customer.Id != Guid.Empty ? (object)customer.Id : DBNull.Value);
                        command.Parameters.AddWithValue("p_name", customer.Name);
                        command.Parameters.AddWithValue("p_email", customer.Email);
                        command.Parameters.AddWithValue("p_address", customer.Address);
                        command.Parameters.AddWithValue("p_gender", customer.Gender);
                        command.Parameters.AddWithValue("p_occupancy", customer.Occupancy);
                        command.Parameters.AddWithValue("p_salary", customer.Salary ?? 0); // Assuming salary is required; adjust null handling as needed

                        // Assuming your stored procedure has an output parameter for a message
                        command.Parameters.Add(new NpgsqlParameter("stat_mess", NpgsqlTypes.NpgsqlDbType.Varchar)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        });

                        await command.ExecuteNonQueryAsync();

                        // Apply the output to the model
                        model.MESSAGE = command.Parameters["stat_mess"].Value.ToString();
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error in InsertOrUpdateCustomerAsync: {ex.Message}");
            }
        }


        public ResCustomerDto GetCustomer(Guid id)
        {
            var customer = _context.MstCustomers
                .Where(c => c.Id == id)
                .Select(c => new ResCustomerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    Gender = c.Gender,
                    Occupancy = c.Occupancy,
                    Salary = c.Salary,
                    DtAdded = c.DtAdded,
                    DtUpdated = c.DtUpdated,
                    IdUserAdded = c.IdUserAdded,
                    IdUserUpdated = c.IdUserUpdated
                }).FirstOrDefault();

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            return customer;
        }

        public void DeleteCustomer(Guid id)
        {
            try
            {
                var customer = _context.MstCustomers
                    .FirstOrDefault(x => x.Id == id);
                if (customer == null)
                {
                    throw new Exception("Customer not found");
                }
                _context.Remove(customer);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete customer: {ex.Message}");
            }
        }

        public async Task<List<ResCustomerDto>> GetCustomerWithView()
        {
            var customers = new List<ResCustomerDto>();
            using (var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT id, name, email, address, gender, occupancy, salary, dt_added, dt_updated, id_user_added, id_user_updated FROM vw_customer", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            customers.Add(new ResCustomerDto
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name")),
                                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                                Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address")),
                                Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? null : reader.GetString(reader.GetOrdinal("gender")),
                                Occupancy = reader.IsDBNull(reader.GetOrdinal("occupancy")) ? null : reader.GetString(reader.GetOrdinal("occupancy")),
                                Salary = reader.IsDBNull(reader.GetOrdinal("salary")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("salary")),
                                DtAdded = reader.IsDBNull(reader.GetOrdinal("dt_added")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dt_added")),
                                DtUpdated = reader.IsDBNull(reader.GetOrdinal("dt_updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dt_updated")),
                                IdUserAdded = reader.IsDBNull(reader.GetOrdinal("id_user_added")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("id_user_added")),
                                IdUserUpdated = reader.IsDBNull(reader.GetOrdinal("id_user_updated")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("id_user_updated")),
                            });
                        }
                    }
                }
            }
            return customers;
        }

        public async Task<List<MstBuy>> GetPurchaseWithView()
        {
            var purchaseDetails = new List<MstBuy>();

            // Assuming you have a method to get a database connection
            using (var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM vw_car_purchase_details", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var detail = new MstBuy
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("purchase_id")),
                                Tenor = reader.GetInt32(reader.GetOrdinal("tenor")),
                                DownPayment = reader.GetDecimal(reader.GetOrdinal("down_payment")),
                                Tax = reader.GetDecimal(reader.GetOrdinal("tax")),
                                Price = reader.GetDecimal(reader.GetOrdinal("price")),
                                PaymentStatus = reader.GetString(reader.GetOrdinal("payment_status")),
                                CustomerName = reader.GetString(reader.GetOrdinal("customer_name")),
                                ModelName = reader.GetString(reader.GetOrdinal("model_name")),
                                BrandName = reader.GetString(reader.GetOrdinal("brand_name")),
                                CarId = reader.GetGuid(reader.GetOrdinal("car_id")),
                                CustId = reader.GetGuid(reader.GetOrdinal("cust_id")),
                            };
                            purchaseDetails.Add(detail);
                        }
                    }
                }
            }

            return purchaseDetails;
        }
    }
}