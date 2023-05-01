using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HCFx.Extender.DataType
{
    public static class SqlExceptionExtender
    {
        public static bool IsDuplicateKey(this SqlException sqlException)
        {
            return sqlException.Number == 2627;
        }

        public static bool IsForeignKeyViolation(this SqlException sqlException)
        {
            return sqlException.Number == 547;
        }
    }
}
