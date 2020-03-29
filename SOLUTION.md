1. _Build and run the app. Register a user account, make some lists, 
add some items – have a play and get familiar with the app._

Migrations have not been applied to the Db. Apply it.

Did auto-migrations apply on startup. Usually this is good approach only in development when you
want to be sure that everything is up to date all the time. On real production defer this reponsibility
to DBA, migration tool or some other entity.

Considerations: Use FluentMigrator?

2. _When todo items are displayed in browser in the details page, they are listed in an arbitrary order. 
Change Views/TodoList/Detail.cshtml so that items are listed by order of importance: High, Medium, Low_

Remark: I have no experience in writing _cshtml_, _javascript_, _html_, _css_ and any other front-end languages
whatsoever.

I ordered `Model.Items` by `Importance`. There is no need to write `IComparer` since `Importance` is a simple enum and
it is already ordered. In more advanced cases writing own `IComparer` might be required.

3. _Run the unit tests. One test should be failing. The process that maps a TodoItem to a TodoItemEditFields 
instance is failing - the Importance field is not copied. Fix the bug and ensure the test passes._

Commit is self-explanatory.

5. _On the details page, add an option to hide items that are marked as done._

I could not complete this task within reasonable time. I added filtering depending on the view model but I did not
manage to bind the checkbox with ViewModel property.

6. _Currently /TodoList shows all todo-lists that the user is owner of. Change this so it also shows todo-lists that 
the user has at least one item where they are marked as the responsible party_

This could have been solved with single Linq Query but I am worried about performance. In order to optimize a bit
I first create distinct collection of `TodoList` which contain at least 1 item that the user is responsible for
and then select all lists that belong to the list.

Single Linq query could be:
```csharp
    return dbContext.TodoLists
                .Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .Where(tl => tl.Owner.Id == userId && tl.Items.Any(i => i.ResponsiblePartyId == userId));
```

7. _Add a Rank property to the TodoItem class. Add an EntityFramework migration to reflect this change. 
Allow a user to set the rank property on the edit page. Add a new option on the details page to order by rank._

Remarks: `Edit.cshtml` code was copied from `Create.cshtml`.

I added ordering of the list via property `TodoItemOrder Order`. Since I do not know how to link it with the `cshtml`
page I hard coded the order to `Importance`. It can be changed in the constructor of the `ToDoListDetailViewmodel` to
see how it works in action.

I also hard-coded `HideDone` to false. When it is changed to `true` all done tasks will be automatically filtered out.

8. _If the users you register have an avatar added to gravatar.com, then you will see that avatar by their email address in the navigation area and beside items in a list. Instead of just showing an email address to identify a user, make an enhancement that uses the gravatar.com API to download profile information (if any), and extract the user's name. Display the name along side the email address. Consider what would happen if the gravatar service was slow to respond or not working._

Remarks:
- I figured out I can inject services into `cshtml` pages with `@inject` at the top of it by looking at other files.
- Initially I wrote my own hasher MD5 hasher only to later discover that it was already implemented. I moved it to
`Gravatar` package for better organization
- Interface `IGravatarClient` could be using any protocols, as long as it provides the service with user name in
asynchronous fashion. I only implemented `HttpGravatarClient` but it could be easily extended.
- I had not made any `HtmlDocument` parsing before. I found online nuget package `HtmlAgilityPack`. I feel that
getting the value of the class using `XPath` is ugly but I had no experience in doing this. I was using extensively
[this blog post](https://dotnetcoretutorials.com/2018/02/27/loading-parsing-web-page-net-core/).
- I used Polly to wait and retry HttpCalls if they fail. Adding fallback values and fallback behavior should be pretty
simple using Polly policies.

9. _The process of adding items to a list is pretty clunky; the user has to go to a new page, fill in a form, then go back to the list detail page. It would be easier for the user to do all that on the list detail page. Replace the "Add New Item" link with UI that allows creation of items without navigating away from the detail page. You will need to use Javascript and an API that you create._

Remarks:
- I do not know Javascript and thus I completely skipped the javascript side of the task.
- Taking into consideration the above, in order for the application to keep working I had to keep the code as it is.
- I added repository layer to take off the load from the controller. 

10. _Add an API that allows setting of the Rank property (added in Task 8). Add Javascript functionality that allows reordering of list items by rank without navigating away from the detail page_

Remarks:
- I do not know Javascript so could not complete part of the task related to it
- Added new methods to repository interface
- Opted to go with HttpPatch as it seems to be the best fit
- Added custom exception when TodoItem is not found
- controller decides what to do with the exceptions