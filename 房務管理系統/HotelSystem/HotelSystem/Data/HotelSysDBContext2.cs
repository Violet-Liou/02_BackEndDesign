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

    public int GetMemberWithTelCount()
    {
        // 這裡可以實現計算 MemberWithTel 的數量的邏輯
        // 例如，返回 MemberTel 的計數
        return this.MemberTel.Count();
    }
    //public virtual DbSet<MemberWithTel> MemberWithTel { get; set; }


    //public async Task<List<object>> getMemberWithTel(string MemberID)//object 只先不宣告會是哪一種的物件，等到後面叫用才會知道
    //{
    //    return await this.MemberWithTel.FromSqlRaw($"exec getMemberWithTel {MemberID}").AsNoTracking();
    //}
}
