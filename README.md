# SalesForceManager

**SalesForceManager class currently those core methods:**
*Connect()* - connects to SalesForce API using clientId/clientKey and SF credentials
*GetObjectsByName(string objectName, string fields = "")* - return any SF object with dynamic access, like Account, Contact or Lead. You could specify second parameter to fetch only specified fields.
*GetObjectFields(string objectName)* - returns all object field names, including custom fields. 
*GetOpportunitiesWithHistory()* - returns all Opportunity objects and their field history.

See *Program.cs* for example of usage of those methods.

**Preparation:**
1. Use this StackOverflow question to create and setup Connected SalesForce Application - https://stackoverflow.com/questions/12794302/salesforce-authentication-failing
Make sure that after app creation you setup all security parameters as described in post. 
2. After creation and setup app you have to obtain ConsumerKey and ConsumerSecret from Application page like here -  http://prntscr.com/mom7xt
3. Then assign your consumerKey, consumerSecret, salesforceLogin and salesForcePassword to appropriate variables at the top of Program.cs. Those values will be passed to SalesForceManager constructor.

**Notes:**
- SalesForceManager class build on top of SalesForceSharp project. It derived from SalesForceSharp.SalesForceClient class which contains basic methods to manage data on SalesForce:
Such as Query() - will send an SOQL query to SalesForce endpoint (will work like an SQL queries with some differences). See SalesForceSharp docs for more info.
- SalesForceManager uses NewtonSoft.Json 7.0.1 and RestSharp 105.0.1 as a dependencies. Those one dpeendencies of SalesForceSharp as well. 
It is compatible with .NET 4.0 and later.
- There is a limits for 15000 requests per 24 hour for SalesForce API. For paid plans there are larger limits. Check https://developer.salesforce.com/docs/atlas.en-us.218.0.api.meta/api/implementation_considerations.htm?SearchType=Stem
for more details.

**Developer's Notes:**
- There are fields in Opportunity model object that is not built-in: BillingCountry, FYCV. Those will be set dynamicaly and could be null if columns not exists.
- List of OpportunityHistory objects that should contain Opportunity object may contain duplicate objects. Means with the same CreatedDate, StageName and OpportunityName. This is easily could be excluded, but I left it as is
because this comes from SalesForce API response.
