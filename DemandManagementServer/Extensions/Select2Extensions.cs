using System.Collections.Generic;

namespace DemandManagementServer.Extensions
{
    public static class Select2Extensions
    {
        public static List<int> Select2StringToList(this string select2String)
        {
            List<int> ids=new List<int>();
            if (select2String == null)
            {
                return ids;
            }
            foreach (var id in select2String.Split(','))
            {
                ids.Add(int.Parse(id));
            }
            return ids;
        }
    }
}
