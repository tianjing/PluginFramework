using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    
    public class MyBll:IBLL.IMyBll
    {
        Dal.MyDal dal = new Dal.MyDal();


        public System.Data.DataTable GetList()
        {
            return new System.Data.DataTable();
        }
        public string hello()
        {
            return dal.Hello();
        }
    }
}
