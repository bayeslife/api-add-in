using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace APIAddIn
{
    public class DataTypeManager
    {

        static public JSchema getCurrencyType()
        {
            return new JSchema
            {
                Type = JSchemaType.String,
                Pattern = @"^(?=\(.*\)|[^()]*$)\(?\d{1,3}(\d{3})?(\.\d\d)?\)?$",
                Description = "A currency represented as a formatted string. https://confluence.iag.com.au/display/NZTU/Representation+of+currency+in+Web+API",
                Title = "Currency"
            };
        }

        static public JSchema getNumberType()
        {
            return new JSchema { Type = JSchemaType.Number };
        }

        static public JSchema getIntegerType()
        {
            return new JSchema { Type = JSchemaType.Integer };
        }

        static public JSchema getDateType()
        {
            return new JSchema
            {
                Type = JSchemaType.String,
                Pattern = @"^([0-9]+)-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$",
                Description = "A date represented as a formatted string. https://confluence.iag.com.au/x/sKT-CQ",
                Title = "Date"
            };
        }

        static public JSchema getDateTimeType()
        {
            return new JSchema
            {
                Type = JSchemaType.String,
                Pattern = @"^([0-9]+)-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])[Tt]([01][0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]|60)(\.[0-9]+)?(([Zz])|([\+|\-]([01][0-9]|2[0-3]):[0-5][0-9]))$",
                Description = "A datetime represented as a formatted string. https://confluence.iag.com.au/x/sKT-CQ",
                Title = "Date Time"
            };
        }

        static public JSchema getBooleanType()
        {
            return new JSchema { Type = JSchemaType.Boolean };
        }

        static public JSchema getStringType()
        {
            return new JSchema { Type = JSchemaType.String };
        }
    }
}
