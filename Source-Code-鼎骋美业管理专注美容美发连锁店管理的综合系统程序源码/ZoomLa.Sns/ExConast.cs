using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ZoomLa.Sns
{
    public class ExConast
    {
        static ExConast()
        {
            UserLevelDT.Columns.Add(new DataColumn("level", typeof(int)));
            UserLevelDT.Columns.Add(new DataColumn("name", typeof(string)));
            string[] levelNames = "会员|银卡|金卡|铂金卡|钻石卡".Split('|');
            for (int i = 0; i < levelNames.Length; i++)
            {
                DataRow dr = UserLevelDT.NewRow();
                dr["level"] = i;
                dr["name"] = levelNames[i];
                UserLevelDT.Rows.Add(dr);
            }
        }
        public const int EmployGroup = 4;//门店员工(可操作收银)
        public const int EmployJSGroup = 6;//门店技师
        public const int EmployBossGroup = 7;//门店老板
        public const int ClientGroup = 5;
        public static DataTable UserLevelDT = new DataTable();
    }
}
