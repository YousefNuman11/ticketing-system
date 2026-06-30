using System.Text;
using TicketingSystem.Repository.Models;

namespace TicketingSystem.Services.AI.Prompts
{
    public interface IRagPromptBuilder
    {
        string BuildSystemPrompt(IEnumerable<Ticket> matchedTickets, string refusalMessage);
    }

    // Builds the grounded system prompt for the RAG answering step from a set
    // of matched resolved tickets. Kept separate from the handler so prompt
    // wording can change without touching retrieval/orchestration logic.
    public class RagPromptBuilder : IRagPromptBuilder
    {
        public string BuildSystemPrompt(IEnumerable<Ticket> matchedTickets, string refusalMessage)
        {
            var contextBuilder = new StringBuilder();

            foreach (var ticket in matchedTickets)
            {
                contextBuilder.AppendLine($"Ticket Title: {ticket.Title}");
                contextBuilder.AppendLine($"Description: {ticket.Description}");
                contextBuilder.AppendLine("---");
            }

            return
                "You are a support assistant for a ticketing system. " +
                "You must answer ONLY using the information in the provided resolved tickets below. " +
                "Do not use any outside knowledge. " +
                "If the provided tickets do not contain enough information to answer the question, " +
                $"respond with exactly: \"{refusalMessage}\"\n\n" +
                "Resolved tickets:\n" + contextBuilder;
        }
    }
}