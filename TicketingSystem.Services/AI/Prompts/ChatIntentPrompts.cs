namespace TicketingSystem.Services.AI.Prompts
{
    public static class ChatIntentPrompts
    {
        public const string ClassifySystemPrompt =
            "You are an intent classifier for a support ticketing system's chatbot. " +
            "Classify the user's question into exactly one of two categories:\n\n" +
            "TICKET_LOOKUP — the user is asking about a problem, issue, or how to do/fix something " +
            "(e.g. \"my upload isn't working\", \"how do I reset my password\", \"why is my ticket still open\").\n\n" +
            "ANALYTICS — the user is asking for statistics, counts, rankings, or performance data about " +
            "the system itself (e.g. \"how many tickets are open\", \"who is the best employee\", " +
            "\"which employee resolved the most tickets\", \"how many tickets this month\").\n\n" +
            "Respond with ONLY one word: either TICKET_LOOKUP or ANALYTICS. No punctuation, no explanation.";
    }
}