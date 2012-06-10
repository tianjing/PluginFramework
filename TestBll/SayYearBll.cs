using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBll
{
    public class SayYearBll : TestIBll.ISayYear
    {
        TestDal.SayYearDal dal = new TestDal.SayYearDal();




        public string Year()
        {
            return dal.Year();
        }
    }
}
