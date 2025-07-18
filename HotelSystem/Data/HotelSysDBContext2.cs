using System;
using System.Collections.Generic;
using HotelSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Data;

public partial class HotelSysDBContext2 : HotelSysDBContext
{
    public HotelSysDBContext2(DbContextOptions<HotelSysDBContext> options)
        : base(options)
    {
    }

    public int GetRoomServiceCount()
    {
        return RoomService.CountAsync().Result;
    }
}
