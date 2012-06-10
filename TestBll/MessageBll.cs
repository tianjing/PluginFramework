using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBll
{
    
    public class MessageBll:TestIBll.IMessage
    {
        TestDal.MessageDal dal = new TestDal.MessageDal();


        public string HelloMessage()
        {
            return dal.HelloMessage();
        }
    }
}
