*Domain layer*

The purpose of this layer is to contain information about database related items like entities, context, migrations, repository and generic repository methods.
Since this is only demo application for authenticating users with BankID and there is no connection to database, this layer is left empty. 
In case where the client responses need to be stored in a database, we need to add the appropriate items previously mentioned along with some ORM like EF or Dapper.