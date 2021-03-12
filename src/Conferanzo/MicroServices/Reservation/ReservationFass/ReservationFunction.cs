using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ReservationFass.Models;
using System;
using System.Collections.Generic;
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

            // Replace "hello" with the name of your Durable Activity Function.
            //outputs.Add(await context.CallActivityAsync<string>("Reservation_Command", "Tokyo"));
            //outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]

        }

        //
        [FunctionName(nameof(AccountValidationEventProducer))]
        //[return: EventGrid(TopicEndpointUri = "MyEventGridTopicUriSetting", TopicKeySetting = "MyEventGridTopicKeySetting")]
        public static async Task<ProducerResult> AccountValidationEventProducer(
            [EventGrid(TopicEndpointUri = "MyEventGridTopicUriSetting", TopicKeySetting = "MyEventGridTopicKeySetting")] IAsyncCollector<EventGridEvent> outputEvents, 
            [ActivityTrigger] CommandProducer command, ILogger log)
        {
            ProducerResult result = new ProducerResult();
            var eventData = new EventGridEvent(command.MessageId.ToString(), nameof(AccountValidationEventProducer), command, nameof(AccountValidationEventProducer), DateTime.UtcNow, "1.0");
            result.Message = eventData;
            await outputEvents.AddAsync(eventData);
            return result;
        }

        [FunctionName(nameof(ValidateAccount))]
        public static async Task<ProducerResult>  ValidateAccount(
            [EventGrid(TopicEndpointUri = "MyEventGridTopicUriSetting", TopicKeySetting = "MyEventGridTopicKeySetting")] IAsyncCollector<EventGridEvent> outputEvents,
            [EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            CommandProducer command = eventGridEvent.Data as CommandProducer;
            ProducerResult result = new ProducerResult();
            var eventData = new EventGridEvent(command.MessageId.ToString(), command.CommandName, command, command.CommandName, DateTime.UtcNow, "1.0");
            result.Message = eventData;
            await outputEvents.AddAsync(eventData);
            return result;
        }


        [FunctionName(nameof(EventReplyReader))]
        public static async Task EventReplyReader(
            [EventGridTrigger()] EventGridEvent eventGridEvent, [DurableClient] IDurableOrchestrationClient client, ILogger log)
        {
            CommandProducer command = eventGridEvent.Data as CommandProducer;
            await client.RaiseEventAsync(command.MessageId, command.Source, true);
        }

        [FunctionName("Function1_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
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