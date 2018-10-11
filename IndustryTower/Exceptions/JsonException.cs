using System;

namespace IndustryTower.Exceptions
{
    [Serializable]
    public  class JsonCustomException: Exception
    {
        public  JsonCustomException(string Message)
            :base(Message)
        { 
            
        }

        public  JsonCustomException(string Message, System.Exception inner)
            : base(Message, inner)
        {

        }
    }
}