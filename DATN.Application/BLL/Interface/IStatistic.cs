﻿using DATN.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IStatistic
    {
        Task<int> StatisticCampus();
        Task<int> StatisticBuilding();
        Task<StatisticRoom> StatisticRoom();
        Task<int> StatisticStudent();
        Task<StatisticInvoice> StatisticInvoice();

    }
}
