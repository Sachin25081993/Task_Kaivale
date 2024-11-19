using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

public class RecordDeclinePlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

        if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
        {
            Entity target = (Entity)context.InputParameters["Target"];

            if (target.LogicalName == "email" && (bool)target["isspam"] == true)
            {
                // Create a new record in the custom table
                Entity declineRecord = new Entity("new_declinerecord");
                declineRecord["new_emailaddress"] = target["to"];
                declineRecord["new_declinereason"] = "Marked as Spam";
                declineRecord["new_timestamp"] = DateTime.Now;

                service.Create(declineRecord);
            }
        }
    }
}
