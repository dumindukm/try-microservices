using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReservationFass.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReservationFass
{
    public static class ReservationFunction
    {
        enum Sources
        {
            AccountValidation
        }

        [FunctionName("Reservation_Orchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
           
            Reservation reservation = context.GetInput<Reservation>();
            CommandProducer producer = new CommandProducer(context.InstanceId , Sources.AccountValidation.ToString());
            producer.ReservationMessage = reservation;

            var validateAccount = await context.CallActivityAsync<ProducerResult>(nameof(AccountValidationEventProducer), producer);

            if (!validateAccount.Valid)
            {
                // Operation Failed Save state as failed
            }

            bool approved = await context.WaitForExternalEvent<bool>(Sources.AccountValidation.ToString());

            if (!approved)
            {
                // Account validation failed. Operation is failed. 

            }


        }

        // Call by orchestrator. 
        // Add event to AccountValidationEventProducerHub for account details validation
        [FunctionName(nameof(AccountValidationEventProducer))]
        public static async Task<ProducerResult> AccountValidationEventProducer(
            [EventHub("AccountValidationEventProducerHub", Connection = "EventHubConnectionAppSetting")] IAsyncCollector<string> outputEvents, 
            [ActivityTrigger] CommandProducer command, ILogger log)
        {
            ProducerResult result = new ProducerResult();

            var commandString = JsonConvert.SerializeObject(command);
            byte[] messageBytes = Encoding.UTF8.GetBytes(commandString);
            await outputEvents.AddAsync(JsonConvert.SerializeObject(messageBytes));

            return result;
        }

        // Listen to events in AccountValidationEventProducerHub and do AccountValidation
        // Results are then write to ReplyEventtHub.
        [FunctionName(nameof(ValidateAccount))]
        public static async Task ValidateAccount(
            [EventHub("ReplyEventtHub", Connection = "EventHubConnectionAppSetting")] IAsyncCollector<string> outputEvents,
            [EventHubTrigger("AccountValidationEventProducerHub", Connection = "EventHubConnectionAppSetting")] EventData eventHubEvent, ILogger log)
        {
            CommandProducer command = JsonConvert.DeserializeObject<CommandProducer>( Encoding.UTF8.GetString(eventHubEvent.Body));
            // using CommdProducer details do the account validation. if success OperationStatus = true

            await outputEvents.AddAsync(JsonConvert.SerializeObject(command));

        }


        [FunctionName(nameof(EventReplyReader))]
        public static async Task EventReplyReader(
            [EventHubTrigger("ReplyEventtHub", Connection = "EventHubConnectionAppSetting")] EventData eventHubEvent,
            [DurableClient] IDurableOrchestrationClient client, ILogger log)
        {
            CommandProducer command = JsonConvert.DeserializeObject<CommandProducer>(Encoding.UTF8.GetString(eventHubEvent.Body));
            await client.RaiseEventAsync(command.MessageId, command.Source, command.OperationStatus);
        }

        [FunctionName("Reservation_Start")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] Reservation reservation,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var trackingId = Guid.NewGuid();
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Reservation_Orchestrator", trackingId.ToString(), reservation);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            //return starter.CreateCheckStatusResponse(req, instanceId);
            return new OkObjectResult(trackingId);
        }
    }
}