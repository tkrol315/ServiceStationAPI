﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Models;
using System.Runtime.InteropServices;

namespace ServiceStationAPI.Services
{
    public interface IVehicleService
    {
        IEnumerable<VehicleDto> GetVehicles();

        VehicleDto GetVehicle(int id);

        int CreateVehicle(CreateVehicleDto dto);

        bool DeleteVehicle(int id);

        bool UpdateVehicle(int id, UpdateVehicleDto dto);
    }

    public class VehicleService : IVehicleService
    {
        private readonly ServiceStationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VehicleService(ServiceStationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<VehicleDto> GetVehicles()
        {
            var vehicles = _dbContext.Vehicles.Include(v => v.Owner).Include(v => v.Type).ToList();
            var vehicleDtos = _mapper.Map<List<VehicleDto>>(vehicles);
            return vehicleDtos;
        }

        public VehicleDto GetVehicle(int id)
        {
            var vehicle = _dbContext.Vehicles.Include(c => c.Owner).Include(v => v.Type).FirstOrDefault(v => v.Id == id);
            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
            return vehicleDto;
        }

        public int CreateVehicle(CreateVehicleDto dto)
        {
            var vehicle = _mapper.Map<CreateVehicleDto, Vehicle>(dto);
            vehicle.Owner = GetOrCreateOwner(dto);
            _dbContext.Vehicles.Add(vehicle);
            _dbContext.SaveChanges();
            return vehicle.Id;
        }

        public bool DeleteVehicle(int id)
        {
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle != null)
            {
                _dbContext.Vehicles.Remove(vehicle);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateVehicle(int id, UpdateVehicleDto dto)
        {
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle != null)
            {
                vehicle.Brand = dto.Brand;
                vehicle.Model = dto.Model;
                vehicle.RegistrationNumber = dto.RegistrationNumber;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        private User GetOrCreateOwner(CreateVehicleDto dto)
        {
            var owner = _dbContext.Users.FirstOrDefault(u =>
                u.Name.ToLower() == dto.OwnerName.ToLower() &&
                u.Surname.ToLower() == dto.OwnerSurname.ToLower() &&
                u.Email.ToLower() == dto.Email.ToLower() &&
                u.PhoneNumber == dto.PhoneNumber);

            if (owner is null)
            {
                owner = new User()
                {
                    Name = dto.OwnerName,
                    Surname = dto.OwnerSurname,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    RoleId = 1
                };
            }

            return owner;
        }
    }
}