﻿using ServiceStationAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceStationAPI.Models
{
    public class CreateOrderNoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int VehicleId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorSurname { get; set; }
        public string CreatorEmail { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}