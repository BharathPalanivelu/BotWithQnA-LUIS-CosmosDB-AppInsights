# Bot data in PowerBI: End to end example
This is an example on how to display bot data such as questions asked, city/country of users using the bot, among other data in Power BI using App Insights and CosmosDB as sources.

>Disclosure. This repo asumes that you have already worked with Bots, QnA and/or LUIS before. If you haven't then these tutorials will sure be useful as the basic parts of such services are not covered here. Please check them first and come back here to learn how to use both services in the same bot: [Azure Bot Service documentation](https://azure.microsoft.com/en-us/services/bot-service/), [Create your first LUIS App](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-get-started-create-app) and [Create your first QnA Maker service](https://www.qnamaker.ai/)

## Quick summary
While it is valuable for a company to create a bot that helps internal/external users whether by accelerating response times from frequently asked questions or by helping them to request a procedure, etc, it is more valuable to gather insights on what said users are asking and seeing if the bot is really answering the way it should. 
Thus, this example gives an easy to follow path on how to gather insights from a bot using Cosmos DB and Application Insights as data sources.    

## Technologies used
- [Azure Bot Framework](https://dev.botframework.com/)
- [Azure Bot Service](https://azure.microsoft.com/en-us/services/bot-service/)
- [LUIS](https://azure.microsoft.com/en-us/services/cognitive-services/language-understanding-intelligent-service/)
- [QnA Maker](https://azure.microsoft.com/en-us/services/cognitive-services/qna-maker/)
- [Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/)
- [Application Insights](https://azure.microsoft.com/en-us/services/application-insights/)
- [Power BI](http://powerbi.com/)

## Architecture

![Architecture](images/architecture.png)

## A different approach on LUIS with QnA
On this example, LUIS service plays a support role instead of being the main actor on a regular LUIS-QnA integration service. 
What does this mean? Basically, that QnA will act as the only answer provider. As it is seen in the architecture above the conversation flow goes like this:
- User writes to bot
- Bot tries to retrieve an answer from QnA
- If there is an answer, it is posted to the user
    - The conversation ends
    - If there is not an answer, the bot sends the question to LUIS and LUIS tries to detect an intent
        - If LUIS doesn't return an intent, the conversation ends with the bot saying that it couldn't understand the question
        - If LUIS retrieves an intent, said intent is send to QnA as a new question
            - If QnA returns an answer, it is posted to the user and the conversation ends
            - If QnA doesn't return an answer, the converrsation ends with the bot saying that it couldn't understand the question

### 'One size fits all' scenario?
No, this scenario is recommended when:
- QnA will be the main knowledge base
- The bot goal is to answer questions from users instead of taking actions depending on commands from such users
- The ways of asking a unique question are complex because of the language / context / scenario of the bot's users
- There's no much need on detecting entities in a sentence using LUIS or other NLP services

## Preparing the scenario
### Creating the QnA maker service
### Creating the LUIS service
### Creating the Azure Bot service 
### Creating the Cosmos DB service 

## Coding time ##
### Creating the dialog flow
### Custom Events in App Insights
### Writing to Cosmos DB

## Reporting time
### Cosmos DB
### Application Insights
#### Continuous export
#### Saving formated blobs
### Integration

## Conclusion 

## References