1. Migrations have not been applied to the Db. Apply it.
Did auto-migrations apply on startup. Usually this is good approach only in development when you
want to be sure that everything is up to date all the time. On real production defer this reponsibility
to DBA, migration tool or some other entity.

Considerations: Use FluentMigrator?