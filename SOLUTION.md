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