﻿using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.WorkItemTracking
{
    /// <summary>
    /// 
    /// Samples for accessing work item field metadata.
    /// 
    /// See https://www.visualstudio.com/docs/integrate/api/wit/fields for more details.
    /// 
    /// </summary>
    [ClientSample(WitConstants.WorkItemTrackingWebConstants.RestAreaName, WitConstants.WorkItemTrackingRestResources.Fields)]
    public class FieldsSample : ClientSample
    {
        [ClientSampleMethod]
        public WorkItemField CreateWorkItemField()
        {
            WorkItemField newlyCreatedWorkItemField = new WorkItemField();

            WorkItemField newWorkItemField = new WorkItemField()
            {
                Name = "New Work Item Field",
                Description = "New work item filed for testing",
                Type = FieldType.String
            };

            VssConnection connection = Context.Connection;
            WorkItemTrackingHttpClient workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                newlyCreatedWorkItemField = workItemTrackingClient.CreateFieldAsync(newWorkItemField).Result;
                Console.WriteLine("Work Item Field Create Succeeded.");
            }
            catch
            {
                Console.WriteLine("Work Item Field Create Failed.");
            }
            //Destroy resource
            try
            {
                workItemTrackingClient.DeleteFieldAsync("New Work Item Field");
                Console.WriteLine("Work Item Field Delete Succeeded.");
            }
            catch
            {
                Console.WriteLine("Work Item Field Delete Failed.");
            }

            return newlyCreatedWorkItemField;
        }

        [ClientSampleMethod]
        public WorkItemField GetFieldDetails()
        {
            string fieldName = "System.Title";

            VssConnection connection = Context.Connection;
            WorkItemTrackingHttpClient workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();

            WorkItemField workitemField = workItemTrackingClient.GetFieldAsync(fieldName).Result;

            Console.WriteLine("Name: " + workitemField.Name);
            Console.WriteLine("Ref name: " + workitemField.ReferenceName);
            Console.WriteLine("Read only? " + workitemField.ReadOnly);

            return workitemField;
        }

        [ClientSampleMethod]
        public void GetReadOnlyWorkItemFields()
        {
            VssConnection connection = Context.Connection;
            WorkItemTrackingHttpClient workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();

            List<WorkItemField> result = workItemTrackingClient.GetFieldsAsync().Result;

            Console.WriteLine("Read only fields:");
            foreach (var workitemField in result.Where(field => field.ReadOnly))
            {
                Console.WriteLine(" * {0} ({1})", workitemField.Name, workitemField.ReferenceName);
            }
        }

        [ClientSampleMethod]
        public void DeleteWorkItemField()
        {
            VssConnection connection = Context.Connection;
            WorkItemTrackingHttpClient workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();

            WorkItemField newWorkItemField = new WorkItemField()
            {
                Name = "New Work Item Field",
                ReferenceName = "SupportedOperations.GreaterThanEquals",
                Description = "New work item filed for testing",
                Type = FieldType.String
            };

            //First create resource
            try
            {
                workItemTrackingClient.CreateFieldAsync(newWorkItemField);
                Console.WriteLine("Work Item Field Create Succeeded.");
            }
            catch
            {
                Console.WriteLine("Work Item Field Create Failed.");
            }

            //Then test DeleteWorkItemField
            try
            {
                workItemTrackingClient.DeleteFieldAsync("New Work Item Field");
                Console.WriteLine("Work Item Field Delete Succeeded.");
            }
            catch
            {
                Console.WriteLine("Work Item Field Delete Failed.");
            }
        }
    }
}
