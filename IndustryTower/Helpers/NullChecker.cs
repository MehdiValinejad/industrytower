using System;

namespace IndustryTower.Helpers
{
    public static class NullChecker
    {
        public static void NullCheck(object[] Params)
        {
            foreach (var item in Params)
            {
                if (item is String)
                {
                    if (String.IsNullOrEmpty((String)item))
                    {
                        throw new NullReferenceException();
                    }
                }
                else
                {
                    if (item == null )
                    {
                        throw new NullReferenceException();
                    }
                }
            }
        }
    }
}