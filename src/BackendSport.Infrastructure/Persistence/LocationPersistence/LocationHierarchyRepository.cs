using MongoDB.Driver;
using MongoDB.Bson;
using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Infrastructure.Persistence.LocationPersistence;

public class LocationHierarchyRepository : ILocationHierarchyRepository
{
    private readonly IMongoDatabase _database;

    public LocationHierarchyRepository(MongoDbContext context)
    {
        _database = context.Database;
    }

    public async Task<LocationHierarchyResponseDto> GetLocationHierarchyAsync(string countryId)
    {
        var pipeline = new[]
        {
            new BsonDocument("$facet", new BsonDocument
            {
                ["countries"] = new BsonArray
                {
                    new BsonDocument("$match", new BsonDocument("isActive", true)),
                    new BsonDocument("$project", new BsonDocument
                    {
                        ["_id"] = 1,
                        ["name"] = 1,
                        ["code"] = 1
                    })
                },
                ["departments"] = new BsonArray
                {
                    new BsonDocument("$match", new BsonDocument
                    {
                        ["countryId"] = countryId,
                        ["isActive"] = true
                    }),
                    new BsonDocument("$project", new BsonDocument
                    {
                        ["_id"] = 1,
                        ["name"] = 1,
                        ["code"] = 1,
                        ["countryId"] = 1
                    })
                },
                ["municipalities"] = new BsonArray
                {
                    new BsonDocument("$match", new BsonDocument("isActive", true)),
                    new BsonDocument("$project", new BsonDocument
                    {
                        ["_id"] = 1,
                        ["name"] = 1,
                        ["code"] = 1,
                        ["departmentId"] = 1,
                        ["type"] = 1,
                        ["isCapital"] = 1
                    })
                },
                ["cities"] = new BsonArray
                {
                    new BsonDocument("$match", new BsonDocument("isActive", true)),
                    new BsonDocument("$project", new BsonDocument
                    {
                        ["_id"] = 1,
                        ["name"] = 1,
                        ["code"] = 1,
                        ["municipalityId"] = 1,
                        ["type"] = 1,
                        ["isCapital"] = 1
                    })
                },
                ["localities"] = new BsonArray
                {
                    new BsonDocument("$match", new BsonDocument("isActive", true)),
                    new BsonDocument("$project", new BsonDocument
                    {
                        ["_id"] = 1,
                        ["name"] = 1,
                        ["code"] = 1,
                        ["municipalityId"] = 1,
                        ["type"] = 1
                    })
                }
            })
        };

        // Ejecutar la agregación en la colección de países (puede ser cualquier colección)
        var result = await _database.GetCollection<Country>("countries")
            .Aggregate<BsonDocument>(pipeline)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return new LocationHierarchyResponseDto();
        }

        return new LocationHierarchyResponseDto
        {
            Countries = result["countries"].AsBsonArray
                .Select(c => new CountryDto
                {
                    Id = c["_id"].AsString,
                    Name = c["name"].AsString,
                    Code = c["code"].AsString
                }).ToList(),
            
            Departments = result["departments"].AsBsonArray
                .Select(d => new DepartmentDto
                {
                    Id = d["_id"].AsString,
                    Name = d["name"].AsString,
                    Code = d["code"].AsString,
                    CountryId = d["countryId"].AsString
                }).ToList(),
            
            Municipalities = result["municipalities"].AsBsonArray
                .Select(m => new MunicipalityDto
                {
                    Id = m["_id"].AsString,
                    Name = m["name"].AsString,
                    Code = m["code"].AsString,
                    DepartmentId = m["departmentId"].AsString,
                    Type = m["type"].AsString,
                    IsCapital = m["isCapital"].AsBoolean
                }).ToList(),
            
            Cities = result["cities"].AsBsonArray
                .Select(c => new CityDto
                {
                    Id = c["_id"].AsString,
                    Name = c["name"].AsString,
                    Code = c["code"].AsString,
                    MunicipalityId = c["municipalityId"].AsString,
                    Type = c["type"].AsString,
                    IsCapital = c["isCapital"].AsBoolean
                }).ToList(),
            
            Localities = result["localities"].AsBsonArray
                .Select(l => new LocalityDto
                {
                    Id = l["_id"].AsString,
                    Name = l["name"].AsString,
                    Code = l["code"].AsString,
                    MunicipalityId = l["municipalityId"].AsString,
                    Type = l["type"].AsString
                }).ToList()
        };
    }
} 