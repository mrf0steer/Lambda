using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OMS_AWS_API_CORE;
using System.Collections;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System.Web;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]


namespace RESPONSIBLE_PERSON;
public class Function
{
    public MySql_class psql1 = new MySql_class();

    public Update FunctionHandler(OrderInput input, ILambdaContext context)
    {
        Update result = new Update();
        
        ArrayList users = psql1.Query("SELECT ID, ID_1C FROM OMS_USERS WHERE ID=" + input.ID + ";");

        if (users.Count > 0)
        {
            Hashtable user = (Hashtable)users[0];
            
            ArrayList orders = psql1.Query("SELECT ID, RESPONSIBLE_ID FROM OMS_ORDERS WHERE ID=" + input.ORDER + ";");

            if (orders.Count > 0)
            {
                foreach (Hashtable order in orders)
                {
                    psql1.QueryNoResult("UPDATE OMS_ORDERS SET RESPONSIBLE_ID ='" + user["ID_1C"].ToString() + "' WHERE ID='" + order["ID"].ToString() + "';");
                }
                result.Message = "Updated.";
            }
            else
            {
                result.Message = "Order not found.";
            }
        }
        else
        {
            result.Message = "User not found.";
        }
        psql1.Dispose();
        return result;
    }
}

