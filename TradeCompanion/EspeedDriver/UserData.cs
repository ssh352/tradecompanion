using System;
using System.Collections.Generic;
using System.Text;

namespace EspeedDriver
{
    public class UserData
    {
        public String clientOrderId;
        public String senderId;

        public UserData()
        {
        }

        public UserData(String clientOrderId, String senderId)
        {
            this.clientOrderId = clientOrderId;
            this.senderId = senderId;
        }
    }
}
