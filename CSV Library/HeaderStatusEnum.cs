﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public enum HeaderStatus
    {
        NoFile,    // 檔案不存在
        HasHeader, // 已有標題
        NoHeader   // 有資料但缺標題
    }

}
